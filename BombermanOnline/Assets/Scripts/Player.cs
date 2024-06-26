using System;
using System.Collections.Generic;
using System.Linq;
using TakeshiLibrary;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : Base
{
    // ===イベント==================================================================================

    protected void Start()
    {
        fps ??= new FPS(map.mapSet, rb, gameObject, mainCamera);
        gameManager = GameManager.Instance;
        InitPlayer();
    }


    private void Update()
    {
        PlayerSettings();
        PlayerSystem();
        PutBomb();
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == ExplosionTag)
        {
            LifeCount--;
            AudioManager.PlayOneShot("被ダメージ",1f);
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
            Debug.Log("アイテムゲット！！");
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
    [SerializeField] private float m_speed;         // 移動スピード
    [SerializeField] private float m_dashSpeed;     // ダッシュスピード
    [SerializeField] private float m_upSpeed;
    [SerializeField] private int m_bombMaxValue;    // 爆弾の最大値
    [SerializeField] private int m_firepower;       // 爆弾の火力
    [SerializeField] private int m_life;            // 体力
    [SerializeField] private int m_lifeMaxValue;    // 体力の最大値

    private List<Bomb> bombList = new();            // 手持ちの爆弾リスト

    private readonly Vector3 mapCameraPos = new Vector3(0, 100, 0); // マップカメラのポジション


    private const string ItemBombTag  = "Item_Bomb";
    private const string ItemFireTag  = "Item_Fire";
    private const string ItemSpeedTag = "Item_Speed";
    private const string ItemLifeTag  = "Item_Life";
    private const string ExplosionTag = "Explosion";


    [Header("オブジェクト参照")]
    [SerializeField] GameObject mainCamera;         // プレイヤーに追従するカメラ
    [SerializeField] GameObject mapCamera;          // マップUIのカメラ
    [SerializeField] Bomb bomb;                     // 生成する爆弾
    GameManager gameManager;


    [Header("コンポーネント")]
    [SerializeField] UIManager ui;
    FPS fps;

    // ===プロパティ================================================================================
    public int Firepower => m_firepower;                        // 火力ゲッター
    public int BombMaxCount => bombList.Count;                  // 手持ちの爆弾最大値
    public int BombCount => bombList.Where(b => b.isHeld).Count();  // 手に持っている爆弾数
    public int LifeCount
    {
        get
        {
            return m_life;
        }
        private set 
        { 
            if(value > m_lifeMaxValue)
            {
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
        fps.CameraViewport();
        fps.PlayerViewport();
        fps.AddForceLocomotion(m_speed, m_dashSpeed);
        fps.ClampMoveRange();
        fps.CursorLock();
        // マップカメラのポジション設定
        Vector3 mapCamPos = transform.position + mapCameraPos;
        mapCamera.transform.position = mapCamPos;
    }


    private void PlayerSystem()
    {
        if(m_life <= 0)
        {
            GameOver();
        }
    }


    /// <summary>
    /// プレイヤーの初期化をします
    /// この関数はStart関数で呼び出します
    /// </summary>
    private void InitPlayer()
    {
        AddBombList();
    }


    /// <summary>
    /// ゲームスタート
    /// </summary>
    public void GameStart()
    {
        Coord = map._startCoords[0];
        enabled = true;
    }


    /// <summary>
    /// ゲームオーバー
    /// </summary>
    public void GameOver()
    {
        gameObj.SetActive(false);
    }


    /// <summary>
    /// キー入力によって爆弾を置きます
    /// </summary>
    private void PutBomb()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Bomb b = bombList.Where(b => b.isHeld).FirstOrDefault();
            if (b == null)
            {
                ui.ShowGameText("No bomb !!",1);
                AudioManager.PlayOneShot("爆弾がない");
                return;
            }
            AudioManager.PlayOneShot("爆弾を置く");
            b.Put(Coord,Firepower);
        }
    }


    /// <summary>
    /// 手持ち爆弾の最大値を増やします
    /// </summary>
    private void AddBombList()
    {
        if (bombList.Count < m_bombMaxValue)
        {
            Bomb b = Instantiate(bomb, CoordPos, Quaternion.identity);
            b.Initialize(map);
            bombList.Add(b);
        }
        else
        {
            AudioManager.PlayOneShot("爆弾がない");
            ui.ShowGameText("Full Stack !!", 1);
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

    private void FierPowerUp()
    {
        m_firepower++;
    }

    private void LifeUp()
    {
        LifeCount++;
    }
}



