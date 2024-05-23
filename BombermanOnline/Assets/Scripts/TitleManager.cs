using SoftGear.Strix.Unity.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleResultManager : StrixBehaviour
{
    // ===�C�x���g�֐�================================================
    private void Start()
    {
        gameManager = GameManager.Instance;
        itemManager = gameManager.itemManager;

        CallSetOwnPointer();
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
            // �Q�[���V�[�����[�h
            if (Input.GetKeyDown(KeyCode.Delete))
            {
                CallFinishGame();
            }
        }
    }


    // ===�ϐ�====================================================
    GameManager gameManager;
    ItemManager itemManager;

    [SerializeField] TextMeshProUGUI[] itemCountText;
    [SerializeField] TextMeshProUGUI[] playerName;

    [SerializeField] GameObject titleCanvas;
    [SerializeField] GameObject finishButton; 
    [SerializeField] GameObject ownPinter;

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
    /// �^�C�g���L�����o�X��InActive�ɂ��A�}�b�v�𐶐����܂�
    /// </summary>
    public void CallSelectStage(int index) { if (StrixNetwork.instance.isRoomOwner) RpcToAll(nameof(SelectStage), index); }
    [StrixRpc]
    private void SelectStage(int index)
    {
        GameMap.Instance.CallCreateMap(index);
    }

    /// <summary>
    /// �^�C�g���L�����o�X��InActive�ɂ��A�}�b�v�𐶐����܂�
    /// </summary>
    public void CallGameStart() { if (StrixNetwork.instance.isRoomOwner) RpcToAll(nameof(GameStart)); }
    [StrixRpc]
    private void GameStart()
    {
        titleCanvas.SetActive(false);
        GameMap.Instance.CallInitializePosition();
    }

    /// <summary>
    /// �u��You�v�̃e�L�X�g�̈ʒu���v���C���[�C���f�b�N�X�ɂ���ĕύX���܂��B
    /// </summary>
    public void CallSetOwnPointer() { RpcToAll(nameof(SetOwnPointer)); }
    [StrixRpc]
    private void SetOwnPointer()
    {
        switch (player.PlayerIndex)
        {
            case 0:
                ownPinter.transform.position = ownPinter.transform.position + new Vector3(240f * player.PlayerIndex, 0, 0);
                break;

            case 1:
                ownPinter.transform.position = ownPinter.transform.position + new Vector3(240f * player.PlayerIndex, 0, 0);
                break;

            case 2:
                ownPinter.transform.position = ownPinter.transform.position + new Vector3(240f * player.PlayerIndex, 0, 0);
                break;

            case 3:
                ownPinter.transform.position = ownPinter.transform.position + new Vector3(240f * player.PlayerIndex, 0, 0);
                break;
        }
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
