using SoftGear.Strix.Unity.Runtime;
using UnityEngine;

public class NormalBomb : BombBase
{
    // ===イベント関数================================================
    private void Update()
    {
        if (isLocal == false) return;
        BombTimer(PredictionFire,Fire);
    }


    /// <summary>
    /// 普通の爆弾の発火
    /// </summary>
    private void Fire()
    {
        base.Fire();

        AudioManager.PlayOneShot("爆発", 0.3f);
        CallInActive();
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
