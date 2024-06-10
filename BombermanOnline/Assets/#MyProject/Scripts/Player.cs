using SoftGear.Strix.Client.Core.Model.Manager.Filter;
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
        fps ??= new FPS(map.m_mapSet, rb, gameObject, m_mainCamera.gameObject);
        InitPlayer();
        InitLocalPlayer();
    }


    private void Update()
    {
        if (isLocal == false) return;
        PlayerSettings();
    }

    private void FixedUpdate()
    {
        if (isLocal == false) return;
        fps.AddForceLocomotion(_currSpeed, m_dashSpeed);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(ExplosionTag))
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
                    AddBombPool();
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
    }


    private void OnCollisionStay(Collision collision)
    {
        if (isLocal == false) return;
        if (collision.gameObject.CompareTag(FrozenFloorTag))
        {
            rb.drag = m_slipDrag;
        }
        else
        {
            rb.drag = m_defaultDrag;
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
    [SerializeField] private float m_defaultDrag;      // 滑る床にいるときの空気抵抗
    [SerializeField] private float m_slipDrag;      // 滑る床にいるときの空気抵抗

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

    private Pool<NormalBomb> _bombPool = new Pool<NormalBomb>();

    private readonly Vector3 mapCameraPos = new Vector3(0, 150, 0);         // マップカメラのポジション

    private const string ItemBombTag = "Item_Bomb";
    private const string ItemFireTag = "Item_Fire";
    private const string ItemSpeedTag = "Item_Speed";
    private const string ItemLifeTag = "Item_Life";
    private const string ExplosionTag = "Explosion";
    private const string FrozenFloorTag = "FrozenFloor";


    [Header("オブジェクト参照")]
    [SerializeField] Camera m_mainCamera;         // プレイヤーに追従するカメラ
    [SerializeField] Camera m_mapCamera;          // マップUIのカメラ
    [SerializeField] NormalBomb m_bomb;           // 生成する爆弾
    [SerializeField] SpesialBomb m_specialBomb1;   // 特殊爆弾1
    [SerializeField] SpesialBomb m_specialBomb2;   // 特殊爆弾2
    [SerializeField] TextMeshProUGUI m_playerInfoText;
    [SerializeField] GameObject m_titleCanvas;

    [Header("コンポーネント")]
    [SerializeField] UIManager ui;
    FPS fps;

    // ===プロパティ================================================================================
    public string PlayerName => StrixNetwork.instance.roomMembers[strixReplicator.ownerUid].GetName();

    /// <summary>火力</summary>
    public int Firepower => m_firepower;
    
    /// <summary>爆弾所持最大数</summary>
    public int BombMaxCount => _bombPool.Count;                  // 手持ちの爆弾最大値
    
    /// <summary>爆弾所持数</summary>
    public int BombCount => _bombPool.list.Where(b => b.isHeld).Count();  // 手に持っている爆弾数

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
        if (gameManager.IsGaming == false) return;
        // カメラ、移動の設定
        fps.PlayerViewport();
        fps.ClampMoveRange();
        //fps.CursorLock();

        // キー入力によるプレイヤーのアクション
        PutBomb();
        PutSpesialBomb();
        PutArtificialStone();

        // マップカメラのポジション設定
        Vector3 mapCamPos = transform.position + mapCameraPos;
        m_mapCamera.transform.position = mapCamPos;
        
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
        _currSpeed = m_speed;
        _currDashSpeed = m_dashSpeed;
        AddBombPool();
        CallSetMembersColor();
        CallShowPlayerName();
    }


    /// <summary>
    /// Localのプレイヤーの初期化をします
    /// この関数はStart関数で呼び出します
    /// </summary>
    private void InitLocalPlayer()
    {

    }


    /// <summary>
    /// ゲームオーバー
    /// </summary>
    private void CallGameOver() { RpcToAll(nameof(GameOver)); }
    [StrixRpc]
    private void GameOver()
    {
        m_mainCamera.transform.position = Pos = mapCameraPos;
        m_mainCamera.transform.rotation = Rot = Quaternion.Euler(90f, 0f, 0f);
        gameObj.SetActive(false);
        m_mainCamera.GetComponent<CameraView>().enabled = false;
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
        //// リストに爆弾がない場合は
        var b = _bombPool.Take(b => b.isHeld);

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


    /// <summary>
    /// キー入力によってスペシャルボムを置きます
    /// </summary>
    private void PutSpesialBomb()
    {
        if(Input.GetMouseButtonDown(0))
        {
            CallGenarateSpesialBomb1();
        }
        if(Input.GetMouseButtonDown(1))
        {
            CallGenarateSpesialBomb2();
        }
    }


    /// <summary>
    /// スペシャルボムを生成します（RPC）
    /// </summary>
    private void CallGenarateSpesialBomb1() { RpcToAll(nameof(GenarateSpesialBomb1)); }
    [StrixRpc]
    private void GenarateSpesialBomb1()
    {
        Vector3 dir = FPS.GetVector3FourDirection(Trafo.rotation.eulerAngles);
     
        m_specialBomb1.GenerateSpesialBomb(Coord, dir, m_firepower);
        AudioManager.PlayOneShot("特殊爆弾を置く");
    }
    private void CallGenarateSpesialBomb2() { RpcToAll(nameof(GenarateSpesialBomb2)); }
    [StrixRpc]
    private void GenarateSpesialBomb2()
    {
        Vector3 dir = FPS.GetVector3FourDirection(Trafo.rotation.eulerAngles);

        m_specialBomb2.GenerateSpesialBomb(Coord, dir, m_firepower);
        AudioManager.PlayOneShot("特殊爆弾を置く");
    }

    // ーーーーーアイテムの処理ーーーーーー

    /// <summary>
    /// 手持ち爆弾の最大値を増やします
    /// </summary>
    private void AddBombPool()
    {
        if (_bombPool.Count < m_bombMaxValue)
        {
            NormalBomb b = Instantiate(m_bomb, CoordPos, Quaternion.identity);
            b.gameObj.SetActive(false);
            b.Initialize(map);
            _bombPool.Add(b);
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
        //isPredictable = Input.GetKey(KeyCode.LeftControl);

        if (isPredictable)
        {
            _currSpeed = _currDashSpeed = m_slowSpeed;
            m_mainCamera.cullingMask |= predictLandmarkMask;
        }
        else
        {
            _currSpeed = m_speed;
            _currDashSpeed = m_dashSpeed;
            m_mainCamera.cullingMask &= ~predictLandmarkMask;
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
        Coord generateCoord = Coord + FPS.GetCoordFourDirection(Rot.eulerAngles);

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

    // ーーーーー特殊爆弾処理ーーーーー
    public void SetSpesialBombType(int slot,SpesialBomb.BombType type)
    {
        if (slot == 0)
        {
            m_specialBomb1.SetBombType(type);
        }
        else if(slot == 1) 
        {
            m_specialBomb2.SetBombType(type);
        }
        else
        {
            Debug.Log("スロットがない！");
        }
    }

    public void AddSpesialBombType(int slot)
    {
        if (slot == 0)
        {
            m_specialBomb1.Add(map);
        }
        else if (slot == 1)
        {
            m_specialBomb2.Add(map);
        }
        else
        {
            Debug.Log("スロットがない！");
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
        m_playerInfoText.text = "P" + (PlayerIndex + 1) + ":" + PlayerName;
    }

}



