Shader"Custom/LightOutLine"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _BaseColor("BaseColor", Color) = (1, 1, 1, 1)
        _Brightness("Brightness", Range(0.5, 2)) = 1

        _OutlineColor("Outline Color", Color) = (1, 1, 1, 1)
        _OutlineWidth("Outline Width", float) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }

        CGPROGRAM
        #pragma surface surf Lambert

struct Input
{
    float2 uv_MainTex;
};

sampler2D _MainTex;
half4 _BaseColor;
half _Brightness;

void surf(Input IN, inout SurfaceOutput o)
{
            // �e�N�X�`���̐F���擾
    half4 texColor = tex2D(_MainTex, IN.uv_MainTex);

            // �x�[�X�J���[��K�p
    o.Albedo = texColor.rgb * _BaseColor.rgb * _Brightness;

            // �A���t�@�l��K�p
    o.Alpha = texColor.a * _BaseColor.a;
}
        ENDCG
    }
}