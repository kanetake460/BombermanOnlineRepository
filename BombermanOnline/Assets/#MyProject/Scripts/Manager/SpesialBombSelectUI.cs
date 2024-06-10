using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class SpesialBombSelectUI : DraggableUI
{
    // ===イベント関数================================================
    protected override void Awake()
    {
        base.Awake();
    }

    /// <summary>
    /// つかんだ瞬間の処理（イベント関数）
    /// </summary>
    /// <param name="eventData"></param>
    public override void OnBeginDrag(PointerEventData eventData)
    {
        base.OnBeginDrag(eventData);
    }


    /// <summary>
    /// つかんでいる間の処理（イベント関数）
    /// </summary>
    public override void OnDrag(PointerEventData eventData)
    {
        base.OnDrag(eventData);

    }


    /// <summary>
    /// はなした瞬間の処理
    /// </summary>
    public override void OnEndDrag(PointerEventData eventData)
    {
        base.OnEndDrag(eventData);
        ResetPosition();
    }

    // ===変数====================================================
    public SpesialBomb.BombType bombType = new SpesialBomb.BombType();

    [TextArea]
    public string bombExplanation;
}
