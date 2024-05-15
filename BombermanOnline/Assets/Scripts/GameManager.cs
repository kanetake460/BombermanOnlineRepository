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
    // ===�C�x���g�֐�================================================
    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {

    }


    private void Update()
    {
        AudioManager.PlayBGM("�Q�[��BGM",0.0f);
        // �Q�[���V�[�����[�h
        if (Input.GetKeyDown(KeyCode.Delete))
        {
            if (StrixNetwork.instance.isRoomOwner)
            {
                SceneManager.LoadScene("Lobby");
            }
        }
    }

    // ===�ϐ�====================================================
    public ItemManager itemManager;


    // ===�v���p�e�B=================================================
    public IList<CustomizableMatchRoomMember> RoomMenbers => StrixNetwork.instance.sortedRoomMembers;

    public List<Player> PlayerList
    {
        get
        {
            var playerObjs = GameObject.FindGameObjectsWithTag("Player");
            return playerObjs.Select(obj => obj.GetComponent<Player>()).ToList();
        }
    }

    // ===�֐�====================================================
}


[Serializable]
public class ItemManager
{
    public Item[] items;

    [SerializeField] float itemY;

    public LayerMask itemLayer;

    [SerializeField] GameMap m_gameMap;
    /// <summary>
    /// �A�C�e���������_���ȃu���b�N�̍��W�ɐ������܂�
    /// </summary>
    public void InstanceItems()
    {
        int allItemCount = 0;       // ���ׂẴA�C�e���̐�

        // �J�E���g����
        foreach (var item in items)
        {
            allItemCount += item.itemNum;
        }

        // �����A�A�C�e���̐����A�X�g�[���u���b�N�̐���葽���ꍇ�͐�������ꏊ������Ȃ��̂ŃG���[
        if (m_gameMap.stoneBlockList.Count < allItemCount)
        {
            Debug.Log(allItemCount);
            Debug.LogError("�A�C�e���̐����������ߐ����ł��܂���");
            return;
        }

        Coord[] randomCoords = new Coord[m_gameMap.stoneBlockList.Count];     // �����_���ȃX�g�[���u���b�N�̍��W�̔z��

        // �������񏇔Ԃɓ����
        for (int i = 0; i < m_gameMap.stoneBlockList.Count; i++)
        {
            randomCoords[i] = m_gameMap.stoneBlockList[i].coord;
        }
        // �V���b�t��
        Algorithm.Shuffle(randomCoords);

        // �������Ă���
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
    /// �A�C�e�����擾�����Ƃ��̃A�N�V�������s���܂��B
    /// </summary>
    /// <param name="tag">�A�C�e���̃^�O</param>
    /// <param name="action">�A�N�V����</param>
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