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
    [Header("�v�[���̐e")]
    [SerializeField] Transform m_poolParent;

    [Header("�p�����[�^�[")]
    [SerializeField] int m_spesialBombMaxCount;
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
    /// ���e�^�C�v���w�肵�ē��ꔚ�e��u���܂�
    /// </summary>
    /// <param name="type">�^�C�v</param>
    /// <param name="map">�}�b�v���</param>
    /// <param name="playerTrafo">�v���C���[�̃g�����X�t�H�[��</param>
    /// <param name="exploLevel">�������x��</param>
    public void Put(BombType type, GameMap map, Transform playerTrafo, int exploLevel)
    {
        Coord coord = map.gridField.GridCoordinate(playerTrafo.position);
        Vector3 dir = FPS.GetVector3FourDirection(playerTrafo.rotation.eulerAngles);

        switch(type)
        {
            // ��֒e
            case BombType.GrenadeBomb:
                {
                    GrenadeBomb bomb = _spesialPool.Get(b => b.GetComponent<GrenadeBomb>().isHeld,
                        () => GameObject.Instantiate(grenadeBomb.gameObject)).GetComponent<GrenadeBomb>(); ;

                    bomb.gameObj.SetActive(false);
                    bomb.Initialize(map);
                    bomb.Throw(coord, dir);
                    break;
                }

            // �n���e
            case BombType.LandmineBomb:
                {
                    LandmineBomb bomb  = _spesialPool.Get(b => b.GetComponent<LandmineBomb>().isHeld,
                        () => GameObject.Instantiate(landmineBomb.gameObject)).GetComponent<LandmineBomb>();

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
                    CrossBomb bomb = _spesialPool.Get(b => b.GetComponent<CrossBomb>().isHeld,
                        () => GameObject.Instantiate(crossBomb.gameObject)).GetComponent<CrossBomb>();

                    bomb.gameObj.SetActive(false);
                    bomb.Initialize(map);
                    bomb.Put(coord, exploLevel);
                    break;
                }

            // �������e
            case BombType.PersistentBomb:
                {
                    NormalBomb bomb = _spesialPool.Get(b => b.GetComponent<NormalBomb>().isHeld,
                        () => GameObject.Instantiate(persistentBomb.gameObject)).GetComponent<NormalBomb>();

                    bomb.gameObj.SetActive(false);
                    bomb.Initialize(map);
                    bomb.Put(coord, exploLevel);
                    break;
                }

            // �������e
            case BombType.TransparentBomb:
                {
                    NormalBomb bomb = _spesialPool.Get(b => b.GetComponent<NormalBomb>().isHeld,
                        () => GameObject.Instantiate(transparentBomb.gameObject)).GetComponent<NormalBomb>();

                    bomb.gameObj.SetActive(false);
                    bomb.Initialize(map);
                    bomb.Put(coord, exploLevel);
                    break;
                }

            // �X�����e
            case BombType.IceBomb:
                {
                    IceBomb bomb = _spesialPool.Get(b => b.GetComponent<IceBomb>().isHeld,
                        () => GameObject.Instantiate(iceBomb.gameObject)).GetComponent<IceBomb>();

                    bomb.gameObj.SetActive(false);
                    bomb.Initialize(map);
                    bomb.Put(coord, exploLevel);
                    break;
                }

            // ���K�g�����e
            case BombType.MegatonBomb:
                {
                    MegatonBomb bomb = _spesialPool.Get(b => b.GetComponent<MegatonBomb>().isHeld,
                        () => GameObject.Instantiate(megatonBomb.gameObject)).GetComponent<MegatonBomb>();
                    
                    bomb.gameObj.SetActive(false);
                    bomb.Initialize(map);
                    bomb.Put(coord, m_MegatonExploLevel);
                    break;
                }

            // �ђʔ��e
            case BombType.PierceBomb:
                {
                    PierceBomb bomb = _spesialPool.Get(b => b.GetComponent<PierceBomb>().isHeld,
                        () => GameObject.Instantiate(pierceBomb.gameObject)).GetComponent<PierceBomb>();

                    bomb.gameObj.SetActive(false);
                    bomb.Initialize(map);
                    bomb.Put(coord, exploLevel);
                    break;
                }

            // �񕜔��e
            case BombType.HealBomb:
                {
                    FogBomb bomb = _spesialPool.Get(b => b.GetComponent<FogBomb>().isHeld,
                        () => GameObject.Instantiate(healBomb.gameObject)).GetComponent<FogBomb>();

                    bomb.gameObj.SetActive(false);
                    bomb.Initialize(map);
                    bomb.Put(coord, exploLevel);
                    break;
                }

            // �Ŕ��e
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
