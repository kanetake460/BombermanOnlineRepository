Shader"Custom/EmissionIce"
{
    Properties
    {
        _BaseColor("BaseColor",Color) = (1,1,1,1)
        _ColorStrength("ColorStrength",Float) = 1.5
        _EmissionProbability("EmissionProbability",Float) = 10
    }
    SubShader
    {
        Tags { "Queue"="Transparent" }
        LOD 200

            CGPROGRAM
            #pragma surface surf Standard alpha:fade
            #pragma target 3.0

            #include "TakeshiCG.cginc"
            
            struct Input
            {
                float3 worldNormal;
                float3 viewDir;
            };

            fixed4 _BaseColor;
            float _ColorStrength;
            float __EmissionProbability;

            void surf(Input IN,inout SurfaceOutputStandard o)
            {
                o.Albedo = _BaseColor;
                float alpha = 1 - (abs(dot(IN.viewDir, IN.worldNormal)));
                o.Alpha = alpha * _ColorStrength;
                o.Alpha = hash();
            }
        ENDCG
    }
    FallBack "Diffuse"
}
