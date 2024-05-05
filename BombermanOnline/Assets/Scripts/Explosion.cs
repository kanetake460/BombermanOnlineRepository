using UnityEngine;

public class Explosion : Base
{
    private void Update()
    {
        if(collisionDurationCount > 0)
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
    private SphereCollider _sphereCollider;

    public bool IsExplosion => _particleSystem.isPlaying;

    public void Initialize(GameMap map,Coord exploCoord)
    {
        base.Initialize(map);
        _particleSystem ??= GetComponent<ParticleSystem>();
        _audioSource ??= GetComponent<AudioSource>();
        _sphereCollider ??= GetComponent<SphereCollider>();
        AudioManager.PlayOneShot("炎",_audioSource,0.1f);
        _particleSystem.Play();
        collisionDurationCount = collisionDuration;
        Coord = exploCoord;
    }

}
