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
public class SpesialBomb
{
    // ===•Ï”====================================================
    [Header("ƒv[ƒ‹‚Ìe")]
    [SerializeField] Transform m_poolParent;

    [Header("ƒpƒ‰ƒ[ƒ^[")]
    private BombType _type;
    [SerializeField] int m_spesialBombMaxCount;
    [SerializeField] int m_bombMaxValue;
    [SerializeField] int m_MegatonExploLevel;

    [Header("Šeíƒ{ƒ€")]
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

    // ===ŠÖ”====================================================
    /// <summary>
    /// ƒ{ƒ€‚ğ•ÏX‚µ‚Ü‚·B
    /// ƒv[ƒ‹‚ğˆê“x‹ó‚É‚·‚é
    /// </summary>
    /// <param name="type">•ÏX‚·‚éƒ^ƒCƒv</param>
    public void SetBombType(BombType type)
    {
        _type = type;
        _spesialPool.list.Clear();
    }


    /// <summary>
    /// ”š’eƒ^ƒCƒv‚ğw’è‚µ‚Ä“Áê”š’e‚ğ’u‚«‚Ü‚·
    /// </summary>
    /// <param name="map">ƒ}ƒbƒvî•ñ</param>
    /// <param name="playerTrafo">ƒvƒŒƒCƒ„[‚Ìƒgƒ‰ƒ“ƒXƒtƒH[ƒ€</param>
    /// <param name="exploLevel">”š”­ƒŒƒxƒ‹</param>
    public void Put(GameMap map, Transform playerTrafo, int exploLevel)
    {
        Coord coord = map.gridField.GridCoordinate(playerTrafo.position);
        Vector3 dir = FPS.GetVector3FourDirection(playerTrafo.rotation.eulerAngles);
        switch (_type)
        {
            // èÖ’e
            case BombType.GrenadeBomb:
                {
                    GameObject bomb = _spesialPool.Take(b => b.GetComponent<GrenadeBomb>().isHeld);
                    
                    if (bomb == null)
                    {
                        Debug.Log("”š’e‚ª‚È‚¢I");
                        return;
                    }
                    GrenadeBomb grenade = bomb.GetComponent<GrenadeBomb>();

                    grenade.Initialize(map);
                    grenade.Throw(coord, dir);
                    break;
                }

            // ’n—‹’e
            case BombType.LandmineBomb:
                {
                    GameObject bomb = _spesialPool.Take(b => b.GetComponent<LandmineBomb>().isHeld);
                    
                    if (bomb == null)
                    {
                        Debug.Log("”š’e‚ª‚È‚¢I");
                        return;
                    }
                    LandmineBomb landmine = bomb.GetComponent<LandmineBomb>();

                    landmine.Initialize(map);
                    landmine.Put(coord,exploLevel);
                    break;
                }

            // ƒNƒƒX”š’e
            case BombType.CrossBomb:
                {
                    GameObject bomb = _spesialPool.Take(b => b.GetComponent<CrossBomb>().isHeld);
                    
                    if (bomb == null)
                    {
                        Debug.Log("”š’e‚ª‚È‚¢I");
                        return;
                    }
                    CrossBomb cross = bomb.GetComponent<CrossBomb>();

                    cross.Initialize(map);
                    cross.Put(coord, exploLevel);
                    break;
                }

            // ‘±”š’e
            case BombType.PersistentBomb:
                {
                    GameObject bomb = _spesialPool.Take(b => b.GetComponent<PersistentBomb>().isHeld);
                    
                    if (bomb == null)
                    {
                        Debug.Log("”š’e‚ª‚È‚¢I");
                        return;
                    }
                    PersistentBomb persistent = bomb.GetComponent<PersistentBomb>();

                    persistent.Initialize(map);
                    persistent.Put(coord, exploLevel);
                    break;
                }

            // “§–¾”š’e
            case BombType.TransparentBomb:
                {
                    GameObject bomb = _spesialPool.Take(b => b.GetComponent<NormalBomb>().isHeld);
                    
                    if (bomb == null)
                    {
                        Debug.Log("”š’e‚ª‚È‚¢I");
                        return;
                    }
                    NormalBomb transparent = bomb.GetComponent<NormalBomb>();

                    transparent.Initialize(map);
                    transparent.Put(coord, exploLevel);
                    break;
                }

            // •XŒ‹”š’e
            case BombType.IceBomb:
                {
                    GameObject bomb = _spesialPool.Take(b => b.GetComponent<IceBomb>().isHeld);
                    
                    if (bomb == null)
                    {
                        Debug.Log("”š’e‚ª‚È‚¢I");
                        return;
                    }
                    IceBomb ice = bomb.GetComponent<IceBomb>();

                    ice.Initialize(map);
                    ice.Put(coord, exploLevel);
                    break;
                }

            // ƒƒKƒgƒ“”š’e
            case BombType.MegatonBomb:
                {
                    GameObject bomb = _spesialPool.Take(b => b.GetComponent<MegatonBomb>().isHeld);
                    
                    if (bomb == null)
                    {
                        Debug.Log("”š’e‚ª‚È‚¢I");
                        return;
                    }
                    MegatonBomb megaton = bomb.GetComponent<MegatonBomb>();

                    megaton.Initialize(map);
                    megaton.Put(coord, m_MegatonExploLevel);
                    break;
                }

            // ŠÑ’Ê”š’e
            case BombType.PierceBomb:
                {
                    GameObject bomb = _spesialPool.Take(b => b.GetComponent<PierceBomb>().isHeld);
                    
                    if (bomb == null)
                    {
                        Debug.Log("”š’e‚ª‚È‚¢I");
                        return;
                    }
                    PierceBomb pierce = bomb.GetComponent<PierceBomb>();

                    pierce.Initialize(map);
                    pierce.Put(coord, exploLevel);
                    break;
                }

            // ‰ñ•œ”š’e
            case BombType.HealBomb:
                {
                    GameObject bomb = _spesialPool.Take(b => b.GetComponent<FogBomb>().isHeld);
                    
                    if (bomb == null)
                    {
                        Debug.Log("”š’e‚ª‚È‚¢I");
                        return;
                    }
                    FogBomb heal = bomb.GetComponent<FogBomb>();

                    heal.Initialize(map);
                    heal.Put(coord, exploLevel);
                    break;
                }

            // “Å”š’e
            case BombType.PoisonBomb:
                {
                    GameObject bomb = _spesialPool.Take(b => b.GetComponent<FogBomb>().isHeld);
                    
                    if (bomb == null)
                    {
                        Debug.Log("”š’e‚ª‚È‚¢I");
                        return;
                    }
                    FogBomb poizon = bomb.GetComponent<FogBomb>();

                    poizon.Initialize(map);
                    poizon.Put(coord, exploLevel);
                    break;
                }
        }
    }

