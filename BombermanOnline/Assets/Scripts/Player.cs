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
        fps ??= new FPS(map.mapSet, rb, gameObject, mainCamera);
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
        PlayerSystem();
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


    [Header("�R���|�[�l���g")]
    [SerializeField] UIManager ui;
    FPS fps;

    // ===�v���p�e�B================================================================================
    public int Firepower => m_firepower;                        // �ἨQ�b�^�[
    public int BombMaxCount => bombList.Count;                  // �莝���̔��e�ő�l
    public int BombCount => bombList.Where(b => b.isHeld).Count();  // ��Ɏ����Ă��锚�e��
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
    }


    private void PlayerSystem()
    {
        if (m_life <= 0)
        {
            GameOver();
        }
        // �Q�[���V�[�����[�h
        if (Input.GetKeyDown(KeyCode.Delete))
        {
            if (StrixNetwork.instance.isRoomOwner)
            {
                CallFinishGame();
            }
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
    }


    /// <summary>
    /// �Q�[���I�[�o�[
    /// </summary>
    public void GameOver()
    {
        Pos = mapCameraPos;
        Rot = Quaternion.identity;
        gameObj.SetActive(false);
    }


    /// <summary>
    /// �Q�[�����I�������A���r�[�ɖ߂�܂�
    /// </summary>
    private void CallFinishGame() { RpcToAll(nameof(FinishGame)); }
    [StrixRpc]
    private void FinishGame()
    {
        SceneManager.LoadScene("Lobby");
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

    private void FierPowerUp()
    {
        m_firepower++;
    }

    private void LifeUp()
    {
        LifeCount++;
    }

    private void CallSetMembersColor() => RpcToAll(nameof(SetMembersColor));
    [StrixRpc]
    private void SetMembersColor()
    {
        if (PlayerIndex == 0)
        {
            SetPlayerColor(Color.black);
        }
        if (PlayerIndex == 1)
        {
            SetPlayerColor(Color.yellow);
        }
        if (PlayerIndex == 2)
        {
            SetPlayerColor(Color.blue);
        }
        if (PlayerIndex == 3)
        {
            SetPlayerColor(Color.red);
        }
    }

    /// <summary>
    /// �v���C���[�̐F��ύX���܂��B
    /// </summary>
    /// <param name="color">�F</param>
    private void SetPlayerColor(Color color) { gameObject.GetComponent<Renderer>().material.color = color; }


    private void CallShowPlayerName() =>RpcToAll(nameof(ShowPlayerName));
    [StrixRpc]
    private void ShowPlayerName()
    {
        playerInfoText.text = "PlayerName\n" + StrixNetwork.instance.playerName + "\nPlayerIndex\n" + PlayerIndex;
    }
}



