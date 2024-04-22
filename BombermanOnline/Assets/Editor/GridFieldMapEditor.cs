using System.Linq;
using UnityEditor;
using UnityEngine;

namespace TakeshiLibrary
{
    [CustomEditor(typeof(GridFieldMapSettings))]
    public class GridFieldMapEditor : Editor
    {
        private void OnEnable()
        {
            m_settings = (GridFieldMapSettings)target;

            InitGUIStyles();     // トグルのスタイル初期化
        }



        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            base.OnInspectorGUI();
            GUILayout.Space(Figures.TwoPowerdByFive);
            // ===ここに処理=============================================================================
            
            ShowMapEditor();            // マップエディタ表示

            // ===================================================================================
            if (Application.isPlaying == false)serializedObject.ApplyModifiedProperties();// 再生されてなければSerializePropatyをApplyする
        }
        
        
        private void OnDisable()
        {
            _spaceTexture = null;
            _wallTexture = null;
            _pushTexture = null;

            m_settings = null;
            _cellToggleStyle = null;
            _coordTextStyle = null;
            _diagonalHeaderStyle = null;
        }

        // 数値
        private float _cellToggleSize = Figures.TwoPowerdByFive; // マップエディタのトグルのサイズ
        private Vector2 _scrollPosition;                          // エディタのスクロールのポジション
        
        // マップエディタのトグルとテキストのスタイル
        GUIStyle _cellToggleStyle = new GUIStyle();             // マップのトグルのスタイル
        GUIStyle _coordTextStyle = new GUIStyle();              // 座標のテキストのスタイル
        GUIStyle _diagonalHeaderStyle = new GUIStyle();         // 対角線ヘッダーのスタイル

        // テクスチャ
        Texture2D _spaceTexture;
        Texture2D _wallTexture;
        Texture2D _pushTexture;

        // セッティングス参照
        GridFieldMapSettings m_settings;

        int TotalCell => m_settings.gridWidth * m_settings.gridDepth;

        int GridWidth => m_settings.gridWidth;

        int GridDepth => m_settings.gridDepth;


        /// <summary>
        /// スタイルを初期化します
        /// </summary>
        private void InitGUIStyles()
        {
            // テクスチャ取得
            _spaceTexture = EditorGUIUtility.Load("Assets/Sprite/EditorTexture/ColorCells/FrameCell.png") as Texture2D;
            _wallTexture = EditorGUIUtility.Load("Assets/Sprite/EditorTexture/ColorCells/BlackCell.png") as Texture2D;
            _pushTexture = EditorGUIUtility.Load("Assets/Sprite/EditorTexture/ColorCells/OrangeCell.png") as Texture2D;
            
            // セルトグル初期化
            GUIUtilityT.SetGUIStyle(_cellToggleStyle, _spaceTexture, _wallTexture, _pushTexture);
            _cellToggleStyle.normal.textColor = Color.white;
            _cellToggleStyle.onNormal.textColor = Color.white;
            _cellToggleStyle.alignment = TextAnchor.UpperLeft;

            // 座標テキスト初期化
            _coordTextStyle.normal.background = GUIUtilityT.MakeTexture(Figures.TwoPowerdByFive, Figures.TwoPowerdByFive, Color.gray);
            _coordTextStyle.normal.textColor = Color.black;
            _coordTextStyle.alignment = TextAnchor.MiddleCenter;

            // 対角線ヘッダーテキスト初期化
            _diagonalHeaderStyle.normal.textColor = Color.white;
            _diagonalHeaderStyle.alignment = TextAnchor.MiddleCenter;
        }


        /// <summary>
        /// スタイルを管理します
        /// </summary>
        private void MgmtGUIStyale()
        {
            // セルトグル管理
            _cellToggleStyle.fontSize = (int)_cellToggleSize / 3;
            
            // 座標テキスト管理
            _coordTextStyle.fontSize = (int)_cellToggleSize / 2;

            // 対角線ヘッダー管理
            _diagonalHeaderStyle.fontSize = (int)_cellToggleSize / 2;
        }


        /// <summary>
        /// セッティングスを初期化します
        /// </summary>
        private void InitSettings()
        {
            // 配列のサイズがセルの合計数と一致しなければ、配列のサイズを確保
            if (m_settings.isSpaceProp.Length != TotalCell) m_settings.isSpaceProp = new bool[TotalCell];
        }


