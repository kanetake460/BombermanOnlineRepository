using SoftGear.Strix.Unity.Runtime;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class StrixDroppedUI : StrixBehaviour, IDropHandler
{
    /// <summary>
    /// ƒhƒƒbƒv‚³‚ê‚½‚Ìˆ—
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
