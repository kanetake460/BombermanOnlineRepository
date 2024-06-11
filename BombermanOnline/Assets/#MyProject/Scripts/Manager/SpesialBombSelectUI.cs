using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SpesialBombSelectUI : DraggableUI
{
    // ===イベント関数================================================
    protected override void Awake()
    {
        base.Awake();
    }


    /// <summary>
    /// マウスカーソルが乗っているときの処理（イベント関数）
    /// </summary>
    /// <param name="eventData"></param>
    public override void OnPointerEnter(PointerEventData eventData)
    {
        explanationTmp.text = explanation;
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
    [Header("パラメーター")]
    [SerializeField]
    private SpesialBomb.BombType type = new SpesialBomb.BombType();

    [SerializeField,TextArea]
    private string explanation;

    [Header("参照")]
    [SerializeField] TextMeshProUGUI explanationTmp;

    // ===プロパティ=================================================
    public SpesialBomb.BombType Type => type;

    public Sprite ImageSprite => GetComponent<Image>().sprite;

}
