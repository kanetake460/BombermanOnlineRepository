using SoftGear.Strix.Unity.Runtime;
using System.Collections.Generic;
using System.Linq;
using TakeshiLibrary;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : Base
{
    // ===�C�x���g==================================================================================

    protected void Start()
    {
        map = GameMap.Instance;
        gameManager = GameManager.Instance;
        fps ??= new FPS(map.m_mapSet, rb, gameObject, mainCamera);
        InitPlayer();
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
            if(listener != null) 
            {
                Destroy(listener);
            }
        }
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

            LifeCount--;
            AudioManager.PlayOneShot("��_���[�W", 1f);
            ui.ShowDamageEffectUI();
        }

        // �A�C�e���̃��C���[�Ȃ�
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
    [SerializeField] private int m_lifeMaxValue;    // �̗͂̍ő�l

    private List<Bomb> bombList = new();            // �莝���̔��e���X�g

    private readonly Vector3 mapCameraPos = new Vector3(0, 150, 0); // �}�b�v�J�����̃|�W�V����

    private const string ItemBombTag = "Item_Bomb";
    private const string ItemFireTag = "Item_Fire";
    private const string ItemSpeedTag = "Item_Speed";
    private const string ItemLifeTag = "Item_Life";
    private const string ExplosionTag = "Explosion";


    [Header("�I�u�W�F�N�g�Q��")]
    [SerializeField] GameObject mainCamera;         // �v���C���[�ɒǏ]����J����
    [SerializeField] GameObject mapCamera;          // �}�b�vUI�̃J����
    [SerializeField] Bomb bomb;                     // �������锚�e
    [SerializeField] TextMeshProUGUI playerInfoText;
    [SerializeField] GameObject titleCanvas;
    [SerializeField] GameObject ownPointer;


    [Header("�R���|�[�l���g")]
    [SerializeField] UIManager ui;
    FPS fps;

    // ===�v���p�e�B================================================================================
    
    /// <summary>�Η�</summary>
    public int Firepower => m_firepower;
    
    /// <summary>���e�����ő吔</summary>
    public int BombMaxCount => bombList.Count;                  // �莝���̔��e�ő�l
    
    /// <summary>���e������</summary>
    public int BombCount => bombList.Where(b => b.isHeld).Count();  // ��Ɏ����Ă��锚�e��

    /// <summary>���C�t��</summary>
    public int LifeCount
    {
        get
        {
            return m_life;
        }
        private set
        {
            if (value > m_lifeMaxValue)
            {
                ui.ShowGameText("Full Life !!", 1);
                AudioManager.PlayOneShot("���e���Ȃ�");
                return;
            }
            m_life = value;
        }
    }


    // ===�֐�================================================================================
    /// <summary>
    /// �v���C���[�̐ݒ�����܂�
    /// ���̊֐���Update�֐��ŌĂяo���܂�
    /// </summary>
    private void PlayerSettings()
    {
        // �J�����A�ړ��̐ݒ�
        fps.PlayerViewport();
        fps.AddForceLocomotion(m_speed, m_dashSpeed);
        fps.ClampMoveRange();
        //fps.CursorLock();
        // �}�b�v�J�����̃|�W�V�����ݒ�
        Vector3 mapCamPos = transform.position + mapCameraPos;
        mapCamera.transform.position = mapCamPos;
        if (m_life <= 0)
        {
            CallGameOver();
        }
    }


    /// <summary>
    /// �v���C���[�̏����������܂�
    /// ���̊֐���Start�֐��ŌĂяo���܂�
    /// </summary>
    private void InitPlayer()
    {
        AddBombList();
        CallSetMembersColor();
        CallShowPlayerName();
        ActiveOwnPointer();
    }


    /// <summary>
    /// �Q�[���I�[�o�[
    /// </summary>
    private void CallGameOver() { RpcToAll(nameof(GameOver)); }
    [StrixRpc]
    private void GameOver()
    {
        mainCamera.transform.position = Pos = mapCameraPos;
        mainCamera.transform.rotation = Rot = Quaternion.Euler(90f, 0f, 0f);
        gameObj.SetActive(false);
        mainCamera.GetComponent<CameraView>().enabled = false;
    }


    /// <summary>
    /// �L�[���͂ɂ���Ĕ��e��u���܂�
    /// </summary>
    private void PutBomb()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            RpcToAll(nameof(GenerateBomb));
        }
    }


    /// <summary>
    /// ���e�𐶐����A�u���܂��B
    /// </summary>
    [StrixRpc]
    void GenerateBomb()
    {
        Bomb b = bombList.Where(b => b.isHeld).FirstOrDefault();
        if (b == null)
        {
            ui.ShowGameText("No bomb !!", 1);
            AudioManager.PlayOneShot("���e���Ȃ�");
            return;
        }
        AudioManager.PlayOneShot("���e��u��");
        b.Put(Coord, Firepower);
    }


    /// <summary>
    /// �莝�����e�̍ő�l�𑝂₵�܂�
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
        else
        {
            AudioManager.PlayOneShot("���e���Ȃ�");
            ui.ShowGameText("Full Stack !!", 1);
        }
    }

    /// <summary>
    /// �X�s�[�h�A�b�v���܂�
    /// </summary>
    private void SpeedUp()
    {
        m_speed += m_upSpeed;
        m_dashSpeed += m_upSpeed;
    }

    /// <summary>
    /// �Η͂��A�b�v�����܂�
    /// </summary>
    private void FierPowerUp()
    {
        m_firepower++;
    }

    /// <summary>
    /// ���C�t�𑝂₵�܂�
    /// </summary>
    private void LifeUp()
    {
        LifeCount++;
    }

    /// <summary>
    /// [RPC]�v���C���[�̐F��ݒ肵�܂��B
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
    /// �v���C���[�̐F��ύX���܂��B
    /// </summary>
    /// <param name="color">�F</param>
    private void SetPlayerColor(Color color) { gameObject.GetComponent<Renderer>().material.color = color; }


    /// <summary>
    /// �v���C���[�̏����X�V���܂�
    /// </summary>
    private void CallShowPlayerName() => RpcToAll(nameof(ShowPlayerName));
    [StrixRpc]
    private void ShowPlayerName()
    {
        playerInfoText.text = "PlayerName\n" + gameManager.RoomMenbers[PlayerIndex].GetName() + "\nPlayerIndex\n" + PlayerIndex;
    }

    /// <summary>
    /// �u��You�v�̃e�L�X�g�̈ʒu���v���C���[�C���f�b�N�X�ɂ���ĕύX���܂��B
    /// </summary>
    private void CallActiveOwnPointer() { RpcToAll(nameof(ActiveOwnPointer));  }
    [StrixRpc]
    private void ActiveOwnPointer()
    {
        ownPointer.transform.position += new Vector3(PlayerIndex * 240f,0,0);
    }
}



