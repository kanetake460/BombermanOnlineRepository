using SoftGear.Strix.Unity.Runtime;
using UnityEngine;

public class Explosion : Base
{
    // ===イベント関数================================================
    private void Update()
    {
        CallExplosionAction();
    }


    // ===変数====================================================
    [Header("パラメーター")]
    [SerializeField] float collisionDuration;
    private float collisionDurationCount;

    [Header("コンポーネント")]
    [SerializeField] SphereCollider _sphereCollider;
    [SerializeField] ParticleSystem _particleSystem;
    private AudioSource _audioSource;

    public bool IsExplosion => _particleSystem.isPlaying;


    // ===関数====================================================
    /// <summary>
    /// 爆発のカウントをし、オブジェクトを非アクティブにする（RPC）
    /// </summary>
    private void CallExplosionAction() { RpcToAll(nameof(ExplosionAction)); }
    [StrixRpc]
    private void ExplosionAction()
    {
        // エフェクト再生していなければ、非アクティブ
        if (!IsExplosion)
        {
            gameObj.SetActive(false);
        }

        // カウントがゼロになったらコライダーを消す
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
    /// エフェクトを初期化
    /// </summary>
    /// <param name="map">マップ</param>
    /// <param name="exploCoord">爆発の座標</param>
    public void Initialize(GameMap map,Coord exploCoord)
    {
        base.Initialize(map);
        gameObj.SetActive(true);
        _audioSource ??= GetComponent<AudioSource>();
        AudioManager.PlayOneShot("炎",_audioSource,0.1f);
        _particleSystem.Play();
        collisionDurationCount = collisionDuration;
        Coord = exploCoord;
    }

}
