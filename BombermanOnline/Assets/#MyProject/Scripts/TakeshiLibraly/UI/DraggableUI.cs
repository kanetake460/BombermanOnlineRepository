using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableUI : MonoBehaviour,IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private Vector3 _firstPos;
    private RectTransform _rectTransform;
    private Canvas _canvas;

    protected virtual void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _canvas = GetComponentInParent<Canvas>();
        _firstPos = _rectTransform.localPosition;
    }


    /// <summary>
    /// ���񂾏u�Ԃ̏����i�C�x���g�֐��j
    /// </summary>
    /// <param name="eventData"></param>
    public virtual void OnBeginDrag(PointerEventData eventData)
    {
    }


    /// <summary>
    /// ����ł���Ԃ̏����i�C�x���g�֐��j
    /// </summary>
    public virtual void OnDrag(PointerEventData eventData)
    {
        if (_canvas == null) return;
        _rectTransform.anchoredPosition += eventData.delta / _canvas.scaleFactor;
    }


    /// <summary>
    /// �͂Ȃ����u�Ԃ̏���
    /// </summary>
    public virtual void OnEndDrag(PointerEventData eventData)
    {
    }


    /// <summary>
    /// UI�̈ʒu���ŏ��ɖ߂�
    /// </summary>
    public void ResetPosition()
    {
        _rectTransform.localPosition = _firstPos;
    }


    /// <summary>
    /// ���[�v�֐��Ń|�W�V������߂��܂��iUpdate�j
    /// </summary>
    public void LeapResetPosition(float speed)
    {
        _rectTransform.localPosition = Vector3.Lerp(_rectTransform.localPosition,_firstPos,Time.deltaTime * speed);
    }
}
