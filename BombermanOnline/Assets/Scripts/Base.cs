using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{
    [SerializeField] protected GameMap _map;
    
    protected Rigidbody _rb;

    [HideInInspector] protected GameObject gameObj;
    [HideInInspector] public Transform trafo;

    protected virtual void Start()
    {
        trafo = gameObject.transform;
        gameObj = gameObject;
        _rb ??= GetComponent<Rigidbody>();
    }

    // ===プロパティ=================================================
    
    /// <summary>
    /// ゲームオブジェクトのグリッド座標
    /// </summary>
    public Coord Coord
    {
        set
        {
            trafo.position = _map.mapSet.gridField[value.x, value.z];
        }
        get
        {
            return _map.mapSet.gridField.GridCoordinate(trafo.position);
        }
    }


    /// <summary>
    /// Vector3座標
    /// </summary>
    public Vector3 Pos
    {
        set
        {
            trafo.position = value;
        }
        get
        {
            return trafo.position;
        }
    }


    /// <summary>
    /// グリッド座標のVector3座標
    /// </summary>
    public Vector3 CoordPos
    {
        get
        {
            return _map.mapSet.gridField[Coord.x,Coord.z];
        }
    }
    // ===関数====================================================
    public void Init(GameMap map)
    {
        _map = map;
    }
}
