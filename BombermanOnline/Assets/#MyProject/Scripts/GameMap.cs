using SoftGear.Strix.Unity.Runtime;
using System.Collections.Generic;
using TakeshiLibrary;
using Unity.VisualScripting;
using UnityEditor;
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
        //Cursor.lockState = CursorLockMode.Locked;
    }


    // ===�ϐ�====================================================
    [SerializeField] Color planeColor;

    public GridFieldMapSettings m_mapSet;               // ��������}�b�v
    [SerializeField] GridFieldMapSettings[] m_mapSets;  // �e�X�e�[�W�z��
    public GridFieldMapObject mapObj;                 // �}�b�v�I�u�W�F�N�g�Ǘ��N���X
    public GridField gridField;                       // �O���b�h�t�B�[���h


    public List<GridFieldMapSettings.Block> stoneBlockList = new List<GridFieldMapSettings.Block>();    // �΃}�X�̃��X�g
    public List<GridFieldMapSettings.Block> wallBlockList = new List<GridFieldMapSettings.Block>();    // �΃}�X�̃��X�g
    [HideInInspector] public Coord[] startCoords;           // �X�^�[�g�n�_�̍��W
    public List<Coord> emptyCoords = new List<Coord>();   // �����Ȃ����W
    public List<Coord> frozenCoords = new List<Coord>();    // ���������̍��W
    private List<PredictLandmark> predictLandmarks = new List<PredictLandmark>();

    [Header("�I�u�W�F�N�g�Q��")]
    [SerializeField] GameObject predictPrefab;

    [Header("�R���|�[�l���g")]
    [SerializeField] Texture m_wallTexture;         // �ǃI�u�W�F�N�g�̃e�N�X�`��
    [SerializeField] Texture m_stoneTexture;        // �΃I�u�W�F�N�g�̃e�N�X�`��
    [SerializeField] Texture m_artificialTexture;   // �l�H�΃I�u�W�F�N�g�̃e�N�X�`��
    [SerializeField] Texture m_planeTexture;        // ���I�u�W�F�N�g�̃e�N�X�`��
    [SerializeField] Material m_frozenMaterial;     // ���菰�̃}�e���A��
    [SerializeField] Material m_standerdMaterial;   // ���ʂ̃}�e���A��
    [SerializeField] Camera m_mapCamera;            // �}�b�v�J����
    GameManager gameManager;                        // �Q�[���}�l�[�W���[

    // ===�֐�====================================================
    /// <summary>
    /// �}�b�v�����������܂��B
    /// </summary>
    [StrixRpc]
    private void InitializeMap(GridFieldMapSettings mapSet)
    {
        // �C���X�^���X����
        gameManager = GameManager.Instance;
        gridField = mapSet.gridField;
        mapObj = new GridFieldMapObject(mapSet);

        // �I�u�W�F�N�g����
        mapObj.GenerateMapObjects();
        // �e�N�X�`���ύX
        mapObj.ChangeAllWallTexture(m_wallTexture);
        mapObj.ChangeAllPlaneTexture(m_planeTexture);
        // PhysicMaterialComponent�ǉ�

        // �ǃI�u�W�F�N�g����������Ă��Ȃ��ꏊ���X�g�[�����X�g�ɓ����
        stoneBlockList = mapSet.WhereBlocks(c => mapSet.blocks[c.x, c.z].isSpace == true);
        // �ǃI�u�W�F�N�g�̏ꏊ��ǃ��X�g�ɓ����
        wallBlockList = mapSet.WhereBlocks(c => mapSet.blocks[c.x, c.z].isSpace == false);

        //// ���j�\���I�u�W�F�N�g�𐶐����A���W�����蓖�Ă�A�e��ݒ肷��
        //gridField.IterateOverGrid(c => {
        //    var predict = gridField.Instantiate(predictPrefab, c, predictPrefab.transform.rotation).GetComponent<PredictLandmark>();
        //    predict.coord = c;
        //    predict.transform.parent = mapSet.transform;
        //    predictLandmarks.Add(predict);
        //    });


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
        mapObj.SetActiveMapWallObjects();

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
            mapObj.SetActiveMapWallObjects();
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
            mapObj.SetActiveMapWallObjects();
            return true;
        }
        return false;
    }


    /// <summary>
    /// �΃u���b�N�𐶐����܂�
    /// </summary>
    /// <param name="coord">����������W</param>
    /// <returns>�����ł��邩�ǂ���</returns>
    public void GenerateStone(Coord coord)
    {
        GridFieldMapSettings.Block b = m_mapSet.blocks[coord.x, coord.z];
        b.isSpace = false;
        stoneBlockList.Add(b);
        emptyCoords.Remove(coord);
        mapObj.SetActiveMapWallObjects();
    }


    /// <summary>
    /// �l�H�΂̃e�N�X�`���ɕύX���܂�
    /// </summary>
    /// <param name="coord">�ύX����u���b�N�̍��W</param>
    public void SetArtificialStoneTexture(Coord coord)
    {
        mapObj.ChangeWallTexture(coord,m_artificialTexture);
    }


    /// <summary>
    /// �w�肵�����W�̏��𓀂点����A�n�����܂�
    /// </summary>
    /// <param name="coord">���W</param>
    /// <param name="frozen">true : ����</param>
    public void SetFrozenFloor(Coord coord,bool frozen)
    {
        if (frozen)
        {
            frozenCoords.Add(coord);
            m_mapSet.blocks[coord.x, coord.z].planeObj.tag = "FrozenFloor";
            m_mapSet.blocks[coord.x, coord.z].planeObj.GetComponent<Renderer>().material = m_frozenMaterial;
        }
        else 
        {
            frozenCoords.Remove(coord);
            m_mapSet.blocks[coord.x, coord.z].planeObj.tag = "Untagged";
            m_mapSet.blocks[coord.x, coord.z].planeObj.GetComponent<Renderer>().material = m_standerdMaterial;
        }
    }



    public void UndoDefaultPlaneColor() { mapObj.ChangeAllPlaneColor(planeColor); }
    public void ChangePlaneColor(Coord coord,Color color) { mapObj.ChangePlaneColor(coord, color); }

    public void ActivePredictLandmark(Coord coord,bool active) 
    {
        foreach(var landmark in predictLandmarks)
        {
            if(landmark.coord == coord)
            {
                landmark.gameObject.SetActive(active);
            }
        }
    }
}