using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;

    public static T Instance
    {
        get 
        {
            // �V�[������ T �̃C���X�^���X��T��
            if(_instance == null)
            {
                _instance = FindObjectOfType<T>();
                // �V�[�����Ō�����Ȃ��ꍇ�͐V���� GameObject ���쐬���� T �̃C���X�^���X���A�^�b�`����
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
