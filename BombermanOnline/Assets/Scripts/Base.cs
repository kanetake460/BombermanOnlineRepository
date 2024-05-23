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

    // ===プロパティ=================================================
    
    /// <summary>
    /// ゲームオブジェクトのグリッド座標
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
    /// Vector3座標
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
    /// Vector3ローカル座標
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
    /// グリッド座標のVector3座標
    /// </summary>
    public Vector3 CoordPos
    {
        get
        {
            return map.m_mapSet.gridField[Coord.x,Coord.z];
        }
    }


    /// <summary>
    /// クォータニオンローテイション
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
    /// クォータニオンローカルローテイション
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
    /// プレイヤーのUID
    /// </summary>
    protected UID UID => strixReplicator.ownerUid;

    /// <summary>
    /// プレイヤーのインデックス
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
    // ===関数====================================================
    public virtual void Initialize(GameMap map)
    {
        this.map = map;
    }
}
