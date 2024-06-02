Shader"Custom/RimLighting"
{
    Properties{
    _BaseColor("BaseColor",Color) = (1,1,1,1)
    _RimColor("RimColor",Color) = (1,1,1,1)
    _RimStrength("RimStrength",Range(0,30)) = 2.5
}
    
    SubShader
    {
        // 不透明に指定
        Tags { "RenderType"="Opaque" }
        LOD 200

            CGPROGRAM
            // これを指定することで、半透明で描くことが可能
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
    
                // 法線ベクトルと、カメラのベクトルの内積を0～1で表し、
                // 反転した値を rim係数 にする
                float rim = 1 - saturate(dot(IN.viewDir, o.Normal));
    
                // 光の色と、色の強さをエミッションに入れる
                o.Emission = _RimColor * pow(rim, _RimStrength);
            }
        ENDCG
    }
    FallBack "Diffuse"
}
