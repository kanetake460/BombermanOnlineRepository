using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TakeshiLibrary
{
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
                if (_instance == null)
                {
                    _instance = FindObjectOfType<T>();
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
            _loadingScene = m_singletonSettings.m_loadingScene;

            if (_instance == null)
            {
                _instance = this as T;
                DontDestroyOnLoad(this);
                SceneManager.activeSceneChanged += OnActiveSceneChanged;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void OnActiveSceneChanged(Scene previousScene, Scene newScene)
        {
            if (newScene.name != _loadingScene)
            {
                Destroy(gameObject);
                Debug.Log($"ÉVÅ[Éì '{_loadingScene}' Ç≈ÇÕÇ»Ç¢ÇÃÇ≈ {gameObject.name} Çîjä¸ÇµÇ‹Ç∑ÅB");
            }
        }

        protected virtual void OnDestroy()
        {
            if (_instance == this)
            {
                SceneManager.activeSceneChanged -= OnActiveSceneChanged;
            }
        }
    }
}