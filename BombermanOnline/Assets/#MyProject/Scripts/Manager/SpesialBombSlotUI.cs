using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SpesialBombSlotUI : DroppedUI
{
    [SerializeField] Player player;
    [SerializeField] int slot;

    /// <summary>
    /// ドロップされた時の処理
    /// </summary>
    public override void OnDrop(PointerEventData eventData)
    {
        base.OnDrop(eventData);
        SpesialBombSelectUI bombInfo = eventData.pointerDrag.GetComponent<SpesialBombSelectUI>();
        if (bombInfo != null)
        {
            player.SetSpesialBombType(slot,bombInfo.bombType);
            player.AddSpesialBombType(slot);
        }
    }
}