        /// <summary>
        /// グリッドマップを設定するトグルを表示します
        /// </summary>
        /// <param name="settings"></param>
        /// <param name="toggleSyale"></param>
        private void ShowMapEditor()
        {
            InitSettings();             // セッティングスを初期化
            MgmtGUIStyale();            // 座標テキストスタイル初期化

            ShowSizeSlider();           // スライダー表示
            GUILayout.Space(Figures.TwoPowerdByFive);

            ShowToggles();              // トグル表示
            GUILayout.Space(Figures.TwoPowerdByFive);

            ShowMapTemplateButton();
        }

        private void ShowMapTemplateButton()
        {
            GUILayout.Label("マップテンプレート");

            if (GUILayout.Button("All Space"))
                m_settings.isSpaceProp = m_settings.isSpaceProp.Select(x => x = true).ToArray();

            if (GUILayout.Button("All Wall"))
                m_settings.isSpaceProp = m_settings.isSpaceProp.Select(x => x = false).ToArray();

            if (GUILayout.Button("Even Grid"))
                for (int i = 0; i < m_settings.isSpaceProp.Length; i++)
                {
                    if (i % (GridWidth * 2) < GridWidth && i % 2  == 0)
                        m_settings.isSpaceProp[i] = false;
                    else
                        m_settings.isSpaceProp[i] = true;
                }

            if (GUILayout.Button("Odd Grid"))
                for (int i = 0; i < m_settings.isSpaceProp.Length; i++)
                {
                    if (i % (GridWidth * 2) >= GridWidth && i % 2 == 0)
                        m_settings.isSpaceProp[i] = false;
                    else
                        m_settings.isSpaceProp[i] = true;
                }

            if (GUILayout.Button("Surround"))
                for (int i = 0; i < m_settings.isSpaceProp.Length; i++)
                {
                    if (i < GridWidth ||                    // 下の行
                        i % GridWidth == 0 ||               // 左の行
                        i > GridWidth * (GridDepth - 1) ||  // 上の行
                        i % GridWidth == GridWidth - 1)
                        m_settings.isSpaceProp[i] = false;
                    else
                        m_settings.isSpaceProp[i] = true;
                }

            GUILayout.Label("＆テンプレート");

            if (GUILayout.Button("Even Grid"))
                for (int i = 0; i < m_settings.isSpaceProp.Length; i++)
                {
                    if (i % (GridWidth * 2) < GridWidth && i % 2 == 0)
                        m_settings.isSpaceProp[i] = false;
                }

            if (GUILayout.Button("Odd Grid"))
                for (int i = 0; i < m_settings.isSpaceProp.Length; i++)
                {
                    if (i % (GridWidth * 2) >= GridWidth && i % 2 == 0)
                        m_settings.isSpaceProp[i] = false;
                }

            if (GUILayout.Button("and Surround"))
                for (int i = 0; i < m_settings.isSpaceProp.Length; i++)
                {
                    if (i < GridWidth ||                    // 下の行
                        i % GridWidth == 0 ||               // 左の行
                        i > GridWidth * (GridDepth - 1) ||  // 上の行
                        i % GridWidth == GridWidth - 1)
                        m_settings.isSpaceProp[i] = false;
                }

        }


        /// <summary>
        /// トグルのサイズを変更するスライダー表示
        /// </summary>
        private void ShowSizeSlider()
        {
            _cellToggleSize = GUILayout.HorizontalSlider(_cellToggleSize, Figures.TwoCubed, Figures.TwoPowerdBySix);
        }


