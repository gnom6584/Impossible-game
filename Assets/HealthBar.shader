Shader "Unlit/HealthBar"
{
    Properties
    {
		_OutLine("Outline",Color) = (0,0,0,1)
		_Healthbar("Healthbar",Color) = (1,0,0,1)
		_Modifer("Modifer",float) = 32
		_ResolutionX("ResolutionX", Int) = 32
		_ResolutionY("ResolutionY", Int) = 6
		[MaterialToggle] _isToggled("Centre", Float) = 0
    }
    SubShader
    {
		Tags {"Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent"}
		LOD 100
		Cull Off
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
                float4 vertex : SV_POSITION;
            };


            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
                return o;
            }
			fixed4 _OutLine;
			fixed4 _Healthbar;
			float _Modifer;
			uint _ResolutionY;
			uint _ResolutionX;
			float _isToggled;
            fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = 0;
				uint pixelx = (i.uv.x*_ResolutionX);
				uint pixely = (i.uv.y*_ResolutionY);
				if (_isToggled != 0) {
					_Modifer /= 2;
					_Modifer += 0.5;
				}
				uint temp = _ResolutionX * _Modifer-1;
				
				if (_isToggled == 0)
				{
					if (pixelx < temp)
						col = _Healthbar;
					if (pixelx == temp)
					{
						col = _OutLine;
					}
					if (pixelx == 0)
					{
						col = _OutLine;
					}			
					if (pixelx <= temp)
					{
						if (pixely == _ResolutionY - 1)
						{
							col = _OutLine;
						}
						if (pixely == 0)
						{
							col = _OutLine;
						}
					}
				}
				else
				{
					
					if (pixelx < temp && pixelx > _ResolutionX - temp-1)
						col = _Healthbar;
					if (pixelx == temp)
					{
						col = _OutLine;
					}
					if (pixelx == _ResolutionX - temp -1)
					{
						col = _OutLine;
					}
					if (pixelx <= temp && pixelx >= _ResolutionX - temp)
					{
						if (pixely == _ResolutionY - 1)
						{
							col = _OutLine;
						}
						if (pixely == 0)
						{
							col = _OutLine;
						}
					}
				}
                return col;
            }
            ENDCG
        }
    }
}
