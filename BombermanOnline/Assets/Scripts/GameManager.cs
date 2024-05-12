using System;
using TakeshiLibrary;
using UnityEngine;
using System.Collections.Generic;

public class GameManager : Singleton<GameManager>
{
    // ===イベント関数================================================

    private void Start()
    {
        InitializePlayerList();
    }


    private void Update()
    {
        AudioManager.PlayBGM("ゲームBGM",0.0f);
    }

    // ===変数====================================================
    public ItemManager itemManager;

    public List<Player> playerList = new List<Player>();


    // ===関数====================================================

    /// <summary>
    /// プレイヤーのリストを作成します
    /// </summary>
    private void InitializePlayerList()
    {
        var playerObjs = GameObject.FindGameObjectsWithTag("player");
        foreach (var playerObj in playerObjs)
        {
            Player player = playerObj.GetComponent<Player>();
            playerList.Add(player);
        }
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

    /// <summary>
    /// アイテムを取得したときのアクションを行います。
    /// </summary>
    /// <param name="tag">アイテムのタグ</param>
    /// <param name="action">アクション</param>
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