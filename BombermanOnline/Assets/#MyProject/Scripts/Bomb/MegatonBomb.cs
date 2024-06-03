using SoftGear.Strix.Unity.Runtime;
using UnityEngine;

// 貫通爆弾
public class MegatonBomb : BombBase
{
    // ===イベント関数================================================
    private void Update()
    {
        if (isLocal == false) return;
        BombTimer(PredictionFire,Fire);
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

        Coord exploCoord;
        for (int x = -firepower; x <= firepower; x++)
        {
            for (int z = -firepower; z <= firepower; z++)
            {
                exploCoord = new Coord(Coord.x + x, Coord.z + z);
                if (map.gridField.CheckOnGridCoord(exploCoord) == false)
                    continue;

                map.BreakStone(exploCoord);
                map.ActivePredictLandmark(exploCoord, false);
                PlayExplosionEffect(exploCoord);
                continue;
            }
        }

        isHeld = true;

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
