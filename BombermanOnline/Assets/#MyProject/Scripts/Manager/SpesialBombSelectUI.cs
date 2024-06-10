using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class SpesialBombSelectUI : DraggableUI
{
    // ===�C�x���g�֐�================================================
    protected override void Awake()
    {
        base.Awake();
    }

    /// <summary>
    /// ���񂾏u�Ԃ̏����i�C�x���g�֐��j
    /// </summary>
    /// <param name="eventData"></param>
    public override void OnBeginDrag(PointerEventData eventData)
    {
        base.OnBeginDrag(eventData);
    }


    /// <summary>
    /// ����ł���Ԃ̏����i�C�x���g�֐��j
    /// </summary>
    public override void OnDrag(PointerEventData eventData)
    {
        base.OnDrag(eventData);

    }


    /// <summary>
    /// �͂Ȃ����u�Ԃ̏���
    /// </summary>
    public override void OnEndDrag(PointerEventData eventData)
    {
        base.OnEndDrag(eventData);
        ResetPosition();
    }

    // ===�ϐ�====================================================
    public SpesialBomb.BombType bombType = new SpesialBomb.BombType();

    [TextArea]
    public string bombExplanation;
}
