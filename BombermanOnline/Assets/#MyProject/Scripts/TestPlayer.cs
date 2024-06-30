using System.Collections.Generic;
using UnityEngine;
using TakeshiLibrary;
using SoftGear.Strix.Unity.Runtime;
using TMPro;
using UnityEngine.SceneManagement;
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
        CallSetMembersColor();

        fps.PlayerViewport();
        
        if(fps.VelocityForceLocomotion(speed,speed,lowSpeed) != 0)
        {
            anim.SetFloat("Blend", speed, 0.3f, Time.deltaTime);
        }
        else
        {
            anim.SetFloat("Blend", 0, 0.1f, Time.deltaTime);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ShowManual();
        }

        // オブジェクト配置
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //Instantiate(test, transform.position, Quaternion.identity);
            CallTest();
        }

        // ゲームシーンロード
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (StrixNetwork.instance.isRoomOwner)
            {
                RpcToAll(nameof(CallLoadScene));
            }
        }

        conectUI.SetActive(!IsConected);

        AudioManager.PlayBGM("タイトルBGM");
    }

    // ===変数====================================================
    FPS fps;
    Rigidbody rb;

    [Header("オブジェクト参照")]
    [SerializeField] GameObject mainCamera;
    [SerializeField] GameObject test;
    [SerializeField] TextMeshProUGUI tmp;
    [SerializeField] GameObject conectUI;
    [SerializeField] CanvasBook manualBook;

    [Header("コンポーネント")]
    [SerializeField] Animator anim;
    [SerializeField] CharacterSkinController skin;

    [Header("パラメーター")]
    [SerializeField] Vector3 cameraPos;
    [SerializeField] float speed;
    [SerializeField] float lowSpeed;
    private float _currSpeed;
    [StrixSyncField]
    public int syncInt = 0;
    public int intValue = 0;

    [StrixSyncField]
    private int _syncInt = 0;
    private int _intValue = 0;

    // ===プロパティ=================================================
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

    /// <summary>サーバーに接続されているかどうか(true:つながっている)</summary>
    bool IsConected => StrixNetwork.instance.playerName != null;

    // ===関数====================================================
    /// <summary>
    /// マニュアルを表示
    /// </summary>
    private void ShowManual()
    {
        manualBook.Open();
    }

    [StrixRpc]
    private void ShowValueText()
    {
        tmp.text = "PlayerIndex" + PlayerIndex +"\n" + "UID" + UID;
    }

    [StrixRpc]
    private void CallLoadScene()
    {
        SceneManager.LoadScene("GameScene");
    }


    private void CallSetMembersColor() => RpcToAll(nameof(SetMembersColor));
    [StrixRpc]
    private void SetMembersColor()
    {
        skin.ColorIndex = PlayerIndex;
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

    private void CallTest() { RpcToAll(nameof(Test)); }
    [StrixRpc]
    private void Test()
    {
        test.SetActive(!test.activeSelf);
    }

    /// <summary>
    /// プレイヤーの色を変更します。
    /// </summary>
    /// <param name="color">色</param>
    private void SetPlayerColor(Color color) { gameObject.GetComponent<Renderer>().material.color = color; }

    private void MoveAnimetion()
    {
        anim.SetFloat("Blend", speed, 0.3f, Time.deltaTime);
    }



    // ストリクスクラウドメモ

    // <RPC>
    // 呼び出す関数は、privateで問題ない
    // インプットからRPC関数で関数を呼び出す場合は、
    // if(Input.A)
    // {
    //      RpcToAll("Active");
    // }
    // とするべき。
    // RPC関数で呼び出す関数の中に
    // if(Input.A)
    // {
    //      Action;
    // }
    // こうすると片方にしか適用されない
}
