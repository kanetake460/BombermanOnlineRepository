using SoftGear.Strix.Unity.Runtime;
using System;
using System.Collections.Generic;
using UnityEngine;

public class BombBase : Base
{
    // ===変数========================================
    [Header("パラメーター")]
    [SerializeField] Vector3 putPos;
    public bool isHeld = true; // 持たれているかどうか
    public int explosionTime;
    public int firepower;

    [Header("オブジェクト参照")]
    [SerializeField] private Explosion m_explosion;

    private Timer counter = new Timer();
    private List<Explosion> exploPool = new List<Explosion>();
    
    // ===関数====================================================
    /// <summary>
    /// 爆弾初期化
    /// </summary>
    /// <param name="map"></param>
    public override void Initialize(GameMap map)
    {
        gameManager = GameManager.Instance;
        base.Initialize(map);
    }


    /// <summary>
    /// 爆発までのタイマー
    /// </summary>
    /// <param name="countAction">爆発するまでのアクション</param>
    /// <param name="fireAction">爆発するときのアクション</param>
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
    /// 爆弾を置いた時の処理
    /// </summary>
    /// <param name="coord">置かれる座標</param>
    /// <param name="exploLevel">爆発レベル</param>
    public void Put(Coord coord,int exploLevel)
    {
        isHeld = false;
        firepower = exploLevel;
        gameObj.SetActive(true);
        Coord = coord;
        Pos += putPos;
    }


    /// <summary>
    /// 爆発のプレハブを生成し、エフェクトを再生します
    /// </summary>
    /// <param name="exploCoord">生成する場所</param>
    protected void PlayExplosionEffect(Coord exploCoord)
    {
        // プールから爆発中じゃないモノを探し出す
        Explosion explo = gameManager.exploPool.Get(e => e.IsExplosion == false,() => Instantiate(m_explosion));

        // 爆発を初期化
        explo.Initialize(map, exploCoord);
    }


    /// <summary>
    /// 四方向のストーンブロックを爆破します
    /// </summary>
    public void Fire()
    {
        // 爆弾の位置
        map.ActivePredictLandmark(Coord, false);
        PlayExplosionEffect(Coord);
        map.BreakStone(Coord);

        Coord exploCoord;
        for (int x = 1; x <= firepower; x++)
        {
            exploCoord = new Coord(Coord.x + x, Coord.z);

            // 何もないマス
            if (map.IsEmpty(exploCoord))
            {
                map.ActivePredictLandmark(exploCoord, false);
                PlayExplosionEffect(exploCoord);
                continue;
            }

            // 壁マス
            if (map.IsWall(exploCoord))
            {
                break;
            }

            // 石マス
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
    /// 爆発が予測されるマスにランドマークを表示します
    /// </summary>
    public void PredictionFire()
    {
        // 爆弾の位置
        map.ActivePredictLandmark(Coord,true);

        Coord exploCoord;
        for (int x = 1; x <= firepower; x++)
        {
            exploCoord = new Coord(Coord.x + x, Coord.z);

            // 何もないマス
            if (map.IsEmpty(exploCoord))
            {
                map.ActivePredictLandmark(exploCoord, true);
                continue;
            }

            // 壁マス
            if (map.IsWall(exploCoord))
            {
                break;
            }

            // 石マス
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
