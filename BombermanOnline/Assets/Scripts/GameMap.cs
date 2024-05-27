using SoftGear.Strix.Unity.Runtime;
using System.Collections.Generic;
using TakeshiLibrary;
using UnityEngine;

public class GameMap : SingletonStrixBehaviour<GameMap>
{
    // ===�C�x���g�֐�================================================

    protected override void Awake()
    {
        base.Awake();
        m_mapSet ??= GetComponent<GridFieldMapSettings>();
    }


    // ===�C���v�b�g�A�N�V�����֐�=======================================
    public void CallCreateMap(int index) { if (isLocal) RpcToAll(nameof(CreateMap),index); }
    [StrixRpc]
    public void CreateMap(int index)
    {
        m_mapSet = m_mapSets[index];
        InitializeMap(m_mapSet);
        InitializePosition();
    }


    // ===�ϐ�====================================================
    public GridFieldMapSettings m_mapSet;               // ��������}�b�v
    [SerializeField] GridFieldMapSettings[] m_mapSets;  // �e�X�e�[�W�z��

    private GridField _gridField;                       // �O���b�h�t�B�[���h
    private GridFieldMapObject _mapObj;                 // �}�b�v�I�u�W�F�N�g�Ǘ��N���X

    public List<GridFieldMapSettings.Block> stoneBlockList = new List<GridFieldMapSettings.Block>();    // �΃}�X�̃��X�g
    public List<GridFieldMapSettings.Block> wallBlockList = new List<GridFieldMapSettings.Block>();    // �΃}�X�̃��X�g
    [HideInInspector] public Coord[] startCoords;           // �X�^�[�g�n�_�̍��W
    public List<Coord> emptyCoords = new List<Coord>();   // �����Ȃ����W


    [Header("�R���|�[�l���g")]
    [SerializeField] Texture m_wallTexture;     // �ǃI�u�W�F�N�g�̃e�N�X�`��
    [SerializeField] Texture m_stoneTexture;    // �΃I�u�W�F�N�g�̃e�N�X�`��
    [SerializeField] Texture m_planeTexture;    // �΃I�u�W�F�N�g�̃e�N�X�`��
    [SerializeField] Camera m_mapCamera;        // �}�b�v�J����
    GameManager gameManager;                    // �Q�[���}�l�[�W���[

    // ===�֐�====================================================
    /// <summary>
    /// �}�b�v�����������܂��B
    /// </summary>
    [StrixRpc]
    private void InitializeMap(GridFieldMapSettings mapSet)
    {
        // �C���X�^���X����
        gameManager = GameManager.Instance;
        _gridField = mapSet.gridField;
        _mapObj = new GridFieldMapObject(mapSet);

        // �I�u�W�F�N�g����
        _mapObj.GenerateMapObjects();
        // �e�N�X�`���ύX
        _mapObj.ChangeAllWallTexture(m_wallTexture);
        _mapObj.ChangeAllPlaneTexture(m_planeTexture);
        // �ǃI�u�W�F�N�g����������Ă��Ȃ��ꏊ���X�g�[�����X�g�ɓ����
        stoneBlockList = mapSet.WhereBlocks(c => mapSet.blocks[c.x, c.z].isSpace == true);
        wallBlockList = mapSet.WhereBlocks(c => mapSet.blocks[c.x, c.z].isSpace == false);

        // 4�̃X�^�[�g�n�_
        startCoords = new Coord[]
        {
            new Coord(1,1),
            new Coord(1,mapSet.gridDepth - 2),
            new Coord(mapSet.gridWidth - 2,1),
            new Coord(mapSet.gridWidth - 2,mapSet.gridDepth - 2)
        };

        // �X�^�[�g�n�_�̎���͉����Ȃ��u���b�N
        for (int i = 0; i < startCoords.Length; i++)
        {
            emptyCoords.Add(startCoords[i]);
            emptyCoords.Add(startCoords[i] + Coord.forward);
            emptyCoords.Add(startCoords[i] + Coord.back);
            emptyCoords.Add(startCoords[i] + Coord.left);
            emptyCoords.Add(startCoords[i] + Coord.right);
        }
        emptyCoords.RemoveAll(c => c.x == 0 || c.z == 0 || c.x == mapSet.gridWidth - 1 || c.z == mapSet.gridDepth - 1);

        // �����Ȃ��}�X�ݒ�
        stoneBlockList.RemoveAll(b => emptyCoords.Contains(b.coord));                      // �����Ȃ��}�X�̓X�g�[�����X�g����폜
        stoneBlockList.ForEach(b => b.isSpace = false);                                     // �ǂɂ���
        stoneBlockList.ForEach(b => b.wallRenderer.material.mainTexture = m_stoneTexture);  // �e�N�X�`���ύX

        // �A�N�e�B�u�Ǘ�
        _mapObj.SetActiveMapWallObjects();

        // �E��̃}�b�v�̃T�C�Y�𒲐�
        m_mapCamera.orthographicSize = mapSet.gridField.FieldMaxLength / 2;
    }


