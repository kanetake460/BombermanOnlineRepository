using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TakeshiLibrary;

public class Base : MonoBehaviour
{
    [SerializeField] protected GameMap map;
    protected GridField gridField;
    protected Rigidbody rb;

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
            Trafo.position = map.mapSet.gridField[value.x, value.z];
        }
        get
        {
            return map.mapSet.gridField.GridCoordinate(Trafo.position);
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
    /// �O���b�h���W��Vector3���W
    /// </summary>
    public Vector3 CoordPos
    {
        get
        {
            return map.mapSet.gridField[Coord.x,Coord.z];
        }
    }
    // ===�֐�====================================================
    public virtual void Initialize(GameMap map)
    {
        this.map = map;
    }
}