using System.Collections.Generic;
using System.Linq;
using TakeshiLibrary;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMovement : Base
{
    // ===�C�x���g==================================================================================

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

    // ===�ϐ�======================================================================================

    [Header("�p�����[�^�[")]
    [SerializeField] private float speed;
    [SerializeField] private float dashSpeed;
    private List<Bomb> bombList = new();
    [SerializeField] private int m_bombMaxValue;
    private readonly Vector3 mapCameraPos = new Vector3(0, 100, 0);

    [Header("�I�u�W�F�N�g�Q��")]
    [SerializeField] GameObject mainCamera;
    [SerializeField] GameObject mapCamera;
    [SerializeField] GameObject bombPrefab;
    [SerializeField] Bomb bomb;


    [Header("�R���|�[�l���g")]
    FPS fps;
    
    // ===�v���p�e�B================================================================================

    // ===�֐�================================================================================
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
    /// �Q�[���X�^�[�g
    /// </summary>
    public void GameStart()
    {
        Coord = map._startCoords[0];
    }


    /// <summary>
    /// �Q�[���I�[�o�[
    /// </summary>
    public void GameOver()
    {
        gameObj.SetActive(false);
    }


    /// <summary>
    /// �L�[���͂ɂ���Ĕ��e��u���܂�
    /// </summary>
    private void PutBomb()
    {
        if(Input.GetKeyDown(KeyCode.Space)) 
        {
            Bomb b = bombList.Where(b => b.isHeld).FirstOrDefault();
            if(b == null)
            {
                Debug.Log("�莝���̃{�����Ȃ�");
                return;
            }
            b.Put(Coord);
            
        }
    }


    /// <summary>
    /// �莝�����e�̍ő�l�𑝂₵�܂�
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
            Debug.Log("����ȏ�͎��ĂȂ��I");
    }
}
