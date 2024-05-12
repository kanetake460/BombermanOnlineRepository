using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TakeshiLibrary;
using SoftGear.Strix.Unity.Runtime;
using System.Linq;
using TMPro;
using UnityEngine.SceneManagement;

public class TestPlayer : StrixBehaviour
{
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        fps = new FPS(null,rb,gameObject,null);
    }

    private void Start()
    {

    }


    void Update()
    {
        if (isLocal == false)
        {
            return;
        }
        fps.AddForceLocomotion();
        fps.PlayerViewport();

        // オブジェクト配置
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Instantiate(test, transform.position, Quaternion.identity);
        }
        // テストオブジェクトのセットアクティブ
        if (Input.GetKeyDown(KeyCode.Q))
        {
            RpcToAll("Active");
        }

        // ゲームシーンロード
        if (Input.GetKeyDown(KeyCode.Return))
        {
            RpcToAll(nameof(CallLoadScene));
        }



        RpcToAll(nameof(DecIntValue));
        RpcToAll(nameof(DecSyncValue));
        RpcToAll(nameof(PrivateMethod));

        RpcToAll(nameof(ShowValueText));
        //RpcToAll("DecIntValue");
        //RpcToAll("DecSyncValue");
        //RpcToAll("PrivateMethod");

        //Debug.Log("シンクロ数値" + syncInt);
        //Debug.Log("数値" + intValue);
        //Debug.Log("privateシンクロ数値" + _syncInt);
        //Debug.Log("private数値" + _intValue);
    }

    [StrixRpc]
    private void ShowValueText()
    {
        tmp.text = "syncInt:" + syncInt + "\n" + "intValue:" + intValue + "\n" + "private syncInt:" + _syncInt + "\n" + "intValue:" + _intValue;
    }

    [StrixRpc]
    private void CallLoadScene()
    {
        Debug.Log("GameScene");
        SceneManager.LoadScene("GameScene");
    }


    [StrixRpc]
    private void Active()
    {
        bool active = !test2.activeSelf;
        test2.SetActive(active);
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



    FPS fps;
    Rigidbody rb;
    [SerializeField] GameObject mainCamera;
    [SerializeField] Vector3 cameraPos;
    [SerializeField] GameObject test;
    [SerializeField] GameObject test2;

    [SerializeField] TextMeshProUGUI tmp;
    [StrixSyncField]
    public int syncInt = 0;
    public int intValue = 0;

    [StrixSyncField]
    private int _syncInt = 0;
    private int _intValue = 0;


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
