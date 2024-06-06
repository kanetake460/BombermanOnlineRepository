using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class DroppedUI : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        DraggableUI draggableUI = eventData.pointerDrag.GetComponent<DraggableUI>();
        if(draggableUI != null )
        {
            // �����ɏ���
            AudioManager.PlayOneShot("�A�C�e���Q�b�g");
            Debug.Log("�A�C�e���Q�b�g");
        }
    }

    private void RegisterItem(int id)
    {
        // �A�C�e���ǉ�����
    }
}
