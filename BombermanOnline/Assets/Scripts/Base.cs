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
            return map.mapSet.gridField[Coord.x,Coord.z];
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
    // ===�֐�====================================================
    public virtual void Initialize(GameMap map)
    {
        this.map = map;
    }
}
