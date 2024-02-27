Shader "Unlit/TransparentWall"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _MulTex("Texture", 2D) = "white" {}
        [HDR]_Color("Color", Color) = (1, 1, 1)
        _PlayerPos("PlayerPos", Vector) = (1, 1, 1, 0)
        _EdgeRange("EdgeRange", Vector) = (2, 6, 0, 0)
        _uvSpeed("uvSpeed", Vector) = (1, 0, 0, 0)
    }
        SubShader
        {
            Tags { "RenderType" = "Transparent" "Queue" = "Transparent" }

            Blend SrcAlpha OneMinusSrcAlpha

            Pass
            {
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag

                #include "UnityCG.cginc"

                struct appdata
                {
                    float4 vertex : POSITION;
                    float2 uv : TEXCOORD0;
                    float2 uv2 : TEXCOORD2;
                };

                struct v2f
                {
                    float2 uv : TEXCOORD0;
                    float2 uv2 : TEXCOORD2;
                    float4 vertex : SV_POSITION;
                    float3 vertexWorldPos : TEXCOORD1;
                };

                sampler2D _MainTex;
                sampler2D _MulTex;
                float4 _MainTex_ST;
                float4 _PlayerPos;
                float4 _MulTex_ST;
                float4 _EdgeRange;
                float4 _Color;
                float4 _uvSpeed;

                v2f vert(appdata v)
                {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                    o.uv2 = TRANSFORM_TEX(v.uv2, _MulTex);
                    o.uv2 += _Time.x * _uvSpeed;
                    o.vertexWorldPos = mul(unity_ObjectToWorld, v.vertex);
                    return o;
                }

                fixed4 frag(v2f i) : SV_Target
                {
                    float dis = abs(distance(i.vertexWorldPos, _PlayerPos));
                    fixed4 tex = tex2D(_MainTex, i.uv);
                    fixed4 tex2 = tex2D(_MulTex, i.uv2);
                    float4 color = _Color;

                    tex.a *= 1 - smoothstep(_EdgeRange.x, _EdgeRange.y, dis);
                    return tex * color * tex2;
                }
                ENDCG
            }
        }
}