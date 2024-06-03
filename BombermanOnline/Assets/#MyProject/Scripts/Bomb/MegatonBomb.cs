using SoftGear.Strix.Unity.Runtime;
using UnityEngine;

// �ђʔ��e
public class MegatonBomb : BombBase
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
