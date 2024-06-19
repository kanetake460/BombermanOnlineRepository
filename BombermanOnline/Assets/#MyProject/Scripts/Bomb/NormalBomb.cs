using SoftGear.Strix.Unity.Runtime;
using UnityEngine;

public class NormalBomb : BombBase
{
    // ===CxgÖ================================================
    private void Update()
    {
        if (isLocal == false) return;
        BombTimer(PredictionFire,Fire);
    }


    /// <summary>
    /// ÊÌeÌ­Î
    /// </summary>
    private void Fire()
    {
        base.Fire();

        AudioManager.PlayOneShot("­", 0.3f);
        CallInActive();
    }


    /// <summary>
    /// eñANeBuiRPCj
    /// </summary>
    public void CallInActive() { RpcToAll(nameof(InActive)); }
    [StrixRpc]
    private void InActive()
    {
        gameObj.SetActive(false);
    }
}
