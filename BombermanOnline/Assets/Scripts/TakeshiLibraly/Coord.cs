using System;
using UnityEngine;
using Unity.VisualScripting;

#if UNITY_EDITOR
using UnityEditor;
#endif

[Serializable]
public struct Coord
{
    [SerializeField] private int m_X;
    [SerializeField] private int m_Z;

    public int x
    {
        get => m_X; set => m_X = value;
    }

    public int z
    {
        get => m_Z; set => m_Z = value;
    }


    private static readonly Coord s_Zero = new Coord(0, 0);

    private static readonly Coord s_One = new Coord(1, 1);

    private static readonly Coord s_Left = new Coord(-1, 0);

    private static readonly Coord s_Right = new Coord(1, 0);

    private static readonly Coord s_Forward = new Coord(0, 1);

    private static readonly Coord s_Back = new Coord(0, -1);

    public static Coord zero { get { return s_Zero; } }
    public static Coord one { get { return s_One; } }
    public static Coord left { get { return s_Left; } }
    public static Coord right { get { return s_Right; } }
    public static Coord forward { get { return s_Forward; } }
    public static Coord back { get { return s_Back; } }

    public int this[int index]
    {
        get
        {
            return index switch
            {
                0 => m_X,
                1 => m_Z,
                _ => throw new IndexOutOfRangeException("Invalid Coord index!"),
            };
        }
        set
        {
            switch (index)
            {
                case 0:
                    m_X = value;
                    break;
                case 1:
                    m_Z = value;
                    break;
                default:
                    throw new IndexOutOfRangeException("Invalid Coord index!");
            }
        }
    }

    public Coord(int x, int z)
    {
        m_X = x;
        m_Z = z;
    }

    public static Coord operator +(Coord a, Coord b)
    {
        return new Coord(a.x + b.x, a.z + b.z);
    }

    public static Coord operator -(Coord a, Coord b)
    {
        return new Coord(a.x - b.x, a.z - b.z);
    }

    public static Coord operator *(Coord a, Coord b)
    {
        return new Coord(a.x * b.x, a.z * b.z);
    }

    public static Coord operator -(Coord a)
    {
        return new Coord(-a.x, -a.z);
    }

    public static Coord operator *(Coord a, int b)
    {
        return new Coord(a.x * b, a.z * b);
    }

    public static Coord operator *(int a, Coord b)
    {
        return new Coord(a * b.x, a * b.z);
    }

    public static Coord operator /(Coord a, int b)
    {
        return new Coord(a.x / b, a.z / b);
    }

    public static bool operator ==(Coord lhs, Coord rhs)
    {
        return lhs.x == rhs.x && lhs.z == rhs.z;
    }

    public static bool operator !=(Coord lhs, Coord rhs)
    {
        return !(lhs == rhs);
    }

    public static bool Equal(Coord lhs, Coord rhs) => lhs == rhs;


#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(Coord))]
    public class CoordDrawer : PropertyDrawer
    {
        static readonly GUIContent X_LABEL = new GUIContent("X");
        static readonly GUIContent Z_LABEL = new GUIContent("Z");

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var xProperty = property.FindPropertyRelative("m_X");
            var zProperty = property.FindPropertyRelative("m_Z");

            // 名前
            label = EditorGUI.BeginProperty(position, label, property);
            Rect contentPosition = EditorGUI.PrefixLabel(position, label);

            // ラベル
            contentPosition.width *= 1f / 2f;
            EditorGUI.indentLevel = 0;
            EditorGUIUtility.labelWidth = 15f;

            // 各要素
            EditorGUI.PropertyField(contentPosition, xProperty, X_LABEL);
            contentPosition.x += contentPosition.width;

            EditorGUI.PropertyField(contentPosition, zProperty, Z_LABEL);
            contentPosition.x += contentPosition.width;

            EditorGUI.EndProperty();
        }

    }
#endif
}


