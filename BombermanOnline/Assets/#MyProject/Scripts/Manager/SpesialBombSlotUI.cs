using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SpesialBombSlotUI : DroppedUI
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
            player.SetSpesialBombType(slot,bombInfo.Type);
            player.AddSpesialBombType(slot);
            SetImage(bombInfo);
            AudioManager.PlayOneShot("爆弾設定");
        }
    }

    // ===変数====================================================
    [SerializeField] Player player;
    [SerializeField] int slot;



    // ===関数====================================================

    private void SetImage(SpesialBombSelectUI bombInfo)
    {
        GetComponent<Image>().sprite = bombInfo.ImageSprite;
    }
}
