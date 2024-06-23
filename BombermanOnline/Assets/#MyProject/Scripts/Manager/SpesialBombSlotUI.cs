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
    /// �h���b�v���ꂽ���̏���
    /// </summary>
    public override void OnDrop(PointerEventData eventData)
    {
        base.OnDrop(eventData);
        SpesialBombSelectUI bombInfo = eventData.pointerDrag.GetComponent<SpesialBombSelectUI>();
        if (bombInfo != null)
        {
            SetInfo(bombInfo.Type);
            SetImage(bombInfo);
            AudioManager.PlayOneShot("���e�ݒ�");
        }
    }

    // ===�ϐ�====================================================
    [SerializeField] Player player;
    [SerializeField] int slot;
    [SerializeField] UIManager uiManager;



    // ===�֐�====================================================

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
