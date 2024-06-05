using System;
using System.Collections;
using System.Collections.Generic;
using TakeshiLibrary;
using UnityEngine;
using UnityEngine.SocialPlatforms;

[Serializable]
public class SpesialBomb
{
    [Header("�v�[���̐e")]
    [SerializeField] Transform m_poolParent;

    [Header("�p�����[�^�[")]
    [SerializeField] int m_bombMaxValue;
    [SerializeField] int m_MegatonExploLevel;

    [Header("�e��{��")]
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
            // ��֒e
            case BombType.GrenadeBomb:
                {
                    GrenadeBomb bomb = GameObject.Instantiate(grenadeBomb, m_poolParent.transform);

                    bomb.gameObj.SetActive(false);
                    bomb.Initialize(map);
                    bomb.Throw(coord, dir.eulerAngles.normalized);
                    break;
                }

            // �n���e
            case BombType.LandmineBomb:
                {
                    LandmineBomb bomb = GameObject.Instantiate(landmineBomb, m_poolParent.transform);

                    bomb.gameObj.SetActive(false);
                    bomb.Initialize(map);
                    bomb.Put(coord,exploLevel);
                    break;
                }

            // �����[�g���e
            case BombType.RemoteBomb:
                {
                    LandmineBomb bomb = GameObject.Instantiate(landmineBomb, m_poolParent.transform);

                    bomb.gameObj.SetActive(false);
                    bomb.Initialize(map);
                    bomb.Put(coord, exploLevel);
                    break;
                }

            // �N���X���e
            case BombType.CrossBomb:
                {
                    CrossBomb bomb = GameObject.Instantiate(crossBomb, m_poolParent.transform);

                    bomb.gameObj.SetActive(false);
                    bomb.Initialize(map);
                    bomb.Put(coord, exploLevel);
                    break;
                }

            // �������e
            case BombType.PersistentBomb:
                {
                    NormalBomb bomb = GameObject.Instantiate(persistentBomb, m_poolParent.transform);

                    bomb.gameObj.SetActive(false);
                    bomb.Initialize(map);
                    bomb.Put(coord, exploLevel);
                    break;
                }

            // �������e
            case BombType.TransparentBomb:
                {
                    NormalBomb bomb = GameObject.Instantiate(transparentBomb, m_poolParent.transform);

                    bomb.gameObj.SetActive(false);
                    bomb.Initialize(map);
                    bomb.Put(coord, exploLevel);
                    break;
                }

            // �X�����e
            case BombType.IceBomb:
                {
                    IceBomb bomb = GameObject.Instantiate(iceBomb, m_poolParent.transform);

                    bomb.gameObj.SetActive(false);
                    bomb.Initialize(map);
                    bomb.Put(coord, exploLevel);
                    break;
                }

            // ���K�g�����e
            case BombType.MegatonBomb:
                {
                    MegatonBomb bomb = GameObject.Instantiate(megatonBomb, m_poolParent.transform);

                    bomb.gameObj.SetActive(false);
                    bomb.Initialize(map);
                    bomb.Put(coord, m_MegatonExploLevel);
                    break;
                }

            // �ђʔ��e
            case BombType.PierceBomb:
                {
                    PierceBomb bomb = GameObject.Instantiate(pierceBomb, m_poolParent.transform);

                    bomb.gameObj.SetActive(false);
                    bomb.Initialize(map);
                    bomb.Put(coord, exploLevel);
                    break;
                }

            // �񕜔��e
            case BombType.HealBomb:
                {
                    FogBomb bomb = GameObject.Instantiate(healBomb, m_poolParent.transform);

                    bomb.gameObj.SetActive(false);
                    bomb.Initialize(map);
                    bomb.Put(coord, exploLevel);
                    break;
                }

            // �Ŕ��e
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
