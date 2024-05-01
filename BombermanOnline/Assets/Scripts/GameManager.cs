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
                // シーン内で GameManager のインスタンスを探す
                _instance = FindObjectOfType<GameManager>();

                // シーン内で見つからない場合は新しい GameObject を作成して GameManager のインスタンスをアタッチする
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
        // インスタンスが重複している場合は破棄する
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject); // シーンを切り替えてもインスタンスが破棄されないようにする
        }
    }

    public void ExampleMethod()
    {
        Debug.Log("Singleton method called");
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        AudioManager.PlayBGM("ゲームBGM",0.0f);
    }
}
