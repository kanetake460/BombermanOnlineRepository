using SoftGear.Strix.Unity.Runtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TakeshiLibrary;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms;

[Serializable]
public class SpesialBomb : StrixBehaviour
{
    // ===変数====================================================
    [Header("プールの親")]
    [SerializeField] Transform m_poolParent;

    [Header("パラメーター")]
    private BombType type;
    [SerializeField] int m_spesialBombMaxCount;
    [SerializeField] int m_bombMaxValue;
    [SerializeField] int m_MegatonExploLevel;

    [Header("各種ボム")]
    [SerializeField] GrenadeBomb grenadeBomb;
    [SerializeField] LandmineBomb landmineBomb;
    [SerializeField] NormalBomb RemoteBomb;
    [SerializeField] CrossBomb crossBomb;
    [SerializeField] PersistentBomb persistentBomb;
    [SerializeField] MegatonBomb megatonBomb;
    [SerializeField] PierceBomb pierceBomb;
    [SerializeField] NormalBomb transparentBomb;
    [SerializeField] IceBomb iceBomb;
    [SerializeField] FogBomb poisonBomb;
    [SerializeField] FogBomb healBomb;

    private Pool<GameObject> _spesialPool = new Pool<GameObject>();

    public enum BombType
    {
        GrenadeBomb,
        LandmineBomb,
        RemoteBomb,
        CrossBomb,
        PersistentBomb,
        TransparentBomb,
        IceBomb,
        MegatonBomb,
        PierceBomb,
        HealBomb,
        PoisonBomb,
    };

    // ===関数====================================================
    /// <summary>
    /// プールを空にする
    /// </summary>
    /// <param name="type">変更するタイプ</param>
    public void ClearBombType()
    {
        _spesialPool.list.Clear();
    }


    /// <summary>
    /// 爆弾タイプを指定して特殊爆弾を置きます
    /// </summary>
    /// <param name="map">マップ情報</param>
    /// <param name="playerTrafo">プレイヤーのトランスフォーム</param>
    /// <param name="exploLevel">爆発レベル</param>
    public void GenerateSpesialBomb(BombType type,Coord coord,Vector3 dir, int exploLevel)
    {
        switch (type)
        {
            // 手榴弾
            case BombType.GrenadeBomb:
                {
                    GameObject bomb = _spesialPool.Take(b => b.GetComponent<GrenadeBomb>().isHeld);
                    
                    if (bomb == null)
                    {
                        Debug.Log("爆弾がない！");
                        return;
                    }
                    GrenadeBomb grenade = bomb.GetComponent<GrenadeBomb>();

                    grenade.Throw(coord, dir);
                    break;
                }

            // 地雷弾
            case BombType.LandmineBomb:
                {
                    GameObject bomb = _spesialPool.Take(b => b.GetComponent<LandmineBomb>().isHeld);
                    
                    if (bomb == null)
                    {
                        Debug.Log("爆弾がない！");
                        return;
                    }
                    LandmineBomb landmine = bomb.GetComponent<LandmineBomb>();

                    landmine.Put(coord, exploLevel);
                    break;
                }

            // クロス爆弾
            case BombType.CrossBomb:
                {
                    GameObject bomb = _spesialPool.Take(b => b.GetComponent<CrossBomb>().isHeld);
                    
                    if (bomb == null)
                    {
                        Debug.Log("爆弾がない！");
                        return;
                    }
                    CrossBomb cross = bomb.GetComponent<CrossBomb>();

                    cross.Put(coord, exploLevel);
                    break;
                }

            // 持続爆弾
            case BombType.PersistentBomb:
                {
                    GameObject bomb = _spesialPool.Take(b => b.GetComponent<PersistentBomb>().isHeld);
                    
                    if (bomb == null)
                    {
                        Debug.Log("爆弾がない！");
                        return;
                    }
                    PersistentBomb persistent = bomb.GetComponent<PersistentBomb>();

                    persistent.Put(coord, exploLevel);
                    break;
                }

            // 透明爆弾
            case BombType.TransparentBomb:
                {
                    GameObject bomb = _spesialPool.Take(b => b.GetComponent<NormalBomb>().isHeld);
                    
                    if (bomb == null)
                    {
                        Debug.Log("爆弾がない！");
                        return;
                    }
                    NormalBomb transparent = bomb.GetComponent<NormalBomb>();

                    transparent.Put(coord, exploLevel);
                    break;
                }

            // 氷結爆弾
            case BombType.IceBomb:
                {
                    GameObject bomb = _spesialPool.Take(b => b.GetComponent<IceBomb>().isHeld);
                    
                    if (bomb == null)
                    {
                        Debug.Log("爆弾がない！");
                        return;
                    }
                    IceBomb ice = bomb.GetComponent<IceBomb>();

                    ice.Put(coord, exploLevel);
                    break;
                }

            // メガトン爆弾
            case BombType.MegatonBomb:
                {
                    GameObject bomb = _spesialPool.Take(b => b.GetComponent<MegatonBomb>().isHeld);
                    
                    if (bomb == null)
                    {
                        Debug.Log("爆弾がない！");
                        return;
                    }
                    MegatonBomb megaton = bomb.GetComponent<MegatonBomb>();

                    megaton.Put(coord, m_MegatonExploLevel);
                    break;
                }

            // 貫通爆弾
            case BombType.PierceBomb:
                {
                    GameObject bomb = _spesialPool.Take(b => b.GetComponent<PierceBomb>().isHeld);
                    
                    if (bomb == null)
                    {
                        Debug.Log("爆弾がない！");
                        return;
                    }
                    PierceBomb pierce = bomb.GetComponent<PierceBomb>();

                    pierce.Put(coord, exploLevel);
                    break;
                }

            // 回復爆弾
            case BombType.HealBomb:
                {
                    GameObject bomb = _spesialPool.Take(b => b.GetComponent<FogBomb>().isHeld);
                    
                    if (bomb == null)
                    {
                        Debug.Log("爆弾がない！");
                        return;
                    }
                    FogBomb heal = bomb.GetComponent<FogBomb>();

                    heal.Put(coord, exploLevel);
                    break;
                }

            // 毒爆弾
            case BombType.PoisonBomb:
                {
                    GameObject bomb = _spesialPool.Take(b => b.GetComponent<FogBomb>().isHeld);
                    
                    if (bomb == null)
                    {
                        Debug.Log("爆弾がない！");
                        return;
                    }
                    FogBomb poizon = bomb.GetComponent<FogBomb>();

                    poizon.Put(coord, exploLevel);
                    break;
                }
        }
    }

