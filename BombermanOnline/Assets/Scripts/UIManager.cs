using SoftGear.Strix.Unity.Runtime;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : StrixBehaviour
{
    private void Update()
    {
        ShowBombUI(player.BombCount);
        ShowLifeUI(player.LifeCount);
        ShowUIText(firepowerText, "FirePower : " + player.Firepower);
        if (uiCount > 0)
        {
            uiCount -= Time.deltaTime;
        }
        else if(uiCount <= 0&& gameText.text != null)
        {
            gameText.text = null;
        }

        if (Mathf.Approximately(damageEffect.color.a, 0) == false)
        {
            Color color = damageEffect.color;
            color.a = Mathf.Lerp(color.a, 0, Time.deltaTime * damageEffectSpeed);
            damageEffect.color = color;
        }
    }


    [Header("オブジェクト参照")]
    [SerializeField] GameObject[] bombUIs;
    [SerializeField] GameObject[] lifeUIs;
    [SerializeField] Image damageEffect;
    [SerializeField] TextMeshProUGUI firepowerText;
    [SerializeField] TextMeshProUGUI gameText;
    [SerializeField] Player player;

    [Header("パラメーター")]
    [SerializeField] float damageEffectSpeed;
    private float uiCount;


    private readonly Color damageColor = new Color(1,0,0,0.8f);

    public void ShowBombUI(int count) => ShowParamGuage(bombUIs, count);
    public void ShowLifeUI(int count) => ShowParamGuage(lifeUIs, count);


    public void ShowParamGuage(GameObject[] uis,int count)
    {
        Array.ForEach(uis, b => b.SetActive(false));
        for (int i = 0; i < count; i++)
        {
            uis[i].SetActive(true);
        }
    }

    public void ShowUIText(TextMeshProUGUI tmp, string text)
    {
        tmp.text = text;
    }
    public void ShowUIText(TextMeshProUGUI tmp,string text,float count)
    {
        uiCount = count;
        tmp.text = text;
    }


    public void CallShowGameText(string text, float count) { RpcToAll(nameof(ShowGameText), text, count); }
    [StrixRpc]
    public void ShowGameText(string text,float count)
    {
        ShowUIText(gameText,text,count);
    }


    public void ShowDamageEffectUI()
    {
        damageEffect.color = damageColor;
    }
}
