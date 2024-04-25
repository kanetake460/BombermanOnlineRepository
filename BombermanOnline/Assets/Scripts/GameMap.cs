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
    /// マップを初期化します。
    /// </summary>
    private void InitializeMap()
    {
        // オブジェクト生成
        _mapObj.InstanceMapObjects();
        // テクスチャ変更
        _mapObj.ChangeAllWallTexture(m_wallTexture);
        // 壁オブジェクトが生成されていない場所をストーンリストに入れる
        stoneBlockList = mapSet.WhereBlocks(c => mapSet.blocks[c.x, c.z].isSpace == true);

        // 4つのスタート地点
        _startCoords = new Coord[]
        {
            new Coord(1,1),
            new Coord(1,mapSet.gridDepth - 2),
            new Coord(mapSet.gridWidth - 2,1),
            new Coord(mapSet.gridWidth - 2,mapSet.gridDepth - 2)
        };

        // スタート地点の周りは何もないブロック
        for (int i = 0; i < _startCoords.Length; i++)
        {
            _emptyCoords.Add(_startCoords[i]);
            _emptyCoords.Add(_startCoords[i] + Coord.forward);
            _emptyCoords.Add(_startCoords[i] + Coord.back);
            _emptyCoords.Add(_startCoords[i] + Coord.left);
            _emptyCoords.Add(_startCoords[i] + Coord.right);
        }

        // 何もないマス設定
        // 何もないマスはストーンリストから削除
        stoneBlockList.RemoveAll(b => _emptyCoords.Contains(b.coord));
        // 壁にする
        stoneBlockList.ForEach(b => b.isSpace = false);
        // テクスチャ変更
        stoneBlockList.ForEach(b => b.wallRenderer.material.mainTexture = m_stoneTexture);

        // アクティブ管理
        _mapObj.ActiveMapWallObjects();

        // 右上のマップのサイズを調節
        m_mapCamera.orthographicSize = _gridField.FieldMaxLength / 2;
    }

    void Update()
    {


    }
}
