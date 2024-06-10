using SoftGear.Strix.Client.Core.Model.Manager.Filter;
using SoftGear.Strix.Unity.Runtime;
using System;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleResultManager : StrixBehaviour
{
    // ===�C�x���g�֐�================================================
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

        finishButton.SetActive(gameManager.IsGameFinish);
    }


    // ===�ϐ�====================================================
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
            CallSetReadyCount(value);
            CallGameReady();

            _ready = value;
        }
    }


    [SerializeField] TextMeshProUGUI[] itemCountText;
    [SerializeField] TextMeshProUGUI[] playerName;

    [SerializeField] GameObject titleCanvas;
    [SerializeField] GameObject finishButton; 

    [SerializeField] Player player;



    // ===�֐�====================================================
    /// <summary>
    /// �Q�[�����I�������A���r�[�ɖ߂�܂�
    /// </summary>
    public void CallFinishGame() { if (StrixNetwork.instance.isRoomOwner) RpcToAll(nameof(FinishGame)); }
    [StrixRpc]
    private void FinishGame()
    {
        SceneManager.LoadScene("Lobby");
    }


    /// <summary>
    /// �g�O���ɂ���ď���OK�J�E���g��ύX���܂�
    /// </summary>
    /// <param name="ready"></param>
    public void CallSetReadyCount(bool ready) { RpcToAll(nameof(SetReadyCount), ready); }
    [StrixRpc]
    private void SetReadyCount(bool ready)
    {
        if (ready)
            gameManager.readyCount++;
        else
            gameManager.readyCount--;
    }


    /// <summary>
    /// �݂�Ȃ�����OK�Ȃ�Q�[�����J�n���܂�
    /// </summary>
    public void CallGameReady() { RpcToAll(nameof(GameReady)); }
    [StrixRpc]
    private void GameReady()
    {
        if (gameManager.RoomMenbers.Count == gameManager.readyCount)
        {
            gameManager.GameStart();
        }
    }

    /// <summary>
    /// �^�C�g���L�����o�X��InActive�ɂ��A�}�b�v�𐶐����܂�
    /// </summary>
    public void CallSelectStage(int index) { if (StrixNetwork.instance.isRoomOwner) RpcToAll(nameof(SelectStage), index); }
    [StrixRpc]
    private void SelectStage(int index)
    {
        GameMap.Instance.CallCreateMap(index);
        player.transform.position += player.posY;
    }


/// <summary>
/// �A�C�e���𑝂₵�܂�
/// </summary>
/// <param name="itemIndex">�A�C�e���C���f�b�N�X</param>
public void CallIncItem(int index) { RpcToAll(nameof(IncItem),index); }
    [StrixRpc]
    public void IncItem(int itemIndex)
    {
        itemManager.items[itemIndex].IncItem();
    }

    /// <summary>
    /// �A�C�e�������炵�܂�
    /// </summary>
    /// <param name="itemIndex">�A�C�e���C���f�b�N�X</param>
    public void CallDecItem(int index) { RpcToAll(nameof(DecItem), index); }
    [StrixRpc]
    public void DecItem(int itemIndex)
    {
        itemManager.items[itemIndex].DecItem();
    }
}
