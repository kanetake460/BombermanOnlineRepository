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
                // �V�[������ GameManager �̃C���X�^���X��T��
                _instance = FindObjectOfType<GameManager>();

                // �V�[�����Ō�����Ȃ��ꍇ�͐V���� GameObject ���쐬���� GameManager �̃C���X�^���X���A�^�b�`����
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
        // �C���X�^���X���d�����Ă���ꍇ�͔j������
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject); // �V�[����؂�ւ��Ă��C���X�^���X���j������Ȃ��悤�ɂ���
        }
    }

    public ItemManager itemManager;


    private void Start()
    {
        
    }

    private void Update()
    {
        AudioManager.PlayBGM("�Q�[��BGM",0.0f);
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
            Debug.Assert(m_gameMap.stoneBlockList.Count < allItemCount, "�A�C�e���̐����������ߐ����ł��܂���");
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