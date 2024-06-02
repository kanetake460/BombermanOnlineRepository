Shader"Custom/RimLighting"
{
    Properties{
    _BaseColor("BaseColor",Color) = (1,1,1,1)
    _RimColor("RimColor",Color) = (1,1,1,1)
    _RimStrength("RimStrength",Range(0,30)) = 2.5
}
    
    SubShader
    {
        // �s�����Ɏw��
        Tags { "RenderType"="Opaque" }
        LOD 200

            CGPROGRAM
            // ������w�肷�邱�ƂŁA�������ŕ`�����Ƃ��\
            #pragma surface surf Standard
            #pragma target 3.0
            
            struct Input
            {
                float2 uv_MainTex;
                float3 worldNormal;
                float3 viewDir;
            };
            
            half4 _BaseColor;
            half4 _RimColor;
            float _RimStrength;

            void surf(Input IN,inout SurfaceOutputStandard o)
            {
                o.Albedo = _BaseColor;
    
                // �@���x�N�g���ƁA�J�����̃x�N�g���̓��ς�0�`1�ŕ\���A
                // ���]�����l�� rim�W�� �ɂ���
                float rim = 1 - saturate(dot(IN.viewDir, o.Normal));
    
                // ���̐F�ƁA�F�̋������G�~�b�V�����ɓ����
                o.Emission = _RimColor * pow(rim, _RimStrength);
            }
        ENDCG
    }
    FallBack "Diffuse"
}
