using SoftGear.Strix.Client.Core.Model.Manager.Filter;
using SoftGear.Strix.Unity.Runtime;
using System;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleResultManager : StrixBehaviour
{
    // ===イベント関数================================================
    private void Start()
    {
        gameManager = GameManager.Instance;
        itemManager = gameManager.itemManager;
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
    }


    // ===変数====================================================
    GameManager gameManager;
    ItemManager itemManager;

    private bool _ready;
    public bool Ready
    {
        get
        {
            return _ready;
        }
        set
        {
            CallSetReadyCount(value,player.PlayerIndex);
            CallGameReady();
            AudioManager.PlayOneShot("準備OK");

            _ready = value;
        }
    }

    [Header("UI参照")]
    [SerializeField] TextMeshProUGUI[] itemCountText;
    [SerializeField] TextMeshProUGUI[] playerName;
    [SerializeField] GameObject titleCanvas;
    [SerializeField] GameObject result;
    [SerializeField] TextMeshProUGUI winnerName;
    [SerializeField] TextMeshProUGUI win;
    [SerializeField] Image selectedStage;
    [SerializeField] Sprite[] stageImages;
    [SerializeField] Image[] okUI;

    [Header("オブジェクト参照")]
    [SerializeField] CanvasBook book;
    [SerializeField] Player player;



    // ===関数====================================================
    
    /// <summary>
    /// 強制終了画面を表示します
    /// </summary>
    public void CallShowShutdown() => RpcToAll(nameof(ShowShutdown));
    [StrixRpc]
    private void ShowShutdown()
    {
        result.SetActive(true);
        winnerName.text = "1Pによって\nゲームが強制終了\nされました。";
        win.text = "";
        Cursor.lockState = CursorLockMode.None;
    }

    /// <summary>
    /// リザルト画面を表示します
    /// </summary>
    public void CallShowResult() { RpcToAll(nameof(ShowResult)); }
    [StrixRpc]
    private void ShowResult()
    {
        result.SetActive(true);
        winnerName.text = gameManager.PlayerList.First().PlayerName;
        Cursor.lockState = CursorLockMode.None;
        AudioManager.PlayOneShot("勝利");
    }


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
    /// トグルによって準備OKカウントを変更します
    /// プレイヤーインデックスを引数に与えることで、
    /// 呼び出した人のインデックスを適用することができる
    /// </summary>
    /// <param name="ready"></param>
    public void CallSetReadyCount(bool ready,int playerIndex) { RpcToAll(nameof(SetReadyCount), ready,playerIndex); }
    [StrixRpc]
    private void SetReadyCount(bool ready, int playerIndex)
    {
        if (ready)
        {
            gameManager.readyCount++;
                okUI[playerIndex].gameObject.SetActive(true);
        }
        else
        {
            gameManager.readyCount--;
                okUI[playerIndex].gameObject.SetActive(false);
        }
    }


    /// <summary>
    /// みんなが準備OKならゲームを開始します
    /// </summary>
    public void CallGameReady() { RpcToAll(nameof(GameReady)); }
    [StrixRpc]
    private void GameReady()
    {
        if (gameManager.RoomMenbers.Count == gameManager.readyCount)
        {
            gameManager.GameStart();
            book.CallClose();
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    /// <summary>
    /// マップを生成しプレイヤーの位置を設定します
    /// </summary>
    public void CallSelectStage(int index) { if (StrixNetwork.instance.isRoomOwner) RpcToAll(nameof(SelectStage), index); }
    [StrixRpc]
    private void SelectStage(int index)
    {
        GameMap.Instance.CallCreateMap(index);
        player.transform.position += player.posY;
        book.NextPage();
        AudioManager.PlayOneShot("爆弾設定");
        SetStageSprite(index);
    }


    /// <summary>
    /// 選択されたステージのスプライトを設定します
    /// </summary>
    /// <param name="index">インデックス</param>
    private void SetStageSprite(int index)
    {
        selectedStage.sprite = stageImages[index];
    }


    /// <summary>
    /// 次のページへ
    /// </summary>
    public void CallNextPage() { RpcToAll(nameof(NextPage)); }
    [StrixRpc]
    public void NextPage()
    {
        book.NextPage();
        AudioManager.PlayOneShot("爆弾設定");
    }


    /// <summary>
    /// ボタンに乗った時のSE
    /// </summary>
    public void OverButton()
    {
        AudioManager.PlayOneShot("カーソル移動");
    }

    /// <summary>
    /// アイテムを増やします
    /// </summary>
    /// <param name="index">アイテムインデックス</param>
    public void CallIncItem(int index) { RpcToAll(nameof(IncItem),index); }
    [StrixRpc]
    public void IncItem(int itemIndex)
    {
        if(itemManager.AllItemCount >= 60) 
        {
            Debug.Log("アイテムが多い！");
            AudioManager.PlayOneShot("爆弾がない");
            return; 
        }
        else
        {
            AudioManager.PlayOneShot("カーソル移動");
        }
        itemManager.items[itemIndex].IncItem();
    }

    /// <summary>
    /// アイテムを減らします
    /// </summary>
    /// <param name="index">アイテムインデックス</param>
    public void CallDecItem(int index) { RpcToAll(nameof(DecItem), index); }
    [StrixRpc]
    public void DecItem(int itemIndex)
    {
        itemManager.items[itemIndex].DecItem();
        AudioManager.PlayOneShot("カーソル移動");

    }
}
