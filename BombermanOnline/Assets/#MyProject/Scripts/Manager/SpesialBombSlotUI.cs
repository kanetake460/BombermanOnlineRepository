using SoftGear.Strix.Unity.Runtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SpesialBombSlotUI : StrixDroppedUI
{
    /// <summary>
    /// ドロップされた時の処理
    /// </summary>
    public override void OnDrop(PointerEventData eventData)
    {
        base.OnDrop(eventData);
        SpesialBombSelectUI bombInfo = eventData.pointerDrag.GetComponent<SpesialBombSelectUI>();
        if (bombInfo != null)
        {
            SetInfo(bombInfo.Type);
            SetImage(bombInfo);
            AudioManager.PlayOneShot("爆弾設定");
        }
    }

    // ===変数====================================================
    [SerializeField] Player player;
    [SerializeField] int slot;
    [SerializeField] UIManager uiManager;



    // ===関数====================================================

    private void SetImage(SpesialBombSelectUI bombInfo)
    {
        GetComponent<Image>().sprite = bombInfo.ImageSprite;
        uiManager.SetSpecialBombUI(slot,bombInfo.ImageSprite);
    }

    private void SetInfo(SpesialBomb.BombType type)
    {
        Debug.Log("セットインフォ");
        player.CallSetSpecialBombType(slot, (int)type);
        player.CallAddSpecialBomb(slot);
    }
}
