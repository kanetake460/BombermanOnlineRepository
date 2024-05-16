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
            // �V�[������ T �̃C���X�^���X��T��
            if (_instance == null)
            {
                _instance = FindObjectOfType<T>();
                // �V�[�����Ō�����Ȃ��ꍇ�͐V���� GameObject ���쐬���� T �̃C���X�^���X���A�^�b�`����
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
            Debug.Log("�V�[�����ݒ肳��Ă��܂���");
            return;
        }
        if (SceneManager.GetActiveScene().name != _loadingScene)
        {
            Destroy(gameObject);
            Debug.Log("�ݒ肳�ꂽ�V�[���ł͂Ȃ��̂ŏ����܂�");
            return;
        }
    }
}