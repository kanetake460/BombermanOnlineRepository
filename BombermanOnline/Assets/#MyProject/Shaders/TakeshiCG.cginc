#include "UnityCG.cginc"

float _NormalizedTime()
{
    return frac(_Time.y);
}

/// 引数に与えた時間分ループした時間を返します
float _LoopingTime(float period)
{
    return fmod(_Time.y, period);
}

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

bool random(float probability)
{
    return hash(_Time.y) < probability;
    
}