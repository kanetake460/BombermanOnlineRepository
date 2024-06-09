using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SpesialBombSlotUI : DroppedUI
{
    [SerializeField] Player player;
    [SerializeField] int slot;

    /// <summary>
    /// ƒhƒƒbƒv‚³‚ê‚½‚Ìˆ—
    /// </summary>
    public override void OnDrop(PointerEventData eventData)
    {
        base.OnDrop(eventData);
        SpesialBombSelectUI bombInfo = eventData.pointerDrag.GetComponent<SpesialBombSelectUI>();
        if (bombInfo != null)
        {
            SetImage(eventData);
            player.SetSpesialBombType(slot,bombInfo.bombType);
            player.AddSpesialBombType(slot);
        }
    }

    private void SetImage(PointerEventData eventData)
    {
        GetComponent<Image>().sprite = eventData.pointerDrag.GetComponent<Image>().sprite;
    }
}
