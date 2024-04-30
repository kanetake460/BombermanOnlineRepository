using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : Base
{
    private ParticleSystem _particleSystem;
    private AudioSource _audioSource;

    public bool IsExplosion => _particleSystem.isPlaying;

    public void Initialize(GameMap map,Coord exploCoord)
    {
        base.Initialize(map);
        _particleSystem ??= GetComponent<ParticleSystem>();
        _audioSource ??= GetComponent<AudioSource>();
        AudioManager.PlayOneShot("‰Š",_audioSource,0.1f);
        _particleSystem.Play();
        Coord = exploCoord;
    }

}
