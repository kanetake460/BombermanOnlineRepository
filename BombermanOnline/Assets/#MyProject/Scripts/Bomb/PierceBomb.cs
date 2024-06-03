using SoftGear.Strix.Unity.Runtime;
using UnityEngine;

// �ђʔ��e
public class PierceBomb : BombBase
{
    // ===�C�x���g�֐�================================================
    private void Update()
    {
        if (isLocal == false) return;
        BombTimer(PredictionFire,Fire);
    }


    /// <summary>
    /// �ђʔ��e�̔���
    /// </summary>
    private void Fire()
    {
        // ���e�̈ʒu
        map.ActivePredictLandmark(Coord, false);
        PlayExplosionEffect(Coord);
        map.BreakStone(Coord);

        Coord exploCoord;
        for (int x = 1; x <= firepower; x++)
        {
            exploCoord = new Coord(Coord.x + x, Coord.z);

            // �ǃ}�X
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

            // �ǃ}�X
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

            // �ǃ}�X
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

            // �ǃ}�X
            if (map.IsWall(exploCoord))
                break;

            map.BreakStone(exploCoord);
            map.ActivePredictLandmark(exploCoord, false);
            PlayExplosionEffect(exploCoord);
            continue;
        }
        isHeld = true;

        AudioManager.PlayOneShot("����", 0.3f);
        CallInActive();
    }


    /// <summary>
    /// ���e��A�N�e�B�u�iRPC�j
    /// </summary>
    public void CallInActive() { RpcToAll(nameof(InActive)); }
    [StrixRpc]
    private void InActive()
    {
        Debug.Log("�C���A�N�e�B�u");
        gameObj.SetActive(false);
    }
}
