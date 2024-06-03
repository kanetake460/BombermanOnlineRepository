Shader"Custom/EmissionIce"
{
    Properties
    {
        _BaseColor("BaseColor", Color) = (1, 1, 1, 1)
        _ColorStrength("ColorStrength", Float) = 1.5
        _EmissionColor("EmissionColor",Color) = (1, 1, 1, 1)
        _LineWidth("LineWidth", Float) = 1.0
        _LineSpeed("LineSpeed", Float) = 50.0
        _LoopDuration("LoopDuration", Float) = 5.0
    }
    SubShader
    {
        Tags { "Queue" = "Transparent" }
LOD 200

        CGPROGRAM
        #pragma surface surf Standard alpha:fade
        #pragma target 3.0

#include "UnityCG.cginc"
#include "TakeshiCG.cginc"


struct Input
{
    float3 worldNormal;
    float3 worldPos;
    float3 viewDir;
};

fixed4 _BaseColor;
float _ColorStrength;
fixed4 _EmissionColor;
float _LineWidth;
float _LineSpeed;
float _LoopDuration;

void surf(Input IN, inout SurfaceOutputStandard o){
    o.Albedo = _BaseColor.rgb;
    float alpha = 1 - (abs(dot(IN.viewDir, IN.worldNormal)));
    o.Alpha = alpha * _ColorStrength;
    // ŽžŠÔ‚ª‚½‚Â‚É˜A‚ê‚Äval‚Í‘å‚«‚­‚È‚é
    float val = IN.worldPos.y - _LoopingTime(_LoopDuration) * _LineSpeed;
    
    if (val > 0 && val < _LineWidth)
    {
        o.Albedo = _EmissionColor;
    }
}
        ENDCG
    }
FallBack"Diffuse"
}
