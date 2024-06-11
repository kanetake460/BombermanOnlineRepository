using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SpesialBombSelectUI : DraggableUI
{
    // ===�C�x���g�֐�================================================
    protected override void Awake()
    {
        base.Awake();
    }


    /// <summary>
    /// �}�E�X�J�[�\��������Ă���Ƃ��̏����i�C�x���g�֐��j
    /// </summary>
    /// <param name="eventData"></param>
    public override void OnPointerEnter(PointerEventData eventData)
    {
        explanationTmp.text = explanation;
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
    [Header("�p�����[�^�[")]
    [SerializeField]
    private SpesialBomb.BombType type = new SpesialBomb.BombType();

    [SerializeField,TextArea]
    private string explanation;

    [Header("�Q��")]
    [SerializeField] TextMeshProUGUI explanationTmp;

    // ===�v���p�e�B=================================================
    public SpesialBomb.BombType Type => type;

    public Sprite ImageSprite => GetComponent<Image>().sprite;

}
