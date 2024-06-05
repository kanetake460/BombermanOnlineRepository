using SoftGear.Strix.Unity.Runtime;
using UnityEngine;

// 霧爆弾
// PoisonBomb・HealBombに使用
public class GrenadeBomb : BombBase
{
    // ===イベント関数================================================
    private void OnCollisionEnter(Collision collision)
    {
        if (isLocal == false) return;
        Fire();
    }


    // ===変数====================================================
    [Header("パラメーター")]
    [SerializeField] float m_throwSpeed;
    
    private Vector3 _throwDirection;


    // ===関数====================================================
    
    /// <summary>
    /// 爆弾を投げる処理
    /// </summary>
    /// <param name="coord">座標</param>
    /// <param name="dir">向き</param>
    public void Throw(Coord coord,Vector3 dir)
    {
        Put(coord,1);
        _throwDirection = dir;
    }


    /// <summary>
    /// 爆弾が飛んでいく処理
    /// </summary>
    private void Fly()
    {
        Pos += _throwDirection * m_throwSpeed;
    }


    /// <summary>
    /// 貫通爆弾の発火
    /// </summary>
    private void Fire()
    {
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
        Debug.Log("インアクティブ");
        gameObj.SetActive(false);
    }
}
