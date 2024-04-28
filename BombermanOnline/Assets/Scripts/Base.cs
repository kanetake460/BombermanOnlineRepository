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

    // ===�v���p�e�B=================================================
    
    /// <summary>
    /// �Q�[���I�u�W�F�N�g�̃O���b�h���W
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
    /// Vector3���W
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
    /// �O���b�h���W��Vector3���W
    /// </summary>
    public Vector3 CoordPos
    {
        get
        {
            return _map.mapSet.gridField[Coord.x,Coord.z];
        }
    }
    // ===�֐�====================================================
    public void Init(GameMap map)
    {
        _map = map;
    }
}
