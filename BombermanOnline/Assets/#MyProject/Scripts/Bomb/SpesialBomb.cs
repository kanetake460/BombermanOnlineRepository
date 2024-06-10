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
    // ===�ϐ�====================================================
    [Header("�v�[���̐e")]
    [SerializeField] Transform m_poolParent;

    [Header("�p�����[�^�[")]
    private BombType _type;
    [SerializeField] int m_spesialBombMaxCount;
    [SerializeField] int m_bombMaxValue;
    [SerializeField] int m_MegatonExploLevel;

    [Header("�e��{��")]
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

    // ===�֐�====================================================
    /// <summary>
    /// �{����ύX���܂��B
    /// �v�[������x��ɂ���
    /// </summary>
    /// <param name="type">�ύX����^�C�v</param>
    public void SetBombType(BombType type)
    {
        _type = type;
        _spesialPool.list.Clear();
    }


    /// <summary>
    /// ���e�^�C�v���w�肵�ē��ꔚ�e��u���܂�
    /// </summary>
    /// <param name="map">�}�b�v���</param>
    /// <param name="playerTrafo">�v���C���[�̃g�����X�t�H�[��</param>
    /// <param name="exploLevel">�������x��</param>
    public void Put(GameMap map, Transform playerTrafo, int exploLevel)
    {
        Coord coord = map.gridField.GridCoordinate(playerTrafo.position);
        Vector3 dir = FPS.GetVector3FourDirection(playerTrafo.rotation.eulerAngles);
        switch (_type)
        {
            // ��֒e
            case BombType.GrenadeBomb:
                {
                    GameObject bomb = _spesialPool.Take(b => b.GetComponent<GrenadeBomb>().isHeld);
                    
                    if (bomb == null)
                    {
                        Debug.Log("���e���Ȃ��I");
                        return;
                    }
                    GrenadeBomb grenade = bomb.GetComponent<GrenadeBomb>();

                    grenade.Initialize(map);
                    grenade.Throw(coord, dir);
                    break;
                }

            // �n���e
            case BombType.LandmineBomb:
                {
                    GameObject bomb = _spesialPool.Take(b => b.GetComponent<LandmineBomb>().isHeld);
                    
                    if (bomb == null)
                    {
                        Debug.Log("���e���Ȃ��I");
                        return;
                    }
                    LandmineBomb landmine = bomb.GetComponent<LandmineBomb>();

                    landmine.Initialize(map);
                    landmine.Put(coord,exploLevel);
                    break;
                }

            // �N���X���e
            case BombType.CrossBomb:
                {
                    GameObject bomb = _spesialPool.Take(b => b.GetComponent<CrossBomb>().isHeld);
                    
                    if (bomb == null)
                    {
                        Debug.Log("���e���Ȃ��I");
                        return;
                    }
                    CrossBomb cross = bomb.GetComponent<CrossBomb>();

                    cross.Initialize(map);
                    cross.Put(coord, exploLevel);
                    break;
                }

            // �������e
            case BombType.PersistentBomb:
                {
                    GameObject bomb = _spesialPool.Take(b => b.GetComponent<PersistentBomb>().isHeld);
                    
                    if (bomb == null)
                    {
                        Debug.Log("���e���Ȃ��I");
                        return;
                    }
                    PersistentBomb persistent = bomb.GetComponent<PersistentBomb>();

                    persistent.Initialize(map);
                    persistent.Put(coord, exploLevel);
                    break;
                }

            // �������e
            case BombType.TransparentBomb:
                {
                    GameObject bomb = _spesialPool.Take(b => b.GetComponent<NormalBomb>().isHeld);
                    
                    if (bomb == null)
                    {
                        Debug.Log("���e���Ȃ��I");
                        return;
                    }
                    NormalBomb transparent = bomb.GetComponent<NormalBomb>();

                    transparent.Initialize(map);
                    transparent.Put(coord, exploLevel);
                    break;
                }

            // �X�����e
            case BombType.IceBomb:
                {
                    GameObject bomb = _spesialPool.Take(b => b.GetComponent<IceBomb>().isHeld);
                    
                    if (bomb == null)
                    {
                        Debug.Log("���e���Ȃ��I");
                        return;
                    }
                    IceBomb ice = bomb.GetComponent<IceBomb>();

                    ice.Initialize(map);
                    ice.Put(coord, exploLevel);
                    break;
                }

            // ���K�g�����e
            case BombType.MegatonBomb:
                {
                    GameObject bomb = _spesialPool.Take(b => b.GetComponent<MegatonBomb>().isHeld);
                    
                    if (bomb == null)
                    {
                        Debug.Log("���e���Ȃ��I");
                        return;
                    }
                    MegatonBomb megaton = bomb.GetComponent<MegatonBomb>();

                    megaton.Initialize(map);
                    megaton.Put(coord, m_MegatonExploLevel);
                    break;
                }

            // �ђʔ��e
            case BombType.PierceBomb:
                {
                    GameObject bomb = _spesialPool.Take(b => b.GetComponent<PierceBomb>().isHeld);
                    
                    if (bomb == null)
                    {
                        Debug.Log("���e���Ȃ��I");
                        return;
                    }
                    PierceBomb pierce = bomb.GetComponent<PierceBomb>();

                    pierce.Initialize(map);
                    pierce.Put(coord, exploLevel);
                    break;
                }

            // �񕜔��e
            case BombType.HealBomb:
                {
                    GameObject bomb = _spesialPool.Take(b => b.GetComponent<FogBomb>().isHeld);
                    
                    if (bomb == null)
                    {
                        Debug.Log("���e���Ȃ��I");
                        return;
                    }
                    FogBomb heal = bomb.GetComponent<FogBomb>();

                    heal.Initialize(map);
                    heal.Put(coord, exploLevel);
                    break;
                }

            // �Ŕ��e
            case BombType.PoisonBomb:
                {
                    GameObject bomb = _spesialPool.Take(b => b.GetComponent<FogBomb>().isHeld);
                    
                    if (bomb == null)
                    {
                        Debug.Log("���e���Ȃ��I");
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
        Debug.Log("����");
        switch (_type)
        {
            // ��֒e
            case BombType.GrenadeBomb:
                {
                    GrenadeBomb bomb = GameObject.Instantiate(grenadeBomb.gameObject).GetComponent<GrenadeBomb>();
                    
                    _spesialPool.Add(bomb.gameObj);
                    bomb.gameObj.SetActive(false);
                    bomb.Initialize(map);
                    break;
                }

            // �n���e
            case BombType.LandmineBomb:
                {
                    LandmineBomb bomb = GameObject.Instantiate(landmineBomb.gameObject).GetComponent<LandmineBomb>();

                    _spesialPool.Add(bomb.gameObj);
                    bomb.gameObj.SetActive(false);
                    bomb.Initialize(map);
                    break;
                }

            // �����[�g���e
            case BombType.RemoteBomb:
                {
                    GrenadeBomb bomb = GameObject.Instantiate(grenadeBomb.gameObject).GetComponent<GrenadeBomb>();

                    _spesialPool.Add(bomb.gameObj);
                    bomb.gameObj.SetActive(false);
                    bomb.Initialize(map);
                    break;
                }

            // �N���X���e
            case BombType.CrossBomb:
                {
                    CrossBomb bomb = GameObject.Instantiate(crossBomb.gameObject).GetComponent<CrossBomb>();

                    _spesialPool.Add(bomb.gameObj);
                    bomb.gameObj.SetActive(false);
                    bomb.Initialize(map);
                    break;
                }

            // �������e
            case BombType.PersistentBomb:
                {
                    PersistentBomb bomb = GameObject.Instantiate(persistentBomb.gameObject).GetComponent<PersistentBomb>();

                    _spesialPool.Add(bomb.gameObj);
                    bomb.gameObj.SetActive(false);
                    bomb.Initialize(map);
                    break;
                }

            // �������e
            case BombType.TransparentBomb:
                {
                    NormalBomb bomb = GameObject.Instantiate(transparentBomb.gameObject).GetComponent<NormalBomb>();

                    _spesialPool.Add(bomb.gameObj);
                    bomb.gameObj.SetActive(false);
                    bomb.Initialize(map);
                    break;
                }

            // �X�����e
            case BombType.IceBomb:
                {
                    IceBomb bomb = GameObject.Instantiate(iceBomb.gameObject).GetComponent<IceBomb>();

                    _spesialPool.Add(bomb.gameObj);
                    bomb.gameObj.SetActive(false);
                    bomb.Initialize(map);
                    break;
                }

            // ���K�g�����e
            case BombType.MegatonBomb:
                {
                    MegatonBomb bomb = GameObject.Instantiate(megatonBomb.gameObject).GetComponent<MegatonBomb>();

                    _spesialPool.Add(bomb.gameObj);
                    bomb.gameObj.SetActive(false);
                    bomb.Initialize(map);
                    break;
                }

            // �ђʔ��e
            case BombType.PierceBomb:
                {
                    PierceBomb bomb = GameObject.Instantiate(pierceBomb.gameObject).GetComponent<PierceBomb>();

                    _spesialPool.Add(bomb.gameObj);
                    bomb.gameObj.SetActive(false);
                    bomb.Initialize(map);
                    break;
                }

            // �񕜔��e
            case BombType.HealBomb:
                {
                    FogBomb bomb = GameObject.Instantiate(healBomb.gameObject).GetComponent<FogBomb>();

                    _spesialPool.Add(bomb.gameObj);
                    bomb.gameObj.SetActive(false);
                    bomb.Initialize(map);
                    break;
                }

            // �Ŕ��e
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
