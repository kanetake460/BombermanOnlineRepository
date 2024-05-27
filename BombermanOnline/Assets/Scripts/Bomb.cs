using SoftGear.Strix.Unity.Runtime;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : Base
{
    // ===�C�x���g�֐�================================================
    protected void Start()
    {
    }

    private void Update()
    {
        if (isLocal == false) return;
        BombTimer();
    }





    // ===�ϐ�========================================
    [SerializeField] Vector3 putPos;
    [SerializeField] Color predictionColor;

    public bool isHeld = true; // ������Ă��邩�ǂ���
    public int explosionTime;
    public int firepower;
    private Timer counter = new Timer();
    private List<Explosion> exploPool = new List<Explosion>();
    [SerializeField] private Explosion m_explosion;
    private AudioSource _audioSource;

    // ===�֐�====================================================
    /// <summary>
    /// ���e������
    /// </summary>
    /// <param name="map"></param>
    public override void Initialize(GameMap map)
    {
        base.Initialize(map);
        _audioSource ??= GetComponent<AudioSource>();
        CallInActive();
    }


    /// <summary>
    /// �����܂ł̃^�C�}�[
    /// </summary>
    private void BombTimer()
    {
        if (isHeld == false)
        {
            counter.Count();
            
            PredictionFire();
        }
        if (counter.Point(explosionTime))
        {
            counter.Reset();

            Fire();
        }
    }


    /// <summary>
    /// ���e��A�N�e�B�u�iRPC�j
    /// </summary>
    private void CallInActive() { RpcToAll(nameof(InActive)); }
    [StrixRpc]
    private void InActive()
    {
        gameObj.SetActive(false);
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
    /// �l�����̃X�g�[���u���b�N�𔚔j���܂�
    /// </summary>
    public void Fire()
    {
        if (map.emptyCoords.Contains(Coord))
        {
            Debug.Log("������Empty");
        }
        // ���e�̈ʒu
        map.ActivePredictLandmark(Coord, false);
        PlayExplosionEffect(Coord);

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
        AudioManager.PlayOneShot("����",0.3f);
        CallInActive();     // ��A�N�e�B�u
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


    /// <summary>
    /// �����̃v���n�u�𐶐����A�G�t�F�N�g���Đ����܂�
    /// </summary>
    /// <param name="exploCoord">��������ꏊ</param>
    private void PlayExplosionEffect(Coord exploCoord)
    {
        // �v�[�����甚��������Ȃ����m��T���o��
        Explosion explo = exploPool.Find(e => e.IsExplosion == false);
        if (explo == null)
        {
            // �Ȃ��Ȃ�A�������A�v�[���ɒǉ�
            explo = map.m_mapSet.gridField.Instantiate(m_explosion, exploCoord, Quaternion.identity) as Explosion;
            exploPool.Add(explo);
        }
        // ������������
        explo.Initialize(map,exploCoord);
    }
}
