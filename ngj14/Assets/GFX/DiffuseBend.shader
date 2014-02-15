Shader "Bend/Diffuse" {

	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Color ("Color (RGBA)", Color) = (1,1,1,1)
	}
	
	SubShader {
		pass{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			#include "Bend.cginc"
			
			float4 _MainTex_ST;
			sampler2D _MainTex;
 		
	 		half4 _Color;
 					
			struct v2f {
			    float4 pos : SV_POSITION;
			    float2 uv : TEXCOORD0;
			};
			
			v2f vert (appdata_base v)
			{
			    v2f o;
			    o.pos = bend_ObjectToClip(v.vertex);
			    o.uv = TRANSFORM_TEX (v.texcoord, _MainTex);
			    return o;
			}
			
			half4 frag (v2f i) : COLOR
			{
				return tex2D (_MainTex, i.uv) * _Color;
			}
			
			ENDCG
		}	
	}
	
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Lambert vertex:vert
		#pragma target 3.0
		#pragma debug
		sampler2D _MainTex;
 		
 		float4 _PolyX;
 		float4 _PolyY;
 		float4 _PolyZ;
 		float4 _Color;
 
		struct Input {
			float2 uv_MainTex;
		};
		
		void vert (inout appdata_full v) 
		{
		  // SOMEHOW SKINNED MESH RENDERER IS FUCKED AT LONG DISTANCE WITH THIS SHADER
		  half4 v_ = v.vertex;
//		  v_.xyz *= unity_Scale.w;
	   	  float4 pos_proj = mul (UNITY_MATRIX_MV, v_); // using MVP changes the polynomial!!
//		  float t = length(pos_proj.xyz);
		  float t = pos_proj.z;
	   	  float3 bend = float3(t * t, t, 1);
//	   	  v.vertex.x += dot(_PolyX.xyz, bend);
	   	  v.vertex.y += dot(_PolyY.xyz, bend);
//	   	  v.vertex.z += dot(_PolyZ.xyz, bend);
//          v.vertex.x += pos_proj.z * pos_proj.z * _PolyX.x + pos_proj.z * _PolyX.y + _PolyX.z;
//          v.vertex.y += pos_proj.z * pos_proj.z * _PolyY.x + pos_proj.z * _PolyY.y + _PolyY.z;
//          v.vertex.z += pos_proj.z * pos_proj.z * _PolyZ.x + pos_proj.z * _PolyZ.y + _PolyZ.z;
      	}

		void surf (Input IN, inout SurfaceOutput o) 
		{
			half4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		ENDCG
	} 
	
	FallBack "Diffuse"
}
