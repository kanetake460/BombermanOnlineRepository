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
        GUILayout.Label("拡張インスペクター");

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

// Method1は、直接オブジェクトのフィールドを変更するため、ゲーム再生中でも変更が即座に反映されます。
// これは、test.valueが直接変更されるため、変更が即座に反映されるという動作になります。
// Method2はシリアライズされたプロパティを変更するため、serializedObject.ApplyModifiedProperties()が呼ばれるまで変更が反映されません。
// したがって、Method2の変更は、ApplyModifiedProperties()が呼ばれるタイミングで適用されます。
// これは、通常はエディターでの操作を反映するために使用されます。
//
// Method1とMethod2の使い分けは、主に次のような場合に適切です。
// 
// 直接オブジェクトを変更する（Method1のような方法）：
// 
// インスペクターでの変更が即座に反映される必要がある場合。
// オブジェクトの操作が簡単であり、特別な処理や制約が不要な場合。
// インスペクターで操作可能なプロパティが少ない場合。
// シリアライズされたプロパティを変更する（Method2のような方法）：
// 
// インスペクターでの操作をカスタマイズしたい場合。
// インスペクターで操作可能なプロパティが多く、それらに特別な制約や処理が必要な場合。
// インスペクターでの操作を検証や制限する必要がある場合。
// 選択する方法は、開発しているゲームやアプリケーションのニーズや、コードの保守性、可読性、拡張性など
// を考慮して決定する必要があります。通常は、シリアライズされたプロパティを操作する方法が
// より柔軟性が高く、拡張性がありますが、シンプルな場合や即座な反映が必要な場合は
// 直接オブジェクトを変更する方法を選択することが適切です。