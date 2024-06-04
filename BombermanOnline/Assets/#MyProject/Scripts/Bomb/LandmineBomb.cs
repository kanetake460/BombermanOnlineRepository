using SoftGear.Strix.Unity.Runtime;
using UnityEngine;

// 霧爆弾
// PoisonBomb・HealBombに使用
public class LandmineBomb : BombBase
{
    // ===イベント関数================================================
    private void Update()
    {
        if (isLocal == false) return;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isLocal == false) return;
        if (collision.gameObject.CompareTag("Player"))
            Fire();

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
