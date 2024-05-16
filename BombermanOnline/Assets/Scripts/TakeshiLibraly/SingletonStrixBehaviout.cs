using UnityEngine;
using SoftGear.Strix.Unity.Runtime;
using UnityEditor;
using UnityEngine.SceneManagement;
using System;

public class SingletonStrixBehaviour<T> : StrixBehaviour where T : StrixBehaviour
{
    private static T _instance;

    private static string _singletonObjName;
    private static string _loadingScene;

    [Serializable]
    private struct SingletonSettings
    {
        [SerializeField] public string m_singletonObjName;
        [SerializeField] public string m_loadingScene;
    }
    [SerializeField] private SingletonSettings singletonSettings;
    public static T Instance
    {
        get
        {
            // シーン内で T のインスタンスを探す
            if (_instance == null)
            {
                _instance = FindObjectOfType<T>();
                // シーン内で見つからない場合は新しい GameObject を作成して T のインスタンスをアタッチする
                if (_instance == null)
                {
                    GameObject singletonObj = new GameObject(_singletonObjName);
                    _instance = singletonObj.AddComponent<T>();
                    DontDestroyOnLoad(singletonObj);
                }
            }
            return _instance;
        }
    }

    protected virtual void Awake()
    {
        gameObject.name = _singletonObjName = singletonSettings.m_singletonObjName;

        if (_instance == null)
        {
            _instance = this as T;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    protected virtual void Update()
    {
        _loadingScene = singletonSettings.m_loadingScene;

        if (_loadingScene == null)
        {
            Destroy(gameObject);
            Debug.Log("シーンが設定されていません");
            return;
        }
        if (SceneManager.GetActiveScene().name != _loadingScene)
        {
            Destroy(gameObject);
            Debug.Log("設定されたシーンではないので消します");
            return;
        }
    }
}