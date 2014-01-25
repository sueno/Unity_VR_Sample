Shader "Custom/hoge Vertex Lit" {
	Properties {
 		_Color ("Main Color", Color) = (1,1,1,1)
 		_SpecColor ("Spec Color", Color) = (1,1,1,0)
 		_Emission ("Emmisive Color", Color) = (0,0,0,0)
 		_Shininess ("Shininess", Range (0.1, 1)) = 0.7
 		_MainTex ("Base (RGB) Trans. (Alpha)", 2D) = "white" { }
//		_MainTex ("Base (RGB)", 2D) = "white" {}
	}
 	Category {
 		ZWrite On
 		Cull Off
 		Alphatest Greater 0
 		Tags {Queue=Transparent}
 		Blend SrcAlpha OneMinusSrcAlpha 
 		SubShader {
 			Material {
 				Diffuse [_Color]
 				Ambient [_Color]
 				Shininess [_Shininess]
 				Specular [_SpecColor]
 				Emission [_Emission]	
 			}
 			Pass {
 				Lighting On
 				SeparateSpecular On
 				SetTexture [_MainTex] {
 					constantColor [_Color]
 					Combine texture * primary DOUBLE, texture * constant 
 				} 
 			}
 		} 
 	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Lambert

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
		};

		void surf (Input IN, inout SurfaceOutput o) {
			half4 c = tex2D (_MainTex, IN.uv_MainTex);
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
