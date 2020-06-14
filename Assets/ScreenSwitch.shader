Shader "Unlit/ScreenSwitch"
{
    Properties
    { 
		_Color("Color",Color) = (0.0,0.0,0.0,0.0)
    }
    SubShader
    {
		Tags {"Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent"}
		LOD 100
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
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv =v.uv;
                return o;
            }
			float4 _Value1;
			fixed4 _Color;
            fixed4 frag (v2f i) : SV_Target
            {
				fixed4 col = _Color;
				if (_Value1.z != 0) 
				{			
					float d = distance(float2(i.uv.x*16.0/9.0,i.uv.y), float2(_Value1.x*16.0/9.0,_Value1.y));
					d = d* d;
					d = max(d, 0.025);
					col.a = d * _Value1.z + _Value1.z;
				}			
				return col;
            }
            ENDCG
        }
    }
}
