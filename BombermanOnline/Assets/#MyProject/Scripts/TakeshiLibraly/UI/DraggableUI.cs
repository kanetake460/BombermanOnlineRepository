using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableUI : MonoBehaviour,IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public int id;
    private Vector3 firstPos;
    private RectTransform _rectTransform;
    private Canvas _canvas;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _canvas = GetComponentInParent<Canvas>();
        firstPos = _rectTransform.localPosition;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // つかんだ瞬間の処理
        AudioManager.PlayOneShot("爆弾がない");

    }

    public void OnDrag(PointerEventData eventData)
    {
        if (_canvas == null) return;
        _rectTransform.anchoredPosition += eventData.delta / _canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        AudioManager.PlayOneShot("爆弾を置く");
        ResetPosition();
    }

    public void ResetPosition()
    {
        _rectTransform.localPosition = firstPos;
    }

}
