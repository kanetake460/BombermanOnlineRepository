using SoftGear.Strix.Unity.Runtime;
using UnityEngine;

// 霧爆弾
// PoisonBomb・HealBombに使用
public class GrenadeBomb : BombBase
{
    // ===イベント関数================================================

    private void Update()
    {
        Fly();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isLocal == false) return;
        Fire();
    }

    // ===変数====================================================
    [Header("パラメーター")]
    [SerializeField] float m_throwSpeed;
    
    private Vector3 _throwDirection;
    private Coord _instanceCoord;


    // ===関数====================================================
    
    /// <summary>
    /// 爆弾を投げる処理
    /// </summary>
    /// <param name="coord">座標</param>
    /// <param name="dir">向き</param>
    public void Throw(Coord coord,Vector3 dir)
    {
        Put(coord,1);
        _instanceCoord = coord;
        _throwDirection = dir;
    }


    /// <summary>
    /// 爆弾が飛んでいく処理
    /// </summary>
    private void Fly()
    {
        rb.velocity = _throwDirection * m_throwSpeed;
    }


    /// <summary>
    /// 貫通爆弾の発火
    /// </summary>
    private void Fire()
    {
        // もし、現在の座標が、投げた座標なら、爆発しない
        if (_instanceCoord == Coord) return;
        // 爆弾の位置
        map.ActivePredictLandmark(Coord, false);
        PlayExplosionEffect(Coord);
        map.BreakStone(Coord);

        isHeld = true;
        CallInActive();

        AudioManager.PlayOneShot("爆発", 0.3f);
    }


    /// <summary>
    /// 爆弾非アクティブ（RPC）
    /// </summary>
    public void CallInActive() { RpcToAll(nameof(InActive)); }
    [StrixRpc]
    private void InActive()
    {
        gameObj.SetActive(false);
    }
}