    /// <summary>
    /// �v���C���[�A�A�C�e���̃|�W�V������ݒ肵�܂��B
    /// </summary>
    public void CallInitializePosition() { if (isLocal) RpcToAll(nameof(InitializePosition)); }
    [StrixRpc]
    private void InitializePosition()
    {
        // �v���C���[�����ׂē�������悤�ɂ��A�X�^�[�g�n�_�ɐݒ肵�܂��B
        gameManager.PlayerList.ForEach(player => player.enabled = true);
        for (int i = 0; i < gameManager.RoomMenbers.Count; i++)
        {
            gameManager.PlayerList[i].Coord = startCoords[gameManager.PlayerList[i].PlayerIndex];
        }
        // �A�C�e���̃C���X�^���X�͈�x�����ł����̂ŁA���[���I�[�i�[�̃X�N���v�g��������
        if (StrixNetwork.instance.isRoomOwner)
        {
            gameManager.itemManager.InstanceItems();
        }
    }


    /// <summary>
    /// �X�g�[���}�X���ǂ������ׂ܂�
    /// </summary>
    /// <param name="coord">���W</param>
    /// <returns>true:�X�g�[���}�X</returns>
    public bool IsStone(Coord coord) { return stoneBlockList.Find(b => b.coord == coord) != null; }


    /// <summary>
    /// �Ȃɂ��Ȃ��}�X���ǂ������ׂ܂�
    /// </summary>
    /// <param name="coord">���W</param>
    /// <returns>true:�����Ȃ��}�X</returns>
    public bool IsEmpty(Coord coord) { return emptyCoords.Contains(coord); }


    /// <summary>
    /// �ǂ܂����ǂ������ׂ܂�
    /// </summary>
    /// <param name="coord">���W</param>
    /// <returns>true:�ǃ}�X</returns>
    public bool IsWall(Coord coord) { return wallBlockList.Find(b => b.coord == coord) != null; }


    /// <summary>
    /// �w�肵�����W�̐΃X�g�[���}�X���Ȃ����܂��B
    /// </summary>
    /// <param name="coord">���W</param>
    /// <returns>�󂹂Ȃ��u���b�N���ǂ����i�󂹂Ȃ��u���b�N�Ffalse�j</returns>
    [StrixRpc]
    public bool ContinueBreakStone(Coord coord)
    {
        // �����̍��W�ɃX�g�[�������邩���ׂ�
        var b = stoneBlockList.Find(b => b.coord == coord);
        // �X�g�[���Ȃ�
        if (b != null)
        {
            b.isSpace = true;
            stoneBlockList.Remove(b);
            emptyCoords.Add(b.coord);
            _mapObj.SetActiveMapWallObjects();
            return true;
        }
        // �Ȃɂ��Ȃ��Ȃ�
        if(emptyCoords.Contains(coord)) 
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// �w�肵�����W�̐΃X�g�[���}�X���Ȃ����܂��B
    /// </summary>
    /// <param name="coord">���W</param>
    /// <returns>false:�X�g�[���}�X�ł͂Ȃ�</returns>
    [StrixRpc]
    public bool BreakStone(Coord coord)
    {
        var b = stoneBlockList.Find(b => b.coord == coord);
        if (b != null)
        {
            b.isSpace = true;
            stoneBlockList.Remove(b);
            emptyCoords.Add(b.coord);
            _mapObj.SetActiveMapWallObjects();
            return true;
        }
        Debug.Log("�����̓X�g�[���}�X����Ȃ��I");
        return false;
    }
}