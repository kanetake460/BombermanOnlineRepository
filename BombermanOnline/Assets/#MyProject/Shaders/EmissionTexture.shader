Shader"Custom/EmissionTexture"
{
    Properties
    {
        _MainTex("Texture",2D) = "white"{}
        _BaseColor("BaseColor", Color) = (1, 1, 1, 1)
        _EmissionColor("EmissionColor",Color) = (1, 1, 1, 1)
        _LineWidth("LineWidth", Float) = 1.0
        _LineSpeed("LineSpeed", Float) = 50.0
        _LoopDuration("LoopDuration", Float) = 5.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
LOD 200

        CGPROGRAM
        #pragma surface surf Standard fullfowardshadows
        #pragma target 3.0

#include "TakeshiCG.cginc"


struct Input
{
    float3 worldPos;
    float2 uv_MainTex;
};

sampler2D _MainTex;
fixed4 _BaseColor;
fixed4 _EmissionColor;
float _LineWidth;
float _LineSpeed;
float _LoopDuration;

void surf(Input IN, inout SurfaceOutputStandard o)
{
    fixed2 uv = IN.uv_MainTex;
    fixed4 c = tex2D(_MainTex, uv) * _BaseColor;
    o.Albedo = c.rgb;

            // ŽžŠÔ‚ª‚½‚Â‚É˜A‚ê‚Äval‚Í‘å‚«‚­‚È‚é
    float val = (uv.x + uv.y) - _LoopingTime(_LoopDuration) * _LineSpeed;

    if (val > 0 && val < _LineWidth)
    {
        o.Emission = _EmissionColor.rgb;
    }
}
        ENDCG
    }
FallBack"Diffuse"
}
