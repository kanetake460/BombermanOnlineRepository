using TakeshiLibrary;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private void Start()
    {
        rb ??= GetComponent<Rigidbody>();
        fps ??= new FPS(_map.mapSet,rb,gameObject,mainCamera);
        Debug.Log(_map.mapSet == null);
    }
    

    private void Update()
    {
        PlayerSettings();
    }



    [Header("パラメーター")]
    [SerializeField] private float speed;
    [SerializeField] private float dashSpeed;

    [Header("オブジェクト参照")]
    [SerializeField] GameObject mainCamera;
    [SerializeField] GameObject mapCamera;


    [Header("コンポーネント")]
    [SerializeField] GameMap _map;
    FPS fps;
    Rigidbody rb;
    
    /// <summary>
    /// プレイヤーの座標
    /// </summary>
    private Coord Coord
    {
        set
        {
            transform.position = _map.mapSet.gridField[value.x,value.z];
        }
        get
        {
            return _map.mapSet.gridField.GridCoordinate(transform.position);
        }
    }
    private void PlayerSettings()
    {
        fps.CameraViewport();
        fps.PlayerViewport();
        fps.AddForceLocomotion(speed, dashSpeed);
        fps.ClampMoveRange();
        Vector3 mapCamPos = transform.position + new Vector3(0, 100, 0);
        mapCamera.transform.position = mapCamPos;
        Debug.Log(_map.mapSet == null);

    }

    public void GameStart()
    {
        Coord = _map._startCoords[0];
    }



}
