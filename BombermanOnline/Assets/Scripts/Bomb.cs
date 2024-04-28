using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TakeshiLibrary;
using UnityEngine;

public class Bomb : Base
{
    // ===�C�x���g�֐�================================================
    protected override void Start()
    {
        base.Start();
    }

    private void Update()
    {
        if (isHeld == false)
        {
            counter.Count();
        }
        if(counter.Point(explosionTime))
        {
            gameObj.SetActive(false);
            counter.Reset();
            isHeld = true;
            Explosion();
            Debug.Log("�΁[��");
        }
    }

    // ===�ϐ�========================================
    public bool isHeld = true; // ������Ă��邩�ǂ���
    public int explosionTime;
    public int explosionLevel;
    private Timer counter = new Timer();
    private const int exploDirNum = 4;

    // ===�֐�====================================================

    public void Put(Coord coord)
    {
        isHeld = false;
        gameObj.SetActive(true);
        Coord = coord;
        Pos += new Vector3(0,2.5f,0);
    }

    public void Explosion()
    {
        Coord exploCoord;
        for (int x = 1; x <= explosionLevel; x++)
        {
            exploCoord = new Coord(Coord.x + x, Coord.z);
            if (_map.BreakStone(exploCoord) == false)
                break;
        }
        for (int x = -1; x >= -explosionLevel; x--)
        {
            exploCoord = new Coord(Coord.x + x, Coord.z);
            if (_map.BreakStone(exploCoord) == false)
                break;
        }
        for (int z = 1; z <= explosionLevel; z++)
        {
            exploCoord = new Coord(Coord.x, Coord.z + z);
            if (_map.BreakStone(exploCoord) == false)
                break;
        }
        for (int z = -1; z >= -explosionLevel; z--)
        {
            exploCoord = new Coord(Coord.x, Coord.z + z);
            if (_map.BreakStone(exploCoord) == false)
                break;
        }
    }




    // ===�v���p�e�B=================================================
}
