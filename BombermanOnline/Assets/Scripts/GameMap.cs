using SoftGear.Strix.Unity.Runtime;
using System.Collections.Generic;
using TakeshiLibrary;
using UnityEngine;



public class GameMap : MonoBehaviour
{

    private static GameMap _instance;

    public static GameMap Instance
    {
        get
        {
            if (_instance == null)
            {
                // シーン内で GameManager のインスタンスを探す
                _instance = FindObjectOfType<GameMap>();

                // シーン内で見つからない場合は新しい GameObject を作成して GameManager のインスタンスをアタッチする
                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject("GameManager");
                    _instance = singletonObject.AddComponent<GameMap>();
                }
            }
            return _instance;
        }
    }


    // ===イベント関数================================================

    private void Awake()
    {
        // インスタンスが重複している場合は破棄する
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject); // シーンを切り替えてもインスタンスが破棄されないようにする
        }

        mapSet ??= GetComponent<GridFieldMapSettings>();
    }

    void Start()
    {
        _mapObj = new GridFieldMapObject(mapSet);
        _gridField = mapSet.gridField;
        gameManager = GameManager.Instance;
        InitializeMap();

    }

    // ===変数====================================================
    public GridFieldMapSettings mapSet;
    private GridField _gridField;
    private GridFieldMapObject _mapObj;

    public List<GridFieldMapSettings.Block> stoneBlockList = new List<GridFieldMapSettings.Block>();

    [HideInInspector] public Coord[] _startCoords;
    private List<Coord> _emptyCoords = new List<Coord>();

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
        stoneBlockList.RemoveAll(b => _emptyCoords.Contains(b.coord));                      // 何もないマスはストーンリストから削除
        stoneBlockList.ForEach(b => b.isSpace = false);                                     // 壁にする
        stoneBlockList.ForEach(b => b.wallRenderer.material.mainTexture = m_stoneTexture);  // テクスチャ変更

        gameManager.itemManager.InstanceItems();

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