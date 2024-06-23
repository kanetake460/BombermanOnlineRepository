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
    /// ÉhÉçÉbÉvÇ≥ÇÍÇΩéûÇÃèàóù
    /// </summary>
    public override void OnDrop(PointerEventData eventData)
    {
        base.OnDrop(eventData);
        SpesialBombSelectUI bombInfo = eventData.pointerDrag.GetComponent<SpesialBombSelectUI>();
        if (bombInfo != null)
        {
            SetInfo(bombInfo.Type);
            SetImage(bombInfo);
            AudioManager.PlayOneShot("îöíeê›íË");
        }
    }

    // ===ïœêî====================================================
    [SerializeField] Player player;
    [SerializeField] int slot;
    [SerializeField] UIManager uiManager;



    // ===ä÷êî====================================================

    private void SetImage(SpesialBombSelectUI bombInfo)
    {
        GetComponent<Image>().sprite = bombInfo.ImageSprite;
        uiManager.SetSpecialBombUI(slot,bombInfo.ImageSprite);
    }

    private void SetInfo(SpesialBomb.BombType type)
    {
        player.CallSetSpecialBombType(slot, (int)type);
    }
}
