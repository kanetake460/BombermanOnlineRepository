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
        ShowSpecialTime();
        ShowLifeUI(player.Life, player.m_lifeMaxValue);
        ShowUIText(firepowerText, "FirePower : " + player.Firepower);
        ShowNOUI();
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
    [SerializeField] Image specialBombUI1;
    [SerializeField] Image specialBombUI2;
    [SerializeField] Image no1;
    [SerializeField] Image no2;
    [SerializeField] TextMeshProUGUI specialTime1tmp;
    [SerializeField] TextMeshProUGUI specialTime2tmp;

    [SerializeField] Player player;

    [Header("�p�����[�^�[")]
    [SerializeField] float damageEffectSpeed;
    private float uiCount;

    private readonly Color damageColor = new Color(1,0,0,0.8f);

    // ===�֐�====================================================
    /// <summary>
    /// ���ꔚ�e�̃��b�N�������Ԃ�\�����܂�
    /// </summary>
    public void ShowSpecialTime()
    {
        if (player.Special1LockTime >= 0)
            specialTime1tmp.text = player.Special1LockTime.ToString();
        else
            specialTime1tmp.enabled = false;

        if (player.Special2LockTime >= 0)
            specialTime2tmp.text = player.Special2LockTime.ToString();
        else
            specialTime2tmp.enabled = false;
    }

    /// <summary>
    /// �X�y�V�����{��UI��ݒ肵�܂��B
    /// </summary>
    /// <param name="slot">�X���b�g</param>
    /// <param name="sprite">�X�v���C�g</param>
    /// <exception cref="Exception">���݂��Ȃ��X���b�g</exception>
    public void SetSpecialBombUI(int slot,Sprite sprite)
    {
        if (slot == 0)
        {
            specialBombUI1.sprite = sprite;
        }
        else if (slot == 1)
        {
            specialBombUI2.sprite = sprite;
        }
        else
        {
            throw new Exception("���̃X���b�g�͂Ȃ��I");
        }
    }

    /// <summary>
    /// NoUI�̕\��
    /// </summary>
    public void ShowNOUI()
    {
        // ���e�̍ő吔��0�Ȃ�NO��\�����邻���łȂ��Ȃ��\��
        if (player.Special1MaxCount == 0)
            no1.enabled = true;
        else
            no1.enabled = false;
        
        if (player.Special2MaxCount == 0)
            no2.enabled = true;
        else
            no2.enabled = false;

        // ���e�̏�������0�Ȃ�F���O���[�ɂ���
        if (player.Special1Count == 0)
            specialBombUI1.color = Color.gray;
        else
            specialBombUI1.color = Color.white;

        if (player.Special2Count == 0)
            specialBombUI2.color = Color.gray;
        else
            specialBombUI2.color = Color.white;

    }


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
