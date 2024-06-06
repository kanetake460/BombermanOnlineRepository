using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TakeshiLibrary;
using UnityEngine;
using UnityEngine.SocialPlatforms;

[Serializable]
public class SpesialBomb
{
    [Header("プールの親")]
    [SerializeField] Transform m_poolParent;

    [Header("パラメーター")]
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


    /// <summary>
    /// 爆弾タイプを指定して特殊爆弾を置きます
    /// </summary>
    /// <param name="type">タイプ</param>
    /// <param name="map">マップ情報</param>
    /// <param name="playerTrafo">プレイヤーのトランスフォーム</param>
    /// <param name="exploLevel">爆発レベル</param>
    public void Put(BombType type, GameMap map, Transform playerTrafo, int exploLevel)
    {
        Coord coord = map.gridField.GridCoordinate(playerTrafo.position);
        Vector3 dir = FPS.GetVector3FourDirection(playerTrafo.rotation.eulerAngles);

        switch(type)
        {
            // 手榴弾
            case BombType.GrenadeBomb:
                {
                    GrenadeBomb bomb = _spesialPool.Get(b => b.GetComponent<GrenadeBomb>().isHeld,
                        () => GameObject.Instantiate(grenadeBomb.gameObject)).GetComponent<GrenadeBomb>(); ;

                    bomb.gameObj.SetActive(false);
                    bomb.Initialize(map);
                    bomb.Throw(coord, dir);
                    break;
                }

            // 地雷弾
            case BombType.LandmineBomb:
                {
                    LandmineBomb bomb  = _spesialPool.Get(b => b.GetComponent<LandmineBomb>().isHeld,
                        () => GameObject.Instantiate(landmineBomb.gameObject)).GetComponent<LandmineBomb>();

                    bomb.gameObj.SetActive(false);
                    bomb.Initialize(map);
                    bomb.Put(coord,exploLevel);
                    break;
                }

            // リモート爆弾
            case BombType.RemoteBomb:
                {
                    LandmineBomb bomb = GameObject.Instantiate(landmineBomb, m_poolParent.transform);

                    bomb.gameObj.SetActive(false);
                    bomb.Initialize(map);
                    bomb.Put(coord, exploLevel);
                    break;
                }

            // クロス爆弾
            case BombType.CrossBomb:
                {
                    CrossBomb bomb = _spesialPool.Get(b => b.GetComponent<CrossBomb>().isHeld,
                        () => GameObject.Instantiate(crossBomb.gameObject)).GetComponent<CrossBomb>();

                    bomb.gameObj.SetActive(false);
                    bomb.Initialize(map);
                    bomb.Put(coord, exploLevel);
                    break;
                }

            // 持続爆弾
            case BombType.PersistentBomb:
                {
                    NormalBomb bomb = _spesialPool.Get(b => b.GetComponent<NormalBomb>().isHeld,
                        () => GameObject.Instantiate(persistentBomb.gameObject)).GetComponent<NormalBomb>();

                    bomb.gameObj.SetActive(false);
                    bomb.Initialize(map);
                    bomb.Put(coord, exploLevel);
                    break;
                }

            // 透明爆弾
            case BombType.TransparentBomb:
                {
                    NormalBomb bomb = _spesialPool.Get(b => b.GetComponent<NormalBomb>().isHeld,
                        () => GameObject.Instantiate(transparentBomb.gameObject)).GetComponent<NormalBomb>();

                    bomb.gameObj.SetActive(false);
                    bomb.Initialize(map);
                    bomb.Put(coord, exploLevel);
                    break;
                }

            // 氷結爆弾
            case BombType.IceBomb:
                {
                    IceBomb bomb = _spesialPool.Get(b => b.GetComponent<IceBomb>().isHeld,
                        () => GameObject.Instantiate(iceBomb.gameObject)).GetComponent<IceBomb>();

                    bomb.gameObj.SetActive(false);
                    bomb.Initialize(map);
                    bomb.Put(coord, exploLevel);
                    break;
                }

            // メガトン爆弾
            case BombType.MegatonBomb:
                {
                    MegatonBomb bomb = _spesialPool.Get(b => b.GetComponent<MegatonBomb>().isHeld,
                        () => GameObject.Instantiate(megatonBomb.gameObject)).GetComponent<MegatonBomb>();
                    
                    bomb.gameObj.SetActive(false);
                    bomb.Initialize(map);
                    bomb.Put(coord, m_MegatonExploLevel);
                    break;
                }

            // 貫通爆弾
            case BombType.PierceBomb:
                {
                    PierceBomb bomb = _spesialPool.Get(b => b.GetComponent<PierceBomb>().isHeld,
                        () => GameObject.Instantiate(pierceBomb.gameObject)).GetComponent<PierceBomb>();

                    bomb.gameObj.SetActive(false);
                    bomb.Initialize(map);
                    bomb.Put(coord, exploLevel);
                    break;
                }

            // 回復爆弾
            case BombType.HealBomb:
                {
                    FogBomb bomb = _spesialPool.Get(b => b.GetComponent<FogBomb>().isHeld,
                        () => GameObject.Instantiate(healBomb.gameObject)).GetComponent<FogBomb>();

                    bomb.gameObj.SetActive(false);
                    bomb.Initialize(map);
                    bomb.Put(coord, exploLevel);
                    break;
                }

            // 毒爆弾
            case BombType.PoisonBomb:
                {
                    FogBomb bomb = _spesialPool.Get(b => b.GetComponent<FogBomb>().isHeld,
                        () => GameObject.Instantiate(poisonBomb.gameObject)).GetComponent<FogBomb>();

                    bomb.gameObj.SetActive(false);
                    bomb.Initialize(map);
                    bomb.Put(coord, exploLevel);
                    break;
                }
        }
    }




}
