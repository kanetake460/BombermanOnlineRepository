using UnityEngine;
using UnityEditor;

public class OpenBrowserWindow : EditorWindow
{
    private string strixCloudUrl = "https://www.strixcloud.net/app/application/ZnAgx8Gr/servers";
    private string chatGPTUrl = "https://chat.openai.com/c/3c2fa3d6-fcaa-4588-9456-312198962c7d";
    private string translationUrl = "https://translate.google.co.jp/?hl=ja&sl=ja&tl=en&op=translate";

    [MenuItem("LinkBoard/Open BrowserWindow")]
    public static void ShowWindow()
    {
        GetWindow(typeof(OpenBrowserWindow));
    }

    private void OnGUI()
    {
        GUILayout.Label("�֗��T�C�g");
        ShowOpneURLButton("�X�g���N�X�N���E�h",strixCloudUrl);
        ShowOpneURLButton("�`���b�gGPT",chatGPTUrl);
        ShowOpneURLButton("�O�[�O���|��", translationUrl);

    }

    private void ShowOpneURLButton(string name, string url) { if (GUILayout.Button(name)) Application.OpenURL(url); }
}