        /// <summary>
        /// グリッド上にトグルを表示します
        /// </summary>
        private void ShowToggles()
        {
            // トグルは左上から右下に下るように表示されるので、表示の仕方を変えます
            // グリッドを表示左下が原点なので、左下から数え上げるようにfor文を回す
            
            _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);
            for (int z = GridDepth - 1; z >= -1; z--)
            {
                EditorGUILayout.BeginHorizontal();
                for (int x = -1; x < GridWidth; x++)
                {
                    // 左下隅は座標もトグルも表示しない
                    if (x + z == -2)
                    {
                        GUILayout.Label("z／x",_diagonalHeaderStyle, GUILayout.Width(_cellToggleSize), GUILayout.Height(_cellToggleSize), GUILayout.ExpandWidth(false));
                        continue;
                    }
                    // 一番下の行はxの座標表示
                    if (z == -1)
                    {
                        GUILayout.Label(x.ToString(),_coordTextStyle , GUILayout.Width(_cellToggleSize), GUILayout.Height(_cellToggleSize), GUILayout.ExpandWidth(false));
                        continue;
                    }
                    // 一番左の列はzの座標表示
                    if (x == -1)
                    {
                        GUILayout.Label(z.ToString(), _coordTextStyle, GUILayout.Width(_cellToggleSize), GUILayout.Height(_cellToggleSize), GUILayout.ExpandWidth(false));
                        continue;
                    }
                    // それ以外はトグル表示
                    else
                    {
                        int index = x + z + z * (m_settings.gridWidth - 1); // インデックス
                        string coord = x + "," + z;                         // 座標
                        m_settings.isSpaceProp[index] = GUILayout.Toggle(m_settings.isSpaceProp[index], index.ToString(), _cellToggleStyle, GUILayout.Width(_cellToggleSize), GUILayout.Height(_cellToggleSize), GUILayout.ExpandWidth(false));
                    }
                }
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndScrollView();
        }
    }

    //   if (GUI.changed)
    //{
    //    EditorUtility.SetDirty(settings);
    //}

    /* 拡張インスペクターで配列の数値をゲームのシーンに与える方法 */
    //
    //public class MyScript : MonoBehaviour
    //{
    //    [SerializeField] public int sliderNum;

    //    public int[] values;

    //    private void Update()
    //    {
    //        for (int i = 0; i < values.Length; i++)
    //        {
    //            Debug.Log(values[i]);
    //        }
    //    }

    //    public void SetValuesSize(int size)
    //    {
    //        values = new int[size];
    //    }
    //}
    // 
    // 
    //[CustomEditor(typeof(MyScript))]
    //public class MyEditor : Editor
    //{
    //    // 設定する値を格納する配列（配列の大きさは変わるのでList）
    //    List<int> setValues = new List<int>();
    //    // シリアライズされた配列（ここに与えた値が、ゲームシーンに適用される）
    //    SerializedProperty sp_serializeValues;

    //    // ここに書かれているシリアライズなプロパティを呼び出すための参照
    //    MyScript myScript;
    //    // シリアライズされたオブジェクト（ここから値を参照する）
    //    SerializedObject serializedObject;


    //     private void OnEnable()
    //    {
    //        myScript = (MyScript)target;
    //        serializedObject = new SerializedObject(target);
    //        sp_serializeValues = serializedObject.FindProperty("value");
    //    }

    //    private void OnInspectorGUI()
    //    {
    //        serializedObject.Update();
    //        DrawDefaultInspector();

    //        /* ①サイズ確保 */
    //        // 実際のサイズ確保
    //        myScript.SetValuesSize(myScript.sliderNum);
    //        // 値を設定する配列のサイズ確保
    //        while(setValues.Count != myScript.sliderNum) 
    //        {
    //            if (setValues.Count < myScript.sliderNum)
    //            {
    //                setValues.Add(0);
    //            }
    //            else if (setValues.Count > myScript.sliderNum)
    //            {
    //                setValues.RemoveAt(setValues.Count - 1);
    //            }
    //        }

    //        /* ②インスペクターから値を設定 */
    //        // インスペクターから値を設定
    //        for(int i =  0; i < myScript.sliderNum; i++)
    //        {
    //            setValues[i] = (int)EditorGUILayout.Slider( "SetValue" + i,setValues[i],0f,100f);
    //        }

    //        /* ③シリアライズされた配列に値を設定 */
    //        // シリアライズされた配列の値に設定する配列の値を設定
    //        // ブロックリストにEditGridFieldMap()メソッドで設定した値を入れる
    //        for (int i = 0; i < sp_serializeValues.arraySize; i++)
    //        {
    //            SerializedProperty element = sp_serializeValues.GetArrayElementAtIndex(i);
    //            element.intValue = setValues[i];
    //        }


    //        serializedObject.ApplyModifiedProperties();
    //    }
    //}
    
    
    
    
}