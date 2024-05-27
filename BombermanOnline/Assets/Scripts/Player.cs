using SoftGear.Strix.Unity.Runtime;
using System.Collections.Generic;
using System.Linq;
using TakeshiLibrary;
using TMPro;
using UnityEngine;

public class Player : Base
{
    // ===イベント==================================================================================

    protected void Start()
    {
        map = GameMap.Instance;
        gameManager = GameManager.Instance;
        fps ??= new FPS(map.m_mapSet, rb, gameObject, mainCamera);
        InitPlayer();
        InitLocalPlayer();
    }



    private void Update()
    {
        if (isLocal == false) return;
        PlayerSettings();
        PutBomb();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == ExplosionTag)
        {
            if (isLocal == false) return;
            if (isInvincible) return;
            Life -= 10f;
            isInvincible = true;
            AudioManager.PlayOneShot("被ダメージ", 1f);
            ui.ShowDamageEffectUI();
        }

        // アイテムのレイヤーなら
        if (gameManager.itemManager.itemLayer == (gameManager.itemManager.itemLayer | (1 << other.gameObject.layer)))
        {
            switch (other.tag)
            {
                case ItemBombTag:
                    AddBombList();
                    break;

                case ItemFireTag:
                    FierPowerUp();
                    break;

                case ItemSpeedTag:
                    SpeedUp();
                    break;

                case ItemLifeTag:
                    LifeUp();
                    break;
            }

            AudioManager.PlayOneShot("アイテムゲット");
            other.gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("アイテムじゃない");
        }
    }
    // ===変数======================================================================================

    [Header("パラメーター")]
    public Vector3 posY;
    
    // スピード
    [SerializeField] private float m_speed;         // 移動スピード
    [SerializeField] private float m_dashSpeed;     // ダッシュスピード
    [SerializeField] private float m_upSpeed;

    // 体力
    [SerializeField] private float m_life;            // 体力
    public float m_lifeMaxValue;    // 体力の最大値

    // 爆弾
    [SerializeField] private int m_bombMaxValue;    // 爆弾の最大値
    [SerializeField] private int m_firepower;       // 爆弾の火力

    // 無敵
    [SerializeField] private float invncebleTime;     // 非ダメージ後の無敵時間
    private bool isInvincible = false;
    private float invncebleCount = 0;

    // 予測
    private bool isPredictable = false;


    private List<Bomb> bombList = new();            // 手持ちの爆弾リスト



    private readonly Vector3 mapCameraPos = new Vector3(0, 150, 0); // マップカメラのポジション

    private const string ItemBombTag = "Item_Bomb";
    private const string ItemFireTag = "Item_Fire";
    private const string ItemSpeedTag = "Item_Speed";
    private const string ItemLifeTag = "Item_Life";
    private const string ExplosionTag = "Explosion";


    [Header("オブジェクト参照")]
    [SerializeField] GameObject mainCamera;         // プレイヤーに追従するカメラ
    [SerializeField] GameObject mapCamera;          // マップUIのカメラ
    [SerializeField] Bomb bomb;                     // 生成する爆弾
    [SerializeField] TextMeshProUGUI playerInfoText;
    [SerializeField] GameObject titleCanvas;


    [Header("コンポーネント")]
    [SerializeField] UIManager ui;
    FPS fps;

    // ===プロパティ================================================================================
    public string PlayerName => StrixNetwork.instance.roomMembers[strixReplicator.ownerUid].GetName();

    /// <summary>火力</summary>
    public int Firepower => m_firepower;
    
    /// <summary>爆弾所持最大数</summary>
    public int BombMaxCount => bombList.Count;                  // 手持ちの爆弾最大値
    
    /// <summary>爆弾所持数</summary>
    public int BombCount => bombList.Where(b => b.isHeld).Count();  // 手に持っている爆弾数

    /// <summary>ライフ数</summary>
    public float Life
    {
        get
        {
            return m_life;
        }
        private set
        {
            if (value > m_lifeMaxValue)
            {
                if (isLocal)
                    ui.ShowGameText("Full Life !!", 1);
                AudioManager.PlayOneShot("爆弾がない");
                return;
            }
            m_life = value;
        }
    }


    // ===関数================================================================================
    /// <summary>
    /// プレイヤーの設定をします
    /// この関数はUpdate関数で呼び出します
    /// </summary>
    private void PlayerSettings()
    {
        // カメラ、移動の設定
        fps.PlayerViewport();
        fps.AddForceLocomotion(m_speed, m_dashSpeed);
        fps.ClampMoveRange();

        //fps.CursorLock();
        // マップカメラのポジション設定
        Vector3 mapCamPos = transform.position + mapCameraPos;
        mapCamera.transform.position = mapCamPos;
        
        // アイテム効果
        Invincible();
        Predictive();

        // ゲームオーバー処理
        if (m_life <= 0)
        {
            CallGameOver();
        }
    }


    /// <summary>
    /// プレイヤーの初期化をします
    /// この関数はStart関数で呼び出します
    /// </summary>
    private void InitPlayer()
    {
        AddBombList();
        CallSetMembersColor();
        CallShowPlayerName();
    }


    /// <summary>
    /// Localのプレイヤーの初期化をします
    /// この関数はStart関数で呼び出します
    /// </summary>
    private void InitLocalPlayer()
    {
        if (isLocal)
        {
            if (GetComponent<AudioListener>() == null)
            {
                gameObj.AddComponent<AudioListener>();
            }
        }
        else
        {
            AudioListener listener = GetComponent<AudioListener>();
            if (listener != null)
            {
                Destroy(listener);
            }
        }
    }


    /// <summary>
    /// ゲームオーバー
    /// </summary>
    private void CallGameOver() { RpcToAll(nameof(GameOver)); }
    [StrixRpc]
    private void GameOver()
    {
        mainCamera.transform.position = Pos = mapCameraPos;
        mainCamera.transform.rotation = Rot = Quaternion.Euler(90f, 0f, 0f);
        gameObj.SetActive(false);
        mainCamera.GetComponent<CameraView>().enabled = false;
        ui.ShowGameText("P" + (PlayerIndex + 1) + ":" + PlayerName + " is Down", 2);
    }


    /// <summary>
    /// キー入力によって爆弾を置きます
    /// </summary>
    private void PutBomb()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // RPCで呼び出さないと、他の人のマップには爆発が反映しない（見た目では置かれるが、爆弾のスクリプトの処理が行われない）
            RpcToAll(nameof(GenerateBomb));
        }
    }


    /// <summary>
    /// 爆弾を生成し、置きます。
    /// </summary>
    [StrixRpc]
    void GenerateBomb()
    {
        Bomb b = bombList.Where(b => b.isHeld).FirstOrDefault();
        // リストに爆弾がない場合は
        if (b == null)
        {
            // 自分だけに処理を行う
            if (isLocal)
            {
                ui.ShowGameText("No bomb !!", 1);
                AudioManager.PlayOneShot("爆弾がない");
            }
            return;
        }
        AudioManager.PlayOneShot("爆弾を置く");
        b.Put(Coord, Firepower);
    }


    /// <summary>
    /// 手持ち爆弾の最大値を増やします
    /// </summary>
    private void AddBombList()
    {
        if (bombList.Count < m_bombMaxValue)
        {
            Bomb b = Instantiate(bomb, CoordPos, Quaternion.identity);
            b.gameObj.SetActive(false);
            b.Initialize(map);
            bombList.Add(b);
        }
        // 手持ちの爆弾が最大所持数なら
        else
        {
            // 自分にだけ処理
            if (isLocal)
            {
                ui.ShowGameText("Full Stack !!", 1);
                AudioManager.PlayOneShot("爆弾がない");
            }
        }
    }


    /// <summary>
    /// 無敵時の処理
    /// </summary>
    private void Invincible()
    {
        if (Input.GetKey(KeyCode.Alpha1))
        {
            isInvincible = true;
        }
        if (isInvincible)
        {
            // 無敵時間のカウント
            invncebleCount += Time.deltaTime;

            if (invncebleCount > invncebleTime)
            {
                isInvincible = false;
                invncebleCount = 0;
            }
        }
    }


    private void Predictive()
    {
        if (isInvincible)
        {
            Debug.Log("予測眼！！");
            map.UndoDefaultPlaneColor();
        }
    }


    /// <summary>
    /// スピードアップします
    /// </summary>
    private void SpeedUp()
    {
        m_speed += m_upSpeed;
        m_dashSpeed += m_upSpeed;
    }


    /// <summary>
    /// 火力をアップさせます
    /// </summary>
    private void FierPowerUp()
    {
        m_firepower++;
    }

    /// <summary>
    /// ライフを増やします
    /// </summary>
    private void LifeUp()
    {
        Life += 10;
    }


    /// <summary>
    /// [RPC]プレイヤーの色を設定します。
    /// </summary>
    private void CallSetMembersColor() => RpcToAll(nameof(SetMembersColor));
    [StrixRpc]
    private void SetMembersColor()
    {
        if (PlayerIndex == 0)
        {
            SetPlayerColor(Color.red);
        }
        if (PlayerIndex == 1)
        {
            SetPlayerColor(Color.blue);
        }
        if (PlayerIndex == 2)
        {
            SetPlayerColor(Color.yellow);
        }
        if (PlayerIndex == 3)
        {
            SetPlayerColor(Color.green);
        }
    }

    /// <summary>
    /// プレイヤーの色を変更します。
    /// </summary>
    /// <param name="color">色</param>
    private void SetPlayerColor(Color color) { gameObject.GetComponent<Renderer>().material.color = color; }


    /// <summary>
    /// プレイヤーの情報を更新します
    /// </summary>
    private void CallShowPlayerName() => RpcToAll(nameof(ShowPlayerName));
    [StrixRpc]
    private void ShowPlayerName()
    {
        playerInfoText.text = "P" + (PlayerIndex + 1) + ":" + PlayerName;
    }

}



