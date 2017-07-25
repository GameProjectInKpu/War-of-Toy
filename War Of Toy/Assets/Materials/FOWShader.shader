Shader "Custom/FOWShader" {
	Properties{
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Focus (RGB)", 2D) = "white" {}
		_MainTex2("Gray (RGB)", 2D) = "white" {}
	}
		SubShader{
		//Tags { "RenderType"="Opaque" }
		//LOD 200

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
#pragma surface surf Lambert alpha:blend

		// Use shader model 3.0 target, to get nicer looking lighting
		//#pragma target 3.0

		sampler2D _MainTex;		// 텍스쳐 2개
		sampler2D _MainTex2;
		fixed4     _Color;


		struct Input {
			float2 uv_MainTex;
		};


		void surf(Input IN, inout SurfaceOutput o) {
			// Albedo comes from a texture tinted by color
			fixed4 colorFocus = tex2D(_MainTex, IN.uv_MainTex);
			fixed4 colorGray = tex2D(_MainTex2, IN.uv_MainTex);
			o.Albedo = _Color;
			float alpha = 0.8f - (colorFocus.g + colorGray.g);// / 1.5f;

			o.Alpha = alpha;
		
		}
		ENDCG
	}
		FallBack "Diffuse"
}

//Shader "Custom/FOWShader" {
//	Properties {
//		_Color ("Color", Color) = (1,1,1,1)
//		_MainTex ("Albedo (RGB)", 2D) = "white" {}
//		_Glossiness ("Smoothness", Range(0,1)) = 0.5
//		_Metallic ("Metallic", Range(0,1)) = 0.0
//	}
//	SubShader {
//		Tags { "RenderType"="Opaque" }
//		LOD 200
//		
//		CGPROGRAM
//		// Physically based Standard lighting model, and enable shadows on all light types
//		#pragma surface surf Standard fullforwardshadows
//
//		// Use shader model 3.0 target, to get nicer looking lighting
//		#pragma target 3.0
//
//		sampler2D _MainTex;
//
//		struct Input {
//			float2 uv_MainTex;
//		};
//
//		half _Glossiness;
//		half _Metallic;
//		fixed4 _Color;
//
//		void surf (Input IN, inout SurfaceOutputStandard o) {
//			// Albedo comes from a texture tinted by color
//			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
//			o.Albedo = c.rgb;
//			// Metallic and smoothness come from slider variables
//			o.Metallic = _Metallic;
//			o.Smoothness = _Glossiness;
//			o.Alpha = c.a;
//		}
//		ENDCG
//	}
//	FallBack "Diffuse"
//}
