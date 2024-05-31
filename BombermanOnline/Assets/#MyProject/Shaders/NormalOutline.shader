Shader"Custm/NormalOutline"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _BaseColor("BaseColor", Color) = (1, 1, 1, 1)
        _OutlineColor("Outline Color", Color) = (1, 1, 1, 1)
        _OutlineWidth("Outline Width", float) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        
        Pass // 1
        {
Cull Front
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
#include "UnityCG.cginc"

struct appdata
{
    float4 vertex : POSITION;
    float3 normal : NORMAL;
};

struct v2f
{
    float4 vertex : SV_POSITION;
};

half _OutlineWidth;
half4 _OutlineColor;

v2f vert(appdata v)
{
    v2f o;
    o.vertex = UnityObjectToClipPos(v.vertex + v.normal * _OutlineWidth / 100);
    return o;
}

fixed4 frag(v2f i) : SV_Target
{
    return _OutlineColor;
}
            ENDCG
        }

        Pass // 2
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
#include "UnityCG.cginc"

struct appdata
{
    float4 vertex : POSITION;
    float2 uv : TEXCOORD0;
};

struct v2f
{
    float2 uv : TEXCOORD0;
    float4 vertex : SV_POSITION;
};

sampler2D _MainTex;
half4 _BaseColor;

v2f vert(appdata v)
{
    v2f o;
    o.vertex = UnityObjectToClipPos(v.vertex);
    o.uv = v.uv;
    return o;
}

fixed4 frag(v2f i) : SV_Target
{
    fixed4 texColor = tex2D(_MainTex, i.uv);
    return texColor * _BaseColor;
}
            ENDCG
        }
    }
}

