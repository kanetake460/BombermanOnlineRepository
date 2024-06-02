Shader"Custom/Texture"
{
    Properties{
        _MainTex("Texture",2D) = "white"{}
        _MoveSpeedX("MoveSpeedX",Float) = 0.1
        _MoveSpeedY("MoveSpeedY",Float) = 0.1
}
    
    SubShader
    {
        // 不透明に設定
        Tags { "RenderType"="Opaque" }

            CGPROGRAM
            // シャドウを描画するサーフェス関数を指定
            #pragma surface surf Standard fullfowardshadows
            #pragma target 3.0
            
struct Input
{
    // uv[テクスチャ変数名]でサーフェイスシェーダーのuv座標が自動的に渡される
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
