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


    private void BombTimer()
    {
        if (isHeld == false)
        {
            counter.Count();
        }
        if (counter.Point(explosionTime))
        {
            counter.Reset();

            Fire();
        }
    }


    // ===�ϐ�========================================
    [SerializeField] Vector3 putPos;

    public bool isHeld = true; // ������Ă��邩�ǂ���
    public int explosionTime;
    public int firepower;
    private Timer counter = new Timer();
    private List<Explosion> exploPool = new List<Explosion>();
    [SerializeField] private Explosion m_explosion;
    private AudioSource _audioSource;

    // ===�֐�====================================================
    public override void Initialize(GameMap map)
    {
        base.Initialize(map);
        _audioSource ??= GetComponent<AudioSource>();
        CallInActive();
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
        // ���e�̈ʒu
        PlayExplosionEffect(Coord);

        Coord exploCoord;
        for (int x = 1; x <= firepower; x++)
        {
            exploCoord = new Coord(Coord.x + x, Coord.z);
            if (map.BreakStone(exploCoord) == false)
                break;
            PlayExplosionEffect(exploCoord);
        }
        for (int x = -1; x >= -firepower; x--)
        {
            exploCoord = new Coord(Coord.x + x, Coord.z);
            if (map.BreakStone(exploCoord) == false)
                break;
            PlayExplosionEffect(exploCoord);
        }
        for (int z = 1; z <= firepower; z++)
        {
            exploCoord = new Coord(Coord.x, Coord.z + z);
            if (map.BreakStone(exploCoord) == false)
                break;
            PlayExplosionEffect(exploCoord);
        }
        for (int z = -1; z >= -firepower; z--)
        {
            exploCoord = new Coord(Coord.x, Coord.z + z);
            if (map.BreakStone(exploCoord) == false)
                break;
            PlayExplosionEffect(exploCoord);
        }
        AudioManager.PlayOneShot("����",0.3f);
        CallInActive();     // ��A�N�e�B�u
        isHeld = true;
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
