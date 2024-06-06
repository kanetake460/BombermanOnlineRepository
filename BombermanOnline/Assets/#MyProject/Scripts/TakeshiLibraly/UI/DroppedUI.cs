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
            // ここに処理
            AudioManager.PlayOneShot("アイテムゲット");
            Debug.Log("アイテムゲット");
        }
    }

    private void RegisterItem(int id)
    {
        // アイテム追加処理
    }
}
