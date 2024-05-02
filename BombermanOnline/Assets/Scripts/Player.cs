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
    // ===�C�x���g==================================================================================

    protected void Start()
    {
        fps ??= new FPS(map.mapSet, rb, gameObject, mainCamera);
        gameManager = GameManager.Instance;
        InitPlayer();
    }


    private void Update()
    {
        PlayerSettings();
        PutBomb();
    }

    private void OnTriggerEnter(Collider other)
    {
        // �A�C�e���̃��C���[�Ȃ�
        if (gameManager.itemManager.itemLayer == (gameManager.itemManager.itemLayer | (1 << other.gameObject.layer)))
        {
            switch (other.tag)
            {
                case "Item_Bomb":
                    AddBombList();
                    break;

                case "Item_Fire":
                    FierPowerUp();
                    break;

                case "Item_Speed":
                    SpeedUp();
                    break;

                case "Item_Life":
                    LifeUp();
                    break;
            }
            Debug.Log("�A�C�e���Q�b�g�I�I");
            AudioManager.PlayOneShot("�A�C�e���Q�b�g");
            other.gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("�A�C�e������Ȃ�");
        }
    }
    // ===�ϐ�======================================================================================

    [Header("�p�����[�^�[")]
    [SerializeField] private float m_speed;         // �ړ��X�s�[�h
    [SerializeField] private float m_dashSpeed;     // �_�b�V���X�s�[�h
    [SerializeField] private float m_upSpeed;
    [SerializeField] private int m_bombMaxValue;    // ���e�̍ő�l
    [SerializeField] private int m_firepower;       // ���e�̉Η�
    [SerializeField] private int m_life;            // �̗�

    private List<Bomb> bombList = new();            // �莝���̔��e���X�g

    private readonly Vector3 mapCameraPos = new Vector3(0, 100, 0); // �}�b�v�J�����̃|�W�V����


    [Header("�I�u�W�F�N�g�Q��")]
    [SerializeField] GameObject mainCamera;         // �v���C���[�ɒǏ]����J����
    [SerializeField] GameObject mapCamera;          // �}�b�vUI�̃J����
    [SerializeField] Bomb bomb;                     // �������锚�e
    GameManager gameManager;


    [Header("�R���|�[�l���g")]
    [SerializeField] UI ui;
    FPS fps;

    // ===�v���p�e�B================================================================================
    public int Firepower => m_firepower;                        // �ἨQ�b�^�[
    public int BombMaxCount => bombList.Count;                  // �莝���̔��e�ő�l
    public int BombCount => bombList.Where(b => b.isHeld).Count();  // ��Ɏ����Ă��锚�e��


    // ===�֐�================================================================================
    /// <summary>
    /// �v���C���[�̐ݒ�����܂�
    /// ���̊֐���Update�֐��ŌĂяo���܂�
    /// </summary>
    private void PlayerSettings()
    {
        // �J�����A�ړ��̐ݒ�
        fps.CameraViewport();
        fps.PlayerViewport();
        fps.AddForceLocomotion(m_speed, m_dashSpeed);
        fps.ClampMoveRange();
        // �}�b�v�J�����̃|�W�V�����ݒ�
        Vector3 mapCamPos = transform.position + mapCameraPos;
        mapCamera.transform.position = mapCamPos;
    }

    /// <summary>
    /// �v���C���[�̏����������܂�
    /// ���̊֐���Start�֐��ŌĂяo���܂�
    /// </summary>
    private void InitPlayer()
    {
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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Bomb b = bombList.Where(b => b.isHeld).FirstOrDefault();
            if (b == null)
            {
                ui.ShowGameText("No bomb !!",1);
                AudioManager.PlayOneShot("���e���Ȃ�");
                return;
            }
            AudioManager.PlayOneShot("���e��u��");
            b.Put(Coord,Firepower);

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
        {
            AudioManager.PlayOneShot("���e���Ȃ�");
            ui.ShowGameText("Full stack !!", 1);
        }
    }

    /// <summary>
    /// �X�s�[�h�A�b�v���܂�
    /// </summary>
    /// <param name="up"></param>
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
        m_life++;
    }
}



