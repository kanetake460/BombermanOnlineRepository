Shader"Custom/Texture"
{
    Properties{
        _MainTex("Texture",2D) = "white"{}
        _MoveSpeedX("MoveSpeedX",Float) = 0.1
        _MoveSpeedY("MoveSpeedY",Float) = 0.1
}
    
    SubShader
    {
        // �s�����ɐݒ�
        Tags { "RenderType"="Opaque" }

            CGPROGRAM
            // �V���h�E��`�悷��T�[�t�F�X�֐����w��
            #pragma surface surf Standard fullfowardshadows
            #pragma target 3.0
            
struct Input
{
    // uv[�e�N�X�`���ϐ���]�ŃT�[�t�F�C�X�V�F�[�_�[��uv���W�������I�ɓn�����
    float2 uv_MainTex;
};

sampler2D _MainTex;
float _MoveSpeedX;
float _MoveSpeedY;

void surf(Input IN, inout SurfaceOutputStandard o)
{
    fixed2 uv = IN.uv_MainTex;
    uv.x += _Time * _MoveSpeedX;
    uv.y += _Time * _MoveSpeedY;
    o.Albedo = tex2D(_MainTex, uv);
}
        ENDCG
    }
FallBack"Diffuse"
}
