using System;
using System.Collections;
using System.Collections.Generic;
using TakeshiLibrary;
using UnityEngine;
using UnityEngine.SocialPlatforms;

[Serializable]
public class SpesialBomb
{
    [Header("ÉvÅ[ÉãÇÃêe")]
    [SerializeField] Transform m_poolParent;

    [Header("ÉpÉâÉÅÅ[É^Å[")]
    [SerializeField] int m_bombMaxValue;
    [SerializeField] int m_MegatonExploLevel;

    [Header("äeéÌÉ{ÉÄ")]
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

    private List<GameObject> spesialPool = new List<GameObject>();

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

    public void Put(BombType type, GameMap map, Transform playerTrafo, int exploLevel)
    {
        Coord coord = map.gridField.GridCoordinate(playerTrafo.position);
        Quaternion dir = FPS.GetFourDirectionEulerAngles(playerTrafo.rotation.eulerAngles);

        switch(type)
        {
            // éËû÷íe
            case BombType.GrenadeBomb:
                {
                    GrenadeBomb bomb = GameObject.Instantiate(grenadeBomb, m_poolParent.transform);

                    bomb.gameObj.SetActive(false);
                    bomb.Initialize(map);
                    bomb.Throw(coord, dir.eulerAngles.normalized);
                    break;
                }

            // ínóãíe
            case BombType.LandmineBomb:
                {
                    LandmineBomb bomb = GameObject.Instantiate(landmineBomb, m_poolParent.transform);

                    bomb.gameObj.SetActive(false);
                    bomb.Initialize(map);
                    bomb.Put(coord,exploLevel);
                    break;
                }

            // ÉäÉÇÅ[Égîöíe
            case BombType.RemoteBomb:
                {
                    LandmineBomb bomb = GameObject.Instantiate(landmineBomb, m_poolParent.transform);

                    bomb.gameObj.SetActive(false);
                    bomb.Initialize(map);
                    bomb.Put(coord, exploLevel);
                    break;
                }

            // ÉNÉçÉXîöíe
            case BombType.CrossBomb:
                {
                    CrossBomb bomb = GameObject.Instantiate(crossBomb, m_poolParent.transform);

                    bomb.gameObj.SetActive(false);
                    bomb.Initialize(map);
                    bomb.Put(coord, exploLevel);
                    break;
                }

            // éùë±îöíe
            case BombType.PersistentBomb:
                {
                    NormalBomb bomb = GameObject.Instantiate(persistentBomb, m_poolParent.transform);

                    bomb.gameObj.SetActive(false);
                    bomb.Initialize(map);
                    bomb.Put(coord, exploLevel);
                    break;
                }

            // ìßñæîöíe
            case BombType.TransparentBomb:
                {
                    NormalBomb bomb = GameObject.Instantiate(transparentBomb, m_poolParent.transform);

                    bomb.gameObj.SetActive(false);
                    bomb.Initialize(map);
                    bomb.Put(coord, exploLevel);
                    break;
                }

            // ïXåãîöíe
            case BombType.IceBomb:
                {
                    IceBomb bomb = GameObject.Instantiate(iceBomb, m_poolParent.transform);

                    bomb.gameObj.SetActive(false);
                    bomb.Initialize(map);
                    bomb.Put(coord, exploLevel);
                    break;
                }

            // ÉÅÉKÉgÉìîöíe
            case BombType.MegatonBomb:
                {
                    MegatonBomb bomb = GameObject.Instantiate(megatonBomb, m_poolParent.transform);

                    bomb.gameObj.SetActive(false);
                    bomb.Initialize(map);
                    bomb.Put(coord, m_MegatonExploLevel);
                    break;
                }

            // ä—í îöíe
            case BombType.PierceBomb:
                {
                    PierceBomb bomb = GameObject.Instantiate(pierceBomb, m_poolParent.transform);

                    bomb.gameObj.SetActive(false);
                    bomb.Initialize(map);
                    bomb.Put(coord, exploLevel);
                    break;
                }

            // âÒïúîöíe
            case BombType.HealBomb:
                {
                    FogBomb bomb = GameObject.Instantiate(healBomb, m_poolParent.transform);

                    bomb.gameObj.SetActive(false);
                    bomb.Initialize(map);
                    bomb.Put(coord, exploLevel);
                    break;
                }

            // ì≈îöíe
            case BombType.PoisonBomb:
                {
                    FogBomb bomb = GameObject.Instantiate(poisonBomb, m_poolParent.transform);

                    bomb.gameObj.SetActive(false);
                    bomb.Initialize(map);
                    bomb.Put(coord, exploLevel);
                    break;
                }


        }
    }



}
