Shader "Custom/BlobShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Ghost Color", Color) = (0.5, 0.8, 1.0, 0.5)
        _FresnelPower ("Fresnel Power", Range(1, 10)) = 3
        _WaveStrength ("Wave Strength", Range(0, 0.2)) = 0.02
        _WaveSpeed ("Wave Speed", Range(0, 100)) = 3.0
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 200

        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        ZWrite Off

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
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float3 worldPos : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _Color;
            float _FresnelPower;
            float _WaveStrength;
            float _WaveSpeed;
        

            v2f vert (appdata v)
            {
                v2f o;
                
                // Apply a sinusoidal wave distortion to the vertex position
                float wave = sin(_Time * _WaveSpeed + v.vertex.x * 5.0) * _WaveStrength;
                v.vertex.y += wave;

                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Sample the main texture
                fixed4 col = tex2D(_MainTex, i.uv);

                // Calculate the fresnel effect
                float3 viewDir = normalize(_WorldSpaceCameraPos - i.worldPos);
                float fresnel = pow(1.0 - abs(dot(viewDir, float3(0, 0, 1))), _FresnelPower);

                // Apply fresnel and color tint to create a ghostly effect
                col.rgb = lerp(col.rgb, _Color.rgb, fresnel);
                col.a *= _Color.a;

                return col;
            }
            ENDCG
        }
    }
}
