using SoftGear.Strix.Unity.Runtime;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class StrixDroppedUI : StrixBehaviour, IDropHandler
{
    /// <summary>
    /// ドロップされた時の処理
    /// </summary>
    /// <param name="eventData"></param>
    public virtual void OnDrop(PointerEventData eventData)
    {
        DraggableUI draggableUI = eventData.pointerDrag.GetComponent<DraggableUI>();
        if(draggableUI != null )
        {

        }
    }
}
