Shader"Custom/CrossLine"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
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
    fixed2 uv_MainTex;
};

sampler2D _MainTex;
fixed4 _BaseColor;
fixed4 _LineColor;
float _LineWidth;
float _LineSpeed;

void surf(Input IN, inout SurfaceOutputStandard o){
    o.Smoothness = 0.5;
    o.Albedo = tex2D(_MainTex,IN.uv_MainTex) * _BaseColor;
    
    fixed2 uv = IN.uv_MainTex;
    
    if(uv.x + 0.05 > uv.y && uv.x < uv.y + 0.05 ||
        1 - uv.x + 0.05 > uv.y && 1 - uv.x < uv.y + 0.05 ||
        uv.x + 0.05 > 1 - uv.y && uv.x < 1 - uv.y + 0.05)
    {
        o.Albedo = _LineColor;
    }
}
        ENDCG
    }
FallBack"Diffuse"
}
