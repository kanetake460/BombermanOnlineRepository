using System;
using TakeshiLibrary;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                // シーン内で GameManager のインスタンスを探す
                _instance = FindObjectOfType<GameManager>();

                // シーン内で見つからない場合は新しい GameObject を作成して GameManager のインスタンスをアタッチする
                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject("GameManager");
                    _instance = singletonObject.AddComponent<GameManager>();
                }
            }
            return _instance;
        }
    }

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
    }

    public ItemManager itemManager;


    private void Start()
    {
        
    }

    private void Update()
    {
        AudioManager.PlayBGM("ゲームBGM",0.0f);
    }
}

[Serializable]
public class ItemManager
{
    public Item[] items;

    [SerializeField] float itemY;

    public LayerMask itemLayer;

    [SerializeField] GameMap m_gameMap;
    /// <summary>
    /// アイテムをランダムなブロックの座標に生成します
    /// </summary>
    public void InstanceItems()
    {
        int allItemCount = 0;       // すべてのアイテムの数

        // カウントする
        foreach (var item in items)
        {
            allItemCount += item.itemNum;
        }

        // もし、アイテムの数が、ストーンブロックの数より多い場合は生成する場所が足りないのでエラー
        if (m_gameMap.stoneBlockList.Count < allItemCount)
        {
            Debug.Log(allItemCount);
            Debug.Assert(m_gameMap.stoneBlockList.Count < allItemCount, "アイテムの数が多いため生成できません");
            return;
        }

        Coord[] randomCoords = new Coord[m_gameMap.stoneBlockList.Count];     // ランダムなストーンブロックの座標の配列

        // いったん順番に入れる
        for (int i = 0; i < m_gameMap.stoneBlockList.Count; i++)
        {
            randomCoords[i] = m_gameMap.stoneBlockList[i].coord;
        }
        // シャッフル
        Algorithm.Shuffle(randomCoords);

        // 生成していく
        int count = 0;
        for (int i = 0; i < items.Length; i++)
        {
            for (int j = 0; j < items[i].itemNum; j++)
            {
                m_gameMap.mapSet.gridField.Instantiate(items[i].itemObject, randomCoords[count], itemY, Quaternion.identity);
                count++;
            }
        }
    }


    public void GetItem(string tag, Action action)
    {
        foreach (var item in items)
        {
            if (item.Tag == tag)
            {
                action();
            }
        }
    }

}