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
    // ===�ϐ�====================================================
    [Header("�v�[���̐e")]
    [SerializeField] Transform m_poolParent;

    [Header("�p�����[�^�[")]
    private BombType type;
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
    /// �v�[������ɂ���
    /// </summary>
    /// <param name="type">�ύX����^�C�v</param>
    public void ClearBombType()
    {
        _spesialPool.list.Clear();
    }


    /// <summary>
    /// ���e�^�C�v���w�肵�ē��ꔚ�e��u���܂�
    /// </summary>
    /// <param name="map">�}�b�v���</param>
    /// <param name="playerTrafo">�v���C���[�̃g�����X�t�H�[��</param>
    /// <param name="exploLevel">�������x��</param>
    public bool GenerateSpesialBomb(BombType type,Coord coord,Vector3 dir, int exploLevel)
    {
        switch (type)
        {
            // ��֒e
            case BombType.GrenadeBomb:
                {
                    GameObject bomb = _spesialPool.Take(b => b.GetComponent<GrenadeBomb>().isHeld);
                    
                    if (bomb == null)
                    {
                        Debug.Log("���e���Ȃ��I");
                        return false;
                    }
                    GrenadeBomb grenade = bomb.GetComponent<GrenadeBomb>();

                    grenade.Throw(coord, dir);
                    return true;
                }

            // �n���e
            case BombType.LandmineBomb:
                {
                    GameObject bomb = _spesialPool.Take(b => b.GetComponent<LandmineBomb>().isHeld);
                    
                    if (bomb == null)
                    {
                        Debug.Log("���e���Ȃ��I");
                        return false;
                    }
                    LandmineBomb landmine = bomb.GetComponent<LandmineBomb>();

                    landmine.Put(coord, exploLevel);
                    return true;
                }

            // �N���X���e
            case BombType.CrossBomb:
                {
                    GameObject bomb = _spesialPool.Take(b => b.GetComponent<CrossBomb>().isHeld);
                    
                    if (bomb == null)
                    {
                        Debug.Log("���e���Ȃ��I");
                        return false;
                    }
                    CrossBomb cross = bomb.GetComponent<CrossBomb>();

                    cross.Put(coord, exploLevel);
                    return true;
                }

            // �������e
            case BombType.PersistentBomb:
                {
                    GameObject bomb = _spesialPool.Take(b => b.GetComponent<PersistentBomb>().isHeld);
                    
                    if (bomb == null)
                    {
                        Debug.Log("���e���Ȃ��I");
                        return false;
                    }
                    PersistentBomb persistent = bomb.GetComponent<PersistentBomb>();

                    persistent.Put(coord, exploLevel);
                    return true;
                }

            // �������e
            case BombType.TransparentBomb:
                {
                    GameObject bomb = _spesialPool.Take(b => b.GetComponent<NormalBomb>().isHeld);
                    
                    if (bomb == null)
                    {
                        Debug.Log("���e���Ȃ��I");
                        return false;
                    }
                    NormalBomb transparent = bomb.GetComponent<NormalBomb>();

                    transparent.Put(coord, exploLevel);
                    return true;
                }

            // �X�����e
            case BombType.IceBomb:
                {
                    GameObject bomb = _spesialPool.Take(b => b.GetComponent<IceBomb>().isHeld);
                    
                    if (bomb == null)
                    {
                        Debug.Log("���e���Ȃ��I");
                        return false;
                    }
                    IceBomb ice = bomb.GetComponent<IceBomb>();

                    ice.Put(coord, exploLevel);
                    return true;
                }

            // ���K�g�����e
            case BombType.MegatonBomb:
                {
                    GameObject bomb = _spesialPool.Take(b => b.GetComponent<MegatonBomb>().isHeld);
                    
                    if (bomb == null)
                    {
                        Debug.Log("���e���Ȃ��I");
                        return false;
                    }
                    MegatonBomb megaton = bomb.GetComponent<MegatonBomb>();

                    megaton.Put(coord, m_MegatonExploLevel);
                    return true;
                }

            // �ђʔ��e
            case BombType.PierceBomb:
                {
                    GameObject bomb = _spesialPool.Take(b => b.GetComponent<PierceBomb>().isHeld);
                    
                    if (bomb == null)
                    {
                        Debug.Log("���e���Ȃ��I");
                        return false;
                    }
                    PierceBomb pierce = bomb.GetComponent<PierceBomb>();

                    pierce.Put(coord, exploLevel);
                    return true;
                }

            // �񕜔��e
            case BombType.HealBomb:
                {
                    GameObject bomb = _spesialPool.Take(b => b.GetComponent<FogBomb>().isHeld);
                    
                    if (bomb == null)
                    {
                        Debug.Log("���e���Ȃ��I");
                        return false;
                    }
                    FogBomb heal = bomb.GetComponent<FogBomb>();

                    heal.Put(coord, exploLevel);
                    return true;
                }

            // �Ŕ��e
            case BombType.PoisonBomb:
                {
                    GameObject bomb = _spesialPool.Take(b => b.GetComponent<FogBomb>().isHeld);
                    
                    if (bomb == null)
                    {
                        Debug.Log("���e���Ȃ��I");
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
        Debug.Log("����");
        switch (type)
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
