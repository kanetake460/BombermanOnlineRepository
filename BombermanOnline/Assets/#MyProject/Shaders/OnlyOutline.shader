Shader"Custm/OnlyOutline"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        [Toggle]_ZWrite ("ZWrite",Float) = 0
        _Alpha("Alpha",Range(0,1)) = 1
        _OutlineColor("Outline Color", Color) = (1, 1, 1, 1)
        _OutlineWidth("Outline Width", float) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Transparent"}
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite [_ZWrite]

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
                float3 normal : NORMAL;
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
                // 視線の方向ベクトルを取得
                float3 viewDir = normalize(_WorldSpaceCameraPos - i.vertex.xyz);

                // 視線の方向と頂点の法線の内積を計算
                float dotProduct = dot(i.normal, viewDir);

                // 内積が0未満の場合、頂点が視点から隠れているとみなし、フラグメントを破棄する
                if (dotProduct < 0.0)
                {
                    discard;
                }
        
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
            half _Alpha;

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
                texColor.a *= _Alpha;
                return texColor;
            }
            ENDCG
        }
    }
}
