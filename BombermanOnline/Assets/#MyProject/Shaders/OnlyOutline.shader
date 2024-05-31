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
                // �����̕����x�N�g�����擾
                float3 viewDir = normalize(_WorldSpaceCameraPos - i.vertex.xyz);

                // �����̕����ƒ��_�̖@���̓��ς��v�Z
                float dotProduct = dot(i.normal, viewDir);

                // ���ς�0�����̏ꍇ�A���_�����_����B��Ă���Ƃ݂Ȃ��A�t���O�����g��j������
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
