using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SpesialBombSlotUI : DroppedUI
{
    /// <summary>
    /// ƒhƒƒbƒv‚³‚ê‚½‚Ìˆ—
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
            AudioManager.PlayOneShot("”š’eİ’è");
        }
    }

    // ===•Ï”====================================================
    [SerializeField] Player player;
    [SerializeField] int slot;



    // ===ŠÖ”====================================================

    private void SetImage(SpesialBombSelectUI bombInfo)
    {
        GetComponent<Image>().sprite = bombInfo.ImageSprite;
    }
}
