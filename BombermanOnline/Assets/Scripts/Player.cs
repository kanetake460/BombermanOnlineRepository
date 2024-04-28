using System.Collections.Generic;
using System.Linq;
using TakeshiLibrary;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMovement : Base
{
    // ===イベント==================================================================================

    protected override void Start()
    {
        base.Start();
        fps ??= new FPS(_map.mapSet,_rb,gameObject,mainCamera);

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
    [SerializeField] private int bombMaxValue;
    private List<Bomb> bombList = new();
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

        GetItem();

        GetItem();
        GetItem();
        GetItem();
    }


    public void GameStart()
    {
        Coord = _map._startCoords[0];
    }

    public void GameOver()
    {
        gameObj.SetActive(false);
    }

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

    private void GetItem()
    {
        Bomb b = Instantiate(bomb,CoordPos,Quaternion.identity);
        b.Init(_map);
        bombList.Add(b);
        
    }

}
