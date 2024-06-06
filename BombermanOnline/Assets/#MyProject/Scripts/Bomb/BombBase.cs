using SoftGear.Strix.Unity.Runtime;
using System;
using System.Collections.Generic;
using UnityEngine;

public class BombBase : Base
{
    // ===�ϐ�========================================
    [Header("�p�����[�^�[")]
    [SerializeField] Vector3 putPos;
    public bool isHeld = true; // ������Ă��邩�ǂ���
    public int explosionTime;
    public int firepower;

    [Header("�I�u�W�F�N�g�Q��")]
    [SerializeField] private Explosion m_explosion;

    private Timer counter = new Timer();
    private List<Explosion> exploPool = new List<Explosion>();
    
    // ===�֐�====================================================
    /// <summary>
    /// ���e������
    /// </summary>
    /// <param name="map"></param>
    public override void Initialize(GameMap map)
    {
        gameManager = GameManager.Instance;
        base.Initialize(map);
    }


    /// <summary>
    /// �����܂ł̃^�C�}�[
    /// </summary>
    /// <param name="countAction">��������܂ł̃A�N�V����</param>
    /// <param name="fireAction">��������Ƃ��̃A�N�V����</param>
    protected void BombTimer(Action countAction,Action fireAction)
    {
        if (isHeld == false)
        {
            counter.Count();

            countAction();
            //PredictionFire();
        }
        if (counter.Point(explosionTime))
        {
            counter.Reset();
            
            fireAction();
        }
    }


    /// <summary>
    /// ���e��u�������̏���
    /// </summary>
    /// <param name="coord">�u�������W</param>
    /// <param name="exploLevel">�������x��</param>
    public void Put(Coord coord,int exploLevel)
    {
        isHeld = false;
        firepower = exploLevel;
        gameObj.SetActive(true);
        Coord = coord;
        Pos += putPos;
    }


    /// <summary>
    /// �����̃v���n�u�𐶐����A�G�t�F�N�g���Đ����܂�
    /// </summary>
    /// <param name="exploCoord">��������ꏊ</param>
    protected void PlayExplosionEffect(Coord exploCoord)
    {
        // �v�[�����甚��������Ȃ����m��T���o��
        Explosion explo = gameManager.exploPool.Get(e => e.IsExplosion == false,() => Instantiate(m_explosion));

        // ������������
        explo.Initialize(map, exploCoord);
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

            if(map.IsEmpty(exploCoord))
            {
                map.ActivePredictLandmark(exploCoord, false);
                PlayExplosionEffect(exploCoord);
                continue;
            }

            if(map.IsWall(exploCoord))
            {
                break;
            }


            if(map.IsStone(exploCoord))
            {
                map.ActivePredictLandmark(exploCoord, false);
                map.BreakStone (exploCoord);
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
        isHeld = true;
    }


    /// <summary>
    /// �������\�������}�X�Ƀ����h�}�[�N��\�����܂�
    /// </summary>
    public void PredictionFire()
    {
        // ���e�̈ʒu
        map.ActivePredictLandmark(Coord,true);

        Coord exploCoord;
        for (int x = 1; x <= firepower; x++)
        {
            exploCoord = new Coord(Coord.x + x, Coord.z);

            // �����Ȃ��}�X
            if (map.IsEmpty(exploCoord))
            {
                map.ActivePredictLandmark(exploCoord, true);
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
                map.ActivePredictLandmark(exploCoord, true);
                break;
            }
        }
        for (int x = -1; x >= -firepower; x--)
        {
            exploCoord = new Coord(Coord.x + x, Coord.z);

            if (map.IsEmpty(exploCoord))
            {
                map.ActivePredictLandmark(exploCoord, true);
                continue;
            }

            if (map.IsWall(exploCoord))
            {
                break;
            }


            if (map.IsStone(exploCoord))
            {
                map.ActivePredictLandmark(exploCoord, true);
                break;
            }
        }
        for (int z = 1; z <= firepower; z++)
        {
            exploCoord = new Coord(Coord.x, Coord.z + z);

            if (map.IsEmpty(exploCoord))
            {
                map.ActivePredictLandmark(exploCoord, true);
                continue;
            }

            if (map.IsWall(exploCoord))
            {
                break;
            }


            if (map.IsStone(exploCoord))
            {
                map.ActivePredictLandmark(exploCoord, true);
                break;
            }
        }
        for (int z = -1; z >= -firepower; z--)
        {
            exploCoord = new Coord(Coord.x, Coord.z + z);

            if (map.IsEmpty(exploCoord))
            {
                map.ActivePredictLandmark(exploCoord, true);
                continue;
            }

            if (map.IsWall(exploCoord))
            {
                break;
            }


            if (map.IsStone(exploCoord))
            {
                map.ActivePredictLandmark(exploCoord, true);
                break;
            }
        }
    }
}
