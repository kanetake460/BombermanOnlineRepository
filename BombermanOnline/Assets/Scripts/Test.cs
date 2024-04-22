using OpenCover.Framework.Model;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField]public int intValue = 10;
    [SerializeField] public float floatValue = 10;
    [SerializeField] public bool boolValue = false;

    [SerializeField] public TextMeshProUGUI tmp;

    private void Update()
    {
        tmp.text = intValue.ToString();
    }
}

[CustomEditor(typeof(Test))]
public class YourScriptEditor : Editor
{
    SerializedObject serializedObject;
    Test test;
    private void OnEnable()
    {
        test = (Test)target;
        serializedObject = new SerializedObject(target);
        Debug.Log(test == target);

    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        GUILayout.Space(Figures.TwoPowerdByFour);
        GUILayout.Label("�g���C���X�y�N�^�[");

        Button();
        GUILayout.Space(Figures.TwoPowerdByFour);

        Slider();
        GUILayout.Space(Figures.TwoPowerdByFour);

        Toggle();
        GUILayout.Space(Figures.TwoPowerdByFour);

        GUILayout.Label("Serialized");

        SerializedButton();
        GUILayout.Space(Figures.TwoPowerdByFour);

        SerializedSlider();
        GUILayout.Space(Figures.TwoPowerdByFour);

        SerializedToggle();
        GUILayout.Space(Figures.TwoPowerdByFour);


        if (GUILayout.Button("Apply"))
            serializedObject.ApplyModifiedProperties();

    }

    private void Awake()
    {

    }

    private void SerializedButton()
    {
        if (GUILayout.Button("0"))
        {
            var valueProp = serializedObject.FindProperty("intValue");
            valueProp.intValue = 0;
        }
        if (GUILayout.Button("10"))
        {
            var valueProp = serializedObject.FindProperty("intValue");
            valueProp.intValue = 10;
        }
        if (GUILayout.Button("100"))
        {
            var valueProp = serializedObject.FindProperty("intValue");
            valueProp.intValue = 100;
        }
    }

    private void Button()
    {
        if (GUILayout.Button("0"))
        {
            test.intValue = 0;
        }
        if (GUILayout.Button("10"))
        {
            test.intValue = 10;
        }
        if (GUILayout.Button("100"))
        {
            test.intValue = 100;
        }
    }

    private void Slider()
    {
        test.floatValue = GUILayout.HorizontalSlider(test.floatValue, 0,100f);
    }

    private void SerializedSlider()
    {
        var valueProp = serializedObject.FindProperty("floatValue");
        valueProp.floatValue = GUILayout.HorizontalSlider(valueProp.floatValue, 0, 100);
    }

    private void Toggle()
    {
        test.boolValue = GUILayout.Toggle(test.boolValue, "bool");
    }

    private void SerializedToggle()
    {
        var valueProp = serializedObject.FindProperty("boolValue");
        valueProp.boolValue = GUILayout.Toggle(valueProp.boolValue,"SerializedBool");
    }
}

// Method1�́A���ڃI�u�W�F�N�g�̃t�B�[���h��ύX���邽�߁A�Q�[���Đ����ł��ύX�������ɔ��f����܂��B
// ����́Atest.value�����ڕύX����邽�߁A�ύX�������ɔ��f�����Ƃ�������ɂȂ�܂��B
// Method2�̓V���A���C�Y���ꂽ�v���p�e�B��ύX���邽�߁AserializedObject.ApplyModifiedProperties()���Ă΂��܂ŕύX�����f����܂���B
// ���������āAMethod2�̕ύX�́AApplyModifiedProperties()���Ă΂��^�C�~���O�œK�p����܂��B
// ����́A�ʏ�̓G�f�B�^�[�ł̑���𔽉f���邽�߂Ɏg�p����܂��B
//
// Method1��Method2�̎g�������́A��Ɏ��̂悤�ȏꍇ�ɓK�؂ł��B
// 
// ���ڃI�u�W�F�N�g��ύX����iMethod1�̂悤�ȕ��@�j�F
// 
// �C���X�y�N�^�[�ł̕ύX�������ɔ��f�����K�v������ꍇ�B
// �I�u�W�F�N�g�̑��삪�ȒP�ł���A���ʂȏ����␧�񂪕s�v�ȏꍇ�B
// �C���X�y�N�^�[�ő���\�ȃv���p�e�B�����Ȃ��ꍇ�B
// �V���A���C�Y���ꂽ�v���p�e�B��ύX����iMethod2�̂悤�ȕ��@�j�F
// 
// �C���X�y�N�^�[�ł̑�����J�X�^�}�C�Y�������ꍇ�B
// �C���X�y�N�^�[�ő���\�ȃv���p�e�B�������A�����ɓ��ʂȐ���⏈�����K�v�ȏꍇ�B
// �C���X�y�N�^�[�ł̑�������؂␧������K�v������ꍇ�B
// �I��������@�́A�J�����Ă���Q�[����A�v���P�[�V�����̃j�[�Y��A�R�[�h�̕ێ琫�A�ǐ��A�g�����Ȃ�
// ���l�����Č��肷��K�v������܂��B�ʏ�́A�V���A���C�Y���ꂽ�v���p�e�B�𑀍삷����@��
// ���_��������A�g����������܂����A�V���v���ȏꍇ�⑦���Ȕ��f���K�v�ȏꍇ��
// ���ڃI�u�W�F�N�g��ύX������@��I�����邱�Ƃ��K�؂ł��B