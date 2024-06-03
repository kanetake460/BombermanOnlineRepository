using SoftGear.Strix.Unity.Runtime;
using UnityEngine;

// �ђʔ��e
public class CrossBomb : BombBase
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
        for (int i = 1; i <= firepower; i++)
        {
            exploCoord = new Coord(Coord.x + i, Coord.z + i);

            // �ǃ}�X
            if (map.IsWall(exploCoord))
                break;

            map.BreakStone(exploCoord);
            map.ActivePredictLandmark(exploCoord, false);
            PlayExplosionEffect(exploCoord);
            continue;
        }
        for (int i = 1; i <= firepower; i++)
        {
            exploCoord = new Coord(Coord.x - i, Coord.z - i);

            // �ǃ}�X
            if (map.IsWall(exploCoord))
                break;

            map.BreakStone(exploCoord);
            map.ActivePredictLandmark(exploCoord, false);
            PlayExplosionEffect(exploCoord);
            continue;
        }
        for (int i = 1; i <= firepower; i++)
        {
            exploCoord = new Coord(Coord.x + i, Coord.z - i);

            // �ǃ}�X
            if (map.IsWall(exploCoord))
                break;

            map.BreakStone(exploCoord);
            map.ActivePredictLandmark(exploCoord, false);
            PlayExplosionEffect(exploCoord);
            continue;
        }
        for (int i = 1; i <= firepower; i++)
        {
            exploCoord = new Coord(Coord.x - i, Coord.z + i);

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
