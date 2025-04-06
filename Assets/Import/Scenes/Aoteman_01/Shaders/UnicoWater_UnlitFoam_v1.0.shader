Shader "UnicoWater_UnlitFoam_v1.0"
{
	Properties
	{
		_SpecularColor("SpecularColor", Color) = (1,1,1,0)
		_SpecularShine("SpecularShine", Range( 30 , 1000)) = 38.82353
		_SpecularDistortionTiling("SpecularDistortionTiling", Range( 5 , 50)) = 0
		_SpecularPanSpeed("SpecularPanSpeed", Range( 0 , 1)) = 0
		_SpecularDistortionTexture("SpecularDistortionTexture", 2D) = "bump" {}
		_CausticDetailTexture("CausticDetailTexture", 2D) = "white" {}
		_CausticDistortionTexture("CausticDistortionTexture", 2D) = "bump" {}
		_CausticDetailTiling("CausticDetailTiling", Range( 1 , 10)) = 0
		_CausticDetailDistortion("CausticDetailDistortion", Range( 1 , 10)) = 0
		_CausticDetailSpeed("CausticDetailSpeed", Range( 0 , 1)) = 0
		_CausticDetailOpacity("CausticDetailOpacity", Range( 0 , 1)) = 0
		_RefractionTexture("RefractionTexture", 2D) = "bump" {}
		_RefractionSpeed("RefractionSpeed", Range( 0 , 1)) = 0
		_CausticGroundSpeed("CausticGroundSpeed", Range( 0 , 1)) = 0
		_CausticGround("CausticGround", 2D) = "white" {}
		_CausticGroundDistortion("CausticGroundDistortion", Range( 0 , 20)) = 0
		_CausticGroundLerp("CausticGroundLerp", Range( 0 , 1)) = 0
		_CausticGroundStatic("CausticGroundStatic", 2D) = "white" {}
		_VerticalWaveFrequency("VerticalWaveFrequency", Range( 0 , 1)) = 0.3082287
		_VerticalWaveOffset("VerticalWaveOffset", Range( 0 , 1)) = 0.6117647
		_VerticalWaveAmplitude("VerticalWaveAmplitude", Range( 0 , 0.5)) = 0.07058823
		_DirectionWaveSpeed("DirectionWaveSpeed", Range( 0 , 5)) = 0.3082287
		_DirectionWaveMagnitude("DirectionWaveMagnitude", Range( 0 , 1)) = 0.3082287
		_DirectionWaveFrequency("DirectionWaveFrequency", Range( 0 , 10)) = 0.3082287
		_FoamTexture("FoamTexture", 2D) = "white" {}
		_FoamTransparent("FoamTransparent", Range( 0 , 1)) = 0
		_FoamDepth("FoamDepth", Range( 0 , 30)) = 10
		_FoamSmooth("FoamSmooth", Range( 0 , 1)) = 0.33
		_WaterTransparent("WaterTransparent", Range( 0 , 1)) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		GrabPass{ }
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#include "UnityCG.cginc"
		#pragma target 2.0
		#pragma surface surf Unlit alpha:fade keepalpha noshadow vertex:vertexDataFunc 
		struct Input
		{
			float3 worldNormal;
			INTERNAL_DATA
			float2 uv_texcoord;
			float3 worldPos;
			float4 screenPos;
		};

		uniform float _VerticalWaveOffset;
		uniform float _VerticalWaveFrequency;
		uniform float _VerticalWaveAmplitude;
		uniform float _DirectionWaveFrequency;
		uniform float _DirectionWaveSpeed;
		uniform float _DirectionWaveMagnitude;
		uniform float4 _SpecularColor;
		uniform sampler2D _SpecularDistortionTexture;
		uniform float _SpecularDistortionTiling;
		uniform float _SpecularPanSpeed;
		uniform float4 _SpecularDistortionTexture_ST;
		uniform float _SpecularShine;
		uniform sampler2D _FoamTexture;
		uniform float _CausticDetailTiling;
		uniform sampler2D _CausticDistortionTexture;
		uniform float _CausticDetailSpeed;
		uniform float _CausticDetailDistortion;
		uniform sampler2D _CausticGroundStatic;
		uniform float4 _CausticGroundStatic_ST;
		uniform sampler2D _CausticGround;
		uniform float _CausticGroundSpeed;
		uniform float4 _CausticGround_ST;
		uniform float _CausticGroundDistortion;
		uniform float _CausticGroundLerp;
		uniform sampler2D _CausticDetailTexture;
		uniform float _CausticDetailOpacity;
		uniform sampler2D _GrabTexture;
		uniform sampler2D _RefractionTexture;
		uniform float _RefractionSpeed;
		uniform float4 _RefractionTexture_ST;
		uniform float _FoamTransparent;
		uniform float _FoamDepth;
		uniform float _FoamSmooth;
		uniform float _WaterTransparent;
		uniform sampler2D _CameraDepthTexture;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float3 vertex3Pos = v.vertex.xyz;
			float4 VerticalMove = (float4(0.0 , ( ( _VerticalWaveOffset * sin( ( _VerticalWaveFrequency * _Time.y * distance( vertex3Pos , float3( 0,0,0 ) ) ) ) ) + vertex3Pos.y + ( _SinTime.w * _VerticalWaveAmplitude ) + ( 0.0 + 0.0 ) ) , 0.0 , 0.0));
			float3 VertexMoveDirection = float3(1,0,1);
			float VertexMoveDot = dot( vertex3Pos , VertexMoveDirection );
			float4 VertexPlaneMove = (float4(0.0 , ( ( sin( ( _DirectionWaveFrequency * ( ( VertexMoveDot / length( VertexMoveDirection ) ) - ( _Time.y * _DirectionWaveSpeed ) ) ) ) * _DirectionWaveMagnitude ) + vertex3Pos.y ) , 0.0 , 0.0));
			v.vertex.xyz += ( VerticalMove + VertexPlaneMove ).xyz;
		}

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			o.Normal = float3(0,0,1);
			#if defined(LIGHTMAP_ON) && ( UNITY_VERSION < 560 || ( defined(LIGHTMAP_SHADOW_MIXING) && !defined(SHADOWS_SHADOWMASK) && defined(SHADOWS_SCREEN) ) )
			float4 lightColor = 0;
			#else 
			float4 lightColor = _LightColor0;
			#endif 
			float2 SpecularTemp = (_SpecularDistortionTiling).xx;
			float2 SpecularUV = i.uv_texcoord * SpecularTemp;
			float2 SpecularPan = (_SpecularPanSpeed).xx;
			float2 SpecularDistort = i.uv_texcoord * _SpecularDistortionTexture_ST.xy + _SpecularDistortionTexture_ST.zw;
			float2 pan_distort = ( _Time.y * SpecularPan + SpecularDistort);
			float3 worldPos = i.worldPos;
			#if defined(LIGHTMAP_ON) && UNITY_VERSION < 560 
			float3 worldlightDir = 0;
			#else 
			float3 worldlightDir = normalize( UnityWorldSpaceLightDir( worldPos ) );
			#endif 
			float3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
			float3 normalLV = normalize( ( worldlightDir + worldViewDir ) );
			float normalDot = dot( (WorldNormalVector( i , UnpackNormal( tex2D( _SpecularDistortionTexture, ( float3( SpecularUV ,  0.0 ) + UnpackNormal( tex2D( _SpecularDistortionTexture, pan_distort ) ) ).xy ) ) )) , normalLV );
			float2 causticDetailValue = (_CausticDetailTiling).xx;
			float2 causticDetailUV = i.uv_texcoord * causticDetailValue;
			float2 causticSpeedValue = (_CausticDetailSpeed).xx;
			float2 causticDistortValue = (_CausticDetailDistortion).xx;
			float2 causticDistortUV = i.uv_texcoord * causticDistortValue;
			float2 causticPanDistory = ( _Time.y * causticSpeedValue + causticDistortUV);
			float3 causticNormalUV = ( float3( causticDetailUV ,  0.0 ) + UnpackNormal( tex2D( _CausticDistortionTexture, causticPanDistory ) ) );
			float2 uv_CausticGroundStatic = i.uv_texcoord * _CausticGroundStatic_ST.xy + _CausticGroundStatic_ST.zw;
			float2 causticGroundValue = (_CausticGroundSpeed).xx;
			float2 uv0_CausticGround = i.uv_texcoord * _CausticGround_ST.xy + _CausticGround_ST.zw;
			float2 causticGroundPan = ( _Time.y * causticGroundValue + uv0_CausticGround);
			float4 causticGroundLerp = lerp( tex2D( _CausticGroundStatic, uv_CausticGroundStatic ) , tex2D( _CausticGround, ( causticGroundPan + _CausticGroundDistortion ) ) , _CausticGroundLerp);
			float4 causticGroundOpacity = lerp( float4( 0,0,0,0 ) , tex2D( _CausticDetailTexture, causticNormalUV.xy ) , _CausticDetailOpacity);
			float3 vertex3Pos = mul( unity_WorldToObject, float4( i.worldPos , 1 ) );
			float4 unityObjectToClipPos47 = UnityObjectToClipPos( vertex3Pos );
			float4 computeGrabScreenPos39 = ComputeGrabScreenPos( unityObjectToClipPos47 );
			float2 RefractionSpeedValue = (_RefractionSpeed).xx;
			float2 uv0_RefractionTexture = i.uv_texcoord * _RefractionTexture_ST.xy + _RefractionTexture_ST.zw;
			float2 RefractionPan = ( _Time.x * RefractionSpeedValue + uv0_RefractionTexture);
			float3 RefractionTex = UnpackNormal( tex2D( _RefractionTexture, RefractionPan ) );
			float3 worldNormal = WorldNormalVector( i, float3( 0, 0, 1 ) );
			float3 worldTangent = WorldNormalVector( i, float3( 1, 0, 0 ) );
			float3 worldBitangent = WorldNormalVector( i, float3( 0, 1, 0 ) );
			float3x3 tangentToWorldFast = float3x3(worldTangent.x,worldBitangent.x,worldNormal.x,worldTangent.y,worldBitangent.y,worldNormal.y,worldTangent.z,worldBitangent.z,worldNormal.z);
			float fresnelNdot = dot( mul(tangentToWorldFast,RefractionTex), worldViewDir );
			float fresnel = ( 0.1 + 1.5 * pow( 1.0 - fresnelNdot, 5.0 ) );
			float4 GrabScreenColor = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_GrabTexture,( computeGrabScreenPos39 + float4( RefractionTex , 0.0 ) + fresnel ).xy/( computeGrabScreenPos39 + float4( RefractionTex , 0.0 ) + fresnel ).w);
			//FoamFade
			float4 screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float Depth = LinearEyeDepth(UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture,UNITY_PROJ_COORD(screenPos))));
			float FoamLerp = lerp( 0 , pow( saturate( ( abs(Depth - screenPos.w) / _FoamDepth ) ) , 1-_FoamSmooth ) , _FoamTransparent);

			float4 Lerp = lerp( tex2D( _FoamTexture, causticNormalUV.xy ) , ( causticGroundLerp * ( causticGroundOpacity + GrabScreenColor ) ) , FoamLerp);
			o.Emission = ( ( lightColor * _SpecularColor * pow( max( normalDot , 0.0 ) , _SpecularShine ) ) + Lerp ).rgb;
			o.Alpha = _WaterTransparent;
		}

		ENDCG
	}
}
