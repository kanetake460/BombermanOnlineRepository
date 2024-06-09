using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CanvasGroup))]
public class DraggableUI : MonoBehaviour,IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private Vector3 _firstPos;
    private RectTransform _rectTransform;
    private Canvas _canvas;
    private CanvasGroup _canvasGroup;

    protected virtual void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _canvas = GetComponentInParent<Canvas>();
        _canvasGroup = GetComponent<CanvasGroup>();
        _firstPos = _rectTransform.localPosition;
    }


    /// <summary>
    /// つかんだ瞬間の処理（イベント関数）
    /// </summary>
    /// <param name="eventData"></param>
    public virtual void OnBeginDrag(PointerEventData eventData)
    {
        _canvasGroup.blocksRaycasts = false;
    }


    /// <summary>
    /// つかんでいる間の処理（イベント関数）
    /// </summary>
    public virtual void OnDrag(PointerEventData eventData)
    {
        if (_canvas == null) return;
        _rectTransform.anchoredPosition += eventData.delta / _canvas.scaleFactor;
    }


    /// <summary>
    /// はなした瞬間の処理
    /// </summary>
    public virtual void OnEndDrag(PointerEventData eventData)
    {
        _canvasGroup.blocksRaycasts = true;
    }


    /// <summary>
    /// UIの位置を最初に戻す
    /// </summary>
    public void ResetPosition()
    {
        _rectTransform.localPosition = _firstPos;
    }


    /// <summary>
    /// リープ関数でポジションを戻します（Update）
    /// </summary>
    public void LeapResetPosition(float speed)
    {
        _rectTransform.localPosition = Vector3.Lerp(_rectTransform.localPosition,_firstPos,Time.deltaTime * speed);
    }
}
