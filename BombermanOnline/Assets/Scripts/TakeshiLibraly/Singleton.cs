using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;

    private static string _objectName;
    private static string _loadingScene;

    [Serializable]
    private struct SingletonSettings
    {
        [SerializeField] public string m_objectName;
        [SerializeField] public string m_loadingScene;
    }

    [SerializeField] private SingletonSettings m_singletonSettings;

    public static T Instance
    {
        get 
        {
            // シーン内で T のインスタンスを探す
            if(_instance == null)
            {
                _instance = FindObjectOfType<T>();
                // シーン内で見つからない場合は新しい GameObject を作成して T のインスタンスをアタッチする
                if (_instance == null)
                {
                    GameObject singletonObj = new GameObject(_objectName);
                    _instance = singletonObj.AddComponent<T>();
                    DontDestroyOnLoad(singletonObj);
                }
            }
            return _instance; 
        }
    }

    protected virtual void Awake()
    {
        gameObject.name = _objectName = m_singletonSettings.m_objectName;

        if(_instance == null)
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
        _loadingScene = m_singletonSettings.m_loadingScene;
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
