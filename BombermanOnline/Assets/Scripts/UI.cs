using Palmmedia.ReportGenerator.Core.Parser.Analysis;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI : MonoBehaviour
{
    private void Update()
    {
        ShowBombUI(player.BombCount);
        ShowUIText(firepowerText, "FirePower : " + player.Firepower);
        if (uiCount > 0)
        {
            uiCount -= Time.deltaTime;
        }
        else
        {
            gameText.text = null;
        }
    }

    static private float uiCount;

    [SerializeField] GameObject[] bombUIs;
    [SerializeField] TextMeshProUGUI firepowerText;
    [SerializeField] TextMeshProUGUI gameText;
    [SerializeField] Player player;

    public void ShowBombUI(int count)
    {
        Array.ForEach(bombUIs, b => b.SetActive(false));
        for (int i = 0; i < count; i++)
        {
            bombUIs[i].SetActive(true);
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


    public void ShowGameText(string text,float count)
    {
        ShowUIText(gameText,text,count);
    }
}
