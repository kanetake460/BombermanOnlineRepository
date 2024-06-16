using UnityEditor;
using UnityEngine;

public class GridFieldWindow : EditorWindow
{
    private GUIStyle _cellToggleStyle;


    const int CellToggleSize = 32;

    private bool[,] isSpace = new bool[4, 4];

    /// <summary>
    /// �E�B���h�E�\��
    /// </summary>
    [MenuItem("Window/GridFieldMap Window")]
    public static void ShowWindow()
    {
        GetWindow<GridFieldWindow>("GridFieldMap");
    }


    private void OnEnable()
    {

    }


    private void OnGUI()
    {
        // �g�O���̃e�N�X�`�����Q��
        Texture2D _spaceTexture = EditorGUIUtility.Load("Assets/Sprite/EditorTexture/ColorCells/FrameCell.png") as Texture2D;
        Texture2D _wallTexture = EditorGUIUtility.Load("Assets/Sprite/EditorTexture/ColorCells/BlackCell.png") as Texture2D;
        Texture2D _pushTexture = EditorGUIUtility.Load("Assets/Sprite/EditorTexture/ColorCells/OrangeCell.png") as Texture2D;
        SetGUIStyle(_cellToggleStyle, _spaceTexture, _wallTexture, _pushTexture);

        EditGridFieldMap();

    }

    /// <summary>
    /// GUI�X�^�C����ݒ肵�܂�
    /// </summary>
    private void SetGUIStyle(GUIStyle gUIStyle, Color trueColor, Color falseColor, Color pushColor)
    {
        var trueTexture = MakeTexture(CellToggleSize, CellToggleSize, trueColor);
        var falseTexture = MakeTexture(CellToggleSize, CellToggleSize, falseColor);
        var pushTexture = MakeTexture(CellToggleSize, CellToggleSize, pushColor);
        SetGUIStyle(gUIStyle, trueTexture, falseTexture, pushTexture);
    }
    private void SetGUIStyle(GUIStyle gUIStyle, Texture2D trueTexture, Texture2D falseTextrue, Texture2D pushTexture)
    {
        // �e�N�X�`����ݒ�
        if (trueTexture == null || trueTexture == null || pushTexture == null)
        {
            Debug.LogError("�e�N�X�`���t�@�C����������܂���I");
        }
        else
        {
            // true
            gUIStyle.onNormal.background = trueTexture;
            // false
            gUIStyle.normal.background = falseTextrue;

            // �ς��u��
            gUIStyle.active.background = pushTexture;
            gUIStyle.onActive.background = pushTexture;
        }
    }


    /// <summary>
    /// �e�N�X�`�����쐬���܂�
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="color"></param>
    /// <returns></returns>
    private Texture2D MakeTexture(int width, int height, Color color)
    {
        Color[] pixels = new Color[width * height];
        for (int i = 0; i < pixels.Length; i++)
        {
            pixels[i] = color;
        }

        Texture2D texture = new Texture2D(width, height);
        texture.SetPixels(pixels);
        texture.Apply();

        return texture;
    }


    /// <summary>
    /// �}�b�v��ҏW����g�O����\�����܂�
    /// </summary>
    private void EditGridFieldMap()
    {
        GUILayout.Label("GridFieldMapEditor");
        for (int x = 3; x >= -1; x--)
        {
            EditorGUILayout.BeginHorizontal();
            for (int z = -1; z < 4; z++)
            {
                if (x + z == -2)
                {
                    EditorGUILayout.LabelField("x�^z", GUILayout.Width(CellToggleSize),GUILayout.Height(CellToggleSize), GUILayout.ExpandWidth(false));
                    continue;
                }
                if(z == -1)
                {
                    EditorGUILayout.LabelField(x.ToString(), GUILayout.Width(CellToggleSize), GUILayout.Height(CellToggleSize), GUILayout.ExpandWidth(false));
                    continue;
                }
                if (x == -1)
                {
                    EditorGUILayout.LabelField(z.ToString(), GUILayout.Width(CellToggleSize), GUILayout.Height(CellToggleSize), GUILayout.ExpandWidth(false));
                    continue;
                }
                else
                {
                    isSpace[x, z] = EditorGUILayout.Toggle(isSpace[x, z], _cellToggleStyle, GUILayout.Width(CellToggleSize), GUILayout.Height(CellToggleSize), GUILayout.ExpandWidth(false));
                }
            }
            EditorGUILayout.EndHorizontal();
        }
    }
}

/*
GUILayout.Button()�F�{�^�����쐬���܂��B
GUILayout.Toggle()�F�g�O���i�`�F�b�N�{�b�N�X�j���쐬���܂��B
GUILayout.TextField()�F�e�L�X�g���̓t�B�[���h���쐬���܂��B
GUILayout.Box()�F�{�b�N�X�i�g�j���쐬���܂��B
GUILayout.Label()�F�e�L�X�g��\�����܂��i�O�q�̒ʂ�j�B
GUILayout.HorizontalSlider()�F�����X���C�_�[���쐬���܂��B
GUILayout.VerticalSlider()�F�����X���C�_�[���쐬���܂��B
GUILayout.BeginHorizontal() / GUILayout.EndHorizontal()�F�����̃��C�A�E�g�O���[�v���J�n�^�I�����܂��B
GUILayout.BeginVertical() / GUILayout.EndVertical()�F�����̃��C�A�E�g�O���[�v���J�n�^�I�����܂��B
 */
