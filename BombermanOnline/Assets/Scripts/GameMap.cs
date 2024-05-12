using SoftGear.Strix.Unity.Runtime;
using System.Collections.Generic;
using TakeshiLibrary;
using UnityEngine;



public class GameMap : StrixBehaviour
{


    // ===イベント関数================================================

    private void Awake()
    {
        //RpcToAll(nameof(InitializeMap));
    }

    private void Start()
    {

    }
    
    // ===インプットアクション関数=======================================
    public void CreateMap1()
    {
        RpcToAll(nameof(InitializeMap));
        gameManager.playerList.ForEach(player => player.enabled = true);
        for (int i = 0; i < gameManager.playerList.Count; i++)
        {
            gameManager.playerList[i].Coord = _startCoords[i];
        }
    }


    // ===変数====================================================
    public GridFieldMapSettings mapSet;
    private GridField _gridField;
    private GridFieldMapObject _mapObj;

    public List<GridFieldMapSettings.Block> stoneBlockList = new List<GridFieldMapSettings.Block>();

    [HideInInspector] public Coord[] _startCoords;
    private List<Coord> _emptyCoords = new List<Coord>();

    public GameObject test;

    [Header("コンポーネント")]
    [SerializeField] GameObject m_player;
    [SerializeField] Texture m_wallTexture;
    [SerializeField] Texture m_stoneTexture;
    [SerializeField] Camera m_mapCamera;
    GameManager gameManager;

    // ===関数====================================================
    /// <summary>
    /// マップを初期化します。
    /// </summary>
    [StrixRpc]
    public void InitializeMap()
    {
        mapSet ??= GetComponent<GridFieldMapSettings>();


        _mapObj = new GridFieldMapObject(mapSet);
        _gridField = mapSet.gridField;
        gameManager = GameManager.Instance;


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
        stoneBlockList.RemoveAll(b => _emptyCoords.Contains(b.coord));                      // 何もないマスはストーンリストから削除
        stoneBlockList.ForEach(b => b.isSpace = false);                                     // 壁にする
        stoneBlockList.ForEach(b => b.wallRenderer.material.mainTexture = m_stoneTexture);  // テクスチャ変更

        CallItemInstance();

        // アクティブ管理
        _mapObj.ActiveMapWallObjects();

        // 右上のマップのサイズを調節
        m_mapCamera.orthographicSize = _gridField.FieldMaxLength / 2;

    }


    /// <summary>
    /// 指定した座標の石ストーンマスをなくします。
    /// </summary>
    /// <param name="coord">座標</param>
    /// <returns>壊せないブロックかどうか（壊せないブロック：false）</returns>
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
    public void CallItemInstance()
    {
        gameManager.itemManager.InstanceItems();
    }
}