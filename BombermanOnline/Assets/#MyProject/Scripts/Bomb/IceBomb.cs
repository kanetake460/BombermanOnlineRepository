using SoftGear.Strix.Unity.Runtime;
using UnityEngine;

public class IceBomb : BombBase
{
    // ===�C�x���g�֐�================================================
    private void Update()
    {
        if (isLocal == false) return;
        BombTimer(PredictionFire,Fire);
    }

    // ===�֐�====================================================
    /// <summary>
    /// �A�C�X�����̃v���n�u�𐶐����A�G�t�F�N�g���Đ����܂�
    /// </summary>
    /// <param name="exploCoord">��������ꏊ</param>
    protected void PlayExplosionEffect(Coord exploCoord)
    {
        // �v�[�����甚��������Ȃ����m��T���o��
        Explosion explo = gameManager.iceExploPool.Get(e => e.IsExplosion == false, () => Instantiate(m_explosion));
        map.SetFrozenFloor(exploCoord,true);
        // ������������
        explo.Initialize(map, exploCoord, collisionDuration);
    }


    /// <summary>
    /// �l�����̃X�g�[���u���b�N�𔚔j���܂�
    /// </summary>
    public void Fire()
    {
        // ���e�̈ʒu
        map.ActivePredictLandmark(Coord, false);
        PlayExplosionEffect(Coord);
        map.BreakStone(Coord);

        Coord exploCoord;
        for (int x = 1; x <= firepower; x++)
        {
            exploCoord = new Coord(Coord.x + x, Coord.z);

            // �����Ȃ��}�X
            if (map.IsEmpty(exploCoord))
            {
                map.ActivePredictLandmark(exploCoord, false);
                PlayExplosionEffect(exploCoord);
                continue;
            }

            // �ǃ}�X
            if (map.IsWall(exploCoord))
            {
                break;
            }

            // �΃}�X
            if (map.IsStone(exploCoord))
            {
                map.ActivePredictLandmark(exploCoord, false);
                map.BreakStone(exploCoord);
                PlayExplosionEffect(exploCoord);
                break;
            }
        }
        for (int x = -1; x >= -firepower; x--)
        {
            exploCoord = new Coord(Coord.x + x, Coord.z);

            if (map.IsEmpty(exploCoord))
            {
                map.ActivePredictLandmark(exploCoord, false);
                PlayExplosionEffect(exploCoord);
                continue;
            }

            if (map.IsWall(exploCoord))
            {
                break;
            }


            if (map.IsStone(exploCoord))
            {
                map.ActivePredictLandmark(exploCoord, false);
                map.BreakStone(exploCoord);
                PlayExplosionEffect(exploCoord);
                break;
            }
        }
        for (int z = 1; z <= firepower; z++)
        {
            exploCoord = new Coord(Coord.x, Coord.z + z);

            if (map.IsEmpty(exploCoord))
            {
                map.ActivePredictLandmark(exploCoord, false);
                PlayExplosionEffect(exploCoord);
                continue;
            }

            if (map.IsWall(exploCoord))
            {
                break;
            }


            if (map.IsStone(exploCoord))
            {
                map.ActivePredictLandmark(exploCoord, false);
                map.BreakStone(exploCoord);
                PlayExplosionEffect(exploCoord);
                break;
            }
        }
        for (int z = -1; z >= -firepower; z--)
        {
            exploCoord = new Coord(Coord.x, Coord.z + z);

            if (map.IsEmpty(exploCoord))
            {
                map.ActivePredictLandmark(exploCoord, false);
                PlayExplosionEffect(exploCoord);
                continue;
            }

            if (map.IsWall(exploCoord))
            {
                break;
            }


            if (map.IsStone(exploCoord))
            {
                map.ActivePredictLandmark(exploCoord, false);
                map.BreakStone(exploCoord);
                PlayExplosionEffect(exploCoord);
                break;
            }
        }

        AudioManager.PlayOneShot("��������", 0.3f);
        CallInActive();
        isHeld = true;
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
