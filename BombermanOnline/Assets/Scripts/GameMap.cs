using SoftGear.Strix.Unity.Runtime;
using System.Collections.Generic;
using TakeshiLibrary;
using UnityEngine;

public class GameMap : SingletonStrixBehaviour<GameMap>
{


    // ===イベント関数================================================

    protected override void Awake()
    {
        base.Awake();
        mapSet ??= GetComponent<GridFieldMapSettings>();
        _gridField = mapSet.gridField;
        _mapObj = new GridFieldMapObject(mapSet);
    }

    private void Start()
    {

    }

    // ===インプットアクション関数=======================================
    public void CreateMap1()
    {
        RpcToAll(nameof(InitializeMap));
    }


    // ===変数====================================================
    public GridFieldMapSettings mapSet;
    private GridField _gridField;
    private GridFieldMapObject _mapObj;

    public List<GridFieldMapSettings.Block> stoneBlockList = new List<GridFieldMapSettings.Block>();

    [HideInInspector] public Coord[] startCoords;
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
    private void InitializeMap()
    {



        gameManager = GameManager.Instance;


        // オブジェクト生成
        _mapObj.InstanceMapObjects();
        // テクスチャ変更
        _mapObj.ChangeAllWallTexture(m_wallTexture);
        // 壁オブジェクトが生成されていない場所をストーンリストに入れる
        stoneBlockList = mapSet.WhereBlocks(c => mapSet.blocks[c.x, c.z].isSpace == true);

        // 4つのスタート地点
        startCoords = new Coord[]
        {
            new Coord(1,1),
            new Coord(1,mapSet.gridDepth - 2),
            new Coord(mapSet.gridWidth - 2,1),
            new Coord(mapSet.gridWidth - 2,mapSet.gridDepth - 2)
        };

        // スタート地点の周りは何もないブロック
        for (int i = 0; i < startCoords.Length; i++)
        {
            _emptyCoords.Add(startCoords[i]);
            _emptyCoords.Add(startCoords[i] + Coord.forward);
            _emptyCoords.Add(startCoords[i] + Coord.back);
            _emptyCoords.Add(startCoords[i] + Coord.left);
            _emptyCoords.Add(startCoords[i] + Coord.right);
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

        // プレイヤーをすべて動かせるようにし、スタート地点に設定します。
        gameManager.playerList.ForEach(player => player.enabled = true);
        for (int i = 0; i < gameManager.playerList.Count; i++)
        {
            gameManager.playerList[i].Coord = startCoords[i];
        }

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
        Debug.Log("マップ生成");
        gameManager.itemManager.InstanceItems();
    }
}