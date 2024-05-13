using UnityEngine;
using TakeshiLibrary;
using SoftGear.Strix.Unity.Runtime;

public class Base : StrixBehaviour
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
        //map = GameMap.Instance;
    }

    // ===プロパティ=================================================
    
    /// <summary>
    /// ゲームオブジェクトのグリッド座標
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
            return map.mapSet.gridField[Coord.x,Coord.z];
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
    // ===関数====================================================
    public virtual void Initialize(GameMap map)
    {
        this.map = map;
    }
}
