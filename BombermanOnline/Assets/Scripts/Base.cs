using UnityEngine;
using TakeshiLibrary;
using SoftGear.Strix.Unity.Runtime;
using SoftGear.Strix.Client.Core;
using System.Collections.Generic;
using System.Linq;
using System;
using SoftGear.Strix.Client.Match.Room.Model;

public class Base : StrixBehaviour
{
    [SerializeField] protected GameMap map;
    protected GridField gridField;
    protected Rigidbody rb;
    protected GameManager gameManager;

    [HideInInspector] public GameObject gameObj { get; set; }
    [HideInInspector] public Transform Trafo { get; set; }

    private void Awake()
    {
        Trafo = gameObject.transform;
        gameObj = gameObject;
        rb ??= GetComponent<Rigidbody>();
    }

    // ===�v���p�e�B=================================================
    
    /// <summary>
    /// �Q�[���I�u�W�F�N�g�̃O���b�h���W
    /// </summary>
    public Coord Coord
    {
        set
        {
            Trafo.position = map.m_mapSet.gridField[value.x, value.z];
        }
        get
        {
            return map.m_mapSet.gridField.GridCoordinate(Trafo.position);
        }
    }


    /// <summary>
    /// Vector3���W
    /// </summary>
    public Vector3 Pos
    {
        set
        {
            Trafo.position = value;
        }
        get
        {
            return Trafo.position;
        }
    }

    /// <summary>
    /// Vector3���[�J�����W
    /// </summary>
    public Vector3 LocPos
    {
        set
        {
            Trafo.localPosition = value;
        }
        get
        {
            return Trafo.localPosition; 
        }
    }


    /// <summary>
    /// �O���b�h���W��Vector3���W
    /// </summary>
    public Vector3 CoordPos
    {
        get
        {
            return map.m_mapSet.gridField[Coord.x,Coord.z];
        }
    }


    /// <summary>
    /// �N�H�[�^�j�I�����[�e�C�V����
    /// </summary>
    public Quaternion Rot
    {
        set
        {
            transform.rotation = value;
        }
        get
        {
            return transform.rotation;
        }
    }


    /// <summary>
    /// �N�H�[�^�j�I�����[�J�����[�e�C�V����
    /// </summary>
    public Quaternion LocRot
    {
        set
        {
            transform.localRotation = value;
        }
        get
        {
            return transform.localRotation;
        }
    }



    /// <summary>
    /// �v���C���[��UID
    /// </summary>
    protected UID UID => strixReplicator.ownerUid;

    /// <summary>
    /// �v���C���[�̃C���f�b�N�X
    /// </summary>
    public int PlayerIndex
    {
        get
        {
            for (int i = 0; i < gameManager.RoomMenbers.Count; i++)
            {
                if (UID.ToString() == gameManager.RoomMenbers[i].GetUid().ToString())
                    return i;
            }
            throw new Exception("UID not found in the list");
        }
    }

    /// <param name="map"></param>
    // ===�֐�====================================================
    public virtual void Initialize(GameMap map)
    {
        this.map = map;
    }
}
