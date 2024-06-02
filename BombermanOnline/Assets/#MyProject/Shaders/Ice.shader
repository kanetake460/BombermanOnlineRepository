Shader"Custom/Ice"
{
    Properties
    {
        _BaseColor("BaseColor",Color) = (1,1,1,1)
        _ColorStrength("ColorStrength",Float) = 1.5
    }
    SubShader
    {
        Tags { "Queue"="Transparent" }
        LOD 200

            CGPROGRAM
            #pragma surface surf Standard alpha:fade
            #pragma target 3.0
            
            struct Input
            {
                float3 worldNormal;
                float3 viewDir;
            };

            fixed4 _BaseColor;
            float _ColorStrength;

            void surf(Input IN,inout SurfaceOutputStandard o)
            {
                o.Albedo = _BaseColor;
                float alpha = 1 - (abs(dot(IN.viewDir, IN.worldNormal)));
                o.Alpha = alpha * _ColorStrength;
            }
        ENDCG
    }
    FallBack "Diffuse"
}
