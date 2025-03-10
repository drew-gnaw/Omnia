Shader "Custom/HighlightCutout"
{
    Properties
    {
        _CutoutPosition ("Cutout Position (UV)", Vector) = (0.5, 0.5, 0, 0)
        _CutoutSize ("Cutout Size", Float) = 0.2
        _OverlayColor ("Overlay Color", Color) = (0, 0, 0, 0.7) // Dark color
    }
    SubShader
    {
        Tags { "Queue"="Overlay" "RenderType"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        ColorMask RGB

        Pass
        {
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
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            float2 _CutoutPosition;
            float _CutoutSize;
            fixed4 _OverlayColor;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            half4 frag (v2f i) : SV_Target
            {
                float dist = distance(i.uv, _CutoutPosition);
                if (dist < _CutoutSize) discard; // Makes the highlight area transparent
                return _OverlayColor; // Dark overlay
            }
            ENDCG
        }
    }
}
