using SoftGear.Strix.Unity.Runtime;
using UnityEngine;

// 貫通爆弾
public class PierceBomb : BombBase
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
        for (int x = 1; x <= firepower; x++)
        {
            exploCoord = new Coord(Coord.x + x, Coord.z);

            // 壁マス
            if (map.IsWall(exploCoord))
                break;

            map.BreakStone(exploCoord);
            map.ActivePredictLandmark(exploCoord, false);
            PlayExplosionEffect(exploCoord);
            continue;
        }
        for (int x = -1; x >= -firepower; x--)
        {
            exploCoord = new Coord(Coord.x + x, Coord.z);

            // 壁マス
            if (map.IsWall(exploCoord))
                break;

            map.BreakStone(exploCoord);
            map.ActivePredictLandmark(exploCoord, false);
            PlayExplosionEffect(exploCoord);
            continue;
        }
        for (int z = 1; z <= firepower; z++)
        {
            exploCoord = new Coord(Coord.x, Coord.z + z);

            // 壁マス
            if (map.IsWall(exploCoord))
                break;

            map.BreakStone(exploCoord);
            map.ActivePredictLandmark(exploCoord, false);
            PlayExplosionEffect(exploCoord);
            continue;
        }
        for (int z = -1; z >= -firepower; z--)
        {
            exploCoord = new Coord(Coord.x, Coord.z + z);

            // 壁マス
            if (map.IsWall(exploCoord))
                break;

            map.BreakStone(exploCoord);
            map.ActivePredictLandmark(exploCoord, false);
            PlayExplosionEffect(exploCoord);
            continue;
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
