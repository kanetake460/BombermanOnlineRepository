using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TakeshiLibrary;
using SoftGear.Strix.Unity.Runtime;
using System.Linq;
using TMPro;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using SoftGear.Strix.Client.Core;
using System;
using SoftGear.Strix.Client.Match.Room.Model;

public class TestPlayer : StrixBehaviour
{
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        fps = new FPS(null,rb,gameObject,null);
    }

    private void Start()
    {
        if (isLocal == false) return;
    }


    void Update()
    {
        if (isLocal == false)
        {
            return;
        }
        fps.AddForceLocomotion();
        fps.PlayerViewport();

        // �I�u�W�F�N�g�z�u
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //Instantiate(test, transform.position, Quaternion.identity);
        }
        // �e�X�g�I�u�W�F�N�g�̃Z�b�g�A�N�e�B�u
        if (Input.GetKeyDown(KeyCode.Q))
        {
            RpcToAll("Active");


        }

        // �Q�[���V�[�����[�h
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (StrixNetwork.instance.isRoomOwner)
            {
                RpcToAll(nameof(CallLoadScene));
            }
        }



        RpcToAll(nameof(DecIntValue));
        RpcToAll(nameof(DecSyncValue));
        RpcToAll(nameof(PrivateMethod));

        RpcToAll(nameof(ShowValueText));
    }

    // ===�ϐ�====================================================
    FPS fps;
    Rigidbody rb;

    [Header("�I�u�W�F�N�g�Q��")]
    [SerializeField] GameObject mainCamera;
    [SerializeField] GameObject test;
    [SerializeField] GameObject test2;
    [SerializeField] TextMeshProUGUI tmp;

    [Header("�p�����[�^�[")]
    [SerializeField] Vector3 cameraPos;
    [StrixSyncField]
    public int syncInt = 0;
    public int intValue = 0;

    [StrixSyncField]
    private int _syncInt = 0;
    private int _intValue = 0;

    // ===�v���p�e�B=================================================
    UID UID => strixReplicator.ownerUid;

    IList<CustomizableMatchRoomMember> RoomMenbers => StrixNetwork.instance.sortedRoomMembers;

    int PlayerIndex
    {
        get
        {
            for (int i = 0; i < RoomMenbers.Count; i++)
            {
                if (UID.ToString() == RoomMenbers[i].GetUid().ToString())
                    return i;
            }
            throw new Exception("UID not found in the list");
        }
    }

    // ===�֐�====================================================
    [StrixRpc]
    private void ShowValueText()
    {
        //tmp.text = "syncInt:" + syncInt + "\n" + "intValue:" + intValue + "\n" + "private syncInt:" + _syncInt + "\n" + "intValue:" + _intValue;
        tmp.text = "PlayerIndex" + PlayerIndex +"\n" + "UID" + UID;
    }

    [StrixRpc]
    private void CallLoadScene()
    {
        SceneManager.LoadScene("GameScene");
    }


    [StrixRpc]
    private void Active()
    {
        bool active = !test2.activeSelf;
        test2.SetActive(active);
        if (PlayerIndex == 0)
        {
            SetPlayerColor(Color.black);
        }
        if (PlayerIndex == 1)
        {
            SetPlayerColor(Color.yellow);
        }
        if (PlayerIndex == 2)
        {
            SetPlayerColor(Color.blue);
        }
        if (PlayerIndex == 3)
        {
            SetPlayerColor(Color.red);
        }
    }

    [StrixRpc]
    public void DecIntValue()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            intValue++;
            _intValue = intValue;
        }
    }

    [StrixRpc]
    public void DecSyncValue()
    {
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            syncInt++;
            _syncInt = syncInt;
        }
    }

    [StrixRpc]
    private void PrivateMethod()
    {
        if(Input.GetKeyDown(KeyCode.Alpha3))
        {
            intValue++;
            _intValue = intValue;

            syncInt++;
            _syncInt = syncInt;

        }
    }

    /// <summary>
    /// �v���C���[�̐F��ύX���܂��B
    /// </summary>
    /// <param name="color">�F</param>
    private void SetPlayerColor(Color color) { gameObject.GetComponent<Renderer>().material.color = color; }





    // �X�g���N�X�N���E�h����

    // <RPC>
    // �Ăяo���֐��́Aprivate�Ŗ��Ȃ�
    // �C���v�b�g����RPC�֐��Ŋ֐����Ăяo���ꍇ�́A
    // if(Input.A)
    // {
    //      RpcToAll("Active");
    // }
    // �Ƃ���ׂ��B
    // RPC�֐��ŌĂяo���֐��̒���
    // if(Input.A)
    // {
    //      Action;
    // }
    // ��������ƕЕ��ɂ����K�p����Ȃ�
}
