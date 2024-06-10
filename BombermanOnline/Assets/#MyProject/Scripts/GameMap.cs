using SoftGear.Strix.Unity.Runtime;
using System.Collections.Generic;
using TakeshiLibrary;
using Unity.VisualScripting;
using UnityEditor;
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
        //Cursor.lockState = CursorLockMode.Locked;
    }


    // ===変数====================================================
    [SerializeField] Color planeColor;

    public GridFieldMapSettings m_mapSet;               // 生成するマップ
    [SerializeField] GridFieldMapSettings[] m_mapSets;  // 各ステージ配列
    public GridFieldMapObject mapObj;                 // マップオブジェクト管理クラス
    public GridField gridField;                       // グリッドフィールド


    public List<GridFieldMapSettings.Block> stoneBlockList = new List<GridFieldMapSettings.Block>();    // 石マスのリスト
    public List<GridFieldMapSettings.Block> wallBlockList = new List<GridFieldMapSettings.Block>();    // 石マスのリスト
    [HideInInspector] public Coord[] startCoords;           // スタート地点の座標
    public List<Coord> emptyCoords = new List<Coord>();   // 何もない座標
    public List<Coord> frozenCoords = new List<Coord>();    // 凍った床の座標
    private List<PredictLandmark> predictLandmarks = new List<PredictLandmark>();

    [Header("オブジェクト参照")]
    [SerializeField] GameObject predictPrefab;

    [Header("コンポーネント")]
    [SerializeField] Texture m_wallTexture;         // 壁オブジェクトのテクスチャ
    [SerializeField] Texture m_stoneTexture;        // 石オブジェクトのテクスチャ
    [SerializeField] Texture m_artificialTexture;   // 人工石オブジェクトのテクスチャ
    [SerializeField] Texture m_planeTexture;        // 床オブジェクトのテクスチャ
    [SerializeField] Material m_frozenMaterial;     // 凍り床のマテリアル
    [SerializeField] Material m_standerdMaterial;   // 普通のマテリアル
    [SerializeField] Camera m_mapCamera;            // マップカメラ
    GameManager gameManager;                        // ゲームマネージャー

    // ===関数====================================================
    /// <summary>
    /// マップを初期化します。
    /// </summary>
    [StrixRpc]
    private void InitializeMap(GridFieldMapSettings mapSet)
    {
        // インスタンスを代入
        gameManager = GameManager.Instance;
        gridField = mapSet.gridField;
        mapObj = new GridFieldMapObject(mapSet);

        // オブジェクト生成
        mapObj.GenerateMapObjects();
        // テクスチャ変更
        mapObj.ChangeAllWallTexture(m_wallTexture);
        mapObj.ChangeAllPlaneTexture(m_planeTexture);
        // PhysicMaterialComponent追加

        // 壁オブジェクトが生成されていない場所をストーンリストに入れる
        stoneBlockList = mapSet.WhereBlocks(c => mapSet.blocks[c.x, c.z].isSpace == true);
        // 壁オブジェクトの場所を壁リストに入れる
        wallBlockList = mapSet.WhereBlocks(c => mapSet.blocks[c.x, c.z].isSpace == false);

        //// 爆破予測オブジェクトを生成し、座標を割り当てる、親を設定する
        //gridField.IterateOverGrid(c => {
        //    var predict = gridField.Instantiate(predictPrefab, c, predictPrefab.transform.rotation).GetComponent<PredictLandmark>();
        //    predict.coord = c;
        //    predict.transform.parent = mapSet.transform;
        //    predictLandmarks.Add(predict);
        //    });


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
            emptyCoords.Add(startCoords[i]);
            emptyCoords.Add(startCoords[i] + Coord.forward);
            emptyCoords.Add(startCoords[i] + Coord.back);
            emptyCoords.Add(startCoords[i] + Coord.left);
            emptyCoords.Add(startCoords[i] + Coord.right);
        }
        emptyCoords.RemoveAll(c => c.x == 0 || c.z == 0 || c.x == mapSet.gridWidth - 1 || c.z == mapSet.gridDepth - 1);

        // 何もないマス設定
        stoneBlockList.RemoveAll(b => emptyCoords.Contains(b.coord));                      // 何もないマスはストーンリストから削除
        stoneBlockList.ForEach(b => b.isSpace = false);                                     // 壁にする
        stoneBlockList.ForEach(b => b.wallRenderer.material.mainTexture = m_stoneTexture);  // テクスチャ変更

        // アクティブ管理
        mapObj.SetActiveMapWallObjects();

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
    /// ストーンマスかどうか調べます
    /// </summary>
    /// <param name="coord">座標</param>
    /// <returns>true:ストーンマス</returns>
    public bool IsStone(Coord coord) { return stoneBlockList.Find(b => b.coord == coord) != null; }


    /// <summary>
    /// なにもないマスかどうか調べます
    /// </summary>
    /// <param name="coord">座標</param>
    /// <returns>true:何もないマス</returns>
    public bool IsEmpty(Coord coord) { return emptyCoords.Contains(coord); }


    /// <summary>
    /// 壁ますかどうか調べます
    /// </summary>
    /// <param name="coord">座標</param>
    /// <returns>true:壁マス</returns>
    public bool IsWall(Coord coord) { return wallBlockList.Find(b => b.coord == coord) != null; }


    /// <summary>
    /// 指定した座標の石ストーンマスをなくします。
    /// </summary>
    /// <param name="coord">座標</param>
    /// <returns>壊せないブロックかどうか（壊せないブロック：false）</returns>
    [StrixRpc]
    public bool ContinueBreakStone(Coord coord)
    {
        // 引数の座標にストーンがあるか調べる
        var b = stoneBlockList.Find(b => b.coord == coord);
        // ストーンなら
        if (b != null)
        {
            b.isSpace = true;
            stoneBlockList.Remove(b);
            emptyCoords.Add(b.coord);
            mapObj.SetActiveMapWallObjects();
            return true;
        }
        // なにもないなら
        if(emptyCoords.Contains(coord)) 
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// 指定した座標の石ストーンマスをなくします。
    /// </summary>
    /// <param name="coord">座標</param>
    /// <returns>false:ストーンマスではない</returns>
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
    /// 石ブロックを生成します
    /// </summary>
    /// <param name="coord">生成する座標</param>
    /// <returns>生成できるかどうか</returns>
    public void GenerateStone(Coord coord)
    {
        GridFieldMapSettings.Block b = m_mapSet.blocks[coord.x, coord.z];
        b.isSpace = false;
        stoneBlockList.Add(b);
        emptyCoords.Remove(coord);
        mapObj.SetActiveMapWallObjects();
    }


    /// <summary>
    /// 人工石のテクスチャに変更します
    /// </summary>
    /// <param name="coord">変更するブロックの座標</param>
    public void SetArtificialStoneTexture(Coord coord)
    {
        mapObj.ChangeWallTexture(coord,m_artificialTexture);
    }


    /// <summary>
    /// 指定した座標の床を凍らせたり、溶かします
    /// </summary>
    /// <param name="coord">座標</param>
    /// <param name="frozen">true : 凍る</param>
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