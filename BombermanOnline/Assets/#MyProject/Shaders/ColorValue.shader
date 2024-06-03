Shader"Custom/ColorValue"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _BlockSize("BlockSize",Int) = 8
        _MoveSpeedX("MoveSpeedX",Float) = 0.1
        _MoveSpeedY("MoveSpeedY",Float) = 0.1
        _Color1("Color1",Color) = (1,1,1,1)
        _Color2("Color2",Color) = (1,1,1,1)
        _Threshold("Threshold",Range(0,1)) = 0.5
    }
    SubShader
    {
        Tags { "Queue"="Transparent" }
        LOD 200

CGPROGRAM
#pragma surface surf Standard fullforwardshadows
#pragma target 3.0

#include "TakeshiCG.cginc"

sampler2D _MainTex;
int _BlockSize;
float _MoveSpeedX;
float _MoveSpeedY;
half4 _Color1;
half4 _Color2;
float _Threshold;

struct Input{
    float2 uv_MainTex;
    float3 worldPos;
};

fixed2 easeCurve(fixed2 t)
{
    return t * t * (3.0 - 2.0 * t);
}

        float valueNoise(fixed2 st)
        {
            fixed2 p = floor(st);   // ®”•”•ª
            fixed2 f = frac(st);    // ¬”•”•ª
    
            float v00 = hash2(p + fixed2(0, 0));
            float v10 = hash2(p + fixed2(1, 0));
            float v01 = hash2(p + fixed2(0, 1));
            float v11 = hash2(p + fixed2(1, 1));
            
            fixed2 u = easeCurve(f);

            float v0010 = lerp(v00, v10, u.x);
            float v0111 = lerp(v01, v11, u.x);
            return lerp(v0010, v0111, u.y);
        }

void surf(Input IN, inout SurfaceOutputStandard o)
{
    fixed2 uv = IN.uv_MainTex;

    uv.x += _Time * _MoveSpeedX;
    uv.y += _Time * _MoveSpeedY;
    
    float c = valueNoise(uv * _BlockSize);
    
    if(c > _Threshold)
    {
        discard;
    }
    
    o.Albedo = lerp(_Color1, _Color2, c);
}

     ENDCG
     }
FallBack"Diffuse"
    }

