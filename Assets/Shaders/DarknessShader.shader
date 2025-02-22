Shader "Custom/DarknessShader"
{
    Properties
    {
        _MousePos ("Mouse Position", Vector) = (0.5, 0.5, 0, 0)
        _Radius ("Radius", Float) = 0.1
    }
    SubShader
    {
        Tags { "Queue"="Overlay" "RenderType"="Transparent" }
        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
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

            float2 _MousePos;
            float _Radius;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float dist = distance(i.uv, _MousePos);
                float alpha = 1 - smoothstep(_Radius, _Radius - 0.05, dist);
                return fixed4(0, 0, 0, alpha);
            }
            ENDCG
        }
    }
}
