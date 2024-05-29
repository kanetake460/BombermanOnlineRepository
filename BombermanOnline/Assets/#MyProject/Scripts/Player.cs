using SoftGear.Strix.Unity.Runtime;
using System.Collections.Generic;
using System.Linq;
using TakeshiLibrary;
using TMPro;
using UnityEngine;

public class Player : Base
{
    // ===�C�x���g==================================================================================

    protected void Start()
    {
        map = GameMap.Instance;
        gameManager = GameManager.Instance;
        fps ??= new FPS(map.m_mapSet, rb, gameObject, mainCamera.gameObject);
        InitPlayer();
        InitLocalPlayer();
    }



    private void Update()
    {
        if (isLocal == false) return;
        PlayerSettings();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == ExplosionTag)
        {
            if (isLocal == false) return;
            if (isInvincible) return;
            Life -= 10f;
            isInvincible = true;
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
    public Vector3 posY;

    // �X�s�[�h
    private float _currSpeed;                       // ���݂̃X�s�[�h
    private float _currDashSpeed;                   // ���݂̃_�b�V���X�s�[�h
    [SerializeField] private float m_speed;         // ���ʂ̃X�s�[�h
    [SerializeField] private float m_dashSpeed;     // �_�b�V���X�s�[�h
    [SerializeField] private float m_slowSpeed;     // �x���X�s�[�h
    [SerializeField] private float m_upSpeed;       // �オ��X�s�[�h

    // �̗�
    [SerializeField] private float m_life;            // �̗�
    public float m_lifeMaxValue;    // �̗͂̍ő�l

    // ���e
    [SerializeField] private int m_bombMaxValue;    // ���e�̍ő�l
    [SerializeField] private int m_firepower;       // ���e�̉Η�

    // ���G
    [SerializeField] private float invncebleTime;     // ��_���[�W��̖��G����
    private bool isInvincible = false;
    private float invncebleCount = 0;

    // �\��
    [SerializeField] LayerMask predictLandmarkMask;
    private bool isPredictable = false;


    private List<Bomb> bombList = new();            // �莝���̔��e���X�g



    private readonly Vector3 mapCameraPos = new Vector3(0, 150, 0); // �}�b�v�J�����̃|�W�V����

    private const string ItemBombTag = "Item_Bomb";
    private const string ItemFireTag = "Item_Fire";
    private const string ItemSpeedTag = "Item_Speed";
    private const string ItemLifeTag = "Item_Life";
    private const string ExplosionTag = "Explosion";


    [Header("�I�u�W�F�N�g�Q��")]
    [SerializeField] Camera mainCamera;         // �v���C���[�ɒǏ]����J����
    [SerializeField] Camera mapCamera;          // �}�b�vUI�̃J����
    [SerializeField] Bomb bomb;                     // �������锚�e
    [SerializeField] TextMeshProUGUI playerInfoText;
    [SerializeField] GameObject titleCanvas;


    [Header("�R���|�[�l���g")]
    [SerializeField] UIManager ui;
    FPS fps;

    // ===�v���p�e�B================================================================================
    public string PlayerName => StrixNetwork.instance.roomMembers[strixReplicator.ownerUid].GetName();

    /// <summary>�Η�</summary>
    public int Firepower => m_firepower;
    
    /// <summary>���e�����ő吔</summary>
    public int BombMaxCount => bombList.Count;                  // �莝���̔��e�ő�l
    
    /// <summary>���e������</summary>
    public int BombCount => bombList.Where(b => b.isHeld).Count();  // ��Ɏ����Ă��锚�e��

    /// <summary>���C�t��</summary>
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
                AudioManager.PlayOneShot("���e���Ȃ�");
                return;
            }
            m_life = value;
        }
    }


    // ===�֐�================================================================================

    // �[�[�[�[�[�v���C���[�̃V�X�e���[�[�[�[�[�[

    /// <summary>
    /// �v���C���[�̐ݒ�����܂�
    /// ���̊֐���Update�֐��ŌĂяo���܂�
    /// </summary>
    private void PlayerSettings()
    {
        // �J�����A�ړ��̐ݒ�
        fps.PlayerViewport();
        fps.AddForceLocomotion(_currSpeed, m_dashSpeed);
        fps.ClampMoveRange();
        //fps.CursorLock();

        // �L�[���͂ɂ��v���C���[�̃A�N�V����
        PutBomb();
        PutArtificialStone();

        // �}�b�v�J�����̃|�W�V�����ݒ�
        Vector3 mapCamPos = transform.position + mapCameraPos;
        mapCamera.transform.position = mapCamPos;
        
        // �A�C�e������
        Invincible();
        PredictiveEye();

        // �Q�[���I�[�o�[����
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
    }


    /// <summary>
    /// Local�̃v���C���[�̏����������܂�
    /// ���̊֐���Start�֐��ŌĂяo���܂�
    /// </summary>
    private void InitLocalPlayer()
    {
        _currSpeed = m_speed;
        _currDashSpeed = m_dashSpeed;
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
            if (listener != null)
            {
                Destroy(listener);
            }
        }
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
        ui.ShowGameText("P" + (PlayerIndex + 1) + ":" + PlayerName + " is Down", 2);
    }

    // �[�[�[�[�[�v���C���[�A�N�V�����[�[�[�[�[

    /// <summary>
    /// �L�[���͂ɂ���Ĕ��e��u���܂�
    /// </summary>
    private void PutBomb()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CallGenerateBomb();
        }
    }


    /// <summary>
    /// ���e�𐶐����A�u���܂��B
    /// RPC�ŌĂяo���Ȃ��ƁA���̐l�̃}�b�v�ɂ͔��������f���Ȃ��i�����ڂł͒u����邪�A���e�̃X�N���v�g�̏������s���Ȃ��j
    /// </summary>
    private void CallGenerateBomb() { RpcToAll(nameof(GenerateBomb)); }
    [StrixRpc]
    private void GenerateBomb()
    {
        Bomb b = bombList.Where(b => b.isHeld).FirstOrDefault();
        // ���X�g�ɔ��e���Ȃ��ꍇ��
        if (b == null)
        {
            // ���������ɏ������s��
            if (isLocal)
            {
                ui.ShowGameText("No bomb !!", 1);
                AudioManager.PlayOneShot("���e���Ȃ�");
            }
            return;
        }
        AudioManager.PlayOneShot("���e��u��");
        b.Put(Coord, Firepower);
    }


    /// <summary>
    /// �L�[���͂ɂ���Đl�H�΃u���b�N�𐶐����܂�
    /// </summary>
    private void PutArtificialStone()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            CallGenerateArtificialStone();
        }
    }

    // �[�[�[�[�[�A�C�e���̏����[�[�[�[�[�[

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
        // �莝���̔��e���ő及�����Ȃ�
        else
        {
            // �����ɂ�������
            if (isLocal)
            {
                ui.ShowGameText("Full Stack !!", 1);
                AudioManager.PlayOneShot("���e���Ȃ�");
            }
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
        Life += 10;
    }


    /// <summary>
    /// ���G���̏���
    /// </summary>
    private void Invincible()
    {
        if (isInvincible)
        {
            // ���G���Ԃ̃J�E���g
            invncebleCount += Time.deltaTime;

            if (invncebleCount > invncebleTime)
            {
                isInvincible = false;
                invncebleCount = 0;
            }
        }
    }


    /// <summary>
    /// �\����̏���
    /// </summary>
    private void PredictiveEye()
    {
        // ���R���g���[���L�[
        isPredictable = Input.GetKey(KeyCode.LeftControl);

        if (isPredictable)
        {
            _currSpeed = _currDashSpeed = m_slowSpeed;
            mainCamera.cullingMask |= predictLandmarkMask;
        }
        else
        {
            _currSpeed = m_speed;
            _currDashSpeed = m_dashSpeed;
            mainCamera.cullingMask &= ~predictLandmarkMask;
        }
    }


    /// <summary>
    /// �v���C���[�̑O�ɐ΂𐶐����܂�
    /// </summary>
    private void CallGenerateArtificialStone() { RpcToAll(nameof(GenerateArtificialStone)); }
    [StrixRpc]
    private void GenerateArtificialStone()
    {
        // �v���C���[�̂ЂƂO�̃}�X�̍��W
        Coord generateCoord = Coord + FPS.GetVector3FourDirection(Rot.eulerAngles);

        // �����Ȃ��}�X�Ȃ�
        if (map.IsEmpty(generateCoord))
        {
            map.GenerateStone(generateCoord);
            map.SetArtificialStoneTexture(generateCoord);
        }
        else
        {
            Debug.Log("�����͂��łɐ΂�����I�I");
        }
    }

    // �[�[�[�[�[���ꂼ��̃v���C���[�̌����Ȃǁ[�[�[�[�[

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
        playerInfoText.text = "P" + (PlayerIndex + 1) + ":" + PlayerName;
    }

}



