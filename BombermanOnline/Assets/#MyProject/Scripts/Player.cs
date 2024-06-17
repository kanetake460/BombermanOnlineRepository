using OpenCover.Framework.Model;
using SoftGear.Strix.Client.Core.Model.Manager.Filter;
using SoftGear.Strix.Unity.Runtime;
using System;
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
        fps.AddForceLocomotion(_currSpeed, m_dashSpeed);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(ExplosionTag))
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
            }

            AudioManager.PlayOneShot("�A�C�e���Q�b�g");
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
    [SerializeField] private float m_defaultDrag;      // ���鏰�ɂ���Ƃ��̋�C��R
    [SerializeField] private float m_slipDrag;      // ���鏰�ɂ���Ƃ��̋�C��R

    // �̗�
    [SerializeField] private float m_life;            // �̗�
    public float m_lifeMaxValue;    // �̗͂̍ő�l

    // ���e
    [SerializeField] private int m_bombMaxValue;    // ���e�̍ő�l
    [SerializeField] private int m_firepower;       // ���e�̉Η�

    // ���ꔚ�e
    [SerializeField] SpesialBomb.BombType m_specialBombType1;
    [SerializeField] SpesialBomb.BombType m_specialBombType2;
    [SerializeField] float m_specialBombLockTime1;
    [SerializeField] float m_specialBombLockTime2;
    private const int Slot1 = 0;
    private const int Slot2 = 1;

    // ���G
    [SerializeField] private float invncebleTime;     // ��_���[�W��̖��G����
    private bool isInvincible = false;
    private float invncebleCount = 0;

    // �\��
    [SerializeField] LayerMask predictLandmarkMask;
    private bool isPredictable = false;

    private Pool<NormalBomb> _bombPool = new Pool<NormalBomb>();

    private readonly Vector3 mapCameraPos = new Vector3(0, 150, 0);         // �}�b�v�J�����̃|�W�V����

    private const string ItemBombTag = "Item_Bomb";
    private const string ItemFireTag = "Item_Fire";
    private const string ItemSpeedTag = "Item_Speed";
    private const string ItemLifeTag = "Item_Life";
    private const string ExplosionTag = "Explosion";
    private const string FrozenFloorTag = "FrozenFloor";


    [Header("�I�u�W�F�N�g�Q��")]
    [SerializeField] Camera m_mainCamera;         // �v���C���[�ɒǏ]����J����
    [SerializeField] Camera m_mapCamera;          // �}�b�vUI�̃J����
    [SerializeField] NormalBomb m_bomb;           // �������锚�e
    [SerializeField] SpesialBomb m_specialBomb1;   // ���ꔚ�e1
    [SerializeField] SpesialBomb m_specialBomb2;   // ���ꔚ�e2
    [SerializeField] TextMeshProUGUI m_playerInfoText;
    [SerializeField] GameObject m_titleCanvas;

    [Header("�R���|�[�l���g")]
    [SerializeField] UIManager ui;
    FPS fps;

    // ===�v���p�e�B================================================================================
    public string PlayerName => StrixNetwork.instance.roomMembers[strixReplicator.ownerUid].GetName();

    /// <summary>�Η�</summary>
    public int Firepower => m_firepower;
    
    /// <summary>���e�����ő吔</summary>
    public int BombMaxCount => _bombPool.Count;                  // �莝���̔��e�ő�l
    
    /// <summary>���e������</summary>
    public int BombCount => _bombPool.list.Where(b => b.isHeld).Count();  // ��Ɏ����Ă��锚�e��

    ///�@<summary>���ꔚ�e������</summary>
    public int Special1Count => m_specialBomb1.PoolList.Where(b => b.activeSelf == false).Count();   // ��Ɏ����Ă������{��1
    public int Special2Count => m_specialBomb2.PoolList.Where(b => b.activeSelf == false).Count();   // ��Ɏ����Ă������{��2

    /// <Summary>���ꔚ�e�ő及����</summary>
    public int Special1MaxCount => m_specialBomb1.PoolList.Count();
    public int Special2MaxCount => m_specialBomb2.PoolList.Count();

    /// <summary>���ꔚ�e�̃��b�N����</summary>
    public float Special1LockTime => m_specialBombLockTime1 - gameManager.GameTime;
    public float Special2LockTime => m_specialBombLockTime2 - gameManager.GameTime;

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
        if (gameManager.IsGaming == false) return;
        // �J�����A�ړ��̐ݒ�
        fps.PlayerViewport();
        fps.ClampMoveRange();
        //fps.CursorLock();

        // �L�[���͂ɂ��v���C���[�̃A�N�V����
        PutBomb();
        PutSpesialBomb();
        PutArtificialStone();

        // �}�b�v�J�����̃|�W�V�����ݒ�
        Vector3 mapCamPos = transform.position + mapCameraPos;
        m_mapCamera.transform.position = mapCamPos;

        // �A�C�e������
        Invincible();
        PredictiveEye();

        // ���e����
        UnlockSpecialBomb();

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
        _currSpeed = m_speed;
        _currDashSpeed = m_dashSpeed;
        AddBombPool();
        CallSetMembersColor();
        CallShowPlayerName();
    }


    /// <summary>
    /// �Q�[���I�[�o�[
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
        //// ���X�g�ɔ��e���Ȃ��ꍇ��
        var b = _bombPool.Take(b => b.isHeld);

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


    /// <summary>
    /// �L�[���͂ɂ���ăX�y�V�����{����u���܂�
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
    /// �X�y�V�����{���𐶐����܂��iRPC�j
    /// </summary>
    private void CallGenarateSpesialBomb1() { RpcToAll(nameof(GenarateSpesialBomb1)); }
    [StrixRpc]
    private void GenarateSpesialBomb1()
    {
        Vector3 dir = FPS.GetVector3FourDirection(Trafo.rotation.eulerAngles);

        if (m_specialBomb1.GenerateSpesialBomb(m_specialBombType1, Coord, dir, m_firepower))
        {
            AudioManager.PlayOneShot("���ꔚ�e��u��");
        }
        else
        {
            AudioManager.PlayOneShot("���e���Ȃ�");
        }
    }
    private void CallGenarateSpesialBomb2() { RpcToAll(nameof(GenarateSpesialBomb2)); }
    [StrixRpc]
    private void GenarateSpesialBomb2()
    {
        Vector3 dir = FPS.GetVector3FourDirection(Trafo.rotation.eulerAngles);

        if (m_specialBomb2.GenerateSpesialBomb(m_specialBombType2, Coord, dir, m_firepower))
        {
            AudioManager.PlayOneShot("���ꔚ�e��u��");
        }
        else
        {
            AudioManager.PlayOneShot("���e���Ȃ�");
        }
    }

    // �[�[�[�[�[�A�C�e���̏����[�[�[�[�[�[

    /// <summary>
    /// �莝�����e�̍ő�l�𑝂₵�܂�
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
        //isPredictable = Input.GetKey(KeyCode.LeftControl);

        if (isPredictable)
        {
            _currSpeed = _currDashSpeed = m_slowSpeed;
            m_mainCamera.cullingMask |= predictLandmarkMask;
        }
        else
        {
            _currSpeed = m_speed;
            _currDashSpeed = m_dashSpeed;
            m_mainCamera.cullingMask &= ~predictLandmarkMask;
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
        Coord generateCoord = Coord + FPS.GetCoordFourDirection(Rot.eulerAngles);

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

    // �[�[�[�[�[���ꔚ�e�����[�[�[�[�[
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
            Debug.Log("�X���b�g���Ȃ��I");
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
            Debug.Log("�X���b�g���Ȃ��I");
        }
    }

    private void UnlockSpecialBomb()
    {
        if (Special1MaxCount <= 0 && Special1LockTime <= 0)
        {
            AddSpecialBomb(Slot1);
        }
        if (Special2MaxCount <= 0 && Special2LockTime <= 0)
        {
            AddSpecialBomb(Slot2);
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
        m_playerInfoText.text = "P" + (PlayerIndex + 1) + ":" + PlayerName;
    }

}