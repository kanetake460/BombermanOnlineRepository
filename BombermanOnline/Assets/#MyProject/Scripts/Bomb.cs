using SoftGear.Strix.Unity.Runtime;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : Base
{
    // ===イベント関数================================================
    protected void Start()
    {
    }

    private void Update()
    {
        if (isLocal == false) return;
        BombTimer();
    }





    // ===変数========================================
    [SerializeField] Vector3 putPos;
    [SerializeField] Color predictionColor;

    public bool isHeld = true; // 持たれているかどうか
    public int explosionTime;
    public int firepower;
    private Timer counter = new Timer();
    private List<Explosion> exploPool = new List<Explosion>();
    [SerializeField] private Explosion m_explosion;
    private AudioSource _audioSource;

    // ===関数====================================================
    /// <summary>
    /// 爆弾初期化
    /// </summary>
    /// <param name="map"></param>
    public override void Initialize(GameMap map)
    {
        base.Initialize(map);
        _audioSource ??= GetComponent<AudioSource>();
        CallInActive();
    }


    /// <summary>
    /// 爆発までのタイマー
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
    /// 爆弾非アクティブ（RPC）
    /// </summary>
    private void CallInActive() { RpcToAll(nameof(InActive)); }
    [StrixRpc]
    private void InActive()
    {
        gameObj.SetActive(false);
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
    /// 四方向のストーンブロックを爆破します
    /// </summary>
    public void Fire()
    {
        if (map.emptyCoords.Contains(Coord))
        {
            Debug.Log("そこはEmpty");
        }
        // 爆弾の位置
        map.ActivePredictLandmark(Coord, false);
        PlayExplosionEffect(Coord);

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
        AudioManager.PlayOneShot("爆発",0.3f);
        CallInActive();     // 非アクティブ
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


    /// <summary>
    /// 爆発のプレハブを生成し、エフェクトを再生します
    /// </summary>
    /// <param name="exploCoord">生成する場所</param>
    private void PlayExplosionEffect(Coord exploCoord)
    {
        // プールから爆発中じゃないモノを探し出す
        Explosion explo = exploPool.Find(e => e.IsExplosion == false);
        if (explo == null)
        {
            // ないなら、生成し、プールに追加
            explo = map.m_mapSet.gridField.Instantiate(m_explosion, exploCoord, Quaternion.identity) as Explosion;
            exploPool.Add(explo);
        }
        // 爆発を初期化
        explo.Initialize(map,exploCoord);
    }
}
