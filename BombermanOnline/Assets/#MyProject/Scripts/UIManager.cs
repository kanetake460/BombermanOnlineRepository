using SoftGear.Strix.Unity.Runtime;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : StrixBehaviour
{
    // ===�C�x���g�֐�================================================
    private void Update()
    {
        ShowBombUI(player.BombCount);
        ShowLifeUI(player.Life, player.m_lifeMaxValue);
        ShowUIText(firepowerText, "FirePower : " + player.Firepower);
        // �Q�[���e�L�X�g�̃J�E���g
        if (uiCount > 0)
        {
            uiCount -= Time.deltaTime;
        }
        else if(uiCount <= 0&& gameText.text != null)
        {
            gameText.text = null;
        }

        // �_���[�W�G�t�F�N�g�̃J�E���g
        if (Mathf.Approximately(damageEffect.color.a, 0) == false)
        {
            Color color = damageEffect.color;
            color.a = Mathf.Lerp(color.a, 0, Time.deltaTime * damageEffectSpeed);
            damageEffect.color = color;
        }
    }

    // ===�ϐ�====================================================
    [Header("�I�u�W�F�N�g�Q��")]
    [SerializeField] GameObject[] bombUIs;
    [SerializeField] Slider hpSlider;
    [SerializeField] Image damageEffect;
    [SerializeField] TextMeshProUGUI firepowerText;
    [SerializeField] TextMeshProUGUI gameText;
    [SerializeField] Player player;

    [Header("�p�����[�^�[")]
    [SerializeField] float damageEffectSpeed;
    private float uiCount;

    private readonly Color damageColor = new Color(1,0,0,0.8f);

    // ===�֐�====================================================
    /// <summary>�{��UI�\��</summary>
    public void ShowBombUI(int count) => ShowParamGuage(bombUIs, count);

    /// <summary>���C�tUI�\��</summary>
    public void ShowLifeUI(float value, float maxValue) => ShowSliderGuage(hpSlider, value, maxValue);
    
    /// <summary>�Q�[���e�L�X�g�\���iRPC�j</summary>
    public void CallShowGameText(string text, float count) { Rpc(nameof(ShowGameText), text, count); }
    [StrixRpc]
    public void ShowGameText(string text, float count)
    {
        if (isLocal)
            ShowUIText(gameText, text, count);
    }

    /// <summary>�_���[�W�G�t�F�N�g�\��</summary>
    public void ShowDamageEffectUI()
    {
        damageEffect.color = damageColor;
    }


    /// <summary>
    /// Int�̌���\������UI�̕\�����s���܂�
    /// </summary>
    /// <param name="uis">�\������UI�I�u�W�F�N�g</param>
    /// <param name="count">��</param>
    public void ShowParamGuage(GameObject[] uis,int count)
    {
        Array.ForEach(uis, b => b.SetActive(false));
        for (int i = 0; i < count; i++)
        {
            uis[i].SetActive(true);
        }
    }


    /// <summary>
    /// �e�L�X�g��\�����܂�
    /// </summary>
    /// <param name="tmp">�e�L�X�g���b�V��</param>
    /// <param name="text">�e�L�X�g</param>
    public void ShowUIText(TextMeshProUGUI tmp, string text)
    {
        tmp.text = text;
    }
    public void ShowUIText(TextMeshProUGUI tmp,string text,float count)
    {
        uiCount = count;
        tmp.text = text;
    }

    /// <summary>
    /// �X���C�_�[��\�����܂�
    /// </summary>
    /// <param name="slider">�X���C�_�[</param>
    /// <param name="value">�l</param>
    public void ShowSliderGuage(Slider slider,float value,float maxValue)
    {
        slider.maxValue = maxValue;
        slider.value = value;
    }
}