    public void Add(BombType type,GameMap map)
    {
        Debug.Log("あど");
        switch (type)
        {
            // 手榴弾
            case BombType.GrenadeBomb:
                {
                    GrenadeBomb bomb = GameObject.Instantiate(grenadeBomb.gameObject).GetComponent<GrenadeBomb>();
                    
                    _spesialPool.Add(bomb.gameObj);
                    bomb.gameObj.SetActive(false);
                    bomb.Initialize(map);
                    break;
                }

            // 地雷弾
            case BombType.LandmineBomb:
                {
                    LandmineBomb bomb = GameObject.Instantiate(landmineBomb.gameObject).GetComponent<LandmineBomb>();

                    _spesialPool.Add(bomb.gameObj);
                    bomb.gameObj.SetActive(false);
                    bomb.Initialize(map);
                    break;
                }

            // リモート爆弾
            case BombType.RemoteBomb:
                {
                    GrenadeBomb bomb = GameObject.Instantiate(grenadeBomb.gameObject).GetComponent<GrenadeBomb>();

                    _spesialPool.Add(bomb.gameObj);
                    bomb.gameObj.SetActive(false);
                    bomb.Initialize(map);
                    break;
                }

            // クロス爆弾
            case BombType.CrossBomb:
                {
                    CrossBomb bomb = GameObject.Instantiate(crossBomb.gameObject).GetComponent<CrossBomb>();

                    _spesialPool.Add(bomb.gameObj);
                    bomb.gameObj.SetActive(false);
                    bomb.Initialize(map);
                    break;
                }

            // 持続爆弾
            case BombType.PersistentBomb:
                {
                    PersistentBomb bomb = GameObject.Instantiate(persistentBomb.gameObject).GetComponent<PersistentBomb>();

                    _spesialPool.Add(bomb.gameObj);
                    bomb.gameObj.SetActive(false);
                    bomb.Initialize(map);
                    break;
                }

            // 透明爆弾
            case BombType.TransparentBomb:
                {
                    NormalBomb bomb = GameObject.Instantiate(transparentBomb.gameObject).GetComponent<NormalBomb>();

                    _spesialPool.Add(bomb.gameObj);
                    bomb.gameObj.SetActive(false);
                    bomb.Initialize(map);
                    break;
                }

            // 氷結爆弾
            case BombType.IceBomb:
                {
                    IceBomb bomb = GameObject.Instantiate(iceBomb.gameObject).GetComponent<IceBomb>();

                    _spesialPool.Add(bomb.gameObj);
                    bomb.gameObj.SetActive(false);
                    bomb.Initialize(map);
                    break;
                }

            // メガトン爆弾
            case BombType.MegatonBomb:
                {
                    MegatonBomb bomb = GameObject.Instantiate(megatonBomb.gameObject).GetComponent<MegatonBomb>();

                    _spesialPool.Add(bomb.gameObj);
                    bomb.gameObj.SetActive(false);
                    bomb.Initialize(map);
                    break;
                }

            // 貫通爆弾
            case BombType.PierceBomb:
                {
                    PierceBomb bomb = GameObject.Instantiate(pierceBomb.gameObject).GetComponent<PierceBomb>();

                    _spesialPool.Add(bomb.gameObj);
                    bomb.gameObj.SetActive(false);
                    bomb.Initialize(map);
                    break;
                }

            // 回復爆弾
            case BombType.HealBomb:
                {
                    FogBomb bomb = GameObject.Instantiate(healBomb.gameObject).GetComponent<FogBomb>();

                    _spesialPool.Add(bomb.gameObj);
                    bomb.gameObj.SetActive(false);
                    bomb.Initialize(map);
                    break;
                }

            // 毒爆弾
            case BombType.PoisonBomb:
                {
                    FogBomb bomb = GameObject.Instantiate(poisonBomb.gameObject).GetComponent<FogBomb>();

                    _spesialPool.Add(bomb.gameObj);
                    bomb.gameObj.SetActive(false);
                    bomb.Initialize(map);
                    break;
                }
        }
    }
}
