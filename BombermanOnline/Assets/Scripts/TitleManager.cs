using SoftGear.Strix.Unity.Runtime;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleResultManager : StrixBehaviour
{
    // ===イベント関数================================================
    private void Start()
    {
        gameManager = GameManager.Instance;
        itemManager = gameManager.itemManager;
        //ActiveOwnPointer();
        for (int i = 0; i < gameManager.RoomMenbers.Count; i++)
        {
            playerName[i].text = gameManager.RoomMenbers[i].GetName();
        }
    }

    private void Update()
    {
        for (int i = 0; i < itemManager.items.Length; i++)
        {
            itemCountText[i].text = itemManager.items[i].itemNum.ToString();
        }

        finishButton.SetActive(gameManager.IsGameFinish);
        if (gameManager.IsGameFinish)
        {
            // ゲームシーンロード
            if (Input.GetKeyDown(KeyCode.Delete))
            {
                CallFinishGame();
            }
        }
    }


    // ===変数====================================================
    GameManager gameManager;
    ItemManager itemManager;

    [SerializeField] TextMeshProUGUI[] itemCountText;
    [SerializeField] TextMeshProUGUI[] playerName;

    [SerializeField] GameObject titleCanvas;
    [SerializeField] GameObject finishButton; 
    [SerializeField] GameObject[] ownPointers;

    [SerializeField] Player player;



    // ===関数====================================================
    /// <summary>
    /// ゲームを終了させ、ロビーに戻ります
    /// </summary>
    public void CallFinishGame() { if (StrixNetwork.instance.isRoomOwner) RpcToAll(nameof(FinishGame)); }
    [StrixRpc]
    private void FinishGame()
    {
        SceneManager.LoadScene("Lobby");
    }

    /// <summary>
    /// タイトルキャンバスをInActiveにし、マップを生成します
    /// </summary>
    public void CallSelectStage(int index) { if (StrixNetwork.instance.isRoomOwner) RpcToAll(nameof(SelectStage), index); }
    [StrixRpc]
    private void SelectStage(int index)
    {
        GameMap.Instance.CallCreateMap(index);
        GameMap.Instance.CallInitializePosition();
        titleCanvas.SetActive(false);
    }
    

    /// <summary>
    /// 「↑You」のテキストの位置をプレイヤーインデックスによって変更します。
    /// </summary>
    private void CallActiceOwnPointer() { RpcToAll(nameof(ActiveOwnPointer)); }
    [StrixRpc]
    private void ActiveOwnPointer()
    {
        ownPointers[player.PlayerIndex].SetActive(true);
    }


/// <summary>
/// アイテムを増やします
/// </summary>
/// <param name="itemIndex">アイテムインデックス</param>
public void CallIncItem(int index) { RpcToAll(nameof(IncItem),index); }
    [StrixRpc]
    public void IncItem(int itemIndex)
    {
        itemManager.items[itemIndex].IncItem();
    }

    /// <summary>
    /// アイテムを減らします
    /// </summary>
    /// <param name="itemIndex">アイテムインデックス</param>
    public void CallDecItem(int index) { RpcToAll(nameof(DecItem), index); }
    [StrixRpc]
    public void DecItem(int itemIndex)
    {
        itemManager.items[itemIndex].DecItem();
    }
}
