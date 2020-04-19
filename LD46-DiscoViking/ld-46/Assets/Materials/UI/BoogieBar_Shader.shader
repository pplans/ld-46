Shader "UI/BoogieBar_Shader"
{
    Properties
    {
        //[Header(Color)]
        _Color ("Main Color", Color) = (0.2,1,0.2,1)
        _InactiveColor ("Inactive color", Color) = (0.5, 0.5, 0.5, 1)
        _MaxBoogie ("Max boogie", int) = 15
        _Boogie ("Boggie", int) = 15
        _BorderWidth ("Border width", Float) = 1
        _ImageSize ("Image Size", Vector) = (100, 100, 0, 0)
        [MaterialToggle] _Invert ("Invert", int) = 0
    }
    SubShader
    {
        Cull Off
        Lighting Off
        ZWrite Off
        ZTest Off
        Blend One OneMinusSrcAlpha

        Tags
        { 
            "Queue"="Transparent" 
            "IgnoreProjector"="True" 
            "RenderType"="Transparent" 
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

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
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 _Color;
            fixed4 _InactiveColor;
            int _MaxBoogie;
            int _Boogie;
            half _BorderWidth;
            float4 _ImageSize;
            int _Invert;

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 c = _Color;

                float chunkPixelSize = _ImageSize.x / _MaxBoogie;
                float pixelPos = i.uv.x * _ImageSize.x;
                int chunkId = int(pixelPos / chunkPixelSize);

                const bool bInvert = _Invert > 0.0;
                const bool bInactive = bInvert ? (_MaxBoogie - chunkId > _Boogie) : ((chunkId+1) > _Boogie);

                if(bInactive)
                {
                    c *= _InactiveColor;
                    c.a = 0.25;
                }
                else
                {
                    c *= _Color;
                }

                if(fmod(pixelPos, chunkPixelSize) < _BorderWidth)
                {
                    c.a = 0;
                }
                  
                c.rgb *= c.a;
                return c;
            }
            ENDCG
        }
    }
}
