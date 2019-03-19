Shader "Unlit/StencilMaskImage"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		[Enum(UnityEngine.Rendering.CompareFunction)] _ZTest ("ZTest", Float) = 0
	}
	SubShader
	{
		// No culling or depth
		Cull Off 
		ZWrite Off 
		ZTest [_ZTest]

		Pass
		{
			Stencil{
				Ref 2
				Comp Equal
				Pass keep
			}
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
				o.vertex = v.vertex * float4(2, 2, 0, 0) + float4(0, 0, 0, 1);
				o.uv = v.uv;
				return o;
			}
			
			sampler2D _MainTex;
			sampler2D _currentFrame;

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_currentFrame, float2(i.uv.x, 1 - i.uv.y));
				return 1 - col;
			}
			ENDCG
		}
	}
}
