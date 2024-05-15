using System;
using TakeshiLibrary;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using SoftGear.Strix.Client.Core;
using SoftGear.Strix.Unity.Runtime;
using SoftGear.Strix.Client.Match.Room.Model;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    // ===イベント関数================================================
    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {

    }


    private void Update()
    {
        AudioManager.PlayBGM("ゲームBGM",0.0f);
        // ゲームシーンロード
        if (Input.GetKeyDown(KeyCode.Delete))
        {
            if (StrixNetwork.instance.isRoomOwner)
            {
                SceneManager.LoadScene("Lobby");
            }
        }
    }

    // ===変数====================================================
    public ItemManager itemManager;


    // ===プロパティ=================================================
    public IList<CustomizableMatchRoomMember> RoomMenbers => StrixNetwork.instance.sortedRoomMembers;

    public List<Player> PlayerList
    {
        get
        {
            var playerObjs = GameObject.FindGameObjectsWithTag("Player");
            return playerObjs.Select(obj => obj.GetComponent<Player>()).ToList();
        }
    }

    // ===関数====================================================
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
            Debug.LogError("アイテムの数が多いため生成できません");
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