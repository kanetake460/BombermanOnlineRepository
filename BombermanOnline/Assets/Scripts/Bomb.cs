using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TakeshiLibrary;
using UnityEngine;

public class Bomb : Base
{
    // ===�C�x���g�֐�================================================
    protected void Start()
    {
    }

    private void Update()
    {
        if (isHeld == false)
        {
            counter.Count();
        }
        if(counter.Point(explosionTime))
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
    private List<Explosion> exploList = new List<Explosion>();
    [SerializeField] private Explosion m_explosion;
    private AudioSource _audioSource;

    // ===�֐�====================================================
    public override void Initialize(GameMap map)
    {
        base.Initialize(map);
        _audioSource ??= GetComponent<AudioSource>();
        gameObj.SetActive(false);
    }

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
        gameObj.SetActive(false);
        isHeld = true;
    }

    private void PlayExplosionEffect(Coord exploCoord)
    {
        Explosion explo = exploList.Find(e => e.IsExplosion == false);
        if (explo == null)
        {
            explo = map.mapSet.gridField.Instantiate(m_explosion, exploCoord, Quaternion.identity) as Explosion;
            exploList.Add(explo);
        }
        explo.Initialize(map,exploCoord);
    }




    // ===�v���p�e�B=================================================
}
