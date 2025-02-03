// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "TetraArts/Tatoon/Built-In/Tatoon_Built-In"
{
	Properties
	{
		_Cutoff( "Mask Clip Value", Float ) = 0.5
		_RimColor("Rim Color", Color) = (0,1,0.8758622,0)
		_DiffuseColor("Diffuse Color", Color) = (1,1,1,0)
		_RimBlend("Rim Blend", Range( 0 , 10)) = 0
		[NoScaleOffset][Normal]_Normal("Normal", 2D) = "bump" {}
		[Toggle]_UseNormalMap("UseNormalMap", Float) = 0
		[NoScaleOffset]_TextureDiffuse("Texture Diffuse", 2D) = "white" {}
		_NormalStrength("Normal Strength", Float) = 1
		_RimSize("Rim Size", Range( 0 , 2)) = 0.5679239
		_ShadowColor("Shadow Color", Color) = (1,0,0,1)
		_ShadowSize("Shadow Size", Range( 0 , 2)) = 0
		[NoScaleOffset]_ShadowTexture("Shadow Texture", 2D) = "white" {}
		_ShadowBlend("ShadowBlend", Range( 0 , 1)) = 0
		[Toggle]_UseRim("UseRim", Float) = 0
		[Toggle]_ShadowTextureViewProjection("Shadow Texture View Projection", Float) = 0
		_ShadowTextureTiling("Shadow Texture Tiling", Float) = 0
		_ShadowTextureRotation("Shadow Texture Rotation", Float) = 0
		[Toggle]_UseShadow("UseShadow", Float) = 0
		[Toggle]_UseGradient("Use Gradient", Float) = 0
		[NoScaleOffset]_RimTexture("Rim Texture", 2D) = "white" {}
		[Toggle]_RimTextureViewProjection("Rim Texture View Projection", Float) = 0
		_ColorB("Color B", Color) = (0,0.1264467,1,0)
		_ColorA("Color A", Color) = (1,0,0,0)
		_RimTextureTiling("Rim Texture Tiling", Float) = 0
		_GradientSize("Gradient Size", Range( 0 , 10)) = 0
		_RimTextureRotation("Rim Texture Rotation", Float) = 0
		_GradientPosition("Gradient Position", Float) = 0
		_GradientRotation("Gradient Rotation", Float) = 0
		[Toggle]_UseSpecular("UseSpecular", Float) = 0
		[HideInInspector][NoScaleOffset][Normal]_Texture0("Texture 0", 2D) = "bump" {}
		[NoScaleOffset]_SpecularMap("Specular Map", 2D) = "gray" {}
		[Toggle]_SpecularTextureViewProjection("Specular Texture View Projection", Float) = 0
		_SpecularTextureTiling("Specular Texture Tiling", Float) = 0
		_SpecularTextureRotation("Specular Texture Rotation", Float) = 0
		_SpecularColor("Specular Color", Color) = (1,0.9575656,0.75,0)
		[Toggle]_SpecLightColor("Spec Light Color", Float) = 0
		_SpecularLightIntensity("Specular Light Intensity", Range( 0 , 10)) = 1
		_SpecularSize("Specular Size", Range( 0 , 1)) = 0.005
		_SpecularBlend("Specular Blend", Range( 0 , 1)) = 0
		[HDR]_OutlineColor("Outline Color", Color) = (0,0,0,0)
		[Toggle]_RimLightColor("Rim Light Color", Float) = 0
		_RimLightIntensity("Rim Light Intensity", Range( 0 , 10)) = 0
		_OutlineSize("Outline Size", Float) = 0.1
		[Toggle]_UseOutline("UseOutline", Float) = 0
		[HDR]_OutlineColor2("OutlineColor2", Color) = (1,0.06464233,0,0)
		[HDR]_OutlineColor1("OutlineColor1", Color) = (0,0.9740796,1,0)
		_Speed("Speed", Range( -1 , 1)) = 0
		_PowerColor1("PowerColor1", Float) = 1
		_PowerColor2("PowerColor2", Float) = 1
		[NoScaleOffset]_Texture("Texture", 2D) = "white" {}
		[Toggle]_UseOutlineFire("UseOutlineFire", Float) = 0
		_NoiseTextureScale("NoiseTextureScale", Float) = 2
		_AttenuationPower("AttenuationPower", Float) = 1
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "TransparentCutout"  "Queue" = "AlphaTest+0"}
		ZWrite Off
		Cull Back
		CGPROGRAM
		#pragma target 3.0
		#pragma surface outlineSurf Outline nofog  keepalpha noshadow noambient novertexlights nolightmap nodynlightmap nodirlightmap nometa noforwardadd vertex:outlineVertexDataFunc 
		
		void outlineVertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float outlineVar = (( _UseOutline )?( _OutlineSize ):( 0.0 ));
			v.vertex.xyz += ( v.normal * outlineVar );
		}
		inline half4 LightingOutline( SurfaceOutput s, half3 lightDir, half atten ) { return half4 ( 0,0,0, s.Alpha); }
		void outlineSurf( Input i, inout SurfaceOutput o )
		{
			float2 uv_TextureDiffuse108 = i.uv_texcoord;
			float4 tex2DNode108 = tex2D( _TextureDiffuse, uv_TextureDiffuse108 );
			float Alpha111 = tex2DNode108.a;
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float3 ase_worldNormal = WorldNormalVector( i, float3( 0, 0, 1 ) );
			float dotResult244 = dot( ase_worldViewDir , ase_worldNormal );
			float clampResult245 = clamp( dotResult244 , 0.0 , 1.0 );
			float saferPower247 = max( clampResult245 , 0.0001 );
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_screenPosNorm = ase_screenPos / ase_screenPos.w;
			ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
			float4 appendResult274 = (float4(0.0 , ( _Speed * _Time.y ) , 0.0 , 0.0));
			float Noise287 = tex2D( _Texture, (( ase_screenPosNorm + appendResult274 )*_NoiseTextureScale + 0.0).xy ).r;
			float clampResult251 = clamp( ( ( pow( saferPower247 , _PowerColor1 ) - Noise287 ) * 5.0 ) , 0.0 , 1.0 );
			float saferPower411 = max( clampResult245 , 0.0001 );
			float clampResult254 = clamp( ( 10.0 * ( pow( saferPower411 , _PowerColor2 ) - Noise287 ) ) , 0.0 , 1.0 );
			float4 OutlineResult293 = ( ( _OutlineColor1 * clampResult251 ) + ( ( clampResult254 - clampResult251 ) * _OutlineColor2 ) );
			float NoiseAlpha290 = clampResult254;
			o.Emission = (( _UseOutlineFire )?( OutlineResult293 ):( float4( ( Alpha111 * (_OutlineColor).rgb ) , 0.0 ) )).rgb;
			clip( (( _UseOutlineFire )?( NoiseAlpha290 ):( 1.0 )) - _Cutoff );
			o.Normal = float3(0,0,-1);
		}
		ENDCG
		

		Tags{ "RenderType" = "Opaque"  "Queue" = "AlphaTest+0" }
		Cull Off
		ZWrite On
		CGINCLUDE
		#include "UnityPBSLighting.cginc"
		#include "UnityShaderVariables.cginc"
		#include "UnityCG.cginc"
		#include "UnityStandardUtils.cginc"
		#include "Lighting.cginc"
		#pragma target 4.0
		#ifdef UNITY_PASS_SHADOWCASTER
			#undef INTERNAL_DATA
			#undef WorldReflectionVector
			#undef WorldNormalVector
			#define INTERNAL_DATA half3 internalSurfaceTtoW0; half3 internalSurfaceTtoW1; half3 internalSurfaceTtoW2;
			#define WorldReflectionVector(data,normal) reflect (data.worldRefl, half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal)))
			#define WorldNormalVector(data,normal) half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal))
		#endif
		struct Input
		{
			float2 uv_texcoord;
			float3 worldPos;
			float3 worldNormal;
			INTERNAL_DATA
			float4 screenPos;
		};

		struct SurfaceOutputCustomLightingCustom
		{
			half3 Albedo;
			half3 Normal;
			half3 Emission;
			half Metallic;
			half Smoothness;
			half Occlusion;
			half Alpha;
			Input SurfInput;
			UnityGIInput GIData;
		};

		uniform float _UseShadow;
		uniform float4 _ShadowColor;
		uniform sampler2D _ShadowTexture;
		uniform float _ShadowTextureViewProjection;
		uniform float _ShadowTextureTiling;
		uniform float _ShadowTextureRotation;
		uniform float _UseGradient;
		uniform float4 _DiffuseColor;
		uniform float4 _ColorA;
		uniform float4 _ColorB;
		uniform float _GradientPosition;
		uniform float _GradientSize;
		uniform float _GradientRotation;
		uniform sampler2D _TextureDiffuse;
		uniform float _ShadowSize;
		uniform float _ShadowBlend;
		uniform float _AttenuationPower;
		uniform float _UseRim;
		uniform float _RimSize;
		uniform float _RimBlend;
		uniform float _RimLightColor;
		uniform float4 _RimColor;
		uniform float _RimLightIntensity;
		uniform sampler2D _RimTexture;
		uniform float _RimTextureViewProjection;
		uniform float _RimTextureTiling;
		uniform float _RimTextureRotation;
		uniform float _UseSpecular;
		uniform sampler2D _SpecularMap;
		uniform float _SpecularTextureViewProjection;
		uniform float _SpecularTextureTiling;
		uniform float _SpecularTextureRotation;
		uniform float _SpecLightColor;
		uniform float4 _SpecularColor;
		uniform float _SpecularLightIntensity;
		uniform float _SpecularSize;
		uniform float _SpecularBlend;
		uniform float _UseNormalMap;
		uniform sampler2D _Texture0;
		uniform sampler2D _Normal;
		uniform float _NormalStrength;
		uniform float _UseOutlineFire;
		uniform float4 _OutlineColor;
		uniform float4 _OutlineColor1;
		uniform float _PowerColor1;
		uniform sampler2D _Texture;
		uniform float _Speed;
		uniform float _NoiseTextureScale;
		uniform float _PowerColor2;
		uniform float4 _OutlineColor2;
		uniform float _Cutoff = 0.5;
		uniform float _UseOutline;
		uniform float _OutlineSize;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float3 Outline219 = 0;
			v.vertex.xyz += Outline219;
			v.vertex.w = 1;
		}

		inline half4 LightingStandardCustomLighting( inout SurfaceOutputCustomLightingCustom s, half3 viewDir, UnityGI gi )
		{
			UnityGIInput data = s.GIData;
			Input i = s.SurfInput;
			half4 c = 0;
			#ifdef UNITY_PASS_FORWARDBASE
			float ase_lightAtten = data.atten;
			if( _LightColor0.a == 0)
			ase_lightAtten = 0;
			#else
			float3 ase_lightAttenRGB = gi.light.color / ( ( _LightColor0.rgb ) + 0.000001 );
			float ase_lightAtten = max( max( ase_lightAttenRGB.r, ase_lightAttenRGB.g ), ase_lightAttenRGB.b );
			#endif
			#if defined(HANDLE_SHADOWS_BLENDING_IN_GI)
			half bakedAtten = UnitySampleBakedOcclusion(data.lightmapUV.xy, data.worldPos);
			float zDist = dot(_WorldSpaceCameraPos - data.worldPos, UNITY_MATRIX_V[2].xyz);
			float fadeDist = UnityComputeShadowFadeDistance(data.worldPos, zDist);
			ase_lightAtten = UnityMixRealtimeAndBakedShadows(data.atten, bakedAtten, UnityComputeShadowFade(fadeDist));
			#endif
			float4 color74 = IsGammaSpace() ? float4(1,1,1,1) : float4(1,1,1,1);
			float2 temp_cast_0 = (_ShadowTextureTiling).xx;
			float2 uv_TexCoord65 = i.uv_texcoord * temp_cast_0;
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldViewDir = Unity_SafeNormalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float cos70 = cos( radians( _ShadowTextureRotation ) );
			float sin70 = sin( radians( _ShadowTextureRotation ) );
			float2 rotator70 = mul( (( _ShadowTextureViewProjection )?( ( ( _ShadowTextureTiling * 1 ) * mul( UNITY_MATRIX_VP, float4( ase_worldViewDir , 0.0 ) ).xyz ) ):( float3( uv_TexCoord65 ,  0.0 ) )).xy - float2( 0,0 ) , float2x2( cos70 , -sin70 , sin70 , cos70 )) + float2( 0,0 );
			float4 ShadowColor80 = (( _UseShadow )?( ( _ShadowColor * tex2D( _ShadowTexture, rotator70 ) ) ):( color74 ));
			#if defined(LIGHTMAP_ON) && ( UNITY_VERSION < 560 || ( defined(LIGHTMAP_SHADOW_MIXING) && !defined(SHADOWS_SHADOWMASK) && defined(SHADOWS_SCREEN) ) )//aselc
			float4 ase_lightColor = 0;
			#else //aselc
			float4 ase_lightColor = _LightColor0;
			#endif //aselc
			float3 ase_worldNormal = WorldNormalVector( i, float3( 0, 0, 1 ) );
			UnityGI gi11 = gi;
			float3 diffNorm11 = ase_worldNormal;
			gi11 = UnityGI_Base( data, 1, diffNorm11 );
			float3 indirectDiffuse11 = gi11.indirect.diffuse + diffNorm11 * 0.0001;
			float4 Ambient181 = ( ase_lightColor * float4( indirectDiffuse11 , 0.0 ) );
			float3 ase_vertex3Pos = mul( unity_WorldToObject, float4( i.worldPos , 1 ) );
			float4 appendResult93 = (float4(ase_vertex3Pos.x , ase_vertex3Pos.y , 0.0 , 0.0));
			float cos95 = cos( radians( _GradientRotation ) );
			float sin95 = sin( radians( _GradientRotation ) );
			float2 rotator95 = mul( appendResult93.xy - float2( 0,0 ) , float2x2( cos95 , -sin95 , sin95 , cos95 )) + float2( 0,0 );
			float smoothstepResult101 = smoothstep( _GradientPosition , ( _GradientPosition + _GradientSize ) , rotator95.x);
			float4 lerpResult102 = lerp( _ColorA , _ColorB , smoothstepResult101);
			float2 uv_TextureDiffuse108 = i.uv_texcoord;
			float4 tex2DNode108 = tex2D( _TextureDiffuse, uv_TextureDiffuse108 );
			float4 Color110 = ( ( Ambient181 + (( _UseGradient )?( lerpResult102 ):( ( ase_lightColor * _DiffuseColor ) )) ) * tex2DNode108 );
			float Atten328 = pow( ase_lightAtten , _AttenuationPower );
			float3 ase_normWorldNormal = normalize( ase_worldNormal );
			#if defined(LIGHTMAP_ON) && UNITY_VERSION < 560 //aseld
			float3 ase_worldlightDir = 0;
			#else //aseld
			float3 ase_worldlightDir = Unity_SafeNormalize( UnityWorldSpaceLightDir( ase_worldPos ) );
			#endif //aseld
			float dotResult3 = dot( ase_normWorldNormal , ase_worldlightDir );
			float NdotL175 = dotResult3;
			float smoothstepResult58 = smoothstep( _ShadowSize , ( _ShadowSize + _ShadowBlend ) , ( Atten328 + NdotL175 ));
			float temp_output_410_0 = ( smoothstepResult58 * Atten328 );
			float dotResult38 = dot( ase_worldNormal , ase_worldViewDir );
			float2 temp_cast_8 = (_RimTextureTiling).xx;
			float2 uv_TexCoord193 = i.uv_texcoord * temp_cast_8;
			float cos198 = cos( radians( _RimTextureRotation ) );
			float sin198 = sin( radians( _RimTextureRotation ) );
			float2 rotator198 = mul( (( _RimTextureViewProjection )?( ( ( _RimTextureTiling * 1 ) * mul( float4( ase_worldViewDir , 0.0 ), UNITY_MATRIX_VP ).xyz ) ):( float3( uv_TexCoord193 ,  0.0 ) )).xy - float2( 0,0 ) , float2x2( cos198 , -sin198 , sin198 , cos198 )) + float2( 0,0 );
			float4 Rim185 = (( _UseRim )?( ( saturate( ( NdotL175 * pow( ( 1.0 - saturate( ( dotResult38 + _RimSize ) ) ) , _RimBlend ) ) ) * ( (( _RimLightColor )?( float4( ( _RimLightIntensity * ase_lightColor.rgb ) , 0.0 ) ):( _RimColor )) * tex2D( _RimTexture, rotator198 ) ) ) ):( float4( 0,0,0,0 ) ));
			float2 temp_cast_13 = (_SpecularTextureTiling).xx;
			float2 uv_TexCoord143 = i.uv_texcoord * temp_cast_13;
			float cos156 = cos( radians( _SpecularTextureRotation ) );
			float sin156 = sin( radians( _SpecularTextureRotation ) );
			float2 rotator156 = mul( (( _SpecularTextureViewProjection )?( ( ( _SpecularTextureTiling * 1 ) * mul( float4( ase_worldViewDir , 0.0 ), UNITY_MATRIX_VP ).xyz ) ):( float3( uv_TexCoord143 ,  0.0 ) )).xy - float2( 0,0 ) , float2x2( cos156 , -sin156 , sin156 , cos156 )) + float2( 0,0 );
			float temp_output_161_0 = ( 1.0 - _SpecularSize );
			float3 normalizeResult148 = normalize( ase_worldlightDir );
			float3 normalizeResult147 = normalize( ase_worldViewDir );
			float3 normalizeResult162 = normalize( ( normalizeResult148 + normalizeResult147 ) );
			float3 normalizeResult157 = normalize( ase_worldNormal );
			float dotResult165 = dot( normalizeResult162 , normalizeResult157 );
			float smoothstepResult169 = smoothstep( temp_output_161_0 , ( temp_output_161_0 + _SpecularBlend ) , dotResult165);
			float4 Specular174 = (( _UseSpecular )?( ( ( ( ( 1.0 - tex2D( _SpecularMap, rotator156 ) ) * (( _SpecLightColor )?( ( ase_lightColor * _SpecularLightIntensity ) ):( _SpecularColor )) ) * smoothstepResult169 ) * NdotL175 ) ):( float4( 0,0,0,0 ) ));
			float4 Result135 = ( ( ( ShadowColor80 * Color110 ) * ( 1.0 - smoothstepResult58 ) ) + ( Color110 * smoothstepResult58 ) + ( temp_output_410_0 * Rim185 ) + ( temp_output_410_0 * Specular174 ) );
			float2 uv_Texture0119 = i.uv_texcoord;
			float3 ase_objectScale = float3( length( unity_ObjectToWorld[ 0 ].xyz ), length( unity_ObjectToWorld[ 1 ].xyz ), length( unity_ObjectToWorld[ 2 ].xyz ) );
			float3 normalizeResult123 = normalize( ase_worldlightDir );
			float dotResult128 = dot( BlendNormals( UnpackNormal( tex2D( _Texture0, uv_Texture0119 ) ) , ase_objectScale ) , normalizeResult123 );
			float2 uv_Normal120 = i.uv_texcoord;
			float dotResult127 = dot( BlendNormals( UnpackScaleNormal( tex2D( _Normal, uv_Normal120 ), _NormalStrength ) , ase_objectScale ) , ase_worldlightDir );
			float4 NormalMap132 = (( _UseNormalMap )?( ( dotResult127 * Result135 ) ):( ( dotResult128 * Result135 ) ));
			c.rgb = ( Result135 + NormalMap132 ).rgb;
			c.a = 1;
			return c;
		}

		inline void LightingStandardCustomLighting_GI( inout SurfaceOutputCustomLightingCustom s, UnityGIInput data, inout UnityGI gi )
		{
			s.GIData = data;
		}

		void surf( Input i , inout SurfaceOutputCustomLightingCustom o )
		{
			o.SurfInput = i;
			o.Normal = float3(0,0,1);
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf StandardCustomLighting keepalpha fullforwardshadows vertex:vertexDataFunc 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 4.0
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			#include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float2 customPack1 : TEXCOORD1;
				float4 tSpace0 : TEXCOORD2;
				float4 tSpace1 : TEXCOORD3;
				float4 tSpace2 : TEXCOORD4;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				vertexDataFunc( v, customInputData );
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				half3 worldTangent = UnityObjectToWorldDir( v.tangent.xyz );
				half tangentSign = v.tangent.w * unity_WorldTransformParams.w;
				half3 worldBinormal = cross( worldNormal, worldTangent ) * tangentSign;
				o.tSpace0 = float4( worldTangent.x, worldBinormal.x, worldNormal.x, worldPos.x );
				o.tSpace1 = float4( worldTangent.y, worldBinormal.y, worldNormal.y, worldPos.y );
				o.tSpace2 = float4( worldTangent.z, worldBinormal.z, worldNormal.z, worldPos.z );
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				return o;
			}
			half4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				surfIN.uv_texcoord = IN.customPack1.xy;
				float3 worldPos = float3( IN.tSpace0.w, IN.tSpace1.w, IN.tSpace2.w );
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.worldPos = worldPos;
				surfIN.worldNormal = float3( IN.tSpace0.z, IN.tSpace1.z, IN.tSpace2.z );
				surfIN.internalSurfaceTtoW0 = IN.tSpace0.xyz;
				surfIN.internalSurfaceTtoW1 = IN.tSpace1.xyz;
				surfIN.internalSurfaceTtoW2 = IN.tSpace2.xyz;
				SurfaceOutputCustomLightingCustom o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputCustomLightingCustom, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "TatoonEditor"
}
/*ASEBEGIN
Version=18712
1920;5;1680;971;7754.842;1052.252;5.570986;True;False
Node;AmplifyShaderEditor.CommentaryNode;136;-5993.825,-533.1808;Inherit;False;3301.643;968.2485;Specular;38;174;173;172;171;170;169;168;167;166;165;164;163;162;161;160;159;158;157;156;155;154;153;152;151;150;149;148;147;146;145;144;143;142;141;140;139;138;137;Specular;0,1,0.07320952,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;88;-5991.914,-1634.721;Inherit;False;2909.998;819.7475;Albedo And Gradient;25;111;110;109;108;106;105;104;103;102;101;100;99;98;97;96;95;94;93;92;91;90;89;397;398;409;MainColor & Gradient;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;138;-5949.691,-338.2537;Inherit;False;Property;_SpecularTextureTiling;Specular Texture Tiling;32;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;137;-5943.825,-129.9065;Inherit;False;World;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.ViewProjectionMatrixNode;139;-5946.168,42.14047;Inherit;False;0;1;FLOAT4x4;0
Node;AmplifyShaderEditor.CommentaryNode;50;-6021.98,1711.117;Inherit;False;2649.39;1146.548;;34;185;202;203;199;200;195;190;34;28;33;201;189;198;197;196;194;193;192;191;24;32;47;31;30;29;177;27;25;38;36;37;204;205;206;Rim;1,0.6396222,0,1;0;0
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;37;-5868.267,2029.523;Float;False;World;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.ScaleNode;141;-5675.535,-203.1597;Inherit;False;1;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;89;-5932.915,-1076.881;Inherit;False;Property;_GradientRotation;Gradient Rotation;27;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;59;-6025.019,685.831;Inherit;False;2598.732;800.7354;Shadow;18;80;60;61;62;64;63;66;67;65;69;68;71;70;72;76;73;74;77;Shadow tex&color;0,0,0,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;286;-2146.729,-372.9341;Inherit;False;1879.355;671.2362;OutlineColor;12;231;233;275;276;274;277;267;230;229;287;297;298;OutlineTextureNoise;0,0.240608,1,1;0;0
Node;AmplifyShaderEditor.PosVertexDataNode;90;-5969.032,-1398.65;Inherit;True;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WorldNormalVector;36;-5916.267,1869.523;Inherit;False;False;1;0;FLOAT3;0,0,0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;140;-5687.842,-101.6336;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT4x4;0,0,0,0,0,1,0,0,0,0,1,0,0,0,0,1;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ViewProjectionMatrixNode;189;-5871.241,2731.58;Inherit;False;0;1;FLOAT4x4;0
Node;AmplifyShaderEditor.RangedFloatNode;142;-5319.325,-173.1437;Inherit;False;Property;_SpecularTextureRotation;Specular Texture Rotation;33;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DotProductOpNode;38;-5612.268,1949.523;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;91;-5943.403,-1214.167;Inherit;False;Constant;_Vector0;Vector 0;45;0;Create;True;0;0;0;False;0;False;0,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;145;-5530.8,-203.1597;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;24;-5796.728,2196.613;Float;False;Property;_RimSize;Rim Size;8;0;Create;True;0;0;0;False;0;False;0.5679239;0;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.ViewProjectionMatrixNode;62;-5977.307,1228.583;Inherit;False;0;1;FLOAT4x4;0
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;61;-5958.734,1314.104;Inherit;False;World;True;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RadiansOpNode;92;-5715.789,-1073.091;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WorldSpaceLightDirHlpNode;144;-5228.096,-91.16151;Inherit;False;False;1;0;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.CommentaryNode;48;-2875.871,3799.353;Inherit;False;814.8123;367.9126;Comment;4;175;3;1;2;N . L;1,0.9979696,0,1;0;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;143;-5539.697,-335.7626;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TimeNode;233;-2085.051,115.3021;Inherit;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;60;-5984.513,1050.303;Inherit;False;Property;_ShadowTextureTiling;Shadow Texture Tiling;15;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;93;-5724.984,-1372.429;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;231;-2096.729,15.92288;Inherit;False;Property;_Speed;Speed;46;0;Create;True;0;0;0;False;0;False;0;0;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;146;-5206.821,58.47347;Inherit;False;World;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;190;-5752.81,2346.504;Inherit;False;Property;_RimTextureTiling;Rim Texture Tiling;23;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;201;-5850.228,2565.114;Inherit;False;World;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;192;-5554.213,2714.874;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT4x4;0,0,0,0,0,1,0,0,0,0,1,0,0,0,0,1;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RotatorNode;95;-5533.714,-1159.381;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.NormalizeNode;148;-4972.478,-91.17062;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;96;-5277.025,-1002.633;Inherit;False;Property;_GradientPosition;Gradient Position;26;0;Create;False;0;0;0;False;0;False;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ScaleNode;191;-5542.082,2566.379;Inherit;False;1;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;25;-5419.089,1953.887;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ToggleSwitchNode;150;-5243.298,-341.4636;Inherit;False;Property;_SpecularTextureViewProjection;Specular Texture View Projection;31;0;Create;True;0;0;0;False;0;False;0;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RadiansOpNode;149;-5020.996,-200.8616;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;52;-2958.417,2899.626;Inherit;False;1224.731;538.1493;Comment;8;328;400;181;7;383;10;11;8;Attenuation and Ambient;0.9974991,1,0,1;0;0
Node;AmplifyShaderEditor.WorldSpaceLightDirHlpNode;2;-2811.87,4007.353;Inherit;False;True;1;0;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;94;-5435.938,-919.6042;Inherit;False;Property;_GradientSize;Gradient Size;24;0;Create;True;0;0;0;False;0;False;0;1;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.WorldNormalVector;1;-2810.089,3863.165;Inherit;False;True;1;0;FLOAT3;0,0,0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;275;-1677.919,70.77916;Inherit;False;Constant;_Float0;Float 0;50;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ScaleNode;63;-5654.857,1162.355;Inherit;False;1;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;276;-1782.857,156.638;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.NormalizeNode;147;-4974.004,62.94247;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;64;-5667.265,1260.111;Inherit;False;2;2;0;FLOAT4x4;0,0,0,0,0,1,0,0,0,0,1,0,0,0,0,1;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.IndirectDiffuseLighting;11;-2904.157,3085.564;Inherit;False;Tangent;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RotatorNode;156;-4844.298,-439.9517;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;195;-5193.207,2623.249;Inherit;False;Property;_RimTextureRotation;Rim Texture Rotation;25;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;193;-5499.564,2420.406;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;194;-5394.802,2598.783;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.WorldNormalVector;152;-4792.089,78.90245;Inherit;False;False;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.LightColorNode;8;-2846.418,2947.626;Inherit;False;0;3;COLOR;0;FLOAT3;1;FLOAT;2
Node;AmplifyShaderEditor.LightColorNode;153;-4426.796,-216.5606;Inherit;False;0;3;COLOR;0;FLOAT3;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleAddOpNode;155;-4737.867,-91.08353;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;154;-4872.282,235.2983;Inherit;False;Property;_SpecularSize;Specular Size;37;0;Create;True;0;0;0;False;0;False;0.005;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;67;-5377.498,1309.334;Inherit;False;Property;_ShadowTextureRotation;Shadow Texture Rotation;16;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;66;-5439.563,1184.837;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;151;-4426.695,-100.2965;Inherit;False;Property;_SpecularLightIntensity;Specular Light Intensity;36;0;Create;True;0;0;0;False;0;False;1;1;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;27;-5259.089,1953.887;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.BreakToComponentsNode;98;-5283.935,-1157.346;Inherit;False;FLOAT2;1;0;FLOAT2;0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.TextureCoordinatesNode;65;-5693.319,1002.072;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DotProductOpNode;3;-2548.845,3939.328;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;274;-1522.521,95.63887;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.ScreenPosInputsNode;229;-2012.011,-162.2129;Float;False;0;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;97;-5027.235,-994.8396;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;298;-1361.657,195.172;Inherit;False;Property;_NoiseTextureScale;NoiseTextureScale;51;0;Create;True;0;0;0;False;0;False;2;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;10;-2336.257,2955.615;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT3;0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;103;-4348.766,-1457.769;Inherit;False;Property;_DiffuseColor;Diffuse Color;2;0;Create;True;0;0;0;False;0;False;1,1,1,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.NormalizeNode;162;-4581.556,-89.18152;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;28;-5195.089,2081.886;Float;False;Property;_RimBlend;Rim Blend;3;0;Create;True;0;0;0;False;0;False;0;0;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;160;-4655.367,-291.0895;Inherit;False;Property;_SpecularColor;Specular Color;34;0;Create;True;0;0;0;False;0;False;1,0.9575656,0.75,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ToggleSwitchNode;197;-5193.411,2466.044;Inherit;False;Property;_RimTextureViewProjection;Rim Texture View Projection;20;0;Create;True;0;0;0;False;0;False;0;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;159;-4779.972,332.6176;Inherit;False;Property;_SpecularBlend;Specular Blend;38;0;Create;True;0;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;161;-4575.961,244.1065;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;29;-5083.089,1953.887;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RadiansOpNode;196;-4986.792,2619.06;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;158;-4572.35,-479.1776;Inherit;True;Property;_SpecularMap;Specular Map;30;1;[NoScaleOffset];Create;True;0;0;0;False;1;;False;-1;None;None;True;0;False;gray;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.NormalizeNode;157;-4584.156,78.29844;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ColorNode;99;-5162.682,-1584.721;Inherit;False;Property;_ColorA;Color A;22;0;Create;True;0;0;0;False;0;False;1,0,0,0;1,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;205;-4882.25,2213.024;Inherit;False;Property;_RimLightIntensity;Rim Light Intensity;41;0;Create;True;0;0;0;False;0;False;0;0;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;277;-1376.457,-24.21251;Inherit;True;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;175;-2316.058,3894.118;Inherit;True;NdotL;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LightColorNode;47;-4767.531,2292.915;Inherit;False;0;3;COLOR;0;FLOAT3;1;FLOAT;2
Node;AmplifyShaderEditor.ToggleSwitchNode;69;-5302.464,1066.589;Inherit;False;Property;_ShadowTextureViewProjection;Shadow Texture View Projection;14;0;Create;True;0;0;0;False;0;False;0;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.LightColorNode;105;-4328.553,-1587.85;Inherit;False;0;3;COLOR;0;FLOAT3;1;FLOAT;2
Node;AmplifyShaderEditor.ColorNode;100;-5159.292,-1409.006;Inherit;False;Property;_ColorB;Color B;21;0;Create;True;0;0;0;False;0;False;0,0.1264467,1,0;0,0.1264467,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;163;-4185.917,-233.7735;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;300;-2128.614,-1677.771;Inherit;False;2974.157;993.6045;Comment;25;259;243;244;248;245;247;289;249;264;263;250;253;254;251;255;239;268;257;256;258;293;290;411;412;413;FireOutline;1,0,0.1493015,1;0;0
Node;AmplifyShaderEditor.SmoothstepOpNode;101;-4893.162,-1172.239;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RadiansOpNode;68;-5078.873,1292.965;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ScaleAndOffsetNode;297;-1138.057,18.37157;Inherit;True;3;0;FLOAT4;0,0,0,0;False;1;FLOAT;1;False;2;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.WorldNormalVector;243;-2078.614,-1134.033;Inherit;False;False;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.PowerNode;30;-4891.09,1953.887;Inherit;True;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;199;-5214.494,2227.081;Inherit;True;Property;_RimTexture;Rim Texture;19;1;[NoScaleOffset];Create;True;0;0;0;False;0;False;None;None;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;204;-4518.986,2221.407;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;181;-1969.304,2953.191;Inherit;True;Ambient;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;102;-4618.069,-1425.466;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ToggleSwitchNode;167;-4059.901,-330.5756;Inherit;False;Property;_SpecLightColor;Spec Light Color;35;0;Create;True;0;0;0;False;0;False;0;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;383;-2768.36,3301.72;Inherit;False;Property;_AttenuationPower;AttenuationPower;52;0;Create;True;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;34;-4649.915,2101.067;Float;False;Property;_RimColor;Rim Color;1;0;Create;True;0;0;0;False;0;False;0,1,0.8758622,0;0,0,0,0;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DotProductOpNode;165;-4394.628,-22.18851;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;259;-2058.932,-1302.628;Inherit;False;World;True;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RotatorNode;70;-4927.57,1074.292;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TexturePropertyNode;71;-4932.15,858.623;Inherit;True;Property;_ShadowTexture;Shadow Texture;11;1;[NoScaleOffset];Create;True;0;0;0;True;0;False;None;None;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.OneMinusNode;166;-4236.134,-473.2517;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RotatorNode;198;-4856.889,2475.03;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TexturePropertyNode;267;-1175.33,-200.1011;Inherit;True;Property;_Texture;Texture;49;1;[NoScaleOffset];Create;True;0;0;0;False;0;False;None;None;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.LightAttenuation;7;-2780.313,3197.52;Inherit;False;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;177;-5120.258,1863.581;Inherit;False;175;NdotL;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;409;-4125.436,-1519.546;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;164;-4369.149,243.6614;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;72;-4592.025,1097.185;Inherit;True;Property;_tex1;tex1;13;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;gray;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DotProductOpNode;244;-1822.334,-1185.414;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;230;-887.5974,-51.22144;Inherit;True;Property;_NoiseTexture;NoiseTexture;43;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;73;-4571.643,860.2289;Inherit;False;Property;_ShadowColor;Shadow Color;9;0;Create;True;0;0;0;False;0;False;1,0,0,1;0.5377358,0.5377358,0.5377358,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;200;-4570.291,2435.353;Inherit;True;Property;_RimTex;RimTex;25;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;168;-3852.004,-481.8966;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TexturePropertyNode;104;-4326.732,-1216.476;Inherit;True;Property;_TextureDiffuse;Texture Diffuse;6;1;[NoScaleOffset];Create;True;0;0;0;False;0;False;None;None;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.ToggleSwitchNode;106;-3998.184,-1341.755;Inherit;False;Property;_UseGradient;Use Gradient;18;0;Create;True;0;0;0;False;0;False;0;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ToggleSwitchNode;206;-4382.064,2118.015;Inherit;False;Property;_RimLightColor;Rim Light Color;40;0;Create;True;0;0;0;False;0;False;0;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.PowerNode;400;-2351.444,3194.151;Inherit;True;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;397;-3921.23,-1464.843;Inherit;False;181;Ambient;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;31;-4633.656,1878.305;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;169;-4203.938,-18.49751;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;32;-4313.722,1880.201;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;171;-3658.063,-483.1807;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;74;-4322.867,717.7863;Inherit;False;Constant;_ShadowColor1;Shadow Color1;5;0;Create;True;0;0;0;False;1;Header(SHADOWS);False;1,1,1,1;0.5377358,0.5377358,0.5377358,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;170;-3700.688,-354.0005;Inherit;False;175;NdotL;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;328;-1968.09,3195.938;Inherit;True;Atten;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;245;-1646.062,-1185.414;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;287;-570.5784,-27.25029;Inherit;True;Noise;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;187;-2888.299,1512.832;Inherit;False;2661.902;947.3845;Comment;21;186;176;221;86;42;83;82;112;81;58;184;57;55;56;135;401;405;406;408;410;331;Mix;0,0.3468349,1,1;0;0
Node;AmplifyShaderEditor.SamplerNode;108;-4089.757,-1220.483;Inherit;True;Property;_TextureSample0;Texture Sample 0;0;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;76;-4248.699,1014.913;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;248;-1782.372,-1382.709;Inherit;False;Property;_PowerColor1;PowerColor1;47;0;Create;True;0;0;0;False;0;False;1;1.158531;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;264;-1799.864,-896.0406;Inherit;False;Property;_PowerColor2;PowerColor2;48;0;Create;True;0;0;0;False;0;False;1;0.1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;33;-4180.497,2210.604;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;398;-3643.992,-1457.542;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;55;-2838.191,2063.605;Inherit;False;Property;_ShadowBlend;ShadowBlend;12;0;Create;True;0;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;56;-2838.299,1974.783;Inherit;False;Property;_ShadowSize;Shadow Size;10;0;Create;True;0;0;0;False;0;False;0;0;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.ToggleSwitchNode;77;-3895.893,980.2164;Inherit;False;Property;_UseShadow;UseShadow;17;0;Create;True;0;0;0;False;0;False;0;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;289;-1403.211,-1120.728;Inherit;False;287;Noise;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;331;-2695.045,1653.499;Inherit;False;328;Atten;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;202;-3980.094,2009.46;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;109;-3503.021,-1304.756;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;172;-3398.92,-486.9495;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.PowerNode;247;-1453.108,-1368.929;Inherit;True;True;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;411;-1500.405,-1020.108;Inherit;False;True;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;184;-2689.111,1761.696;Inherit;False;175;NdotL;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;57;-2477.043,2042.46;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ToggleSwitchNode;203;-3776.793,1984.755;Inherit;False;Property;_UseRim;UseRim;13;0;Create;True;0;0;0;False;0;False;0;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;413;-895.0276,-1116.975;Inherit;False;Constant;_Float3;Float 2;47;0;Create;True;0;0;0;False;0;False;10;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ToggleSwitchNode;173;-3160.313,-484.7285;Inherit;False;Property;_UseSpecular;UseSpecular;28;0;Create;True;0;0;0;False;0;False;0;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;80;-3666.843,981.9041;Inherit;True;ShadowColor;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;401;-2491.866,1723.626;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;249;-1172.423,-1353.479;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;110;-3282.032,-1293.459;Inherit;True;Color;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;263;-894.4454,-1193.493;Inherit;False;Constant;_Float2;Float 2;47;0;Create;True;0;0;0;False;0;False;5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;412;-1212.064,-1016.843;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;405;-2244.902,2228.721;Inherit;False;328;Atten;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;58;-2296.376,1956.751;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;112;-1830.895,1876.778;Inherit;False;110;Color;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;253;-691.1462,-1014.809;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;185;-3585.179,1964.917;Inherit;True;Rim;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;81;-1855.861,1601.568;Inherit;True;80;ShadowColor;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;250;-707.5972,-1325.954;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;174;-2944.521,-475.9826;Inherit;True;Specular;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;113;-6002.787,3075.623;Inherit;False;2167.901;1060.778;normal;19;132;131;130;129;128;127;126;125;124;123;122;121;120;119;118;117;116;115;114;NormalMap;1,0.3058823,0.9274651,1;0;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;83;-1571.558,1598.742;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ClampOpNode;254;-463.9844,-1015.002;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;251;-459.9897,-1325.034;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;82;-1993.922,1824.869;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;186;-1492.576,2207.399;Inherit;False;185;Rim;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;410;-1988.876,2183.164;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;176;-1536.082,2368.8;Inherit;False;174;Specular;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;86;-1292.667,1620.947;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;255;-182.6052,-1220.805;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;268;-179.871,-991.2811;Inherit;False;Property;_OutlineColor2;OutlineColor2;44;1;[HDR];Create;True;0;0;0;False;0;False;1,0.06464233,0,0;0,0.9740796,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;115;-5952.787,3634.304;Inherit;False;Property;_NormalStrength;Normal Strength;7;0;Create;False;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;406;-1254.834,2105.583;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;42;-1538.765,1933.303;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;408;-1251.427,2337.273;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;239;-446.8422,-1527.855;Inherit;False;Property;_OutlineColor1;OutlineColor1;45;1;[HDR];Create;True;0;0;0;False;0;False;0,0.9740796,1,0;0,0.9740796,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TexturePropertyNode;114;-5952.902,3139.161;Inherit;True;Property;_Texture0;Texture 0;29;3;[HideInInspector];[NoScaleOffset];[Normal];Create;True;0;0;0;False;0;False;None;None;False;bump;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;257;78.47801,-1070.38;Inherit;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;120;-5718.684,3588.847;Inherit;True;Property;_Normal;Normal;4;2;[NoScaleOffset];[Normal];Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;bump;Auto;True;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WorldSpaceLightDirHlpNode;117;-5351.599,3342.346;Inherit;False;False;1;0;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SamplerNode;119;-5707.034,3146.879;Inherit;True;Property;_TextureSample1;Texture Sample 1;42;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;True;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;221;-708.1893,1910.663;Inherit;True;4;4;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;256;-182.872,-1481.658;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ObjectScaleNode;116;-5612.645,3799.176;Inherit;False;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.CommentaryNode;208;-2127.136,565.2449;Inherit;False;1818.731;790.378;Outline;12;209;219;302;291;288;217;213;212;211;215;301;313;Outline;1,0.6029412,0.7097364,1;0;0
Node;AmplifyShaderEditor.ObjectScaleNode;118;-5643.634,3351.618;Inherit;False;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.BlendNormalsNode;124;-5321.125,3618.05;Inherit;False;0;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.BlendNormalsNode;122;-5254.833,3160.667;Inherit;False;0;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;135;-445.3568,1918.602;Inherit;True;Result;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.WorldSpaceLightDirHlpNode;121;-5432.029,3895.625;Inherit;False;False;1;0;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RegisterLocalVarNode;111;-3688.246,-1126.124;Inherit;False;Alpha;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.NormalizeNode;123;-5048.504,3365.851;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ColorNode;211;-2071.136,818.9837;Inherit;False;Property;_OutlineColor;Outline Color;39;1;[HDR];Create;True;0;0;0;False;0;False;0,0,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;258;362.0362,-1255.529;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;293;621.5422,-1254.455;Inherit;True;OutlineResult;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;290;-181.2091,-805.1909;Inherit;False;NoiseAlpha;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;126;-4746.348,3828.683;Inherit;False;135;Result;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;125;-4754.75,3550.235;Inherit;False;135;Result;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.DotProductOpNode;127;-5088.738,3830.414;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;212;-1772.924,724.9177;Inherit;False;111;Alpha;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.ComponentMaskNode;213;-1794.73,820.0009;Inherit;False;True;True;True;False;1;0;COLOR;0,0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.DotProductOpNode;128;-4859.701,3216.643;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;209;-1560.539,1113.42;Inherit;False;Property;_OutlineSize;Outline Size;42;0;Create;True;0;0;0;False;0;False;0.1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;291;-1549.669,989.1348;Inherit;False;290;NoiseAlpha;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;288;-1549.117,854.5036;Inherit;False;293;OutlineResult;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;217;-1511.178,729.4261;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT3;0.3382353,0.3382353,0.3382353;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;129;-4550.873,3535.574;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;130;-4544.474,3701.997;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ToggleSwitchNode;131;-4320.872,3612.839;Inherit;False;Property;_UseNormalMap;UseNormalMap;5;0;Create;True;0;0;0;False;0;False;0;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ToggleSwitchNode;313;-1327.136,949.188;Inherit;False;Property;_UseOutlineFire;UseOutlineFire;50;0;Create;True;0;0;0;False;0;False;0;2;0;FLOAT;1;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ToggleSwitchNode;301;-1327.043,819.4116;Inherit;False;Property;_UseOutlineFire;UseOutlineFire;50;0;Create;True;0;0;0;False;0;False;0;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ToggleSwitchNode;215;-1329.576,1080.469;Inherit;False;Property;_UseOutline;UseOutline;43;0;Create;True;0;0;0;False;0;False;0;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;188;551.147,1458.296;Inherit;False;819.4518;615.8219;Comment;5;220;228;134;180;133;Master;1,1,1,1;0;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;132;-4074.991,3609.848;Inherit;True;NormalMap;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.OutlineNode;302;-1001.842,940.1406;Inherit;False;0;True;Masked;2;0;Back;3;0;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;219;-678.0784,942.4607;Inherit;False;Outline;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;180;608.5537,1605.338;Inherit;False;135;Result;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;133;604.1656,1711.563;Inherit;False;132;NormalMap;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;134;840.5972,1647.607;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;220;852.5453,1897.429;Inherit;False;219;Outline;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;228;1113.926,1555.918;Float;False;True;-1;4;TatoonEditor;0;0;CustomLighting;TetraArts/Tatoon/Built-In/Tatoon_Built-In;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;True;0;True;Opaque;;AlphaTest;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0.9;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;141;0;138;0
WireConnection;140;0;137;0
WireConnection;140;1;139;0
WireConnection;38;0;36;0
WireConnection;38;1;37;0
WireConnection;145;0;141;0
WireConnection;145;1;140;0
WireConnection;92;0;89;0
WireConnection;143;0;138;0
WireConnection;93;0;90;1
WireConnection;93;1;90;2
WireConnection;192;0;201;0
WireConnection;192;1;189;0
WireConnection;95;0;93;0
WireConnection;95;1;91;0
WireConnection;95;2;92;0
WireConnection;148;0;144;0
WireConnection;191;0;190;0
WireConnection;25;0;38;0
WireConnection;25;1;24;0
WireConnection;150;0;143;0
WireConnection;150;1;145;0
WireConnection;149;0;142;0
WireConnection;63;0;60;0
WireConnection;276;0;231;0
WireConnection;276;1;233;2
WireConnection;147;0;146;0
WireConnection;64;0;62;0
WireConnection;64;1;61;0
WireConnection;156;0;150;0
WireConnection;156;2;149;0
WireConnection;193;0;190;0
WireConnection;194;0;191;0
WireConnection;194;1;192;0
WireConnection;155;0;148;0
WireConnection;155;1;147;0
WireConnection;66;0;63;0
WireConnection;66;1;64;0
WireConnection;27;0;25;0
WireConnection;98;0;95;0
WireConnection;65;0;60;0
WireConnection;3;0;1;0
WireConnection;3;1;2;0
WireConnection;274;0;275;0
WireConnection;274;1;276;0
WireConnection;97;0;96;0
WireConnection;97;1;94;0
WireConnection;10;0;8;0
WireConnection;10;1;11;0
WireConnection;162;0;155;0
WireConnection;197;0;193;0
WireConnection;197;1;194;0
WireConnection;161;0;154;0
WireConnection;29;0;27;0
WireConnection;196;0;195;0
WireConnection;158;1;156;0
WireConnection;157;0;152;0
WireConnection;277;0;229;0
WireConnection;277;1;274;0
WireConnection;175;0;3;0
WireConnection;69;0;65;0
WireConnection;69;1;66;0
WireConnection;163;0;153;0
WireConnection;163;1;151;0
WireConnection;101;0;98;0
WireConnection;101;1;96;0
WireConnection;101;2;97;0
WireConnection;68;0;67;0
WireConnection;297;0;277;0
WireConnection;297;1;298;0
WireConnection;30;0;29;0
WireConnection;30;1;28;0
WireConnection;204;0;205;0
WireConnection;204;1;47;1
WireConnection;181;0;10;0
WireConnection;102;0;99;0
WireConnection;102;1;100;0
WireConnection;102;2;101;0
WireConnection;167;0;160;0
WireConnection;167;1;163;0
WireConnection;165;0;162;0
WireConnection;165;1;157;0
WireConnection;70;0;69;0
WireConnection;70;2;68;0
WireConnection;166;0;158;0
WireConnection;198;0;197;0
WireConnection;198;2;196;0
WireConnection;409;0;105;0
WireConnection;409;1;103;0
WireConnection;164;0;161;0
WireConnection;164;1;159;0
WireConnection;72;0;71;0
WireConnection;72;1;70;0
WireConnection;244;0;259;0
WireConnection;244;1;243;0
WireConnection;230;0;267;0
WireConnection;230;1;297;0
WireConnection;200;0;199;0
WireConnection;200;1;198;0
WireConnection;168;0;166;0
WireConnection;168;1;167;0
WireConnection;106;0;409;0
WireConnection;106;1;102;0
WireConnection;206;0;34;0
WireConnection;206;1;204;0
WireConnection;400;0;7;0
WireConnection;400;1;383;0
WireConnection;31;0;177;0
WireConnection;31;1;30;0
WireConnection;169;0;165;0
WireConnection;169;1;161;0
WireConnection;169;2;164;0
WireConnection;32;0;31;0
WireConnection;171;0;168;0
WireConnection;171;1;169;0
WireConnection;328;0;400;0
WireConnection;245;0;244;0
WireConnection;287;0;230;1
WireConnection;108;0;104;0
WireConnection;76;0;73;0
WireConnection;76;1;72;0
WireConnection;33;0;206;0
WireConnection;33;1;200;0
WireConnection;398;0;397;0
WireConnection;398;1;106;0
WireConnection;77;0;74;0
WireConnection;77;1;76;0
WireConnection;202;0;32;0
WireConnection;202;1;33;0
WireConnection;109;0;398;0
WireConnection;109;1;108;0
WireConnection;172;0;171;0
WireConnection;172;1;170;0
WireConnection;247;0;245;0
WireConnection;247;1;248;0
WireConnection;411;0;245;0
WireConnection;411;1;264;0
WireConnection;57;0;56;0
WireConnection;57;1;55;0
WireConnection;203;1;202;0
WireConnection;173;1;172;0
WireConnection;80;0;77;0
WireConnection;401;0;331;0
WireConnection;401;1;184;0
WireConnection;249;0;247;0
WireConnection;249;1;289;0
WireConnection;110;0;109;0
WireConnection;412;0;411;0
WireConnection;412;1;289;0
WireConnection;58;0;401;0
WireConnection;58;1;56;0
WireConnection;58;2;57;0
WireConnection;253;0;413;0
WireConnection;253;1;412;0
WireConnection;185;0;203;0
WireConnection;250;0;249;0
WireConnection;250;1;263;0
WireConnection;174;0;173;0
WireConnection;83;0;81;0
WireConnection;83;1;112;0
WireConnection;254;0;253;0
WireConnection;251;0;250;0
WireConnection;82;0;58;0
WireConnection;410;0;58;0
WireConnection;410;1;405;0
WireConnection;86;0;83;0
WireConnection;86;1;82;0
WireConnection;255;0;254;0
WireConnection;255;1;251;0
WireConnection;406;0;410;0
WireConnection;406;1;186;0
WireConnection;42;0;112;0
WireConnection;42;1;58;0
WireConnection;408;0;410;0
WireConnection;408;1;176;0
WireConnection;257;0;255;0
WireConnection;257;1;268;0
WireConnection;120;5;115;0
WireConnection;119;0;114;0
WireConnection;221;0;86;0
WireConnection;221;1;42;0
WireConnection;221;2;406;0
WireConnection;221;3;408;0
WireConnection;256;0;239;0
WireConnection;256;1;251;0
WireConnection;124;0;120;0
WireConnection;124;1;116;0
WireConnection;122;0;119;0
WireConnection;122;1;118;0
WireConnection;135;0;221;0
WireConnection;111;0;108;4
WireConnection;123;0;117;0
WireConnection;258;0;256;0
WireConnection;258;1;257;0
WireConnection;293;0;258;0
WireConnection;290;0;254;0
WireConnection;127;0;124;0
WireConnection;127;1;121;0
WireConnection;213;0;211;0
WireConnection;128;0;122;0
WireConnection;128;1;123;0
WireConnection;217;0;212;0
WireConnection;217;1;213;0
WireConnection;129;0;128;0
WireConnection;129;1;125;0
WireConnection;130;0;127;0
WireConnection;130;1;126;0
WireConnection;131;0;129;0
WireConnection;131;1;130;0
WireConnection;313;1;291;0
WireConnection;301;0;217;0
WireConnection;301;1;288;0
WireConnection;215;1;209;0
WireConnection;132;0;131;0
WireConnection;302;0;301;0
WireConnection;302;2;313;0
WireConnection;302;1;215;0
WireConnection;219;0;302;0
WireConnection;134;0;180;0
WireConnection;134;1;133;0
WireConnection;228;13;134;0
WireConnection;228;11;220;0
ASEEND*/
//CHKSM=B6A5DB70BA378EA8DB88A26C61F4DB33B2153420