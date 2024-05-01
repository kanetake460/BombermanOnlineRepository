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

    public void ExampleMethod()
    {
        Debug.Log("Singleton method called");
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        AudioManager.PlayBGM("�Q�[��BGM",0.0f);
    }
}
