using UnityEditor;
using UnityEngine;

public class GridFieldWindow : EditorWindow
{
    private GUIStyle _cellToggleStyle;


    const int CellToggleSize = 32;

    private bool[,] isSpace = new bool[4, 4];

    /// <summary>
    /// ウィンドウ表示
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
        // トグルのテクスチャを参照
        Texture2D _spaceTexture = EditorGUIUtility.Load("Assets/Sprite/EditorTexture/ColorCells/FrameCell.png") as Texture2D;
        Texture2D _wallTexture = EditorGUIUtility.Load("Assets/Sprite/EditorTexture/ColorCells/BlackCell.png") as Texture2D;
        Texture2D _pushTexture = EditorGUIUtility.Load("Assets/Sprite/EditorTexture/ColorCells/OrangeCell.png") as Texture2D;
        SetGUIStyle(_cellToggleStyle, _spaceTexture, _wallTexture, _pushTexture);

        EditGridFieldMap();

    }

    /// <summary>
    /// GUIスタイルを設定します
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
        // テクスチャを設定
        if (trueTexture == null || trueTexture == null || pushTexture == null)
        {
            Debug.LogError("テクスチャファイルが見つかりません！");
        }
        else
        {
            // true
            gUIStyle.onNormal.background = trueTexture;
            // false
            gUIStyle.normal.background = falseTextrue;

            // 変わる瞬間
            gUIStyle.active.background = pushTexture;
            gUIStyle.onActive.background = pushTexture;
        }
    }


    /// <summary>
    /// テクスチャを作成します
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
    /// マップを編集するトグルを表示します
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
                    EditorGUILayout.LabelField("x／z", GUILayout.Width(CellToggleSize),GUILayout.Height(CellToggleSize), GUILayout.ExpandWidth(false));
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
GUILayout.Button()：ボタンを作成します。
GUILayout.Toggle()：トグル（チェックボックス）を作成します。
GUILayout.TextField()：テキスト入力フィールドを作成します。
GUILayout.Box()：ボックス（枠）を作成します。
GUILayout.Label()：テキストを表示します（前述の通り）。
GUILayout.HorizontalSlider()：水平スライダーを作成します。
GUILayout.VerticalSlider()：垂直スライダーを作成します。
GUILayout.BeginHorizontal() / GUILayout.EndHorizontal()：水平のレイアウトグループを開始／終了します。
GUILayout.BeginVertical() / GUILayout.EndVertical()：垂直のレイアウトグループを開始／終了します。
 */
