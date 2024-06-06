using SoftGear.Strix.Unity.Runtime;
using UnityEngine;

// 霧爆弾
// PoisonBomb・HealBombに使用
public class LandmineBomb : BombBase
{
    // ===イベント関数================================================
    private void Update()
    {
        _safeCount--;
    }


    private void OnTriggerStay(Collider other)
    {
        if (isLocal == false) return;

        if (other.gameObject.CompareTag("Player") ||
            other.gameObject.CompareTag("Explosion"))
            Fire();
    }

    // ===変数====================================================
    private int _safeCount = 0;
    [SerializeField] int m_safeMaxCount = 120;

    // ===関数====================================================
    public void Put(Coord coord,int exploLevel)
    {
        base.Put(coord,exploLevel);
        _safeCount = m_safeMaxCount;
    }


    /// <summary>
    /// 貫通爆弾の発火
    /// </summary>
    private void Fire()
    {
        // もし、爆発しないカウントが0より大きいなら爆発しない
        if (_safeCount > 0) return;
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
