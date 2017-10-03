Shader "Custom/FOWShader" {
	Properties{
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Focus (RGB)", 2D) = "white" {}
		_MainTex2("Gray (RGB)", 2D) = "white" {}
		_MainTex3("Focus2 (RGB)", 2D) = "white" {}
		
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
		sampler2D _MainTex3;



	fixed4     _Color;


	struct Input {
		float2 uv_MainTex;

	};


	void surf(Input IN, inout SurfaceOutput o) {
		// Albedo comes from a texture tinted by color
		fixed4 colorFocus = tex2D(_MainTex, IN.uv_MainTex);	// unit
		fixed4 colorGray = tex2D(_MainTex2, IN.uv_MainTex);	// clear fog
		fixed4 colorFocus2 = tex2D(_MainTex3, IN.uv_MainTex);	// building


		o.Albedo = _Color;

		float alpha = 0.78f - (colorFocus.g + colorGray.g*0.25f + colorFocus2.g) ;	
		
		

		o.Alpha = alpha;

	}
	ENDCG
	}
		FallBack "Diffuse"
}

