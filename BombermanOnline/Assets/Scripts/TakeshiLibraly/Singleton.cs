using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;

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
                    GameObject singletonObj = new GameObject("GameManager");
                    _instance = singletonObj.AddComponent<T>();
                    DontDestroyOnLoad(singletonObj);
                }
            }
            return _instance; 
        }
    }

    protected virtual void Awake()
    {
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
}
