Shader"Custom/MoveLine"
{
    Properties
    {
        _BaseColor("BaseColor", Color) = (1, 1, 1, 1)
        _LineColor("LineColor",Color) = (1, 1, 1, 1)
        _LineWidth("LineWidth", Range(0,1)) = 0.5
        _LineSpeed("LineSpeed", Float) = 50.0
    }
    SubShader
    {
        Tags { "Queue"="Transparent" }
LOD 200

        CGPROGRAM
        #pragma surface surf Standard
        #pragma target 3.0

#include "TakeshiCG.cginc"


struct Input
{
    float3 worldPos;
};

fixed4 _BaseColor;
fixed4 _LineColor;
float _LineWidth;
float _LineSpeed;

void surf(Input IN, inout SurfaceOutputStandard o){
    o.Smoothness = 0.5;
    o.Albedo = _BaseColor.rgb;
    // ŽžŠÔ‚ª‚½‚Â‚É˜A‚ê‚Äval‚Í‘å‚«‚­‚È‚é

    float val = abs(sin(IN.worldPos.y * 3.0 -_Time * _LineSpeed));
    
    if (val < _LineWidth)
    {
        o.Albedo = _LineColor;
    }
}
        ENDCG
    }
FallBack"Diffuse"
}
