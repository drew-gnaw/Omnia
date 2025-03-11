Shader "Custom/HighlightMask"
{
    Properties
    {
        _CutoutPosition ("Cutout Position", Vector) = (0.5, 0.5, 0, 0)
        _CutoutSize ("Cutout Size", Float) = 0.2
        _CutoutEnabled ("Cutout Enabled", Float) = 0 // 0 = Disabled, 1 = Enabled
        _MaskColor ("Mask Color", Color) = (0, 0, 0, 0.5)
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Overlay" }
        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            float4 _CutoutPosition;
            float _CutoutSize;
            float _CutoutEnabled;
            float4 _MaskColor;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // If the effect is disabled, return transparent
                if (_CutoutEnabled == 0) return fixed4(0, 0, 0, 0);

                // Calculate the distance from the center of the cutout
                float dist = distance(i.uv, _CutoutPosition.xy);

                // Create a smooth circular cutout
                float mask = smoothstep(_CutoutSize, _CutoutSize * 0.9, dist);

                // Apply the masking color (grey) around the cutout
                return lerp(_MaskColor, fixed4(0, 0, 0, 0), mask);
            }
            ENDCG
        }
    }
}
