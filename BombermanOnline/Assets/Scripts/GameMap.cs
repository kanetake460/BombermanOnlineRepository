using SoftGear.Strix.Unity.Runtime;
using System.Collections.Generic;
using TakeshiLibrary;
using UnityEngine;



public class GameMap : MonoBehaviour
{


    // ===�C�x���g�֐�================================================

    private void Awake()
    {
        mapSet ??= GetComponent<GridFieldMapSettings>();
    }

    void Start()
    {
        _mapObj = new GridFieldMapObject(mapSet);
        _gridField = mapSet.gridField;
        gameManager = GameManager.Instance;
        InitializeMap();

    }

    // ===�ϐ�====================================================
    public GridFieldMapSettings mapSet;
    private GridField _gridField;
    private GridFieldMapObject _mapObj;

    public List<GridFieldMapSettings.Block> stoneBlockList = new List<GridFieldMapSettings.Block>();

    [HideInInspector] public Coord[] _startCoords;
    private List<Coord> _emptyCoords = new List<Coord>();

    public GameObject test;

    [Header("�R���|�[�l���g")]
    [SerializeField] GameObject m_player;
    [SerializeField] Texture m_wallTexture;
    [SerializeField] Texture m_stoneTexture;
    [SerializeField] Camera m_mapCamera;
    GameManager gameManager;

    // ===�֐�====================================================
    /// <summary>
    /// �}�b�v�����������܂��B
    /// </summary>
    private void InitializeMap()
    {
        // �I�u�W�F�N�g����
        _mapObj.InstanceMapObjects();
        // �e�N�X�`���ύX
        _mapObj.ChangeAllWallTexture(m_wallTexture);
        // �ǃI�u�W�F�N�g����������Ă��Ȃ��ꏊ���X�g�[�����X�g�ɓ����
        stoneBlockList = mapSet.WhereBlocks(c => mapSet.blocks[c.x, c.z].isSpace == true);

        // 4�̃X�^�[�g�n�_
        _startCoords = new Coord[]
        {
            new Coord(1,1),
            new Coord(1,mapSet.gridDepth - 2),
            new Coord(mapSet.gridWidth - 2,1),
            new Coord(mapSet.gridWidth - 2,mapSet.gridDepth - 2)
        };

        // �X�^�[�g�n�_�̎���͉����Ȃ��u���b�N
        for (int i = 0; i < _startCoords.Length; i++)
        {
            _emptyCoords.Add(_startCoords[i]);
            _emptyCoords.Add(_startCoords[i] + Coord.forward);
            _emptyCoords.Add(_startCoords[i] + Coord.back);
            _emptyCoords.Add(_startCoords[i] + Coord.left);
            _emptyCoords.Add(_startCoords[i] + Coord.right);
        }

        // �����Ȃ��}�X�ݒ�
        stoneBlockList.RemoveAll(b => _emptyCoords.Contains(b.coord));                      // �����Ȃ��}�X�̓X�g�[�����X�g����폜
        stoneBlockList.ForEach(b => b.isSpace = false);                                     // �ǂɂ���
        stoneBlockList.ForEach(b => b.wallRenderer.material.mainTexture = m_stoneTexture);  // �e�N�X�`���ύX

        gameManager.itemManager.InstanceItems();

        // �A�N�e�B�u�Ǘ�
        _mapObj.ActiveMapWallObjects();

        // �E��̃}�b�v�̃T�C�Y�𒲐�
        m_mapCamera.orthographicSize = _gridField.FieldMaxLength / 2;

    }


    /// <summary>
    /// �w�肵�����W�̐΃X�g�[���}�X���Ȃ����܂��B
    /// </summary>
    /// <param name="coord">���W</param>
    /// <returns>�󂹂Ȃ��u���b�N���ǂ����i�󂹂Ȃ��u���b�N�Ffalse�j</returns>
    [StrixRpc]
    public bool BreakStone(Coord coord)
    {
        var b = stoneBlockList.Find(b => b.coord == coord);
        if (b != null)
        {
            b.isSpace = true;
            stoneBlockList.Remove(b);
            _emptyCoords.Add(b.coord);
            _mapObj.ActiveMapWallObjects();
            return true;
        }
        if(_emptyCoords.Contains(coord)) 
        {
            return true;
        }
        return false;
    }

    [StrixRpc]
    public void SetActiveObjectTest()
    {

            Debug.Log("Test1");
            //if (mapSet.blocks[1, 3].wallObj.GetComponent<StrixTransformSynchronizer>() == null)
            //{
            //    mapSet.blocks[1, 3].wallObj.AddComponent<StrixTransformSynchronizer>();
            //}
            bool active = mapSet.blocks[1, 3].wallObj.activeSelf;
            mapSet.blocks[1, 3].wallObj.SetActive(!active);

            test.SetActive(active);
    }
}