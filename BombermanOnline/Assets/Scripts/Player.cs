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
        fps ??= new FPS(map.m_mapSet, rb, gameObject, mainCamera.gameObject);
        InitPlayer();
        InitLocalPlayer();
    }



    private void Update()
    {
        if (isLocal == false) return;
        PlayerSettings();
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
    private float _currSpeed;                       // 現在のスピード
    private float _currDashSpeed;                   // 現在のダッシュスピード
    [SerializeField] private float m_speed;         // 普通のスピード
    [SerializeField] private float m_dashSpeed;     // ダッシュスピード
    [SerializeField] private float m_slowSpeed;     // 遅いスピード
    [SerializeField] private float m_upSpeed;       // 上がるスピード

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
    [SerializeField] LayerMask predictLandmarkMask;
    private bool isPredictable = false;


    private List<Bomb> bombList = new();            // 手持ちの爆弾リスト



    private readonly Vector3 mapCameraPos = new Vector3(0, 150, 0); // マップカメラのポジション

    private const string ItemBombTag = "Item_Bomb";
    private const string ItemFireTag = "Item_Fire";
    private const string ItemSpeedTag = "Item_Speed";
    private const string ItemLifeTag = "Item_Life";
    private const string ExplosionTag = "Explosion";


    [Header("オブジェクト参照")]
    [SerializeField] Camera mainCamera;         // プレイヤーに追従するカメラ
    [SerializeField] Camera mapCamera;          // マップUIのカメラ
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

    // ーーーーープレイヤーのシステムーーーーーー

    /// <summary>
    /// プレイヤーの設定をします
    /// この関数はUpdate関数で呼び出します
    /// </summary>
    private void PlayerSettings()
    {
        // カメラ、移動の設定
        fps.PlayerViewport();
        fps.AddForceLocomotion(_currSpeed, m_dashSpeed);
        fps.ClampMoveRange();
        //fps.CursorLock();

        // キー入力によるプレイヤーのアクション
        PutBomb();
        PutArtificialStone();

        // マップカメラのポジション設定
        Vector3 mapCamPos = transform.position + mapCameraPos;
        mapCamera.transform.position = mapCamPos;
        
        // アイテム効果
        Invincible();
        PredictiveEye();

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
        _currSpeed = m_speed;
        _currDashSpeed = m_dashSpeed;
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

    // ーーーーープレイヤーアクションーーーーー

    /// <summary>
    /// キー入力によって爆弾を置きます
    /// </summary>
    private void PutBomb()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CallGenerateBomb();
        }
    }


    /// <summary>
    /// 爆弾を生成し、置きます。
    /// RPCで呼び出さないと、他の人のマップには爆発が反映しない（見た目では置かれるが、爆弾のスクリプトの処理が行われない）
    /// </summary>
    private void CallGenerateBomb() { RpcToAll(nameof(GenerateBomb)); }
    [StrixRpc]
    private void GenerateBomb()
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
    /// キー入力によって人工石ブロックを生成します
    /// </summary>
    private void PutArtificialStone()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            CallGenerateArtificialStone();
        }
    }

    // ーーーーーアイテムの処理ーーーーーー

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
    /// 無敵時の処理
    /// </summary>
    private void Invincible()
    {
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


    /// <summary>
    /// 予測眼の処理
    /// </summary>
    private void PredictiveEye()
    {
        // 左コントロールキー
        isPredictable = Input.GetKey(KeyCode.LeftControl);

        if (isPredictable)
        {
            _currSpeed = _currDashSpeed = m_slowSpeed;
            mainCamera.cullingMask |= predictLandmarkMask;
        }
        else
        {
            _currSpeed = m_speed;
            _currDashSpeed = m_dashSpeed;
            mainCamera.cullingMask &= ~predictLandmarkMask;
        }
    }


    /// <summary>
    /// プレイヤーの前に石を生成します
    /// </summary>
    private void CallGenerateArtificialStone() { RpcToAll(nameof(GenerateArtificialStone)); }
    [StrixRpc]
    private void GenerateArtificialStone()
    {
        // プレイヤーのひとつ前のマスの座標
        Coord generateCoord = Coord + FPS.GetVector3FourDirection(Rot.eulerAngles);

        // 何もないマスなら
        if (map.IsEmpty(generateCoord))
        {
            map.GenerateStone(generateCoord);
            map.SetArtificialStoneTexture(generateCoord);
        }
        else
        {
            Debug.Log("そこはすでに石がある！！");
        }
    }

    // ーーーーーそれぞれのプレイヤーの見たなどーーーーー

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



