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

            InitGUIStyles();     // �g�O���̃X�^�C��������
        }



        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            base.OnInspectorGUI();
            GUILayout.Space(Figures.TwoPowerdByFive);
            // ===�����ɏ���=============================================================================
            
            ShowMapEditor();            // �}�b�v�G�f�B�^�\��

            // ===================================================================================
            if (Application.isPlaying == false)serializedObject.ApplyModifiedProperties();// �Đ�����ĂȂ����SerializePropaty��Apply����
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

        // ���l
        private float _cellToggleSize = Figures.TwoPowerdByFive; // �}�b�v�G�f�B�^�̃g�O���̃T�C�Y
        private Vector2 _scrollPosition;                          // �G�f�B�^�̃X�N���[���̃|�W�V����
        
        // �}�b�v�G�f�B�^�̃g�O���ƃe�L�X�g�̃X�^�C��
        GUIStyle _cellToggleStyle = new GUIStyle();             // �}�b�v�̃g�O���̃X�^�C��
        GUIStyle _coordTextStyle = new GUIStyle();              // ���W�̃e�L�X�g�̃X�^�C��
        GUIStyle _diagonalHeaderStyle = new GUIStyle();         // �Ίp���w�b�_�[�̃X�^�C��

        // �e�N�X�`��
        Texture2D _spaceTexture;
        Texture2D _wallTexture;
        Texture2D _pushTexture;

        // �Z�b�e�B���O�X�Q��
        GridFieldMapSettings m_settings;

        int TotalCell => m_settings.gridWidth * m_settings.gridDepth;

        int GridWidth => m_settings.gridWidth;

        int GridDepth => m_settings.gridDepth;


        /// <summary>
        /// �X�^�C�������������܂�
        /// </summary>
        private void InitGUIStyles()
        {
            // �e�N�X�`���擾
            _spaceTexture = EditorGUIUtility.Load("Assets/Sprite/EditorTexture/ColorCells/FrameCell.png") as Texture2D;
            _wallTexture = EditorGUIUtility.Load("Assets/Sprite/EditorTexture/ColorCells/BlackCell.png") as Texture2D;
            _pushTexture = EditorGUIUtility.Load("Assets/Sprite/EditorTexture/ColorCells/OrangeCell.png") as Texture2D;
            
            // �Z���g�O��������
            GUIUtilityT.SetGUIStyle(_cellToggleStyle, _spaceTexture, _wallTexture, _pushTexture);
            _cellToggleStyle.normal.textColor = Color.white;
            _cellToggleStyle.onNormal.textColor = Color.white;
            _cellToggleStyle.alignment = TextAnchor.UpperLeft;

            // ���W�e�L�X�g������
            _coordTextStyle.normal.background = GUIUtilityT.MakeTexture(Figures.TwoPowerdByFive, Figures.TwoPowerdByFive, Color.gray);
            _coordTextStyle.normal.textColor = Color.black;
            _coordTextStyle.alignment = TextAnchor.MiddleCenter;

            // �Ίp���w�b�_�[�e�L�X�g������
            _diagonalHeaderStyle.normal.textColor = Color.white;
            _diagonalHeaderStyle.alignment = TextAnchor.MiddleCenter;
        }


        /// <summary>
        /// �X�^�C�����Ǘ����܂�
        /// </summary>
        private void MgmtGUIStyale()
        {
            // �Z���g�O���Ǘ�
            _cellToggleStyle.fontSize = (int)_cellToggleSize / 3;
            
            // ���W�e�L�X�g�Ǘ�
            _coordTextStyle.fontSize = (int)_cellToggleSize / 2;

            // �Ίp���w�b�_�[�Ǘ�
            _diagonalHeaderStyle.fontSize = (int)_cellToggleSize / 2;
        }


        /// <summary>
        /// �Z�b�e�B���O�X�����������܂�
        /// </summary>
        private void InitSettings()
        {
            // �z��̃T�C�Y���Z���̍��v���ƈ�v���Ȃ���΁A�z��̃T�C�Y���m��
            if (m_settings.isSpaceProp.Length != TotalCell) m_settings.isSpaceProp = new bool[TotalCell];
        }


        /// <summary>
        /// �O���b�h�}�b�v��ݒ肷��g�O����\�����܂�
        /// </summary>
        /// <param name="settings"></param>
        /// <param name="toggleSyale"></param>
        private void ShowMapEditor()
        {
            InitSettings();             // �Z�b�e�B���O�X��������
            MgmtGUIStyale();            // ���W�e�L�X�g�X�^�C��������

            ShowSizeSlider();           // �X���C�_�[�\��
            GUILayout.Space(Figures.TwoPowerdByFive);

            ShowToggles();              // �g�O���\��
            GUILayout.Space(Figures.TwoPowerdByFive);

            ShowMapTemplateButton();
        }

        private void ShowMapTemplateButton()
        {
            GUILayout.Label("�}�b�v�e���v���[�g");

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
                    if (i < GridWidth ||                    // ���̍s
                        i % GridWidth == 0 ||               // ���̍s
                        i > GridWidth * (GridDepth - 1) ||  // ��̍s
                        i % GridWidth == GridWidth - 1)
                        m_settings.isSpaceProp[i] = false;
                    else
                        m_settings.isSpaceProp[i] = true;
                }

            GUILayout.Label("���e���v���[�g");

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
                    if (i < GridWidth ||                    // ���̍s
                        i % GridWidth == 0 ||               // ���̍s
                        i > GridWidth * (GridDepth - 1) ||  // ��̍s
                        i % GridWidth == GridWidth - 1)
                        m_settings.isSpaceProp[i] = false;
                }

        }


        /// <summary>
        /// �g�O���̃T�C�Y��ύX����X���C�_�[�\��
        /// </summary>
        private void ShowSizeSlider()
        {
            _cellToggleSize = GUILayout.HorizontalSlider(_cellToggleSize, Figures.TwoCubed, Figures.TwoPowerdBySix);
        }


        /// <summary>
        /// �O���b�h��Ƀg�O����\�����܂�
        /// </summary>
        private void ShowToggles()
        {
            // �g�O���͍��ォ��E���ɉ���悤�ɕ\�������̂ŁA�\���̎d����ς��܂�
            // �O���b�h��\�����������_�Ȃ̂ŁA�������琔���グ��悤��for������
            
            _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);
            for (int z = GridDepth - 1; z >= -1; z--)
            {
                EditorGUILayout.BeginHorizontal();
                for (int x = -1; x < GridWidth; x++)
                {
                    // �������͍��W���g�O�����\�����Ȃ�
                    if (x + z == -2)
                    {
                        GUILayout.Label("z�^x",_diagonalHeaderStyle, GUILayout.Width(_cellToggleSize), GUILayout.Height(_cellToggleSize), GUILayout.ExpandWidth(false));
                        continue;
                    }
                    // ��ԉ��̍s��x�̍��W�\��
                    if (z == -1)
                    {
                        GUILayout.Label(x.ToString(),_coordTextStyle , GUILayout.Width(_cellToggleSize), GUILayout.Height(_cellToggleSize), GUILayout.ExpandWidth(false));
                        continue;
                    }
                    // ��ԍ��̗��z�̍��W�\��
                    if (x == -1)
                    {
                        GUILayout.Label(z.ToString(), _coordTextStyle, GUILayout.Width(_cellToggleSize), GUILayout.Height(_cellToggleSize), GUILayout.ExpandWidth(false));
                        continue;
                    }
                    // ����ȊO�̓g�O���\��
                    else
                    {
                        int index = x + z + z * (m_settings.gridWidth - 1); // �C���f�b�N�X
                        string coord = x + "," + z;                         // ���W
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

    /* �g���C���X�y�N�^�[�Ŕz��̐��l���Q�[���̃V�[���ɗ^������@ */
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
    //    // �ݒ肷��l���i�[����z��i�z��̑傫���͕ς��̂�List�j
    //    List<int> setValues = new List<int>();
    //    // �V���A���C�Y���ꂽ�z��i�����ɗ^�����l���A�Q�[���V�[���ɓK�p�����j
    //    SerializedProperty sp_serializeValues;

    //    // �����ɏ�����Ă���V���A���C�Y�ȃv���p�e�B���Ăяo�����߂̎Q��
    //    MyScript myScript;
    //    // �V���A���C�Y���ꂽ�I�u�W�F�N�g�i��������l���Q�Ƃ���j
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

    //        /* �@�T�C�Y�m�� */
    //        // ���ۂ̃T�C�Y�m��
    //        myScript.SetValuesSize(myScript.sliderNum);
    //        // �l��ݒ肷��z��̃T�C�Y�m��
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

    //        /* �A�C���X�y�N�^�[����l��ݒ� */
    //        // �C���X�y�N�^�[����l��ݒ�
    //        for(int i =  0; i < myScript.sliderNum; i++)
    //        {
    //            setValues[i] = (int)EditorGUILayout.Slider( "SetValue" + i,setValues[i],0f,100f);
    //        }

    //        /* �B�V���A���C�Y���ꂽ�z��ɒl��ݒ� */
    //        // �V���A���C�Y���ꂽ�z��̒l�ɐݒ肷��z��̒l��ݒ�
    //        // �u���b�N���X�g��EditGridFieldMap()���\�b�h�Őݒ肵���l������
    //        for (int i = 0; i < sp_serializeValues.arraySize; i++)
    //        {
    //            SerializedProperty element = sp_serializeValues.GetArrayElementAtIndex(i);
    //            element.intValue = setValues[i];
    //        }


    //        serializedObject.ApplyModifiedProperties();
    //    }
    //}
    
    
    
    
}