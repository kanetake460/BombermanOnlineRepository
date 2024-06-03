#include "UnityCG.cginc"
// ===時間========================================================
float _NormalizedTime()
{
    return frac(_Time.y);
}

/// 引数に与えた時間分ループした時間を返します
float _LoopingTime(float period)
{
    return fmod(_Time.y, period);
}

// ===ランダム========================================================

/// シード値から一次元疑似乱数を返します
    float hash
    (
    float n)
{
        return frac(sin(n) * 43758.5453123);
    }


/// シード値から二次元疑似乱数を返します
    float2 hash2
    (
    float2 p)
{
        return frac(sin(float2(dot(p, float2(127.1, 311.7)), dot(p, float2(269.5, 183.3)))) * 43758.5453123);
    }

/// 丸めて、ランダムな値に変更します
float floorHash2(fixed2 st)
{
    fixed2 p = floor(st);
    return hash2(p);
}

/// ランダムなタイミングでtrueになります
/// 引数：確率
bool random(float probability)
{
    return hash(_Time.y) < probability;
    
}

// ===色========================================================
/// HSV色空間からRGB色空間に変換します
/// 引数1:色相
/// 引数2:彩度
/// 引数3:明度
fixed3 HSVtoRGB(fixed h, fixed s, fixed v)
{
    fixed r, g, b;

    int i = int(floor(h * 6));
    fixed f = h * 6 - i;
    fixed p = v * (1 - s);
    fixed q = v * (1 - f * s);
    fixed t = v * (1 - (1 - f) * s);

    if (i % 6 == 0)
    {
        r = v;
        g = t;
        b = p;
    }
    else if (i == 1)
    {
        r = q;
        g = v;
        b = p;
    }
    else if (i == 2)
    {
        r = p;
        g = v;
        b = t;
    }
    else if (i == 3)
    {
        r = p;
        g = q;
        b = v;
    }
    else if (i == 4)
    {
        r = t;
        g = p;
        b = v;
    }
    else
    {
        r = v;
        g = p;
        b = q;
    }

    return fixed3(r, g, b);
}
