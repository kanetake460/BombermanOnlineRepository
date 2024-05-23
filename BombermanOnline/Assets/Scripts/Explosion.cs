using SoftGear.Strix.Unity.Runtime;
using UnityEngine;

public class Explosion : Base
{
    // ===�C�x���g�֐�================================================
    private void Update()
    {
        CallExplosionAction();
    }


    // ===�ϐ�====================================================
    [Header("�p�����[�^�[")]
    [SerializeField] float collisionDuration;
    private float collisionDurationCount;

    [Header("�R���|�[�l���g")]
    [SerializeField] SphereCollider _sphereCollider;
    [SerializeField] ParticleSystem _particleSystem;
    private AudioSource _audioSource;

    public bool IsExplosion => _particleSystem.isPlaying;


    // ===�֐�====================================================
    /// <summary>
    /// �����̃J�E���g�����A�I�u�W�F�N�g���A�N�e�B�u�ɂ���iRPC�j
    /// </summary>
    private void CallExplosionAction() { RpcToAll(nameof(ExplosionAction)); }
    [StrixRpc]
    private void ExplosionAction()
    {
        // �G�t�F�N�g�Đ����Ă��Ȃ���΁A��A�N�e�B�u
        if (!IsExplosion)
        {
            gameObj.SetActive(false);
        }

        // �J�E���g���[���ɂȂ�����R���C�_�[������
        if (collisionDurationCount > 0)
        {
            _sphereCollider.enabled = true;
            collisionDurationCount--;
        }
        else
        {
            _sphereCollider.enabled = false;
        }
    }


    /// <summary>
    /// �G�t�F�N�g��������
    /// </summary>
    /// <param name="map">�}�b�v</param>
    /// <param name="exploCoord">�����̍��W</param>
    public void Initialize(GameMap map,Coord exploCoord)
    {
        base.Initialize(map);
        gameObj.SetActive(true);
        _audioSource ??= GetComponent<AudioSource>();
        AudioManager.PlayOneShot("��",_audioSource,0.1f);
        _particleSystem.Play();
        collisionDurationCount = collisionDuration;
        Coord = exploCoord;
    }

}