    public void Add(GameMap map)
    {
        Debug.Log("‚ ‚Ç");
        switch (_type)
        {
            // èÖ’e
            case BombType.GrenadeBomb:
                {
                    GrenadeBomb bomb = GameObject.Instantiate(grenadeBomb.gameObject).GetComponent<GrenadeBomb>();
                    
                    _spesialPool.Add(bomb.gameObj);
                    bomb.gameObj.SetActive(false);
                    bomb.Initialize(map);
                    break;
                }

            // ’n—‹’e
            case BombType.LandmineBomb:
                {
                    LandmineBomb bomb = GameObject.Instantiate(landmineBomb.gameObject).GetComponent<LandmineBomb>();

                    _spesialPool.Add(bomb.gameObj);
                    bomb.gameObj.SetActive(false);
                    bomb.Initialize(map);
                    break;
                }

            // ƒŠƒ‚[ƒg”š’e
            case BombType.RemoteBomb:
                {
                    GrenadeBomb bomb = GameObject.Instantiate(grenadeBomb.gameObject).GetComponent<GrenadeBomb>();

                    _spesialPool.Add(bomb.gameObj);
                    bomb.gameObj.SetActive(false);
                    bomb.Initialize(map);
                    break;
                }

            // ƒNƒƒX”š’e
            case BombType.CrossBomb:
                {
                    CrossBomb bomb = GameObject.Instantiate(crossBomb.gameObject).GetComponent<CrossBomb>();

                    _spesialPool.Add(bomb.gameObj);
                    bomb.gameObj.SetActive(false);
                    bomb.Initialize(map);
                    break;
                }

            // ‘±”š’e
            case BombType.PersistentBomb:
                {
                    PersistentBomb bomb = GameObject.Instantiate(persistentBomb.gameObject).GetComponent<PersistentBomb>();

                    _spesialPool.Add(bomb.gameObj);
                    bomb.gameObj.SetActive(false);
                    bomb.Initialize(map);
                    break;
                }

            // “§–¾”š’e
            case BombType.TransparentBomb:
                {
                    NormalBomb bomb = GameObject.Instantiate(transparentBomb.gameObject).GetComponent<NormalBomb>();

                    _spesialPool.Add(bomb.gameObj);
                    bomb.gameObj.SetActive(false);
                    bomb.Initialize(map);
                    break;
                }

            // •XŒ‹”š’e
            case BombType.IceBomb:
                {
                    IceBomb bomb = GameObject.Instantiate(iceBomb.gameObject).GetComponent<IceBomb>();

                    _spesialPool.Add(bomb.gameObj);
                    bomb.gameObj.SetActive(false);
                    bomb.Initialize(map);
                    break;
                }

            // ƒƒKƒgƒ“”š’e
            case BombType.MegatonBomb:
                {
                    MegatonBomb bomb = GameObject.Instantiate(megatonBomb.gameObject).GetComponent<MegatonBomb>();

                    _spesialPool.Add(bomb.gameObj);
                    bomb.gameObj.SetActive(false);
                    bomb.Initialize(map);
                    break;
                }

            // ŠÑ’Ê”š’e
            case BombType.PierceBomb:
                {
                    PierceBomb bomb = GameObject.Instantiate(pierceBomb.gameObject).GetComponent<PierceBomb>();

                    _spesialPool.Add(bomb.gameObj);
                    bomb.gameObj.SetActive(false);
                    bomb.Initialize(map);
                    break;
                }

            // ‰ñ•œ”š’e
            case BombType.HealBomb:
                {
                    FogBomb bomb = GameObject.Instantiate(healBomb.gameObject).GetComponent<FogBomb>();

                    _spesialPool.Add(bomb.gameObj);
                    bomb.gameObj.SetActive(false);
                    bomb.Initialize(map);
                    break;
                }

            // “Å”š’e
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
