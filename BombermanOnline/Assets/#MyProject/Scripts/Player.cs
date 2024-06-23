using SoftGear.Strix.Unity.Runtime;
using System;
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
    }


    private void Update()
    {
        if (isLocal == false) return;
        PlayerSettings();
    }


    private void FixedUpdate()
    {
        if (isLocal == false) return;
        fps.AddForceLocomotion(_currSpeed, _currSpeed);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(ExplosionTag))
        {
            if (isLocal == false) return;
            if (isInvincible) return;
            TakenDamage();
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

                case ItemBrickTag:
                    BrickUp();
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

    [Header("移動スピード")]
    [SerializeField] private float m_speed;         // 普通のスピード
    [SerializeField] private float m_slowSpeed;     // 遅いスピード
    [SerializeField] private float m_upSpeed;       // 上がるスピード
    [SerializeField] private float m_defaultDrag;      // 滑る床にいるときの空気抵抗
    [SerializeField] private float m_slipDrag;      // 滑る床にいるときの空気抵抗
    private float _currSpeed;                       // 現在のスピード

    [Header("ダッシュ")]
    [SerializeField] private float m_dashbleTime;
    [SerializeField] private float m_dashSpeed;     // ダッシュスピード
    [SerializeField] private float m_upDashSpeed;       // 上がるダッシュスピード
    private bool _isDashble = true;
    private float _dashbleCount = 0;

    [Header("体力")]
    [SerializeField] private int m_life;            // 体力
    public int m_lifeMaxValue;              // 体力の最大値
    public int m_healLife;                  // 回復

    [Header("爆弾")]
    [SerializeField] private int m_bombMaxValue;    // 爆弾の最大値
    [SerializeField] private int m_firepower;       // 爆弾の火力

    [Header("特殊爆弾")]
    [SerializeField] SpesialBomb.BombType m_specialBombType1;
    [SerializeField] SpesialBomb.BombType m_specialBombType2;
    [SerializeField] float m_specialBombLockTime1;
    [SerializeField] float m_specialBombLockTime2;
    private const int Slot1 = 0;
    private const int Slot2 = 1;

    [Header("人工石")]
    private int _brickCount = 0;                    // 持っているレンガの数
    [SerializeField] private int m_brickUpValue;    // アイテムを拾った時の上昇量
    [SerializeField] private int m_brickValue;      // 生成に必要なレンガの量

    [Header("無敵")]
    [SerializeField] private float invncebleTime;     // 非ダメージ後の無敵時間
    private bool isInvincible = false;
    private float invncebleCount = 0;

    [Header("スタン")]
    [SerializeField] private float stunTime;
    private float stunCount;
    private bool isStun = false;

    [Header("予測")]
    [SerializeField] LayerMask predictLandmarkMask;
    private bool isPredictable = false;


    private Pool<NormalBomb> _bombPool = new Pool<NormalBomb>();

    private readonly Vector3 mapCameraPos = new Vector3(0, 150, 0);         // マップカメラのポジション

    private const int _Damage = 1;

    private const string ItemBombTag = "Item_Bomb";
    private const string ItemFireTag = "Item_Fire";
    private const string ItemSpeedTag = "Item_Speed";
    private const string ItemLifeTag = "Item_Life";
    private const string ItemBrickTag = "Item_Brick";
    private const string ExplosionTag = "Explosion";
    private const string FrozenFloorTag = "FrozenFloor";

    [Space]
    [Header("オブジェクト参照")]
    [SerializeField] Camera m_mainCamera;         // プレイヤーに追従するカメラ
    [SerializeField] Camera m_mapCamera;          // マップUIのカメラ
    [SerializeField] NormalBomb m_bomb;           // 生成する爆弾
    [SerializeField] SpesialBomb m_specialBomb1;   // 特殊爆弾1
    [SerializeField] SpesialBomb m_specialBomb2;   // 特殊爆弾2
    [SerializeField] TextMeshProUGUI m_playerInfoText;
    [SerializeField] GameObject m_titleCanvas;

    [Space]
    [Header("コンポーネント")]
    [SerializeField] UIManager ui;
    [SerializeField] TitleResultManager titleResultManager;
    FPS fps;

    // ===プロパティ================================================================================
    public string PlayerName => StrixNetwork.instance.roomMembers[strixReplicator.ownerUid].GetName();

    public bool IsDashble => _isDashble;

    /// <summary>火力</summary>
    public int Firepower => m_firepower;

    /// <summary>レンガ所持数</summary></summary>
    public int BrickCount => _brickCount;
    
    /// <summary>爆弾所持最大数</summary>
    public int BombMaxCount => _bombPool.Count;                  // 手持ちの爆弾最大値
    
    /// <summary>爆弾所持数</summary>
    public int BombCount => _bombPool.list.Where(b => b.isHeld).Count();  // 手に持っている爆弾数

    ///　<summary>特殊爆弾所持数</summary>
    public int Special1Count => m_specialBomb1.PoolList.Where(b => b.activeSelf == false).Count();   // 手に持っている特殊ボム1
    public int Special2Count => m_specialBomb2.PoolList.Where(b => b.activeSelf == false).Count();   // 手に持っている特殊ボム2

    /// <Summary>特殊爆弾最大所持数</summary>
    public int Special1MaxCount => m_specialBomb1.PoolList.Count();
    public int Special2MaxCount => m_specialBomb2.PoolList.Count();

    /// <summary>特殊爆弾のロック時間</summary>
    public float Special1LockTime => m_specialBombLockTime1 - gameManager.GameTime;
    public float Special2LockTime => m_specialBombLockTime2 - gameManager.GameTime;

    /// <summary>ライフ数</summary>
    public int Life
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
        Dash();
        //fps.CursorLock();

        // キー入力によるプレイヤーのアクション
        PutBomb();
        PutSpesialBomb();
        PutArtificialStone();
        InputDash();


        // マップカメラのポジション設定
        Vector3 mapCamPos = transform.position + mapCameraPos;
        m_mapCamera.transform.position = mapCamPos;

        // アイテム効果
        Invincible();
        PredictiveEye();

        // 爆弾処理
        UnlockSpecialBomb();

        // ゲームオーバー処理
        if (m_life <= 0)
        {
            CallGameOver();
            if (gameManager.IsGameFinish)
                titleResultManager.CallShowResult();
        }
        if (Input.GetKeyDown(KeyCode.Delete)) 
        {
            if(gameManager.IsGameFinish)
                titleResultManager.CallShowResult();
        }
    }


    /// <summary>
    /// プレイヤーの初期化をします
    /// この関数はStart関数で呼び出します
    /// </summary>
    private void InitPlayer()
    {
        _currSpeed = m_speed;
        AddBombPool();
        CallSetMembersColor();
        CallShowPlayerName();
    }


    /// <summary>
    /// ゲームオーバー
    /// </summary>
    private void CallGameOver() { RpcToAll(nameof(GameOver)); }
    [StrixRpc]
    private void GameOver()
    {
        if (isLocal)
        {
            m_mainCamera.transform.position = Pos = mapCameraPos;
            m_mainCamera.transform.rotation = Rot = Quaternion.Euler(90f, 0f, 0f);
            gameObj.SetActive(false);
            m_mainCamera.GetComponent<CameraView>().enabled = false;
        }
        gameManager.PlayerList.Remove(this);
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
            if (_brickCount >= m_brickValue)
            {
                _brickCount -= m_brickValue;
                CallGenerateArtificialStone();
            }
            else 
            {
                Debug.Log("レンガが足りない！");
            }
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

        if (m_specialBomb1.GenerateSpesialBomb(m_specialBombType1, Coord, dir, m_firepower))
        {
            AudioManager.PlayOneShot("特殊爆弾を置く");
        }
        else
        {
            AudioManager.PlayOneShot("爆弾がない");
        }
    }
    private void CallGenarateSpesialBomb2() { RpcToAll(nameof(GenarateSpesialBomb2)); }
    [StrixRpc]
    private void GenarateSpesialBomb2()
    {
        Vector3 dir = FPS.GetVector3FourDirection(Trafo.rotation.eulerAngles);

        if (m_specialBomb2.GenerateSpesialBomb(m_specialBombType2, Coord, dir, m_firepower))
        {
            AudioManager.PlayOneShot("特殊爆弾を置く");
        }
        else
        {
            AudioManager.PlayOneShot("爆弾がない");
        }
    }


    /// <summary>
    /// キー入力によってダッシュします
    /// </summary>
    private void InputDash()
    {
        if (_isDashble)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                _isDashble = false;
                rb.AddForce(transform.forward * m_dashSpeed, ForceMode.VelocityChange);
            }
        }
    }

    /// <summary>
    /// ダッシュ処理
    /// </summary>
    private void Dash()
    {
        if (_isDashble == false)
        {
            // 無敵時間のカウント
            _dashbleCount += Time.deltaTime;

            if (_dashbleCount > m_dashbleTime)
            {
                _isDashble = true;
                _dashbleCount = 0;
            }
        }
    }

    /// <summary>
    /// ダメージ処理
    /// </summary>
    private void TakenDamage()
    {
        Life -= _Damage;
        isInvincible = true;
        AudioManager.PlayOneShot("被ダメージ", 1f);
        ui.ShowDamageEffectUI();
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
        m_dashSpeed += m_upDashSpeed;
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
        Life += m_healLife;
    }

    /// <summary>
    /// レンガを増やします
    /// </summary>
    private void BrickUp()
    {
        _brickCount += m_brickUpValue;
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
            _currSpeed = m_slowSpeed;
            m_mainCamera.cullingMask |= predictLandmarkMask;
        }
        else
        {
            _currSpeed = m_speed;
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
    public void CallSetSpecialBombType(int slot,int type) { RpcToAll(nameof(SetSpecialBombType),slot,type); }
    [StrixRpc]
    public void SetSpecialBombType(int slot,int type)
    {
        if (slot == Slot1)
        {
            m_specialBombType1 = (SpesialBomb.BombType)Enum.ToObject(typeof(SpesialBomb.BombType), type);
            m_specialBomb1.ClearBombType();
        }
        else if(slot == Slot2) 
        {
            m_specialBombType2 = (SpesialBomb.BombType)Enum.ToObject(typeof(SpesialBomb.BombType), type);
            m_specialBomb2.ClearBombType();
        }
        else
        {
            Debug.Log("スロットがない！");
        }
    }

    public void CallAddSpecialBomb(int slot) { RpcToAll(nameof(AddSpecialBomb), slot); }
    [StrixRpc]
    public void AddSpecialBomb(int slot)
    {
        if (slot == Slot1)
        {
            m_specialBomb1.Add(m_specialBombType1,map);
        }
        else if (slot == Slot2)
        {
            m_specialBomb2.Add(m_specialBombType2,map);
        }
        else
        {
            Debug.Log("スロットがない！");
        }
    }

    private void UnlockSpecialBomb()
    {
        if (Special1MaxCount <= 0 && Special1LockTime <= 0)
        {
            CallAddSpecialBomb(Slot1);
        }
        if (Special2MaxCount <= 0 && Special2LockTime <= 0)
        {
            CallAddSpecialBomb(Slot2);
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