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


    // ===変数========================================
    [SerializeField] Vector3 putPos;

    public bool isHeld = true; // 持たれているかどうか
    public int explosionTime;
    public int firepower;
    private Timer counter = new Timer();
    private List<Explosion> exploPool = new List<Explosion>();
    [SerializeField] private Explosion m_explosion;
    private AudioSource _audioSource;

    // ===関数====================================================
    public override void Initialize(GameMap map)
    {
        base.Initialize(map);
        _audioSource ??= GetComponent<AudioSource>();
        CallInActive();
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
        Debug.Log(map.emptyCoords.Count);
        if (map.emptyCoords.Contains(Coord))
        {
            Debug.Log("そこはEmpty");
        }
        // 爆弾の位置
        PlayExplosionEffect(Coord);

        Coord exploCoord;
        for (int x = 1; x <= firepower; x++)
        {
            exploCoord = new Coord(Coord.x + x, Coord.z);

            // 爆発マスが、Emptyなら
            if (map.emptyCoords.Contains(exploCoord))
            {
                // 破壊するマスが、ストーンブロックか調べて破壊する
                // 壁ブロックの場合は、break
                if (map.ContinueBreakStone(exploCoord) == false)
                    break;

                // 破壊したのでエフェクト再生
                PlayExplosionEffect(exploCoord);
            }
            // ストーンブロックまたは、壁ブロックなら
            else
            {
                // 破壊するマスが、ストーンブロックか調べて破壊する
                // 壁ブロックの場合、ストーンブロックの場合は、break
                if (map.BreakStone(exploCoord) == false)
                {
                    // 破壊したのでエフェクト再生し、破壊ループ終了
                    PlayExplosionEffect(exploCoord);
                    break;
                }
            }
        }
        for (int x = -1; x >= -firepower; x--)
        {
            exploCoord = new Coord(Coord.x + x, Coord.z);

            // 爆発マスが、Emptyなら
            if (map.emptyCoords.Contains(exploCoord))
            {
                // 破壊するマスが、ストーンブロックか調べて破壊する
                // 壁ブロックの場合は、break
                if (map.ContinueBreakStone(exploCoord) == false)
                    break;

                // 破壊したのでエフェクト再生
                PlayExplosionEffect(exploCoord);
            }
            // ストーンブロックまたは、壁ブロックなら
            else
            {
                // 破壊するマスが、ストーンブロックか調べて破壊する
                // 壁ブロックの場合、ストーンブロックの場合は、break
                if (map.BreakStone(exploCoord) == false)
                {
                    // 破壊したのでエフェクト再生し、破壊ループ終了
                    PlayExplosionEffect(exploCoord);
                    break;
                }
            }
        }
        for (int z = 1; z <= firepower; z++)
        {
            exploCoord = new Coord(Coord.x, Coord.z + z);

            // 爆発マスが、Emptyなら
            if (map.emptyCoords.Contains(exploCoord))
            {
                // 破壊するマスが、ストーンブロックか調べて破壊する
                // 壁ブロックの場合は、break
                if (map.ContinueBreakStone(exploCoord) == false)
                    break;

                // 破壊したのでエフェクト再生
                PlayExplosionEffect(exploCoord);
            }
            // ストーンブロックまたは、壁ブロックなら
            else
            {
                // 破壊するマスが、ストーンブロックか調べて破壊する
                // 壁ブロックの場合、ストーンブロックの場合は、break
                if (map.BreakStone(exploCoord) == false)
                {
                    // 破壊したのでエフェクト再生し、破壊ループ終了
                    PlayExplosionEffect(exploCoord);
                    break;
                }
            }
        }
        for (int z = -1; z >= -firepower; z--)
        {
            exploCoord = new Coord(Coord.x, Coord.z + z);

            if (map.emptyCoords.Contains(exploCoord) == false)
            {
                // 破壊するマスが、ストーンブロックか調べて破壊する
                // 壁ブロックの場合は、break
                if (map.ContinueBreakStone(exploCoord) == false)
                    break;

                // 破壊したのでエフェクト再生
                PlayExplosionEffect(exploCoord);
            }
        }
        AudioManager.PlayOneShot("爆発",0.3f);
        CallInActive();     // 非アクティブ
        isHeld = true;
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
