Shader "Custom/VHSPauseEffect"
{
    Properties
    {
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _NoiseTex ("Noise Texture", 2D) = "white" {}
        _DistortionStrength ("Distortion Strength", Range(0, 1)) = 0.5
    }
    
    SubShader
    {
        Tags { "RenderType"="Transparent" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows

        sampler2D _MainTex;
        sampler2D _NoiseTex;
        float _DistortionStrength;

        struct Input
        {
            float2 uv_MainTex;
        };

        void surf(Input IN, inout SurfaceOutputStandard o)
        {
            // Base color
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex);

            // Add noise for VHS effect
            float2 noiseUV = IN.uv_MainTex;
            noiseUV.y += _Time.y * 0.5; // Scroll noise texture vertically
            float noise = tex2D(_NoiseTex, noiseUV).r;
            
            // Apply distortion to create the VHS pause effect
            float2 distortedUV = IN.uv_MainTex;
            distortedUV.x += (noise - 0.5) * _DistortionStrength;
            fixed4 finalColor = tex2D(_MainTex, distortedUV);
            
            o.Albedo = finalColor.rgb;
            o.Alpha = finalColor.a;
        }
        ENDCG
    }

    FallBack "Diffuse"
}
