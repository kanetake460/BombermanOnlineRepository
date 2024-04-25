using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TakeshiLibrary;
using UnityEngine;
using UnityEngine.UIElements;



public class GameMap : MonoBehaviour
{
    public GridFieldMapSettings mapSet;
    private GridField _gridField;
    private GridFieldMapObject _mapObj;

    private List<GridFieldMapSettings.Block> stoneBlockList = new List<GridFieldMapSettings.Block>();

    [HideInInspector] public Coord[] _startCoords;
    private List<Coord> _emptyCoords = new List<Coord>();

    [SerializeField] GameObject m_player;
    [SerializeField] Texture m_wallTexture;
    [SerializeField] Texture m_stoneTexture;
    [SerializeField] Camera m_mapCamera;

    private void Awake()
    {
        mapSet ??= GetComponent<GridFieldMapSettings>();

    }
    void Start()
    {
        _mapObj = new GridFieldMapObject(mapSet);
        _gridField = mapSet.gridField;
        InitializeMap();

    }
    

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
        // �����Ȃ��}�X�̓X�g�[�����X�g����폜
        stoneBlockList.RemoveAll(b => _emptyCoords.Contains(b.coord));
        // �ǂɂ���
        stoneBlockList.ForEach(b => b.isSpace = false);
        // �e�N�X�`���ύX
        stoneBlockList.ForEach(b => b.wallRenderer.material.mainTexture = m_stoneTexture);

        // �A�N�e�B�u�Ǘ�
        _mapObj.ActiveMapWallObjects();

        // �E��̃}�b�v�̃T�C�Y�𒲐�
        m_mapCamera.orthographicSize = _gridField.FieldMaxLength / 2;
    }

    void Update()
    {


    }
}
