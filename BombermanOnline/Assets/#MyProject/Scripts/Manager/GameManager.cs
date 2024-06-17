using System;
using TakeshiLibrary;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using SoftGear.Strix.Unity.Runtime;
using SoftGear.Strix.Client.Match.Room.Model;
using UnityEditor;
using UnityEngine.UI;

public class GameManager : SingletonStrixBehaviour<GameManager>
{
    // ===�C�x���g�֐�================================================
    protected override void Awake()
    {
        base.Awake();
    }


    protected override void Update()
    {
        base.Update();

        if(IsGaming)
            gameTime += Time.deltaTime;
        
        AudioManager.PlayBGM("�Q�[��BGM", 0.0f);

    }

    // ===�ϐ�====================================================
    public ItemManager itemManager;

    public Pool<Explosion> exploPool = new Pool<Explosion>();
    public Pool<Explosion> longExploPool = new Pool<Explosion>();
    public Pool<Explosion> iceExploPool = new Pool<Explosion>();


    /// �ʂ̃N���X�ɍ���āA�Q�Ƃ���ƁA�V���N���ϐ��ɂȂ�̂ŁA�Q�[���}�l�[�W���[�ɍ��
    public int readyCount = 0;

    private List<Player> playerList;
    private float gameTime = 0;
    // ===�v���p�e�B=================================================
    /// <summary>���[�������o�[���X�g</summary>
    public IList<CustomizableMatchRoomMember> RoomMenbers => StrixNetwork.instance.sortedRoomMembers;

    /// <summary>�v���C���[�̃��X�g</summary>
    public List<Player> PlayerList
    {
        get
        {
            if(playerList == null)
            {
                playerList = GameObject.FindGameObjectsWithTag("Player").Select(obj => obj.GetComponent<Player>()).ToList();
            }
            return playerList;
        }
    }

    public bool IsGameFinish => PlayerList.Count <= 1;

    public bool IsGaming { get; set; } = false;

    public float GameTime => Mathf.Floor(gameTime);

    // ===�֐�====================================================

    public void GameStart()
    {
        IsGaming = true;
    }
}


[Serializable]
public class ItemManager
{
    // ===�ϐ�====================================================
    public Item[] items;                // �A�C�e���z��i��ށj
    [SerializeField] float itemY;       // ��������A�C�e���̍���
    public LayerMask itemLayer;         // �A�C�e���̃��C���[
    [SerializeField] GameMap m_gameMap; // �Q�[���}�b�v


    // ===�֐�====================================================
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
                m_gameMap.m_mapSet.gridField.Instantiate(items[i].itemObject, randomCoords[count], itemY, Quaternion.identity);
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