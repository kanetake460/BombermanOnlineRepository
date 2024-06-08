using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TakeshiLibrary;
using UnityEngine;
using UnityEngine.SocialPlatforms;

[Serializable]
public class SpesialBomb
{
    // ===変数====================================================
    [Header("プールの親")]
    [SerializeField] Transform m_poolParent;

    [Header("パラメーター")]
    private BombType _type;
    [SerializeField] int m_spesialBombMaxCount;
    [SerializeField] int m_bombMaxValue;
    [SerializeField] int m_MegatonExploLevel;

    [Header("各種ボム")]
    [SerializeField] GrenadeBomb grenadeBomb;
    [SerializeField] LandmineBomb landmineBomb;
    [SerializeField] NormalBomb RemoteBomb;
    [SerializeField] CrossBomb crossBomb;
    [SerializeField] NormalBomb persistentBomb;
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
    /// ボムを変更します。
    /// プールを一度空にする
    /// </summary>
    /// <param name="type">変更するタイプ</param>
    public void SetBombType(BombType type)
    {
        _type = type;
        _spesialPool.list.Clear();
    }


    /// <summary>
    /// 爆弾タイプを指定して特殊爆弾を置きます
    /// </summary>
    /// <param name="map">マップ情報</param>
    /// <param name="playerTrafo">プレイヤーのトランスフォーム</param>
    /// <param name="exploLevel">爆発レベル</param>
    public void Put(GameMap map, Transform playerTrafo, int exploLevel)
    {
        Coord coord = map.gridField.GridCoordinate(playerTrafo.position);
        Vector3 dir = FPS.GetVector3FourDirection(playerTrafo.rotation.eulerAngles);
        switch (_type)
        {
            // 手榴弾
            case BombType.GrenadeBomb:
                {
                    GrenadeBomb bomb =
                        _spesialPool.Take(b => b.GetComponent<GrenadeBomb>().isHeld).GetComponent<GrenadeBomb>();
                    if (bomb == null)
                    {
                        Debug.Log("爆弾がない！");
                        return;
                    }

                    bomb.Throw(coord, dir);
                    break;
                }

            // 地雷弾
            case BombType.LandmineBomb:
                {
                    LandmineBomb bomb =
                        _spesialPool.Take(b => b.GetComponent<LandmineBomb>().isHeld).GetComponent<LandmineBomb>();
                    if (bomb == null)
                    {
                        Debug.Log("爆弾がない！");
                        return;
                    }

                    bomb.gameObj.SetActive(false);
                    bomb.Initialize(map);
                    bomb.Put(coord,exploLevel);
                    break;
                }

            // リモート爆弾
            case BombType.RemoteBomb:
                {
                    LandmineBomb bomb = GameObject.Instantiate(landmineBomb, m_poolParent.transform);
                    if (bomb == null)
                    {
                        Debug.Log("爆弾がない！");
                        return;
                    }

                    bomb.gameObj.SetActive(false);
                    bomb.Initialize(map);
                    bomb.Put(coord, exploLevel);
                    break;
                }

            // クロス爆弾
            case BombType.CrossBomb:
                {
                    Debug.Log(_spesialPool.Count);
                    GameObject bomb = _spesialPool.Take(b => b.GetComponent<CrossBomb>().isHeld);
                    if (bomb == null)
                    {
                        Debug.Log("爆弾がない！");
                        return;
                    }
                    CrossBomb cross = bomb.GetComponent<CrossBomb>();

                    bomb.SetActive(false);
                    cross.Initialize(map);
                    cross.Put(coord, exploLevel);
                    break;
                }

            // 持続爆弾
            case BombType.PersistentBomb:
                {
                    NormalBomb bomb =
                        _spesialPool.Take(b => b.GetComponent<NormalBomb>().isHeld).GetComponent<NormalBomb>();
                    if (bomb == null)
                    {
                        Debug.Log("爆弾がない！");
                        return;
                    }

                    bomb.gameObj.SetActive(false);
                    bomb.Initialize(map);
                    bomb.Put(coord, exploLevel);
                    break;
                }

            // 透明爆弾
            case BombType.TransparentBomb:
                {
                    NormalBomb bomb =
                        _spesialPool.Take(b => b.GetComponent<NormalBomb>().isHeld).GetComponent<NormalBomb>();
                    if (bomb == null)
                    {
                        Debug.Log("爆弾がない！");
                        return;
                    }

                    bomb.gameObj.SetActive(false);
                    bomb.Initialize(map);
                    bomb.Put(coord, exploLevel);
                    break;
                }

            // 氷結爆弾
            case BombType.IceBomb:
                {
                    IceBomb bomb =
                        _spesialPool.Take(b => b.GetComponent<IceBomb>().isHeld).GetComponent<IceBomb>();
                    if (bomb == null)
                    {
                        Debug.Log("爆弾がない！");
                        return;
                    }

                    bomb.gameObj.SetActive(false);
                    bomb.Initialize(map);
                    bomb.Put(coord, exploLevel);
                    break;
                }

            // メガトン爆弾
            case BombType.MegatonBomb:
                {
                    MegatonBomb bomb =
                        _spesialPool.Take(b => b.GetComponent<MegatonBomb>().isHeld).GetComponent<MegatonBomb>();
                    if (bomb == null)
                    {
                        Debug.Log("爆弾がない！");
                        return;
                    }

                    bomb.gameObj.SetActive(false);
                    bomb.Initialize(map);
                    bomb.Put(coord, m_MegatonExploLevel);
                    break;
                }

            // 貫通爆弾
            case BombType.PierceBomb:
                {
                    PierceBomb bomb =
                        _spesialPool.Take(b => b.GetComponent<PierceBomb>().isHeld).GetComponent<PierceBomb>();
                    if (bomb == null)
                    {
                        Debug.Log("爆弾がない！");
                        return;
                    }

                    bomb.gameObj.SetActive(false);
                    bomb.Initialize(map);
                    bomb.Put(coord, exploLevel);
                    break;
                }

            // 回復爆弾
            case BombType.HealBomb:
                {
                    FogBomb bomb =
                        _spesialPool.Take(b => b.GetComponent<FogBomb>().isHeld).GetComponent<FogBomb>();
                    if (bomb == null)
                    {
                        Debug.Log("爆弾がない！");
                        return;
                    }

                    bomb.gameObj.SetActive(false);
                    bomb.Initialize(map);
                    bomb.Put(coord, exploLevel);
                    break;
                }

            // 毒爆弾
            case BombType.PoisonBomb:
                {
                    FogBomb bomb =
                        _spesialPool.Take(b => b.GetComponent<FogBomb>().isHeld).GetComponent<FogBomb>();
                    if (bomb == null)
                    {
                        Debug.Log("爆弾がない！");
                        return;
                    }

                    bomb.gameObj.SetActive(false);
                    bomb.Initialize(map);
                    bomb.Put(coord, exploLevel);
                    break;
                }
        }
    }

    public void Add(GameMap map)
    {
        Debug.Log("あど");
        switch (_type)
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
                    Debug.Log("クロス");
                    CrossBomb bomb = GameObject.Instantiate(crossBomb.gameObject).GetComponent<CrossBomb>();

                    _spesialPool.Add(bomb.gameObj);
                    bomb.gameObj.SetActive(false);
                    bomb.Initialize(map);
                    break;
                }

            // 持続爆弾
            case BombType.PersistentBomb:
                {
                    NormalBomb bomb = GameObject.Instantiate(persistentBomb.gameObject).GetComponent<NormalBomb>();

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
