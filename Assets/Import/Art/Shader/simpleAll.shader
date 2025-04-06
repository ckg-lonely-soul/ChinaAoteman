// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "SimpleAll"
{
	Properties
	{
		[Enum(AlphaBlend,10,Additive,1)]_Dst("材质模式", Float) = 1
		[Enum(UnityEngine.Rendering.CullMode)]_CullMode("剔除模式", Float) = 0
		[Toggle(_A_R_ON)] _A_R("A_R", Float) = 1
		[HDR]_MainColor("MainColor", Color) = (1,1,1,1)
		[Header(MainTex)]_MainTex("MainTex", 2D) = "white" {}
		[Toggle(_ONE_UV_ON)] _one_UV("one_UV", Float) = 0
		_MainTex_PannerSpeedU("MainTex_PannerSpeedU", Float) = 0
		_MainTex_PannerSpeedV("MainTex_PannerSpeedV", Float) = 0
		[Header(MASKTEX)]_MaskTex("MaskTex", 2D) = "white" {}
		[Header(DissovleTex)]_DissovleTex("DissovleTex", 2D) = "white" {}
		[Toggle(_USE_DISSLOVE_ON)] _use_disslove("use_disslove", Float) = 0
		[Toggle(_DISSSC_ON)] _DissSC("Diss,S/C", Float) = 0
		_smooth("smooth", Range( 0.5 , 1)) = 0.5
		_Disspower("Disspower", Range( 0 , 1)) = 0.5
		_Dissovle_U_speed("Dissovle_U_speed", Float) = 0
		_Dissovle_V_speed("Dissovle_V_speed", Float) = 0
		[Header(NIUQU_Tex)]_NoiseTex("NoiseTex", 2D) = "white" {}
		[Toggle]_NiuquOn("NiuquOn", Float) = 1
		_NoiseIntensity("NoiseIntensity", Float) = 0
		_NoiseTex_PannerSpeedU("NoiseTex_PannerSpeedU", Float) = 0
		_NoiseTex_PannerSpeedV("NoiseTex_PannerSpeedV", Float) = 0
		_Sorft("Sorft", Float) = 0
		[HDR]_EainColor("EainColor", Color) = (1,1,1,1)
		_TextureSample0("Texture Sample 0", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] _texcoord2( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Custom"  "Queue" = "Transparent+0" "ForceNoShadowCasting" = "True" "IsEmissive" = "true"  }
		Cull [_CullMode]
		ZWrite Off
		Blend SrcAlpha [_Dst]
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#include "UnityCG.cginc"
		#pragma target 3.0
		#pragma shader_feature _ONE_UV_ON
		#pragma shader_feature _USE_DISSLOVE_ON
		#pragma shader_feature _DISSSC_ON
		#pragma shader_feature _A_R_ON
		#pragma surface surf Unlit keepalpha noshadow noambient novertexlights nolightmap  nodynlightmap nodirlightmap nofog 
		#undef TRANSFORM_TEX
		#define TRANSFORM_TEX(tex,name) float4(tex.xy * name##_ST.xy + name##_ST.zw, tex.z, tex.w)
		struct Input
		{
			float2 uv_texcoord;
			float4 uv2_texcoord2;
			float4 vertexColor : COLOR;
			float4 screenPos;
		};

		uniform half _CullMode;
		uniform half _Dst;
		uniform sampler2D _MainTex;
		uniform half _MainTex_PannerSpeedU;
		uniform half _MainTex_PannerSpeedV;
		uniform float4 _MainTex_ST;
		uniform half _NoiseIntensity;
		uniform sampler2D _NoiseTex;
		uniform half _NoiseTex_PannerSpeedU;
		uniform half _NoiseTex_PannerSpeedV;
		uniform float4 _NoiseTex_ST;
		uniform half _NiuquOn;
		uniform half4 _MainColor;
		uniform half _smooth;
		uniform sampler2D _DissovleTex;
		uniform half _Dissovle_U_speed;
		uniform half _Dissovle_V_speed;
		uniform float4 _DissovleTex_ST;
		uniform half _Disspower;
		uniform sampler2D _TextureSample0;
		uniform float4 _TextureSample0_ST;
		uniform half4 _EainColor;
		uniform sampler2D _MaskTex;
		uniform float4 _MaskTex_ST;
		UNITY_DECLARE_DEPTH_TEXTURE( _CameraDepthTexture );
		uniform float4 _CameraDepthTexture_TexelSize;
		uniform half _Sorft;

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			half2 appendResult12 = (half2(( _MainTex_PannerSpeedU * _Time.y ) , ( _MainTex_PannerSpeedV * _Time.y )));
			float2 uv_MainTex = i.uv_texcoord * _MainTex_ST.xy + _MainTex_ST.zw;
			half2 appendResult51 = (half2(i.uv2_texcoord2.x , i.uv2_texcoord2.y));
			half2 MainUVWave218 = appendResult51;
			#ifdef _ONE_UV_ON
				half2 staticSwitch52 = ( uv_MainTex + MainUVWave218 );
			#else
				half2 staticSwitch52 = ( appendResult12 + uv_MainTex );
			#endif
			half2 appendResult179 = (half2(_NoiseTex_PannerSpeedU , _NoiseTex_PannerSpeedV));
			float2 uv_NoiseTex = i.uv_texcoord * _NoiseTex_ST.xy + _NoiseTex_ST.zw;
			half2 panner175 = ( 1.0 * _Time.y * appendResult179 + uv_NoiseTex);
			half lerpResult225 = lerp( 0.0 , ( _NoiseIntensity * tex2D( _NoiseTex, panner175 ).r ) , _NiuquOn);
			half nq197 = lerpResult225;
			half4 tex2DNode1 = tex2D( _MainTex, ( staticSwitch52 + nq197 ) );
			half2 appendResult186 = (half2(_Dissovle_U_speed , _Dissovle_V_speed));
			float2 uv_DissovleTex = i.uv_texcoord * _DissovleTex_ST.xy + _DissovleTex_ST.zw;
			half2 panner183 = ( 1.0 * _Time.y * appendResult186 + ( nq197 + uv_DissovleTex ));
			half DissovleNum215 = i.uv2_texcoord2.w;
			#ifdef _DISSSC_ON
				half staticSwitch171 = DissovleNum215;
			#else
				half staticSwitch171 = _Disspower;
			#endif
			half smoothstepResult41 = smoothstep( ( 1.0 - _smooth ) , _smooth , ( ( tex2D( _DissovleTex, panner183 ).r + 1.0 ) - ( staticSwitch171 * 2.0 ) ));
			#ifdef _USE_DISSLOVE_ON
				half staticSwitch47 = smoothstepResult41;
			#else
				half staticSwitch47 = 1.0;
			#endif
			half Rongjie190 = staticSwitch47;
			float2 uv_TextureSample0 = i.uv_texcoord * _TextureSample0_ST.xy + _TextureSample0_ST.zw;
			#ifdef _ONE_UV_ON
				half2 staticSwitch233 = ( MainUVWave218 + uv_TextureSample0 );
			#else
				half2 staticSwitch233 = ( appendResult12 + uv_TextureSample0 );
			#endif
			half4 tex2DNode222 = tex2D( _TextureSample0, ( nq197 + staticSwitch233 ) );
			o.Emission = ( ( ( tex2DNode1 * _MainColor * Rongjie190 ) + ( tex2DNode222 * _EainColor * _EainColor.a * tex2DNode222.a ) ) * i.vertexColor ).rgb;
			#ifdef _A_R_ON
				half staticSwitch187 = tex2DNode1.a;
			#else
				half staticSwitch187 = tex2DNode1.r;
			#endif
			float2 uv_MaskTex = i.uv_texcoord * _MaskTex_ST.xy + _MaskTex_ST.zw;
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			half4 ase_screenPosNorm = ase_screenPos / ase_screenPos.w;
			ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
			float screenDepth202 = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE( _CameraDepthTexture, ase_screenPosNorm.xy ));
			half distanceDepth202 = abs( ( screenDepth202 - LinearEyeDepth( ase_screenPosNorm.z ) ) / ( _Sorft ) );
			half clampResult203 = clamp( distanceDepth202 , 0.0 , 1.0 );
			half smoothstepResult220 = smoothstep( 0.0 , 1.0 , clampResult203);
			o.Alpha = ( i.vertexColor.a * _MainColor.a * staticSwitch187 * tex2D( _MaskTex, uv_MaskTex ).r * Rongjie190 * smoothstepResult220 );
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18935
-41;392;1902;928;1910.434;1070.247;1.647089;True;False
Node;AmplifyShaderEditor.CommentaryNode;181;-4867.858,357.6961;Inherit;False;1515.276;569.0006;扭曲;11;166;197;167;172;175;179;174;176;177;168;225;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;176;-4855.922,536.8973;Inherit;False;Property;_NoiseTex_PannerSpeedU;NoiseTex_PannerSpeedU;20;0;Create;True;0;0;0;False;0;False;0;0.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;177;-4864.034,611.4391;Inherit;False;Property;_NoiseTex_PannerSpeedV;NoiseTex_PannerSpeedV;21;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;179;-4634.077,579.7075;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;174;-4836.229,389.6378;Inherit;False;0;166;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;175;-4573.251,363.7909;Inherit;True;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;166;-4261.03,477.0869;Inherit;True;Property;_NoiseTex;NoiseTex;17;1;[Header];Create;True;1;NIUQU_Tex;0;0;False;0;False;-1;None;f3ae1b09e79ff454f9222ce687d4d6d2;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;172;-4332.189,425.9956;Inherit;False;Property;_NoiseIntensity;NoiseIntensity;19;0;Create;True;0;0;0;False;0;False;0;0.05;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;168;-3902.151,750.7513;Inherit;False;Property;_NiuquOn;NiuquOn;18;1;[Toggle];Create;True;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;167;-3926.249,475.4066;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;225;-3681.522,464.2279;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;44;-3334.66,245.9301;Inherit;False;2294.923;710.0167;溶解;22;190;47;94;198;38;216;48;41;43;36;42;37;34;33;39;35;171;183;196;186;184;185;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;197;-3537.187,614.9479;Inherit;False;nq;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;50;-3642.406,-666.3568;Inherit;False;1;-1;4;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;185;-3302.555,589.8056;Inherit;False;Property;_Dissovle_V_speed;Dissovle_V_speed;16;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;94;-3293.625,362.9837;Inherit;False;0;33;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;184;-3301.26,491.5757;Inherit;False;Property;_Dissovle_U_speed;Dissovle_U_speed;15;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;198;-3244.748,288.8062;Inherit;False;197;nq;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;196;-3040.15,307.0147;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;215;-3398.185,-550.1063;Inherit;False;DissovleNum;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;186;-3099.485,529.4152;Inherit;True;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;216;-2932.224,754.7242;Inherit;True;215;DissovleNum;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;38;-2875.212,503.5468;Inherit;False;Property;_Disspower;Disspower;14;0;Create;True;0;0;0;False;0;False;0.5;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;183;-2890.878,347.5081;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.CommentaryNode;30;-2806.188,-1278.837;Inherit;False;1151.51;591.7782;UV流动;9;52;5;15;12;11;10;14;9;13;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleTimeNode;9;-2729.592,-1134.83;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;39;-2583.262,674.1678;Inherit;False;Constant;_Float1;Float 1;11;0;Create;True;0;0;0;False;0;False;2;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;14;-2759.865,-1005.541;Inherit;False;Property;_MainTex_PannerSpeedV;MainTex_PannerSpeedV;8;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;13;-2731.663,-1236.465;Inherit;False;Property;_MainTex_PannerSpeedU;MainTex_PannerSpeedU;7;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;51;-3373.232,-642.0042;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.StaticSwitch;171;-2617.044,575.8887;Inherit;False;Property;_DissSC;Diss,S/C;12;0;Create;True;0;0;0;False;0;False;0;0;1;True;;Toggle;2;Key0;Key1;Create;False;True;All;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;35;-2575.111,496.9176;Inherit;False;Constant;_Float0;Float 0;10;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;33;-2706.288,260.6174;Inherit;True;Property;_DissovleTex;DissovleTex;10;1;[Header];Create;True;1;DissovleTex;0;0;False;0;False;-1;None;f3ae1b09e79ff454f9222ce687d4d6d2;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;218;-3163.771,-647.2578;Inherit;False;MainUVWave;-1;True;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;42;-2151.938,597.0389;Inherit;False;Property;_smooth;smooth;13;0;Create;True;0;0;0;False;0;False;0.5;0.726;0.5;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;34;-2398.531,337.2845;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;11;-2454.605,-1125.625;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;10;-2450.446,-1223.8;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;37;-2390.934,589.1087;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;12;-2243.188,-1171.196;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;219;-2486.382,-522.6931;Inherit;False;218;MainUVWave;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;226;-2492.518,-316.5577;Inherit;False;0;222;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;43;-1956.661,459.6806;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;36;-2180.986,339.4373;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;15;-2559.681,-968.6773;Inherit;False;0;1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;232;-2058.728,-337.3336;Inherit;True;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;5;-2050.673,-1114.263;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;53;-2148.559,-737.908;Inherit;True;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SmoothstepOpNode;41;-1792.313,440.8488;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;48;-1725.623,321.4713;Inherit;False;Constant;_Float2;Float 2;14;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;234;-1879.964,-596.7407;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;200;-1656.391,-833.3648;Inherit;False;197;nq;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;233;-1628.521,-436.2202;Inherit;True;Property;_one_UV1;one_UV;6;0;Create;True;0;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Reference;52;False;True;All;9;1;FLOAT2;0,0;False;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT2;0,0;False;6;FLOAT2;0,0;False;7;FLOAT2;0,0;False;8;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.StaticSwitch;52;-1889.739,-940.2698;Inherit;True;Property;_one_UV;one_UV;6;0;Create;True;0;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;False;True;All;9;1;FLOAT2;0,0;False;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT2;0,0;False;6;FLOAT2;0,0;False;7;FLOAT2;0,0;False;8;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.StaticSwitch;47;-1513.602,390.5229;Inherit;False;Property;_use_disslove;use_disslove;11;0;Create;True;0;0;0;False;0;False;0;0;1;True;;Toggle;2;Key0;Key1;Create;False;True;All;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;227;-806.4162,-489.8524;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;204;-78.42273,465.171;Inherit;False;Property;_Sorft;Sorft;22;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;190;-1265.599,392.4347;Inherit;False;Rongjie;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;173;-1288.991,-953.1116;Inherit;True;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;222;-375.3048,-638.9958;Inherit;True;Property;_TextureSample0;Texture Sample 0;24;0;Create;True;0;0;0;False;0;False;-1;None;7f0c3d29d4289414c872ed497a76b184;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;136;-991.1163,89.57479;Inherit;False;854.5182;282.0116;MASK;2;130;45;;1,1,1,1;0;0
Node;AmplifyShaderEditor.DepthFade;202;114.3827,448.0206;Inherit;False;True;False;True;2;1;FLOAT3;0,0,0;False;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;188;189.7693,-399.7783;Inherit;False;190;Rongjie;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;1;-938.8505,-982.6295;Inherit;True;Property;_MainTex;MainTex;5;1;[Header];Create;True;1;MainTex;0;0;False;0;False;-1;None;1986bc1923083214fa77f09fc186acfa;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;214;-315.8822,-419.7786;Inherit;False;Property;_EainColor;EainColor;23;1;[HDR];Create;True;0;0;0;False;0;False;1,1,1,1;2,2,2,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;2;-547.3962,-830.5002;Inherit;False;Property;_MainColor;MainColor;4;1;[HDR];Create;True;0;0;0;False;0;False;1,1,1,1;0.6577306,0.6577306,0.6577306,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;130;-810.4621,163.0253;Inherit;False;0;45;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ClampOpNode;203;433.722,432.3729;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;224;208.6858,-642.5615;Inherit;True;4;4;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;4;-233.2962,-1054.249;Inherit;True;3;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StaticSwitch;187;-365.3806,-112.1144;Inherit;True;Property;_A_R;A_R;3;0;Create;True;0;0;0;False;0;False;0;1;1;True;;Toggle;2;Key0;Key1;Create;True;True;All;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;45;-495.5408,142.1358;Inherit;True;Property;_MaskTex;MaskTex;9;1;[Header];Create;True;1;MASKTEX;0;0;False;0;False;-1;None;dce7b0e55ec79ac42a23a00fa57def9d;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SmoothstepOpNode;220;741.5793,412.7292;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;228;454.5533,-745.4522;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;217;388.426,257.3423;Inherit;False;190;Rongjie;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.VertexColorNode;3;393.4702,-183.2528;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;212;1029.489,-267.0674;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;137;819.384,114.4189;Inherit;True;6;6;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;142;1394.788,-155.5156;Inherit;False;Property;_CullMode;剔除模式;1;1;[Enum];Create;False;0;0;1;UnityEngine.Rendering.CullMode;True;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;135;1393.304,-238.5472;Inherit;False;Property;_Dst;材质模式;0;1;[Enum];Create;False;0;2;AlphaBlend;10;Additive;1;0;True;0;False;1;10;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1328.835,-77.42953;Half;False;True;-1;2;ASEMaterialInspector;0;0;Unlit;SimpleAll;False;False;False;False;True;True;True;True;True;True;False;False;False;False;False;True;False;False;False;False;False;Off;2;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;False;0;True;Custom;;Transparent;All;16;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;2;5;False;-1;10;True;135;0;0;False;-1;0;False;-1;0;True;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;True;Relative;0;;2;-1;-1;-1;0;False;0;0;True;142;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;179;0;176;0
WireConnection;179;1;177;0
WireConnection;175;0;174;0
WireConnection;175;2;179;0
WireConnection;166;1;175;0
WireConnection;167;0;172;0
WireConnection;167;1;166;1
WireConnection;225;1;167;0
WireConnection;225;2;168;0
WireConnection;197;0;225;0
WireConnection;196;0;198;0
WireConnection;196;1;94;0
WireConnection;215;0;50;4
WireConnection;186;0;184;0
WireConnection;186;1;185;0
WireConnection;183;0;196;0
WireConnection;183;2;186;0
WireConnection;51;0;50;1
WireConnection;51;1;50;2
WireConnection;171;1;38;0
WireConnection;171;0;216;0
WireConnection;33;1;183;0
WireConnection;218;0;51;0
WireConnection;34;0;33;1
WireConnection;34;1;35;0
WireConnection;11;0;14;0
WireConnection;11;1;9;0
WireConnection;10;0;13;0
WireConnection;10;1;9;0
WireConnection;37;0;171;0
WireConnection;37;1;39;0
WireConnection;12;0;10;0
WireConnection;12;1;11;0
WireConnection;43;0;42;0
WireConnection;36;0;34;0
WireConnection;36;1;37;0
WireConnection;232;0;219;0
WireConnection;232;1;226;0
WireConnection;5;0;12;0
WireConnection;5;1;15;0
WireConnection;53;0;15;0
WireConnection;53;1;219;0
WireConnection;41;0;36;0
WireConnection;41;1;43;0
WireConnection;41;2;42;0
WireConnection;234;0;12;0
WireConnection;234;1;226;0
WireConnection;233;1;234;0
WireConnection;233;0;232;0
WireConnection;52;1;5;0
WireConnection;52;0;53;0
WireConnection;47;1;48;0
WireConnection;47;0;41;0
WireConnection;227;0;200;0
WireConnection;227;1;233;0
WireConnection;190;0;47;0
WireConnection;173;0;52;0
WireConnection;173;1;200;0
WireConnection;222;1;227;0
WireConnection;202;0;204;0
WireConnection;1;1;173;0
WireConnection;203;0;202;0
WireConnection;224;0;222;0
WireConnection;224;1;214;0
WireConnection;224;2;214;4
WireConnection;224;3;222;4
WireConnection;4;0;1;0
WireConnection;4;1;2;0
WireConnection;4;2;188;0
WireConnection;187;1;1;1
WireConnection;187;0;1;4
WireConnection;45;1;130;0
WireConnection;220;0;203;0
WireConnection;228;0;4;0
WireConnection;228;1;224;0
WireConnection;212;0;228;0
WireConnection;212;1;3;0
WireConnection;137;0;3;4
WireConnection;137;1;2;4
WireConnection;137;2;187;0
WireConnection;137;3;45;1
WireConnection;137;4;217;0
WireConnection;137;5;220;0
WireConnection;0;2;212;0
WireConnection;0;9;137;0
ASEEND*/
//CHKSM=C689C9441CA5AC9888D2D8387834B56DD824A809