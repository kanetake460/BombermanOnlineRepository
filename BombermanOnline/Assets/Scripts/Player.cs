using System.Collections.Generic;
using System.Linq;
using TakeshiLibrary;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMovement : Base
{
    // ===イベント==================================================================================

    protected void Start()
    {
        fps ??= new FPS(map.mapSet,rb,gameObject,mainCamera);

        InitPlayer();
    }
    

    private void Update()
    {
        PlayerSettings();
        PutBomb();
    }

    // ===変数======================================================================================

    [Header("パラメーター")]
    [SerializeField] private float speed;
    [SerializeField] private float dashSpeed;
    private List<Bomb> bombList = new();
    [SerializeField] private int m_bombMaxValue;
    private readonly Vector3 mapCameraPos = new Vector3(0, 100, 0);

    [Header("オブジェクト参照")]
    [SerializeField] GameObject mainCamera;
    [SerializeField] GameObject mapCamera;
    [SerializeField] GameObject bombPrefab;
    [SerializeField] Bomb bomb;


    [Header("コンポーネント")]
    FPS fps;
    
    // ===プロパティ================================================================================

    // ===関数================================================================================
    private void PlayerSettings()
    {
        fps.CameraViewport();
        fps.PlayerViewport();
        fps.AddForceLocomotion(speed, dashSpeed);
        fps.ClampMoveRange();

    }


    private void InitPlayer()
    {
        Vector3 mapCamPos = transform.position + mapCameraPos;
        mapCamera.transform.position = mapCamPos;

        AddBombList();
    }


    /// <summary>
    /// ゲームスタート
    /// </summary>
    public void GameStart()
    {
        Coord = map._startCoords[0];
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
        if(Input.GetKeyDown(KeyCode.Space)) 
        {
            Bomb b = bombList.Where(b => b.isHeld).FirstOrDefault();
            if(b == null)
            {
                Debug.Log("手持ちのボムがない");
                return;
            }
            b.Put(Coord);
            
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
            Debug.Log("それ以上は持てない！");
    }
}
