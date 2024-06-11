using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SpesialBombSlotUI : DroppedUI
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
            player.SetSpesialBombType(slot,bombInfo.Type);
            player.AddSpesialBombType(slot);
            SetImage(bombInfo);
            AudioManager.PlayOneShot("���e�ݒ�");
        }
    }

    // ===�ϐ�====================================================
    [SerializeField] Player player;
    [SerializeField] int slot;



    // ===�֐�====================================================

    private void SetImage(SpesialBombSelectUI bombInfo)
    {
        GetComponent<Image>().sprite = bombInfo.ImageSprite;
    }
}
