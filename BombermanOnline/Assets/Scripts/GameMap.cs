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
        m_mapSet ??= GetComponent<GridFieldMapSettings>();
    }


    // ===インプットアクション関数=======================================
    public void CallCreateMap(int index) { if (isLocal) RpcToAll(nameof(CreateMap),index); }
    [StrixRpc]
    public void CreateMap(int index)
    {
        m_mapSet = m_mapSets[index];
        InitializeMap(m_mapSet);
        InitializePosition();
    }


    // ===変数====================================================
    public GridFieldMapSettings m_mapSet;               // 生成するマップ
    [SerializeField] GridFieldMapSettings[] m_mapSets;  // 各ステージ配列

    private GridField _gridField;                       // グリッドフィールド
    private GridFieldMapObject _mapObj;                 // マップオブジェクト管理クラス

    public List<GridFieldMapSettings.Block> stoneBlockList = new List<GridFieldMapSettings.Block>();    // 石マスのリスト
    [HideInInspector] public Coord[] startCoords;           // スタート地点の座標
    private List<Coord> _emptyCoords = new List<Coord>();   // 何もない座標


    [Header("コンポーネント")]
    [SerializeField] Texture m_wallTexture;     // 壁オブジェクトのテクスチャ
    [SerializeField] Texture m_stoneTexture;    // 石オブジェクトのテクスチャ
    [SerializeField] Texture m_planeTexture;    // 石オブジェクトのテクスチャ
    [SerializeField] Camera m_mapCamera;        // マップカメラ
    GameManager gameManager;                    // ゲームマネージャー

    // ===関数====================================================
    /// <summary>
    /// マップを初期化します。
    /// </summary>
    [StrixRpc]
    private void InitializeMap(GridFieldMapSettings mapSet)
    {
        // インスタンスを代入
        gameManager = GameManager.Instance;
        _gridField = mapSet.gridField;
        _mapObj = new GridFieldMapObject(mapSet);

        // オブジェクト生成
        _mapObj.GenerateMapObjects();
        // テクスチャ変更
        _mapObj.ChangeAllWallTexture(m_wallTexture);
        _mapObj.ChangeAllPlaneTexture(m_planeTexture);
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


        // アクティブ管理
        _mapObj.ActiveMapWallObjects();

        // 右上のマップのサイズを調節
        m_mapCamera.orthographicSize = mapSet.gridField.FieldMaxLength / 2;
    }


    /// <summary>
    /// プレイヤー、アイテムのポジションを設定します。
    /// </summary>
    public void CallInitializePosition() { if (isLocal) RpcToAll(nameof(InitializePosition)); }
    [StrixRpc]
    private void InitializePosition()
    {
        // プレイヤーをすべて動かせるようにし、スタート地点に設定します。
        gameManager.PlayerList.ForEach(player => player.enabled = true);
        for (int i = 0; i < gameManager.RoomMenbers.Count; i++)
        {
            gameManager.PlayerList[i].Coord = startCoords[gameManager.PlayerList[i].PlayerIndex];
        }
        // アイテムのインスタンスは一度だけでいいので、ルームオーナーのスクリプトだけ生成
        if (StrixNetwork.instance.isRoomOwner)
        {
            gameManager.itemManager.InstanceItems();
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
}