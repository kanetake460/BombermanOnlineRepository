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
    // ===•Ï”====================================================
    [Header("ƒv[ƒ‹‚Ìe")]
    [SerializeField] Transform m_poolParent;

    [Header("ƒpƒ‰ƒ[ƒ^[")]
    private BombType type;
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
    /// ƒv[ƒ‹‚ğ‹ó‚É‚·‚é
    /// </summary>
    /// <param name="type">•ÏX‚·‚éƒ^ƒCƒv</param>
    public void ClearBombType()
    {
        _spesialPool.list.Clear();
    }


    /// <summary>
    /// ”š’eƒ^ƒCƒv‚ğw’è‚µ‚Ä“Áê”š’e‚ğ’u‚«‚Ü‚·
    /// </summary>
    /// <param name="map">ƒ}ƒbƒvî•ñ</param>
    /// <param name="playerTrafo">ƒvƒŒƒCƒ„[‚Ìƒgƒ‰ƒ“ƒXƒtƒH[ƒ€</param>
    /// <param name="exploLevel">”š”­ƒŒƒxƒ‹</param>
    public bool GenerateSpesialBomb(BombType type,Coord coord,Vector3 dir, int exploLevel)
    {
        switch (type)
        {
            // èÖ’e
            case BombType.GrenadeBomb:
                {
                    GameObject bomb = _spesialPool.Take(b => b.GetComponent<GrenadeBomb>().isHeld);
                    
                    if (bomb == null)
                    {
                        Debug.Log("”š’e‚ª‚È‚¢I");
                        return false;
                    }
                    GrenadeBomb grenade = bomb.GetComponent<GrenadeBomb>();

                    grenade.Throw(coord, dir);
                    return true;
                }

            // ’n—‹’e
            case BombType.LandmineBomb:
                {
                    GameObject bomb = _spesialPool.Take(b => b.GetComponent<LandmineBomb>().isHeld);
                    
                    if (bomb == null)
                    {
                        Debug.Log("”š’e‚ª‚È‚¢I");
                        return false;
                    }
                    LandmineBomb landmine = bomb.GetComponent<LandmineBomb>();

                    landmine.Put(coord, exploLevel);
                    return true;
                }

            // ƒNƒƒX”š’e
            case BombType.CrossBomb:
                {
                    GameObject bomb = _spesialPool.Take(b => b.GetComponent<CrossBomb>().isHeld);
                    
                    if (bomb == null)
                    {
                        Debug.Log("”š’e‚ª‚È‚¢I");
                        return false;
                    }
                    CrossBomb cross = bomb.GetComponent<CrossBomb>();

                    cross.Put(coord, exploLevel);
                    return true;
                }

            // ‘±”š’e
            case BombType.PersistentBomb:
                {
                    GameObject bomb = _spesialPool.Take(b => b.GetComponent<PersistentBomb>().isHeld);
                    
                    if (bomb == null)
                    {
                        Debug.Log("”š’e‚ª‚È‚¢I");
                        return false;
                    }
                    PersistentBomb persistent = bomb.GetComponent<PersistentBomb>();

                    persistent.Put(coord, exploLevel);
                    return true;
                }

            // “§–¾”š’e
            case BombType.TransparentBomb:
                {
                    GameObject bomb = _spesialPool.Take(b => b.GetComponent<NormalBomb>().isHeld);
                    
                    if (bomb == null)
                    {
                        Debug.Log("”š’e‚ª‚È‚¢I");
                        return false;
                    }
                    NormalBomb transparent = bomb.GetComponent<NormalBomb>();

                    transparent.Put(coord, exploLevel);
                    return true;
                }

            // •XŒ‹”š’e
            case BombType.IceBomb:
                {
                    GameObject bomb = _spesialPool.Take(b => b.GetComponent<IceBomb>().isHeld);
                    
                    if (bomb == null)
                    {
                        Debug.Log("”š’e‚ª‚È‚¢I");
                        return false;
                    }
                    IceBomb ice = bomb.GetComponent<IceBomb>();

                    ice.Put(coord, exploLevel);
                    return true;
                }

            // ƒƒKƒgƒ“”š’e
            case BombType.MegatonBomb:
                {
                    GameObject bomb = _spesialPool.Take(b => b.GetComponent<MegatonBomb>().isHeld);
                    
                    if (bomb == null)
                    {
                        Debug.Log("”š’e‚ª‚È‚¢I");
                        return false;
                    }
                    MegatonBomb megaton = bomb.GetComponent<MegatonBomb>();

                    megaton.Put(coord, m_MegatonExploLevel);
                    return true;
                }

            // ŠÑ’Ê”š’e
            case BombType.PierceBomb:
                {
                    GameObject bomb = _spesialPool.Take(b => b.GetComponent<PierceBomb>().isHeld);
                    
                    if (bomb == null)
                    {
                        Debug.Log("”š’e‚ª‚È‚¢I");
                        return false;
                    }
                    PierceBomb pierce = bomb.GetComponent<PierceBomb>();

                    pierce.Put(coord, exploLevel);
                    return true;
                }

            // ‰ñ•œ”š’e
            case BombType.HealBomb:
                {
                    GameObject bomb = _spesialPool.Take(b => b.GetComponent<FogBomb>().isHeld);
                    
                    if (bomb == null)
                    {
                        Debug.Log("”š’e‚ª‚È‚¢I");
                        return false;
                    }
                    FogBomb heal = bomb.GetComponent<FogBomb>();

                    heal.Put(coord, exploLevel);
                    return true;
                }

            // “Å”š’e
            case BombType.PoisonBomb:
                {
                    GameObject bomb = _spesialPool.Take(b => b.GetComponent<FogBomb>().isHeld);
                    
                    if (bomb == null)
                    {
                        Debug.Log("”š’e‚ª‚È‚¢I");
                        return false;
                    }
                    FogBomb poizon = bomb.GetComponent<FogBomb>();

                    poizon.Put(coord, exploLevel);
                    return true;
                }
        }
        return false;
    }

    public void Add(BombType type,GameMap map)
    {
        Debug.Log("‚ ‚Ç");
        switch (type)
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
