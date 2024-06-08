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
                    GrenadeBomb bomb =
                        _spesialPool.Take(b => b.GetComponent<GrenadeBomb>().isHeld).GetComponent<GrenadeBomb>();
                    if (bomb == null)
                    {
                        Debug.Log("���e���Ȃ��I");
                        return;
                    }

                    bomb.Throw(coord, dir);
                    break;
                }

            // �n���e
            case BombType.LandmineBomb:
                {
                    LandmineBomb bomb =
                        _spesialPool.Take(b => b.GetComponent<LandmineBomb>().isHeld).GetComponent<LandmineBomb>();
                    if (bomb == null)
                    {
                        Debug.Log("���e���Ȃ��I");
                        return;
                    }

                    bomb.gameObj.SetActive(false);
                    bomb.Initialize(map);
                    bomb.Put(coord,exploLevel);
                    break;
                }

            // �����[�g���e
            case BombType.RemoteBomb:
                {
                    LandmineBomb bomb = GameObject.Instantiate(landmineBomb, m_poolParent.transform);
                    if (bomb == null)
                    {
                        Debug.Log("���e���Ȃ��I");
                        return;
                    }

                    bomb.gameObj.SetActive(false);
                    bomb.Initialize(map);
                    bomb.Put(coord, exploLevel);
                    break;
                }

            // �N���X���e
            case BombType.CrossBomb:
                {
                    Debug.Log(_spesialPool.Count);
                    GameObject bomb = _spesialPool.Take(b => b.GetComponent<CrossBomb>().isHeld);
                    if (bomb == null)
                    {
                        Debug.Log("���e���Ȃ��I");
                        return;
                    }
                    CrossBomb cross = bomb.GetComponent<CrossBomb>();

                    bomb.SetActive(false);
                    cross.Initialize(map);
                    cross.Put(coord, exploLevel);
                    break;
                }

            // �������e
            case BombType.PersistentBomb:
                {
                    NormalBomb bomb =
                        _spesialPool.Take(b => b.GetComponent<NormalBomb>().isHeld).GetComponent<NormalBomb>();
                    if (bomb == null)
                    {
                        Debug.Log("���e���Ȃ��I");
                        return;
                    }

                    bomb.gameObj.SetActive(false);
                    bomb.Initialize(map);
                    bomb.Put(coord, exploLevel);
                    break;
                }

            // �������e
            case BombType.TransparentBomb:
                {
                    NormalBomb bomb =
                        _spesialPool.Take(b => b.GetComponent<NormalBomb>().isHeld).GetComponent<NormalBomb>();
                    if (bomb == null)
                    {
                        Debug.Log("���e���Ȃ��I");
                        return;
                    }

                    bomb.gameObj.SetActive(false);
                    bomb.Initialize(map);
                    bomb.Put(coord, exploLevel);
                    break;
                }

            // �X�����e
            case BombType.IceBomb:
                {
                    IceBomb bomb =
                        _spesialPool.Take(b => b.GetComponent<IceBomb>().isHeld).GetComponent<IceBomb>();
                    if (bomb == null)
                    {
                        Debug.Log("���e���Ȃ��I");
                        return;
                    }

                    bomb.gameObj.SetActive(false);
                    bomb.Initialize(map);
                    bomb.Put(coord, exploLevel);
                    break;
                }

            // ���K�g�����e
            case BombType.MegatonBomb:
                {
                    MegatonBomb bomb =
                        _spesialPool.Take(b => b.GetComponent<MegatonBomb>().isHeld).GetComponent<MegatonBomb>();
                    if (bomb == null)
                    {
                        Debug.Log("���e���Ȃ��I");
                        return;
                    }

                    bomb.gameObj.SetActive(false);
                    bomb.Initialize(map);
                    bomb.Put(coord, m_MegatonExploLevel);
                    break;
                }

            // �ђʔ��e
            case BombType.PierceBomb:
                {
                    PierceBomb bomb =
                        _spesialPool.Take(b => b.GetComponent<PierceBomb>().isHeld).GetComponent<PierceBomb>();
                    if (bomb == null)
                    {
                        Debug.Log("���e���Ȃ��I");
                        return;
                    }

                    bomb.gameObj.SetActive(false);
                    bomb.Initialize(map);
                    bomb.Put(coord, exploLevel);
                    break;
                }

            // �񕜔��e
            case BombType.HealBomb:
                {
                    FogBomb bomb =
                        _spesialPool.Take(b => b.GetComponent<FogBomb>().isHeld).GetComponent<FogBomb>();
                    if (bomb == null)
                    {
                        Debug.Log("���e���Ȃ��I");
                        return;
                    }

                    bomb.gameObj.SetActive(false);
                    bomb.Initialize(map);
                    bomb.Put(coord, exploLevel);
                    break;
                }

            // �Ŕ��e
            case BombType.PoisonBomb:
                {
                    FogBomb bomb =
                        _spesialPool.Take(b => b.GetComponent<FogBomb>().isHeld).GetComponent<FogBomb>();
                    if (bomb == null)
                    {
                        Debug.Log("���e���Ȃ��I");
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
                    Debug.Log("�N���X");
                    CrossBomb bomb = GameObject.Instantiate(crossBomb.gameObject).GetComponent<CrossBomb>();

                    _spesialPool.Add(bomb.gameObj);
                    bomb.gameObj.SetActive(false);
                    bomb.Initialize(map);
                    break;
                }

            // �������e
            case BombType.PersistentBomb:
                {
                    NormalBomb bomb = GameObject.Instantiate(persistentBomb.gameObject).GetComponent<NormalBomb>();

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
