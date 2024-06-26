using SoftGear.Strix.Unity.Runtime;
using UnityEngine;

// ÑÊe
public class PierceBomb : BombBase
{
    // ===CxgÖ================================================
    private void Update()
    {
        if (isLocal == false) return;
        BombTimer(PredictionFire,Fire);
    }


    /// <summary>
    /// ÑÊeÌ­Î
    /// </summary>
    private void Fire()
    {
        // eÌÊu
        map.ActivePredictLandmark(Coord, false);
        PlayExplosionEffect(Coord);
        map.BreakStone(Coord);

        Coord exploCoord;
        for (int x = 1; x <= firepower; x++)
        {
            exploCoord = new Coord(Coord.x + x, Coord.z);

            if (map.m_mapSet.CheckMap(exploCoord) == false)
                continue;

            map.BreakStone(exploCoord);
            map.ActivePredictLandmark(exploCoord, false);
            PlayExplosionEffect(exploCoord);
        }
        for (int x = -1; x >= -firepower; x--)
        {
            exploCoord = new Coord(Coord.x + x, Coord.z);

            if (map.m_mapSet.CheckMap(exploCoord) == false)
                continue;

            map.BreakStone(exploCoord);
            map.ActivePredictLandmark(exploCoord, false);
            PlayExplosionEffect(exploCoord);
        }
        for (int z = 1; z <= firepower; z++)
        {
            exploCoord = new Coord(Coord.x, Coord.z + z);

            if (map.m_mapSet.CheckMap(exploCoord) == false)
                continue;

            map.BreakStone(exploCoord);
            map.ActivePredictLandmark(exploCoord, false);
            PlayExplosionEffect(exploCoord);
        }
        for (int z = -1; z >= -firepower; z--)
        {
            exploCoord = new Coord(Coord.x, Coord.z + z);

            if (map.m_mapSet.CheckMap(exploCoord) == false)
                continue;

            map.BreakStone(exploCoord);
            map.ActivePredictLandmark(exploCoord, false);
            PlayExplosionEffect(exploCoord);
        }
        isHeld = true;

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
        Debug.Log("CANeBu");
        gameObj.SetActive(false);
    }
}
