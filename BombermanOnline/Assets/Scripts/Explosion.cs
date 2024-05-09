using SoftGear.Strix.Unity.Runtime;
using UnityEngine;

public class Explosion : Base
{
    private void Update()
    {
        RpcToAll(nameof(CallExplosionAction));
    }

    [StrixRpc]
    private void CallExplosionAction()
    {
        if (collisionDurationCount > 0)
        {
            _sphereCollider.enabled = true;
            collisionDurationCount--;
        }
        else
            _sphereCollider.enabled = false;
    }

    [Header("パラメーター")]
    [SerializeField] float collisionDuration;
    private float collisionDurationCount;

    private ParticleSystem _particleSystem;
    private AudioSource _audioSource;
    [SerializeField] SphereCollider _sphereCollider;

    public bool IsExplosion => _particleSystem.isPlaying;

    public void Initialize(GameMap map,Coord exploCoord)
    {
        base.Initialize(map);
        _particleSystem ??= GetComponent<ParticleSystem>();
        _audioSource ??= GetComponent<AudioSource>();
        AudioManager.PlayOneShot("炎",_audioSource,0.1f);
        _particleSystem.Play();
        collisionDurationCount = collisionDuration;
        Coord = exploCoord;
    }

}
