using SoftGear.Strix.Unity.Runtime;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : StrixBehaviour
{
    // ===イベント関数================================================
    private void Update()
    {
        ShowBombUI(player.BombCount);
        ShowEmptyBombUI(player.BombMaxCount);
        ShowSpecialTime();
        ShowLifeUI(player.Life, player.m_lifeMaxValue);
        ShowUIText(firepowerText, "× " + player.Firepower);
        ShowUIText(brickCountText, "× " + player.BrickCount);
        ShowNOUI();
        ShowDashUI();

        // エフェクト
        ShowFreezedEffect();
        ShowDamageEffect();
        ShowInvincibleEffect();
        // ゲームテキストのカウント
        if (uiCount > 0)
        {
            uiCount -= Time.deltaTime;
        }
        else if(uiCount <= 0&& gameText.text != null)
        {
            gameText.text = null;
        }

    }

    // ===変数====================================================
    [Header("オブジェクト参照")]
    [SerializeField] Player player;
    [Header("爆弾UI")]
    [SerializeField] GameObject[] bombUIs;
    [SerializeField] GameObject[] emptyBombUIs;
    [Header("インフォUI")]
    [SerializeField] Slider hpSlider;
    [SerializeField] Image sliderFillImage;
    [Header("エフェクトUI")]
    [SerializeField] Image damageEffect;
    [SerializeField] Image freezeEffect;
    [SerializeField] Image invincibleEffect;
    [Header("特殊爆弾UI")]
    [SerializeField] Image specialBombUI1;
    [SerializeField] Image specialBombUI2;
    [SerializeField] Image no1;
    [SerializeField] Image no2;
    [Header("ダッシュ")]
    [SerializeField] Image dashUI;
    [Header("テキストUI")]
    [SerializeField] TextMeshProUGUI firepowerText;
    [SerializeField] TextMeshProUGUI brickCountText;
    [SerializeField] TextMeshProUGUI gameText;
    [SerializeField] TextMeshProUGUI specialTime1tmp;
    [SerializeField] TextMeshProUGUI specialTime2tmp;


    [Header("パラメーター")]
    [SerializeField] float damageEffectSpeed;
    private float uiCount;

    private readonly Color damageColor = new Color(1,0,0,0.8f);
    private readonly Color freezeColor = new Color(0, 1, 1, 0.5f);
    private readonly Color invincibleColor = new Color(1, 1, 1, 0.2f);

    private bool isDamageEffect = false;    // ダメージエフェクト再生中かどうか
    
    // ひん死のHP
    [SerializeField] private int m_dyingHp;

    // ===関数====================================================

    /// <summary>
    /// ダメージエフェクトを出します
    /// </summary>
    private void ShowDamageEffect()
    {
        // ダメージエフェクトのカウント
        if (Mathf.Approximately(damageEffect.color.a, 0) == false)
        {
            Color color = damageEffect.color;
            color.a = Mathf.Lerp(color.a, 0, Time.deltaTime * damageEffectSpeed);
            damageEffect.color = color;
        }

        if(damageEffect.color.a < 0.1)
        {
            isDamageEffect = true;
        }
        else
        {
            isDamageEffect = false;
        }
    }

    /// <summary>
    /// 無敵エフェクトを出します
    /// </summary>
    private void ShowInvincibleEffect()
    {
        if(player.IsInvincible)
        {
            invincibleEffect.color = invincibleColor;
        }
        else
        {
            invincibleEffect.color = Color.clear;
        }
    }

    /// <summary>
    /// 凍りエフェクトを出します
    /// </summary>
    private void ShowFreezedEffect()
    {
        if (player.IsStun)
        {
            freezeEffect.color = freezeColor;
        }
        else
        {
            freezeEffect.color = Color.clear;
        }
    }


    private void ShowDashUI()
    {
        if(player.IsDashble)
        {
            dashUI.color = Color.white;
        }
        else
        {
            dashUI.color = Color.gray;
        }
    }


    /// <summary>
    /// 特殊爆弾のロック解除時間を表示します
    /// </summary>
    private void ShowSpecialTime()
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
    /// スペシャルボムUIを設定します。
    /// </summary>
    /// <param name="slot">スロット</param>
    /// <param name="sprite">スプライト</param>
    /// <exception cref="Exception">存在しないスロット</exception>
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
            throw new Exception("そのスロットはない！");
        }
    }

    /// <summary>
    /// NoUIの表示
    /// </summary>
    private void ShowNOUI()
    {
        // 爆弾の最大数が0ならNOを表示するそうでないなら非表示
        if (player.Special1MaxCount == 0)
            no1.enabled = true;
        else
            no1.enabled = false;
        
        if (player.Special2MaxCount == 0)
            no2.enabled = true;
        else
            no2.enabled = false;

        // 爆弾の所持数が0なら色をグレーにする
        if (player.Special1Count == 0)
            specialBombUI1.color = Color.gray;
        else
            specialBombUI1.color = Color.white;

        if (player.Special2Count == 0)
            specialBombUI2.color = Color.gray;
        else
            specialBombUI2.color = Color.white;

    }


    /// <summary>ボムUI表示</summary>
    private void ShowBombUI(int count) => ShowParamGuage(bombUIs, count);    
    
    /// <summary>空のボムUI表示</summary>
    private void ShowEmptyBombUI(int count) => ShowParamGuage(emptyBombUIs, count);

    /// <summary>ライフUI表示</summary>
    private void ShowLifeUI(int value, int maxValue) => ShowSliderGuage(hpSlider,sliderFillImage, value, maxValue);
    
    /// <summary>ゲームテキスト表示（RPC）</summary>
    public void CallShowGameText(string text, float count) { Rpc(nameof(ShowGameText), text, count); }
    [StrixRpc]
    public void ShowGameText(string text, float count)
    {
        if (isLocal)
            ShowUIText(gameText, text, count);
    }

    /// <summary>ダメージエフェクト表示</summary>
    public void ShowDamageEffectUI()
    {
        damageEffect.color = damageColor;
    }


    /// <summary>
    /// Intの個数を表現するUIの表示を行います
    /// </summary>
    /// <param name="uis">表現するUIオブジェクト</param>
    /// <param name="count">個数</param>
    public void ShowParamGuage(GameObject[] uis,int count)
    {
        Array.ForEach(uis, b => b.SetActive(false));
        for (int i = 0; i < count; i++)
        {
            uis[i].SetActive(true);
        }
    }


    /// <summary>
    /// テキストを表示します
    /// </summary>
    /// <param name="tmp">テキストメッシュ</param>
    /// <param name="text">テキスト</param>
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
    /// スライダーを表示します
    /// </summary>
    /// <param name="slider">スライダー</param>
    /// <param name="value">値</param>
    public void ShowSliderGuage(Slider slider,Image fillImage,int value,float maxValue)
    {
        slider.maxValue = maxValue;
        slider.value = value;
        if(value <= m_dyingHp)
        {
            fillImage.color = Color.red;
        }
        else
        {
            fillImage.color = Color.green;
        }
    }
}