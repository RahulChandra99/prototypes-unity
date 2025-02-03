// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "TetraArts/Tatoon/URP/Tatoon_URP_Transparent"
{
	Properties
	{
		[HideInInspector] _EmissionColor("Emission Color", Color) = (1,1,1,1)
		[HideInInspector] _AlphaCutoff("Alpha Cutoff ", Range(0, 1)) = 0.5
		[ASEBegin]_RimColor("Rim Color", Color) = (0,1,0.8758622,0)
		_DiffuseColor("Diffuse Color", Color) = (1,1,1,0)
		_RimBlend("Rim Blend", Range( 0 , 10)) = 0
		[NoScaleOffset][Normal]_Normal("Normal", 2D) = "bump" {}
		[Toggle]_UseNormalMap("UseNormalMap", Float) = 0
		[NoScaleOffset]_TextureDiffuse("Texture Diffuse", 2D) = "white" {}
		_NormalStrength("Normal Strength", Float) = 1
		_RimSize("Rim Size", Range( 0 , 2)) = 0.5679239
		_ShadowColor("Shadow Color", Color) = (0,0,0,1)
		_ShadowSize("Shadow Size", Range( 0 , 2)) = 1
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
		[Toggle]_RimLightColor("Rim Light Color", Float) = 0
		_RimLightIntensity("Rim Light Intensity", Range( 0 , 10)) = 0
		_AttenuationPower("AttenuationPower", Range( 0.01 , 50)) = 1
		[ASEEnd]_Opacity("Opacity", Range( 0 , 1)) = 0.5
		[HideInInspector] _texcoord( "", 2D ) = "white" {}

		//_TransmissionShadow( "Transmission Shadow", Range( 0, 1 ) ) = 0.5
		//_TransStrength( "Trans Strength", Range( 0, 50 ) ) = 1
		//_TransNormal( "Trans Normal Distortion", Range( 0, 1 ) ) = 0.5
		//_TransScattering( "Trans Scattering", Range( 1, 50 ) ) = 2
		//_TransDirect( "Trans Direct", Range( 0, 1 ) ) = 0.9
		//_TransAmbient( "Trans Ambient", Range( 0, 1 ) ) = 0.1
		//_TransShadow( "Trans Shadow", Range( 0, 1 ) ) = 0.5
		//_TessPhongStrength( "Tess Phong Strength", Range( 0, 1 ) ) = 0.5
		//_TessValue( "Tess Max Tessellation", Range( 1, 32 ) ) = 16
		//_TessMin( "Tess Min Distance", Float ) = 10
		//_TessMax( "Tess Max Distance", Float ) = 25
		//_TessEdgeLength ( "Tess Edge length", Range( 2, 50 ) ) = 16
		//_TessMaxDisp( "Tess Max Displacement", Float ) = 25
	}

	SubShader
	{
		LOD 0

		

		Tags { "RenderPipeline"="UniversalPipeline" "RenderType"="Transparent" "Queue"="Transparent" }
		Cull Back
		AlphaToMask Off
		HLSLINCLUDE
		#pragma target 3.0

		float4 FixedTess( float tessValue )
		{
			return tessValue;
		}
		
		float CalcDistanceTessFactor (float4 vertex, float minDist, float maxDist, float tess, float4x4 o2w, float3 cameraPos )
		{
			float3 wpos = mul(o2w,vertex).xyz;
			float dist = distance (wpos, cameraPos);
			float f = clamp(1.0 - (dist - minDist) / (maxDist - minDist), 0.01, 1.0) * tess;
			return f;
		}

		float4 CalcTriEdgeTessFactors (float3 triVertexFactors)
		{
			float4 tess;
			tess.x = 0.5 * (triVertexFactors.y + triVertexFactors.z);
			tess.y = 0.5 * (triVertexFactors.x + triVertexFactors.z);
			tess.z = 0.5 * (triVertexFactors.x + triVertexFactors.y);
			tess.w = (triVertexFactors.x + triVertexFactors.y + triVertexFactors.z) / 3.0f;
			return tess;
		}

		float CalcEdgeTessFactor (float3 wpos0, float3 wpos1, float edgeLen, float3 cameraPos, float4 scParams )
		{
			float dist = distance (0.5 * (wpos0+wpos1), cameraPos);
			float len = distance(wpos0, wpos1);
			float f = max(len * scParams.y / (edgeLen * dist), 1.0);
			return f;
		}

		float DistanceFromPlane (float3 pos, float4 plane)
		{
			float d = dot (float4(pos,1.0f), plane);
			return d;
		}

		bool WorldViewFrustumCull (float3 wpos0, float3 wpos1, float3 wpos2, float cullEps, float4 planes[6] )
		{
			float4 planeTest;
			planeTest.x = (( DistanceFromPlane(wpos0, planes[0]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlane(wpos1, planes[0]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlane(wpos2, planes[0]) > -cullEps) ? 1.0f : 0.0f );
			planeTest.y = (( DistanceFromPlane(wpos0, planes[1]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlane(wpos1, planes[1]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlane(wpos2, planes[1]) > -cullEps) ? 1.0f : 0.0f );
			planeTest.z = (( DistanceFromPlane(wpos0, planes[2]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlane(wpos1, planes[2]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlane(wpos2, planes[2]) > -cullEps) ? 1.0f : 0.0f );
			planeTest.w = (( DistanceFromPlane(wpos0, planes[3]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlane(wpos1, planes[3]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlane(wpos2, planes[3]) > -cullEps) ? 1.0f : 0.0f );
			return !all (planeTest);
		}

		float4 DistanceBasedTess( float4 v0, float4 v1, float4 v2, float tess, float minDist, float maxDist, float4x4 o2w, float3 cameraPos )
		{
			float3 f;
			f.x = CalcDistanceTessFactor (v0,minDist,maxDist,tess,o2w,cameraPos);
			f.y = CalcDistanceTessFactor (v1,minDist,maxDist,tess,o2w,cameraPos);
			f.z = CalcDistanceTessFactor (v2,minDist,maxDist,tess,o2w,cameraPos);

			return CalcTriEdgeTessFactors (f);
		}

		float4 EdgeLengthBasedTess( float4 v0, float4 v1, float4 v2, float edgeLength, float4x4 o2w, float3 cameraPos, float4 scParams )
		{
			float3 pos0 = mul(o2w,v0).xyz;
			float3 pos1 = mul(o2w,v1).xyz;
			float3 pos2 = mul(o2w,v2).xyz;
			float4 tess;
			tess.x = CalcEdgeTessFactor (pos1, pos2, edgeLength, cameraPos, scParams);
			tess.y = CalcEdgeTessFactor (pos2, pos0, edgeLength, cameraPos, scParams);
			tess.z = CalcEdgeTessFactor (pos0, pos1, edgeLength, cameraPos, scParams);
			tess.w = (tess.x + tess.y + tess.z) / 3.0f;
			return tess;
		}

		float4 EdgeLengthBasedTessCull( float4 v0, float4 v1, float4 v2, float edgeLength, float maxDisplacement, float4x4 o2w, float3 cameraPos, float4 scParams, float4 planes[6] )
		{
			float3 pos0 = mul(o2w,v0).xyz;
			float3 pos1 = mul(o2w,v1).xyz;
			float3 pos2 = mul(o2w,v2).xyz;
			float4 tess;

			if (WorldViewFrustumCull(pos0, pos1, pos2, maxDisplacement, planes))
			{
				tess = 0.0f;
			}
			else
			{
				tess.x = CalcEdgeTessFactor (pos1, pos2, edgeLength, cameraPos, scParams);
				tess.y = CalcEdgeTessFactor (pos2, pos0, edgeLength, cameraPos, scParams);
				tess.z = CalcEdgeTessFactor (pos0, pos1, edgeLength, cameraPos, scParams);
				tess.w = (tess.x + tess.y + tess.z) / 3.0f;
			}
			return tess;
		}
		ENDHLSL

		
		Pass
		{
			
			Name "Forward"
			Tags { "LightMode"="UniversalForward" }
			
			Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
			ZWrite Off
			ZTest LEqual
			Offset 0 , 0
			ColorMask RGBA
			Stencil
			{
				Ref 255
				Comp Always
				Pass Keep
				Fail Keep
				ZFail Keep
			}

			HLSLPROGRAM
			#define _NORMAL_DROPOFF_TS 1
			#pragma multi_compile_instancing
			#pragma multi_compile_fog
			#define ASE_FOG 1
			#define ASE_FINAL_COLOR_ALPHA_MULTIPLY 1
			#define _EMISSION
			#define ASE_SRP_VERSION 70301

			#pragma prefer_hlslcc gles
			#pragma exclude_renderers d3d11_9x

			#pragma multi_compile _ _MAIN_LIGHT_SHADOWS
			#pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
			#pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
			#pragma multi_compile _ _ADDITIONAL_LIGHT_SHADOWS
			#pragma multi_compile _ _SHADOWS_SOFT
			#pragma multi_compile _ _MIXED_LIGHTING_SUBTRACTIVE
			
			#pragma multi_compile _ DIRLIGHTMAP_COMBINED
			#pragma multi_compile _ LIGHTMAP_ON

			#pragma vertex vert
			#pragma fragment frag

			#define SHADERPASS_FORWARD

			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/UnityInstancing.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
			
			#if ASE_SRP_VERSION <= 70108
			#define REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR
			#endif

			#if defined(UNITY_INSTANCING_ENABLED) && defined(_TERRAIN_INSTANCED_PERPIXEL_NORMAL)
			    #define ENABLE_TERRAIN_PERPIXEL_NORMAL
			#endif

			#define ASE_NEEDS_FRAG_WORLD_VIEW_DIR
			#define ASE_NEEDS_VERT_TEXTURE_COORDINATES1
			#define ASE_NEEDS_FRAG_WORLD_NORMAL
			#define ASE_NEEDS_FRAG_POSITION
			#define ASE_NEEDS_FRAG_WORLD_POSITION
			#define ASE_NEEDS_FRAG_SHADOWCOORDS


			struct VertexInput
			{
				float4 vertex : POSITION;
				float3 ase_normal : NORMAL;
				float4 ase_tangent : TANGENT;
				float4 texcoord1 : TEXCOORD1;
				float4 texcoord : TEXCOORD0;
				
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 clipPos : SV_POSITION;
				float4 lightmapUVOrVertexSH : TEXCOORD0;
				half4 fogFactorAndVertexLight : TEXCOORD1;
				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
				float4 shadowCoord : TEXCOORD2;
				#endif
				float4 tSpace0 : TEXCOORD3;
				float4 tSpace1 : TEXCOORD4;
				float4 tSpace2 : TEXCOORD5;
				#if defined(ASE_NEEDS_FRAG_SCREEN_POSITION)
				float4 screenPos : TEXCOORD6;
				#endif
				float4 ase_texcoord7 : TEXCOORD7;
				float4 ase_texcoord8 : TEXCOORD8;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			CBUFFER_START(UnityPerMaterial)
			float4 _SpecularColor;
			float4 _ColorB;
			float4 _ColorA;
			float4 _DiffuseColor;
			float4 _RimColor;
			float4 _ShadowColor;
			float _SpecularLightIntensity;
			float _SpecularBlend;
			float _UseNormalMap;
			float _RimLightIntensity;
			float _SpecLightColor;
			float _SpecularTextureRotation;
			float _SpecularTextureTiling;
			float _SpecularTextureViewProjection;
			float _UseSpecular;
			float _RimTextureRotation;
			float _RimTextureTiling;
			float _RimTextureViewProjection;
			float _SpecularSize;
			float _UseShadow;
			float _RimBlend;
			float _NormalStrength;
			float _RimSize;
			float _UseRim;
			float _AttenuationPower;
			float _ShadowBlend;
			float _ShadowSize;
			float _GradientRotation;
			float _GradientSize;
			float _GradientPosition;
			float _UseGradient;
			float _ShadowTextureRotation;
			float _ShadowTextureTiling;
			float _ShadowTextureViewProjection;
			float _RimLightColor;
			float _Opacity;
			#ifdef _TRANSMISSION_ASE
				float _TransmissionShadow;
			#endif
			#ifdef _TRANSLUCENCY_ASE
				float _TransStrength;
				float _TransNormal;
				float _TransScattering;
				float _TransDirect;
				float _TransAmbient;
				float _TransShadow;
			#endif
			#ifdef TESSELLATION_ON
				float _TessPhongStrength;
				float _TessValue;
				float _TessMin;
				float _TessMax;
				float _TessEdgeLength;
				float _TessMaxDisp;
			#endif
			CBUFFER_END
			sampler2D _ShadowTexture;
			sampler2D _TextureDiffuse;
			sampler2D _RimTexture;
			sampler2D _SpecularMap;
			sampler2D _Texture0;
			sampler2D _Normal;


			float3 ASEIndirectDiffuse( float2 uvStaticLightmap, float3 normalWS )
			{
			#ifdef LIGHTMAP_ON
				return SampleLightmap( uvStaticLightmap, normalWS );
			#else
				return SampleSH(normalWS);
			#endif
			}
			
			float3 AdditionalLightsLambert( float3 WorldPosition, float3 WorldNormal )
			{
				float3 Color = 0;
				#ifdef _ADDITIONAL_LIGHTS
				int numLights = GetAdditionalLightsCount();
				for(int i = 0; i<numLights;i++)
				{
					Light light = GetAdditionalLight(i, WorldPosition);
					half3 AttLightColor = light.color *(light.distanceAttenuation * light.shadowAttenuation);
					Color +=LightingLambert(AttLightColor, light.direction, WorldNormal);
					
				}
				#endif
				return Color;
			}
			

			VertexOutput VertexFunction( VertexInput v  )
			{
				VertexOutput o = (VertexOutput)0;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

				o.ase_texcoord7.xy = v.texcoord.xy;
				o.ase_texcoord8 = v.vertex;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord7.zw = 0;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.vertex.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif
				float3 vertexValue = defaultVertexValue;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					v.vertex.xyz = vertexValue;
				#else
					v.vertex.xyz += vertexValue;
				#endif
				v.ase_normal = v.ase_normal;

				float3 positionWS = TransformObjectToWorld( v.vertex.xyz );
				float3 positionVS = TransformWorldToView( positionWS );
				float4 positionCS = TransformWorldToHClip( positionWS );

				VertexNormalInputs normalInput = GetVertexNormalInputs( v.ase_normal, v.ase_tangent );

				o.tSpace0 = float4( normalInput.normalWS, positionWS.x);
				o.tSpace1 = float4( normalInput.tangentWS, positionWS.y);
				o.tSpace2 = float4( normalInput.bitangentWS, positionWS.z);

				OUTPUT_LIGHTMAP_UV( v.texcoord1, unity_LightmapST, o.lightmapUVOrVertexSH.xy );
				OUTPUT_SH( normalInput.normalWS.xyz, o.lightmapUVOrVertexSH.xyz );

				#if defined(ENABLE_TERRAIN_PERPIXEL_NORMAL)
					o.lightmapUVOrVertexSH.zw = v.texcoord;
					o.lightmapUVOrVertexSH.xy = v.texcoord * unity_LightmapST.xy + unity_LightmapST.zw;
				#endif

				half3 vertexLight = VertexLighting( positionWS, normalInput.normalWS );
				#ifdef ASE_FOG
					half fogFactor = ComputeFogFactor( positionCS.z );
				#else
					half fogFactor = 0;
				#endif
				o.fogFactorAndVertexLight = half4(fogFactor, vertexLight);
				
				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
				VertexPositionInputs vertexInput = (VertexPositionInputs)0;
				vertexInput.positionWS = positionWS;
				vertexInput.positionCS = positionCS;
				o.shadowCoord = GetShadowCoord( vertexInput );
				#endif
				
				o.clipPos = positionCS;
				#if defined(ASE_NEEDS_FRAG_SCREEN_POSITION)
				o.screenPos = ComputeScreenPos(positionCS);
				#endif
				return o;
			}
			
			#if defined(TESSELLATION_ON)
			struct VertexControl
			{
				float4 vertex : INTERNALTESSPOS;
				float3 ase_normal : NORMAL;
				float4 ase_tangent : TANGENT;
				float4 texcoord : TEXCOORD0;
				float4 texcoord1 : TEXCOORD1;
				
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl vert ( VertexInput v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.vertex = v.vertex;
				o.ase_normal = v.ase_normal;
				o.ase_tangent = v.ase_tangent;
				o.texcoord = v.texcoord;
				o.texcoord1 = v.texcoord1;
				
				return o;
			}

			TessellationFactors TessellationFunction (InputPatch<VertexControl,3> v)
			{
				TessellationFactors o;
				float4 tf = 1;
				float tessValue = _TessValue; float tessMin = _TessMin; float tessMax = _TessMax;
				float edgeLength = _TessEdgeLength; float tessMaxDisp = _TessMaxDisp;
				#if defined(ASE_FIXED_TESSELLATION)
				tf = FixedTess( tessValue );
				#elif defined(ASE_DISTANCE_TESSELLATION)
				tf = DistanceBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, tessValue, tessMin, tessMax, GetObjectToWorldMatrix(), _WorldSpaceCameraPos );
				#elif defined(ASE_LENGTH_TESSELLATION)
				tf = EdgeLengthBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams );
				#elif defined(ASE_LENGTH_CULL_TESSELLATION)
				tf = EdgeLengthBasedTessCull(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, tessMaxDisp, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams, unity_CameraWorldClipPlanes );
				#endif
				o.edge[0] = tf.x; o.edge[1] = tf.y; o.edge[2] = tf.z; o.inside = tf.w;
				return o;
			}

			[domain("tri")]
			[partitioning("fractional_odd")]
			[outputtopology("triangle_cw")]
			[patchconstantfunc("TessellationFunction")]
			[outputcontrolpoints(3)]
			VertexControl HullFunction(InputPatch<VertexControl, 3> patch, uint id : SV_OutputControlPointID)
			{
			   return patch[id];
			}

			[domain("tri")]
			VertexOutput DomainFunction(TessellationFactors factors, OutputPatch<VertexControl, 3> patch, float3 bary : SV_DomainLocation)
			{
				VertexInput o = (VertexInput) 0;
				o.vertex = patch[0].vertex * bary.x + patch[1].vertex * bary.y + patch[2].vertex * bary.z;
				o.ase_normal = patch[0].ase_normal * bary.x + patch[1].ase_normal * bary.y + patch[2].ase_normal * bary.z;
				o.ase_tangent = patch[0].ase_tangent * bary.x + patch[1].ase_tangent * bary.y + patch[2].ase_tangent * bary.z;
				o.texcoord = patch[0].texcoord * bary.x + patch[1].texcoord * bary.y + patch[2].texcoord * bary.z;
				o.texcoord1 = patch[0].texcoord1 * bary.x + patch[1].texcoord1 * bary.y + patch[2].texcoord1 * bary.z;
				
				#if defined(ASE_PHONG_TESSELLATION)
				float3 pp[3];
				for (int i = 0; i < 3; ++i)
					pp[i] = o.vertex.xyz - patch[i].ase_normal * (dot(o.vertex.xyz, patch[i].ase_normal) - dot(patch[i].vertex.xyz, patch[i].ase_normal));
				float phongStrength = _TessPhongStrength;
				o.vertex.xyz = phongStrength * (pp[0]*bary.x + pp[1]*bary.y + pp[2]*bary.z) + (1.0f-phongStrength) * o.vertex.xyz;
				#endif
				UNITY_TRANSFER_INSTANCE_ID(patch[0], o);
				return VertexFunction(o);
			}
			#else
			VertexOutput vert ( VertexInput v )
			{
				return VertexFunction( v );
			}
			#endif

			half4 frag ( VertexOutput IN  ) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID(IN);
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(IN);

				#ifdef LOD_FADE_CROSSFADE
					LODDitheringTransition( IN.clipPos.xyz, unity_LODFade.x );
				#endif

				#if defined(ENABLE_TERRAIN_PERPIXEL_NORMAL)
					float2 sampleCoords = (IN.lightmapUVOrVertexSH.zw / _TerrainHeightmapRecipSize.zw + 0.5f) * _TerrainHeightmapRecipSize.xy;
					float3 WorldNormal = TransformObjectToWorldNormal(normalize(SAMPLE_TEXTURE2D(_TerrainNormalmapTexture, sampler_TerrainNormalmapTexture, sampleCoords).rgb * 2 - 1));
					float3 WorldTangent = -cross(GetObjectToWorldMatrix()._13_23_33, WorldNormal);
					float3 WorldBiTangent = cross(WorldNormal, -WorldTangent);
				#else
					float3 WorldNormal = normalize( IN.tSpace0.xyz );
					float3 WorldTangent = IN.tSpace1.xyz;
					float3 WorldBiTangent = IN.tSpace2.xyz;
				#endif
				float3 WorldPosition = float3(IN.tSpace0.w,IN.tSpace1.w,IN.tSpace2.w);
				float3 WorldViewDirection = _WorldSpaceCameraPos.xyz  - WorldPosition;
				float4 ShadowCoords = float4( 0, 0, 0, 0 );
				#if defined(ASE_NEEDS_FRAG_SCREEN_POSITION)
				float4 ScreenPos = IN.screenPos;
				#endif

				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
					ShadowCoords = IN.shadowCoord;
				#elif defined(MAIN_LIGHT_CALCULATE_SHADOWS)
					ShadowCoords = TransformWorldToShadowCoord( WorldPosition );
				#endif
	
				WorldViewDirection = SafeNormalize( WorldViewDirection );

				float4 color426 = IsGammaSpace() ? float4(0,0,0,1) : float4(0,0,0,1);
				
				float4 color74 = IsGammaSpace() ? float4(1,1,1,1) : float4(1,1,1,1);
				float2 temp_cast_1 = (_ShadowTextureTiling).xx;
				float2 texCoord65 = IN.ase_texcoord7.xy * temp_cast_1 + float2( 0,0 );
				float cos70 = cos( radians( _ShadowTextureRotation ) );
				float sin70 = sin( radians( _ShadowTextureRotation ) );
				float2 rotator70 = mul( (( _ShadowTextureViewProjection )?( ( ( _ShadowTextureTiling * 1 ) * mul( UNITY_MATRIX_VP, float4( WorldViewDirection , 0.0 ) ).xyz ) ):( float3( texCoord65 ,  0.0 ) )).xy - float2( 0,0 ) , float2x2( cos70 , -sin70 , sin70 , cos70 )) + float2( 0,0 );
				float4 ShadowColor80 = (( _UseShadow )?( ( _ShadowColor * tex2D( _ShadowTexture, rotator70 ) ) ):( color74 ));
				float3 bakedGI11 = ASEIndirectDiffuse( IN.lightmapUVOrVertexSH.xy, WorldNormal);
				float4 Ambient181 = ( _MainLightColor * float4( bakedGI11 , 0.0 ) );
				float4 appendResult93 = (float4(IN.ase_texcoord8.xyz.x , IN.ase_texcoord8.xyz.y , 0.0 , 0.0));
				float cos95 = cos( radians( _GradientRotation ) );
				float sin95 = sin( radians( _GradientRotation ) );
				float2 rotator95 = mul( appendResult93.xy - float2( 0,0 ) , float2x2( cos95 , -sin95 , sin95 , cos95 )) + float2( 0,0 );
				float smoothstepResult101 = smoothstep( _GradientPosition , ( _GradientPosition + _GradientSize ) , rotator95.x);
				float4 lerpResult102 = lerp( _ColorA , _ColorB , smoothstepResult101);
				float2 uv_TextureDiffuse108 = IN.ase_texcoord7.xy;
				float4 tex2DNode108 = tex2D( _TextureDiffuse, uv_TextureDiffuse108 );
				float4 Color110 = ( ( Ambient181 + (( _UseGradient )?( lerpResult102 ):( ( float4( _MainLightColor.rgb , 0.0 ) * _DiffuseColor ) )) ) * tex2DNode108 );
				float ase_lightAtten = 0;
				Light ase_lightAtten_mainLight = GetMainLight( ShadowCoords );
				ase_lightAtten = ase_lightAtten_mainLight.distanceAttenuation * ase_lightAtten_mainLight.shadowAttenuation;
				float Atten328 = pow( abs( ase_lightAtten ) , _AttenuationPower );
				float3 normalizedWorldNormal = normalize( WorldNormal );
				float dotResult3 = dot( normalizedWorldNormal , SafeNormalize(_MainLightPosition.xyz) );
				float NdotL175 = dotResult3;
				float smoothstepResult58 = smoothstep( _ShadowSize , ( _ShadowSize + _ShadowBlend ) , ( Atten328 + NdotL175 ));
				float4 temp_output_86_0 = ( ( ShadowColor80 * Color110 ) * ( 1.0 - smoothstepResult58 ) );
				float dotResult38 = dot( WorldNormal , WorldViewDirection );
				float3 WorldPosition5_g3 = WorldPosition;
				float3 WorldNormal5_g3 = WorldNormal;
				float3 localAdditionalLightsLambert5_g3 = AdditionalLightsLambert( WorldPosition5_g3 , WorldNormal5_g3 );
				float2 temp_cast_10 = (_RimTextureTiling).xx;
				float2 texCoord193 = IN.ase_texcoord7.xy * temp_cast_10 + float2( 0,0 );
				float cos198 = cos( radians( _RimTextureRotation ) );
				float sin198 = sin( radians( _RimTextureRotation ) );
				float2 rotator198 = mul( (( _RimTextureViewProjection )?( ( ( _RimTextureTiling * 1 ) * mul( float4( WorldViewDirection , 0.0 ), UNITY_MATRIX_VP ).xyz ) ):( float3( texCoord193 ,  0.0 ) )).xy - float2( 0,0 ) , float2x2( cos198 , -sin198 , sin198 , cos198 )) + float2( 0,0 );
				float3 WorldPosition5_g2 = WorldPosition;
				float3 WorldNormal5_g2 = WorldNormal;
				float3 localAdditionalLightsLambert5_g2 = AdditionalLightsLambert( WorldPosition5_g2 , WorldNormal5_g2 );
				float4 MainTexture506 = tex2DNode108;
				float4 AdditionalLights511 = ( float4( localAdditionalLightsLambert5_g2 , 0.0 ) * MainTexture506 );
				float4 Rim185 = (( _UseRim )?( ( saturate( ( NdotL175 * pow( ( 1.0 - saturate( ( dotResult38 + _RimSize ) ) ) , _RimBlend ) ) ) * ( (( _RimLightColor )?( ( ( _RimLightIntensity * _MainLightColor ) + float4( localAdditionalLightsLambert5_g3 , 0.0 ) ) ):( _RimColor )) * tex2D( _RimTexture, rotator198 ) ) * AdditionalLights511 ) ):( float4( 0,0,0,0 ) ));
				float2 temp_cast_16 = (_SpecularTextureTiling).xx;
				float2 texCoord143 = IN.ase_texcoord7.xy * temp_cast_16 + float2( 0,0 );
				float cos156 = cos( radians( _SpecularTextureRotation ) );
				float sin156 = sin( radians( _SpecularTextureRotation ) );
				float2 rotator156 = mul( (( _SpecularTextureViewProjection )?( ( ( _SpecularTextureTiling * 1 ) * mul( float4( WorldViewDirection , 0.0 ), UNITY_MATRIX_VP ).xyz ) ):( float3( texCoord143 ,  0.0 ) )).xy - float2( 0,0 ) , float2x2( cos156 , -sin156 , sin156 , cos156 )) + float2( 0,0 );
				float3 WorldPosition5_g4 = WorldPosition;
				float3 WorldNormal5_g4 = WorldNormal;
				float3 localAdditionalLightsLambert5_g4 = AdditionalLightsLambert( WorldPosition5_g4 , WorldNormal5_g4 );
				float temp_output_161_0 = ( 1.0 - _SpecularSize );
				float3 normalizeResult148 = normalize( SafeNormalize(_MainLightPosition.xyz) );
				float3 normalizeResult147 = normalize( WorldViewDirection );
				float3 normalizeResult162 = normalize( ( normalizeResult148 + normalizeResult147 ) );
				float3 normalizeResult157 = normalize( WorldNormal );
				float dotResult165 = dot( normalizeResult162 , normalizeResult157 );
				float smoothstepResult169 = smoothstep( temp_output_161_0 , ( temp_output_161_0 + _SpecularBlend ) , dotResult165);
				float4 Specular174 = (( _UseSpecular )?( ( ( ( ( 1.0 - tex2D( _SpecularMap, rotator156 ) ) * (( _SpecLightColor )?( ( ( _MainLightColor * _SpecularLightIntensity ) + float4( localAdditionalLightsLambert5_g4 , 0.0 ) ) ):( _SpecularColor )) ) * smoothstepResult169 ) * NdotL175 * AdditionalLights511 ) ):( float4( 0,0,0,0 ) ));
				float4 Result135 = ( temp_output_86_0 + ( Color110 * smoothstepResult58 ) + ( Rim185 * Atten328 ) + ( Atten328 * Specular174 ) );
				float2 uv_Texture0119 = IN.ase_texcoord7.xy;
				float3 ase_objectScale = float3( length( GetObjectToWorldMatrix()[ 0 ].xyz ), length( GetObjectToWorldMatrix()[ 1 ].xyz ), length( GetObjectToWorldMatrix()[ 2 ].xyz ) );
				float3 normalizeResult123 = normalize( SafeNormalize(_MainLightPosition.xyz) );
				float dotResult128 = dot( BlendNormal( UnpackNormalScale( tex2D( _Texture0, uv_Texture0119 ), 1.0f ) , ase_objectScale ) , normalizeResult123 );
				float2 uv_Normal120 = IN.ase_texcoord7.xy;
				float3 unpack120 = UnpackNormalScale( tex2D( _Normal, uv_Normal120 ), _NormalStrength );
				unpack120.z = lerp( 1, unpack120.z, saturate(_NormalStrength) );
				float dotResult127 = dot( BlendNormal( unpack120 , ase_objectScale ) , SafeNormalize(_MainLightPosition.xyz) );
				float4 NormalMap132 = (( _UseNormalMap )?( ( dotResult127 * Result135 ) ):( ( dotResult128 * Result135 ) ));
				
				float3 Albedo = color426.rgb;
				float3 Normal = float3(0, 0, 1);
				float3 Emission = ( Result135 + NormalMap132 + AdditionalLights511 ).rgb;
				float3 Specular = 0.5;
				float Metallic = color426.r;
				float Smoothness = color426.r;
				float Occlusion = 1;
				float Alpha = _Opacity;
				float AlphaClipThreshold = 0.5;
				float AlphaClipThresholdShadow = 0.5;
				float3 BakedGI = 0;
				float3 RefractionColor = 1;
				float RefractionIndex = 1;
				float3 Transmission = 1;
				float3 Translucency = 1;

				#ifdef _ALPHATEST_ON
					clip(Alpha - AlphaClipThreshold);
				#endif

				InputData inputData;
				inputData.positionWS = WorldPosition;
				inputData.viewDirectionWS = WorldViewDirection;
				inputData.shadowCoord = ShadowCoords;

				#ifdef _NORMALMAP
					#if _NORMAL_DROPOFF_TS
					inputData.normalWS = TransformTangentToWorld(Normal, half3x3( WorldTangent, WorldBiTangent, WorldNormal ));
					#elif _NORMAL_DROPOFF_OS
					inputData.normalWS = TransformObjectToWorldNormal(Normal);
					#elif _NORMAL_DROPOFF_WS
					inputData.normalWS = Normal;
					#endif
					inputData.normalWS = NormalizeNormalPerPixel(inputData.normalWS);
				#else
					inputData.normalWS = WorldNormal;
				#endif

				#ifdef ASE_FOG
					inputData.fogCoord = IN.fogFactorAndVertexLight.x;
				#endif

				inputData.vertexLighting = IN.fogFactorAndVertexLight.yzw;
				#if defined(ENABLE_TERRAIN_PERPIXEL_NORMAL)
					float3 SH = SampleSH(inputData.normalWS.xyz);
				#else
					float3 SH = IN.lightmapUVOrVertexSH.xyz;
				#endif

				inputData.bakedGI = SAMPLE_GI( IN.lightmapUVOrVertexSH.xy, SH, inputData.normalWS );
				#ifdef _ASE_BAKEDGI
					inputData.bakedGI = BakedGI;
				#endif
				half4 color = UniversalFragmentPBR(
					inputData, 
					Albedo, 
					Metallic, 
					Specular, 
					Smoothness, 
					Occlusion, 
					Emission, 
					Alpha);

				#ifdef _TRANSMISSION_ASE
				{
					float shadow = _TransmissionShadow;

					Light mainLight = GetMainLight( inputData.shadowCoord );
					float3 mainAtten = mainLight.color * mainLight.distanceAttenuation;
					mainAtten = lerp( mainAtten, mainAtten * mainLight.shadowAttenuation, shadow );
					half3 mainTransmission = max(0 , -dot(inputData.normalWS, mainLight.direction)) * mainAtten * Transmission;
					color.rgb += Albedo * mainTransmission;

					#ifdef _ADDITIONAL_LIGHTS
						int transPixelLightCount = GetAdditionalLightsCount();
						for (int i = 0; i < transPixelLightCount; ++i)
						{
							Light light = GetAdditionalLight(i, inputData.positionWS);
							float3 atten = light.color * light.distanceAttenuation;
							atten = lerp( atten, atten * light.shadowAttenuation, shadow );

							half3 transmission = max(0 , -dot(inputData.normalWS, light.direction)) * atten * Transmission;
							color.rgb += Albedo * transmission;
						}
					#endif
				}
				#endif

				#ifdef _TRANSLUCENCY_ASE
				{
					float shadow = _TransShadow;
					float normal = _TransNormal;
					float scattering = _TransScattering;
					float direct = _TransDirect;
					float ambient = _TransAmbient;
					float strength = _TransStrength;

					Light mainLight = GetMainLight( inputData.shadowCoord );
					float3 mainAtten = mainLight.color * mainLight.distanceAttenuation;
					mainAtten = lerp( mainAtten, mainAtten * mainLight.shadowAttenuation, shadow );

					half3 mainLightDir = mainLight.direction + inputData.normalWS * normal;
					half mainVdotL = pow( saturate( dot( inputData.viewDirectionWS, -mainLightDir ) ), scattering );
					half3 mainTranslucency = mainAtten * ( mainVdotL * direct + inputData.bakedGI * ambient ) * Translucency;
					color.rgb += Albedo * mainTranslucency * strength;

					#ifdef _ADDITIONAL_LIGHTS
						int transPixelLightCount = GetAdditionalLightsCount();
						for (int i = 0; i < transPixelLightCount; ++i)
						{
							Light light = GetAdditionalLight(i, inputData.positionWS);
							float3 atten = light.color * light.distanceAttenuation;
							atten = lerp( atten, atten * light.shadowAttenuation, shadow );

							half3 lightDir = light.direction + inputData.normalWS * normal;
							half VdotL = pow( saturate( dot( inputData.viewDirectionWS, -lightDir ) ), scattering );
							half3 translucency = atten * ( VdotL * direct + inputData.bakedGI * ambient ) * Translucency;
							color.rgb += Albedo * translucency * strength;
						}
					#endif
				}
				#endif

				#ifdef _REFRACTION_ASE
					float4 projScreenPos = ScreenPos / ScreenPos.w;
					float3 refractionOffset = ( RefractionIndex - 1.0 ) * mul( UNITY_MATRIX_V, WorldNormal ).xyz * ( 1.0 - dot( WorldNormal, WorldViewDirection ) );
					projScreenPos.xy += refractionOffset.xy;
					float3 refraction = SHADERGRAPH_SAMPLE_SCENE_COLOR( projScreenPos ) * RefractionColor;
					color.rgb = lerp( refraction, color.rgb, color.a );
					color.a = 1;
				#endif

				#ifdef ASE_FINAL_COLOR_ALPHA_MULTIPLY
					color.rgb *= color.a;
				#endif

				#ifdef ASE_FOG
					#ifdef TERRAIN_SPLAT_ADDPASS
						color.rgb = MixFogColor(color.rgb, half3( 0, 0, 0 ), IN.fogFactorAndVertexLight.x );
					#else
						color.rgb = MixFog(color.rgb, IN.fogFactorAndVertexLight.x);
					#endif
				#endif
				
				return color;
			}

			ENDHLSL
		}

		
		Pass
		{
			
			Name "Meta"
			Tags { "LightMode"="Meta" }

			Cull Off

			HLSLPROGRAM
			#define _NORMAL_DROPOFF_TS 1
			#pragma multi_compile_instancing
			#pragma multi_compile_fog
			#define ASE_FOG 1
			#define ASE_FINAL_COLOR_ALPHA_MULTIPLY 1
			#define _EMISSION
			#define ASE_SRP_VERSION 70301

			#pragma prefer_hlslcc gles
			#pragma exclude_renderers d3d11_9x

			#pragma vertex vert
			#pragma fragment frag

			#define SHADERPASS_META

			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/MetaInput.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"

			#define ASE_NEEDS_FRAG_WORLD_POSITION
			#define ASE_NEEDS_VERT_TEXTURE_COORDINATES1
			#define ASE_NEEDS_VERT_NORMAL
			#define ASE_NEEDS_FRAG_POSITION
			#define ASE_NEEDS_FRAG_SHADOWCOORDS
			#pragma multi_compile _ DIRLIGHTMAP_COMBINED
			#pragma multi_compile _ LIGHTMAP_ON
			#pragma multi_compile _ _MAIN_LIGHT_SHADOWS
			#pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
			#pragma multi_compile _ _SHADOWS_SOFT
			#pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
			#pragma multi_compile _ _ADDITIONAL_LIGHT_SHADOWS


			#pragma shader_feature _ _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A

			struct VertexInput
			{
				float4 vertex : POSITION;
				float3 ase_normal : NORMAL;
				float4 texcoord1 : TEXCOORD1;
				float4 texcoord2 : TEXCOORD2;
				float4 ase_texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 clipPos : SV_POSITION;
				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				float3 worldPos : TEXCOORD0;
				#endif
				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR) && defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
				float4 shadowCoord : TEXCOORD1;
				#endif
				float4 ase_texcoord2 : TEXCOORD2;
				float4 lightmapUVOrVertexSH : TEXCOORD3;
				float4 ase_texcoord4 : TEXCOORD4;
				float4 ase_texcoord5 : TEXCOORD5;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			CBUFFER_START(UnityPerMaterial)
			float4 _SpecularColor;
			float4 _ColorB;
			float4 _ColorA;
			float4 _DiffuseColor;
			float4 _RimColor;
			float4 _ShadowColor;
			float _SpecularLightIntensity;
			float _SpecularBlend;
			float _UseNormalMap;
			float _RimLightIntensity;
			float _SpecLightColor;
			float _SpecularTextureRotation;
			float _SpecularTextureTiling;
			float _SpecularTextureViewProjection;
			float _UseSpecular;
			float _RimTextureRotation;
			float _RimTextureTiling;
			float _RimTextureViewProjection;
			float _SpecularSize;
			float _UseShadow;
			float _RimBlend;
			float _NormalStrength;
			float _RimSize;
			float _UseRim;
			float _AttenuationPower;
			float _ShadowBlend;
			float _ShadowSize;
			float _GradientRotation;
			float _GradientSize;
			float _GradientPosition;
			float _UseGradient;
			float _ShadowTextureRotation;
			float _ShadowTextureTiling;
			float _ShadowTextureViewProjection;
			float _RimLightColor;
			float _Opacity;
			#ifdef _TRANSMISSION_ASE
				float _TransmissionShadow;
			#endif
			#ifdef _TRANSLUCENCY_ASE
				float _TransStrength;
				float _TransNormal;
				float _TransScattering;
				float _TransDirect;
				float _TransAmbient;
				float _TransShadow;
			#endif
			#ifdef TESSELLATION_ON
				float _TessPhongStrength;
				float _TessValue;
				float _TessMin;
				float _TessMax;
				float _TessEdgeLength;
				float _TessMaxDisp;
			#endif
			CBUFFER_END
			sampler2D _ShadowTexture;
			sampler2D _TextureDiffuse;
			sampler2D _RimTexture;
			sampler2D _SpecularMap;
			sampler2D _Texture0;
			sampler2D _Normal;


			float3 ASEIndirectDiffuse( float2 uvStaticLightmap, float3 normalWS )
			{
			#ifdef LIGHTMAP_ON
				return SampleLightmap( uvStaticLightmap, normalWS );
			#else
				return SampleSH(normalWS);
			#endif
			}
			
			float3 AdditionalLightsLambert( float3 WorldPosition, float3 WorldNormal )
			{
				float3 Color = 0;
				#ifdef _ADDITIONAL_LIGHTS
				int numLights = GetAdditionalLightsCount();
				for(int i = 0; i<numLights;i++)
				{
					Light light = GetAdditionalLight(i, WorldPosition);
					half3 AttLightColor = light.color *(light.distanceAttenuation * light.shadowAttenuation);
					Color +=LightingLambert(AttLightColor, light.direction, WorldNormal);
					
				}
				#endif
				return Color;
			}
			

			VertexOutput VertexFunction( VertexInput v  )
			{
				VertexOutput o = (VertexOutput)0;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

				float3 ase_worldNormal = TransformObjectToWorldNormal(v.ase_normal);
				OUTPUT_LIGHTMAP_UV( v.texcoord1, unity_LightmapST, o.lightmapUVOrVertexSH.xy );
				OUTPUT_SH( ase_worldNormal, o.lightmapUVOrVertexSH.xyz );
				o.ase_texcoord4.xyz = ase_worldNormal;
				
				o.ase_texcoord2.xy = v.ase_texcoord.xy;
				o.ase_texcoord5 = v.vertex;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord2.zw = 0;
				o.ase_texcoord4.w = 0;
				
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.vertex.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif
				float3 vertexValue = defaultVertexValue;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					v.vertex.xyz = vertexValue;
				#else
					v.vertex.xyz += vertexValue;
				#endif

				v.ase_normal = v.ase_normal;

				float3 positionWS = TransformObjectToWorld( v.vertex.xyz );
				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				o.worldPos = positionWS;
				#endif

				o.clipPos = MetaVertexPosition( v.vertex, v.texcoord1.xy, v.texcoord1.xy, unity_LightmapST, unity_DynamicLightmapST );
				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR) && defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
					VertexPositionInputs vertexInput = (VertexPositionInputs)0;
					vertexInput.positionWS = positionWS;
					vertexInput.positionCS = o.clipPos;
					o.shadowCoord = GetShadowCoord( vertexInput );
				#endif
				return o;
			}

			#if defined(TESSELLATION_ON)
			struct VertexControl
			{
				float4 vertex : INTERNALTESSPOS;
				float3 ase_normal : NORMAL;
				float4 texcoord1 : TEXCOORD1;
				float4 texcoord2 : TEXCOORD2;
				float4 ase_texcoord : TEXCOORD0;

				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl vert ( VertexInput v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.vertex = v.vertex;
				o.ase_normal = v.ase_normal;
				o.texcoord1 = v.texcoord1;
				o.texcoord2 = v.texcoord2;
				o.ase_texcoord = v.ase_texcoord;
				return o;
			}

			TessellationFactors TessellationFunction (InputPatch<VertexControl,3> v)
			{
				TessellationFactors o;
				float4 tf = 1;
				float tessValue = _TessValue; float tessMin = _TessMin; float tessMax = _TessMax;
				float edgeLength = _TessEdgeLength; float tessMaxDisp = _TessMaxDisp;
				#if defined(ASE_FIXED_TESSELLATION)
				tf = FixedTess( tessValue );
				#elif defined(ASE_DISTANCE_TESSELLATION)
				tf = DistanceBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, tessValue, tessMin, tessMax, GetObjectToWorldMatrix(), _WorldSpaceCameraPos );
				#elif defined(ASE_LENGTH_TESSELLATION)
				tf = EdgeLengthBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams );
				#elif defined(ASE_LENGTH_CULL_TESSELLATION)
				tf = EdgeLengthBasedTessCull(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, tessMaxDisp, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams, unity_CameraWorldClipPlanes );
				#endif
				o.edge[0] = tf.x; o.edge[1] = tf.y; o.edge[2] = tf.z; o.inside = tf.w;
				return o;
			}

			[domain("tri")]
			[partitioning("fractional_odd")]
			[outputtopology("triangle_cw")]
			[patchconstantfunc("TessellationFunction")]
			[outputcontrolpoints(3)]
			VertexControl HullFunction(InputPatch<VertexControl, 3> patch, uint id : SV_OutputControlPointID)
			{
			   return patch[id];
			}

			[domain("tri")]
			VertexOutput DomainFunction(TessellationFactors factors, OutputPatch<VertexControl, 3> patch, float3 bary : SV_DomainLocation)
			{
				VertexInput o = (VertexInput) 0;
				o.vertex = patch[0].vertex * bary.x + patch[1].vertex * bary.y + patch[2].vertex * bary.z;
				o.ase_normal = patch[0].ase_normal * bary.x + patch[1].ase_normal * bary.y + patch[2].ase_normal * bary.z;
				o.texcoord1 = patch[0].texcoord1 * bary.x + patch[1].texcoord1 * bary.y + patch[2].texcoord1 * bary.z;
				o.texcoord2 = patch[0].texcoord2 * bary.x + patch[1].texcoord2 * bary.y + patch[2].texcoord2 * bary.z;
				o.ase_texcoord = patch[0].ase_texcoord * bary.x + patch[1].ase_texcoord * bary.y + patch[2].ase_texcoord * bary.z;
				#if defined(ASE_PHONG_TESSELLATION)
				float3 pp[3];
				for (int i = 0; i < 3; ++i)
					pp[i] = o.vertex.xyz - patch[i].ase_normal * (dot(o.vertex.xyz, patch[i].ase_normal) - dot(patch[i].vertex.xyz, patch[i].ase_normal));
				float phongStrength = _TessPhongStrength;
				o.vertex.xyz = phongStrength * (pp[0]*bary.x + pp[1]*bary.y + pp[2]*bary.z) + (1.0f-phongStrength) * o.vertex.xyz;
				#endif
				UNITY_TRANSFER_INSTANCE_ID(patch[0], o);
				return VertexFunction(o);
			}
			#else
			VertexOutput vert ( VertexInput v )
			{
				return VertexFunction( v );
			}
			#endif

			half4 frag(VertexOutput IN  ) : SV_TARGET
			{
				UNITY_SETUP_INSTANCE_ID(IN);
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( IN );

				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				float3 WorldPosition = IN.worldPos;
				#endif
				float4 ShadowCoords = float4( 0, 0, 0, 0 );

				#if defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
					#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
						ShadowCoords = IN.shadowCoord;
					#elif defined(MAIN_LIGHT_CALCULATE_SHADOWS)
						ShadowCoords = TransformWorldToShadowCoord( WorldPosition );
					#endif
				#endif

				float4 color426 = IsGammaSpace() ? float4(0,0,0,1) : float4(0,0,0,1);
				
				float4 color74 = IsGammaSpace() ? float4(1,1,1,1) : float4(1,1,1,1);
				float2 temp_cast_1 = (_ShadowTextureTiling).xx;
				float2 texCoord65 = IN.ase_texcoord2.xy * temp_cast_1 + float2( 0,0 );
				float3 ase_worldViewDir = ( _WorldSpaceCameraPos.xyz - WorldPosition );
				ase_worldViewDir = SafeNormalize( ase_worldViewDir );
				float cos70 = cos( radians( _ShadowTextureRotation ) );
				float sin70 = sin( radians( _ShadowTextureRotation ) );
				float2 rotator70 = mul( (( _ShadowTextureViewProjection )?( ( ( _ShadowTextureTiling * 1 ) * mul( UNITY_MATRIX_VP, float4( ase_worldViewDir , 0.0 ) ).xyz ) ):( float3( texCoord65 ,  0.0 ) )).xy - float2( 0,0 ) , float2x2( cos70 , -sin70 , sin70 , cos70 )) + float2( 0,0 );
				float4 ShadowColor80 = (( _UseShadow )?( ( _ShadowColor * tex2D( _ShadowTexture, rotator70 ) ) ):( color74 ));
				float3 ase_worldNormal = IN.ase_texcoord4.xyz;
				float3 bakedGI11 = ASEIndirectDiffuse( IN.lightmapUVOrVertexSH.xy, ase_worldNormal);
				float4 Ambient181 = ( _MainLightColor * float4( bakedGI11 , 0.0 ) );
				float4 appendResult93 = (float4(IN.ase_texcoord5.xyz.x , IN.ase_texcoord5.xyz.y , 0.0 , 0.0));
				float cos95 = cos( radians( _GradientRotation ) );
				float sin95 = sin( radians( _GradientRotation ) );
				float2 rotator95 = mul( appendResult93.xy - float2( 0,0 ) , float2x2( cos95 , -sin95 , sin95 , cos95 )) + float2( 0,0 );
				float smoothstepResult101 = smoothstep( _GradientPosition , ( _GradientPosition + _GradientSize ) , rotator95.x);
				float4 lerpResult102 = lerp( _ColorA , _ColorB , smoothstepResult101);
				float2 uv_TextureDiffuse108 = IN.ase_texcoord2.xy;
				float4 tex2DNode108 = tex2D( _TextureDiffuse, uv_TextureDiffuse108 );
				float4 Color110 = ( ( Ambient181 + (( _UseGradient )?( lerpResult102 ):( ( float4( _MainLightColor.rgb , 0.0 ) * _DiffuseColor ) )) ) * tex2DNode108 );
				float ase_lightAtten = 0;
				Light ase_lightAtten_mainLight = GetMainLight( ShadowCoords );
				ase_lightAtten = ase_lightAtten_mainLight.distanceAttenuation * ase_lightAtten_mainLight.shadowAttenuation;
				float Atten328 = pow( abs( ase_lightAtten ) , _AttenuationPower );
				float3 normalizedWorldNormal = normalize( ase_worldNormal );
				float dotResult3 = dot( normalizedWorldNormal , SafeNormalize(_MainLightPosition.xyz) );
				float NdotL175 = dotResult3;
				float smoothstepResult58 = smoothstep( _ShadowSize , ( _ShadowSize + _ShadowBlend ) , ( Atten328 + NdotL175 ));
				float4 temp_output_86_0 = ( ( ShadowColor80 * Color110 ) * ( 1.0 - smoothstepResult58 ) );
				ase_worldViewDir = normalize(ase_worldViewDir);
				float dotResult38 = dot( ase_worldNormal , ase_worldViewDir );
				float3 WorldPosition5_g3 = WorldPosition;
				float3 WorldNormal5_g3 = ase_worldNormal;
				float3 localAdditionalLightsLambert5_g3 = AdditionalLightsLambert( WorldPosition5_g3 , WorldNormal5_g3 );
				float2 temp_cast_10 = (_RimTextureTiling).xx;
				float2 texCoord193 = IN.ase_texcoord2.xy * temp_cast_10 + float2( 0,0 );
				float cos198 = cos( radians( _RimTextureRotation ) );
				float sin198 = sin( radians( _RimTextureRotation ) );
				float2 rotator198 = mul( (( _RimTextureViewProjection )?( ( ( _RimTextureTiling * 1 ) * mul( float4( ase_worldViewDir , 0.0 ), UNITY_MATRIX_VP ).xyz ) ):( float3( texCoord193 ,  0.0 ) )).xy - float2( 0,0 ) , float2x2( cos198 , -sin198 , sin198 , cos198 )) + float2( 0,0 );
				float3 WorldPosition5_g2 = WorldPosition;
				float3 WorldNormal5_g2 = ase_worldNormal;
				float3 localAdditionalLightsLambert5_g2 = AdditionalLightsLambert( WorldPosition5_g2 , WorldNormal5_g2 );
				float4 MainTexture506 = tex2DNode108;
				float4 AdditionalLights511 = ( float4( localAdditionalLightsLambert5_g2 , 0.0 ) * MainTexture506 );
				float4 Rim185 = (( _UseRim )?( ( saturate( ( NdotL175 * pow( ( 1.0 - saturate( ( dotResult38 + _RimSize ) ) ) , _RimBlend ) ) ) * ( (( _RimLightColor )?( ( ( _RimLightIntensity * _MainLightColor ) + float4( localAdditionalLightsLambert5_g3 , 0.0 ) ) ):( _RimColor )) * tex2D( _RimTexture, rotator198 ) ) * AdditionalLights511 ) ):( float4( 0,0,0,0 ) ));
				float2 temp_cast_16 = (_SpecularTextureTiling).xx;
				float2 texCoord143 = IN.ase_texcoord2.xy * temp_cast_16 + float2( 0,0 );
				float cos156 = cos( radians( _SpecularTextureRotation ) );
				float sin156 = sin( radians( _SpecularTextureRotation ) );
				float2 rotator156 = mul( (( _SpecularTextureViewProjection )?( ( ( _SpecularTextureTiling * 1 ) * mul( float4( ase_worldViewDir , 0.0 ), UNITY_MATRIX_VP ).xyz ) ):( float3( texCoord143 ,  0.0 ) )).xy - float2( 0,0 ) , float2x2( cos156 , -sin156 , sin156 , cos156 )) + float2( 0,0 );
				float3 WorldPosition5_g4 = WorldPosition;
				float3 WorldNormal5_g4 = ase_worldNormal;
				float3 localAdditionalLightsLambert5_g4 = AdditionalLightsLambert( WorldPosition5_g4 , WorldNormal5_g4 );
				float temp_output_161_0 = ( 1.0 - _SpecularSize );
				float3 normalizeResult148 = normalize( SafeNormalize(_MainLightPosition.xyz) );
				float3 normalizeResult147 = normalize( ase_worldViewDir );
				float3 normalizeResult162 = normalize( ( normalizeResult148 + normalizeResult147 ) );
				float3 normalizeResult157 = normalize( ase_worldNormal );
				float dotResult165 = dot( normalizeResult162 , normalizeResult157 );
				float smoothstepResult169 = smoothstep( temp_output_161_0 , ( temp_output_161_0 + _SpecularBlend ) , dotResult165);
				float4 Specular174 = (( _UseSpecular )?( ( ( ( ( 1.0 - tex2D( _SpecularMap, rotator156 ) ) * (( _SpecLightColor )?( ( ( _MainLightColor * _SpecularLightIntensity ) + float4( localAdditionalLightsLambert5_g4 , 0.0 ) ) ):( _SpecularColor )) ) * smoothstepResult169 ) * NdotL175 * AdditionalLights511 ) ):( float4( 0,0,0,0 ) ));
				float4 Result135 = ( temp_output_86_0 + ( Color110 * smoothstepResult58 ) + ( Rim185 * Atten328 ) + ( Atten328 * Specular174 ) );
				float2 uv_Texture0119 = IN.ase_texcoord2.xy;
				float3 ase_objectScale = float3( length( GetObjectToWorldMatrix()[ 0 ].xyz ), length( GetObjectToWorldMatrix()[ 1 ].xyz ), length( GetObjectToWorldMatrix()[ 2 ].xyz ) );
				float3 normalizeResult123 = normalize( SafeNormalize(_MainLightPosition.xyz) );
				float dotResult128 = dot( BlendNormal( UnpackNormalScale( tex2D( _Texture0, uv_Texture0119 ), 1.0f ) , ase_objectScale ) , normalizeResult123 );
				float2 uv_Normal120 = IN.ase_texcoord2.xy;
				float3 unpack120 = UnpackNormalScale( tex2D( _Normal, uv_Normal120 ), _NormalStrength );
				unpack120.z = lerp( 1, unpack120.z, saturate(_NormalStrength) );
				float dotResult127 = dot( BlendNormal( unpack120 , ase_objectScale ) , SafeNormalize(_MainLightPosition.xyz) );
				float4 NormalMap132 = (( _UseNormalMap )?( ( dotResult127 * Result135 ) ):( ( dotResult128 * Result135 ) ));
				
				
				float3 Albedo = color426.rgb;
				float3 Emission = ( Result135 + NormalMap132 + AdditionalLights511 ).rgb;
				float Alpha = _Opacity;
				float AlphaClipThreshold = 0.5;

				#ifdef _ALPHATEST_ON
					clip(Alpha - AlphaClipThreshold);
				#endif

				MetaInput metaInput = (MetaInput)0;
				metaInput.Albedo = Albedo;
				metaInput.Emission = Emission;
				
				return MetaFragment(metaInput);
			}
			ENDHLSL
		}

		
		Pass
		{
			
			Name "Universal2D"
			Tags { "LightMode"="Universal2D" }

			Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
			ZWrite Off
			ZTest LEqual
			Offset 0 , 0
			ColorMask RGBA

			HLSLPROGRAM
			#define _NORMAL_DROPOFF_TS 1
			#pragma multi_compile_instancing
			#pragma multi_compile_fog
			#define ASE_FOG 1
			#define ASE_FINAL_COLOR_ALPHA_MULTIPLY 1
			#define _EMISSION
			#define ASE_SRP_VERSION 70301

			#pragma enable_d3d11_debug_symbols
			#pragma prefer_hlslcc gles
			#pragma exclude_renderers d3d11_9x

			#pragma vertex vert
			#pragma fragment frag

			#define SHADERPASS_2D

			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/UnityInstancing.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
			
			#pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
			#pragma multi_compile _ _ADDITIONAL_LIGHT_SHADOWS


			#pragma shader_feature _ _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A

			struct VertexInput
			{
				float4 vertex : POSITION;
				float3 ase_normal : NORMAL;
				
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 clipPos : SV_POSITION;
				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				float3 worldPos : TEXCOORD0;
				#endif
				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR) && defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
				float4 shadowCoord : TEXCOORD1;
				#endif
				
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			CBUFFER_START(UnityPerMaterial)
			float4 _SpecularColor;
			float4 _ColorB;
			float4 _ColorA;
			float4 _DiffuseColor;
			float4 _RimColor;
			float4 _ShadowColor;
			float _SpecularLightIntensity;
			float _SpecularBlend;
			float _UseNormalMap;
			float _RimLightIntensity;
			float _SpecLightColor;
			float _SpecularTextureRotation;
			float _SpecularTextureTiling;
			float _SpecularTextureViewProjection;
			float _UseSpecular;
			float _RimTextureRotation;
			float _RimTextureTiling;
			float _RimTextureViewProjection;
			float _SpecularSize;
			float _UseShadow;
			float _RimBlend;
			float _NormalStrength;
			float _RimSize;
			float _UseRim;
			float _AttenuationPower;
			float _ShadowBlend;
			float _ShadowSize;
			float _GradientRotation;
			float _GradientSize;
			float _GradientPosition;
			float _UseGradient;
			float _ShadowTextureRotation;
			float _ShadowTextureTiling;
			float _ShadowTextureViewProjection;
			float _RimLightColor;
			float _Opacity;
			#ifdef _TRANSMISSION_ASE
				float _TransmissionShadow;
			#endif
			#ifdef _TRANSLUCENCY_ASE
				float _TransStrength;
				float _TransNormal;
				float _TransScattering;
				float _TransDirect;
				float _TransAmbient;
				float _TransShadow;
			#endif
			#ifdef TESSELLATION_ON
				float _TessPhongStrength;
				float _TessValue;
				float _TessMin;
				float _TessMax;
				float _TessEdgeLength;
				float _TessMaxDisp;
			#endif
			CBUFFER_END
			

			
			VertexOutput VertexFunction( VertexInput v  )
			{
				VertexOutput o = (VertexOutput)0;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );

				
				
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.vertex.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif
				float3 vertexValue = defaultVertexValue;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					v.vertex.xyz = vertexValue;
				#else
					v.vertex.xyz += vertexValue;
				#endif

				v.ase_normal = v.ase_normal;

				float3 positionWS = TransformObjectToWorld( v.vertex.xyz );
				float4 positionCS = TransformWorldToHClip( positionWS );

				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				o.worldPos = positionWS;
				#endif

				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR) && defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
					VertexPositionInputs vertexInput = (VertexPositionInputs)0;
					vertexInput.positionWS = positionWS;
					vertexInput.positionCS = positionCS;
					o.shadowCoord = GetShadowCoord( vertexInput );
				#endif

				o.clipPos = positionCS;
				return o;
			}

			#if defined(TESSELLATION_ON)
			struct VertexControl
			{
				float4 vertex : INTERNALTESSPOS;
				float3 ase_normal : NORMAL;
				
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl vert ( VertexInput v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.vertex = v.vertex;
				o.ase_normal = v.ase_normal;
				
				return o;
			}

			TessellationFactors TessellationFunction (InputPatch<VertexControl,3> v)
			{
				TessellationFactors o;
				float4 tf = 1;
				float tessValue = _TessValue; float tessMin = _TessMin; float tessMax = _TessMax;
				float edgeLength = _TessEdgeLength; float tessMaxDisp = _TessMaxDisp;
				#if defined(ASE_FIXED_TESSELLATION)
				tf = FixedTess( tessValue );
				#elif defined(ASE_DISTANCE_TESSELLATION)
				tf = DistanceBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, tessValue, tessMin, tessMax, GetObjectToWorldMatrix(), _WorldSpaceCameraPos );
				#elif defined(ASE_LENGTH_TESSELLATION)
				tf = EdgeLengthBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams );
				#elif defined(ASE_LENGTH_CULL_TESSELLATION)
				tf = EdgeLengthBasedTessCull(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, tessMaxDisp, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams, unity_CameraWorldClipPlanes );
				#endif
				o.edge[0] = tf.x; o.edge[1] = tf.y; o.edge[2] = tf.z; o.inside = tf.w;
				return o;
			}

			[domain("tri")]
			[partitioning("fractional_odd")]
			[outputtopology("triangle_cw")]
			[patchconstantfunc("TessellationFunction")]
			[outputcontrolpoints(3)]
			VertexControl HullFunction(InputPatch<VertexControl, 3> patch, uint id : SV_OutputControlPointID)
			{
			   return patch[id];
			}

			[domain("tri")]
			VertexOutput DomainFunction(TessellationFactors factors, OutputPatch<VertexControl, 3> patch, float3 bary : SV_DomainLocation)
			{
				VertexInput o = (VertexInput) 0;
				o.vertex = patch[0].vertex * bary.x + patch[1].vertex * bary.y + patch[2].vertex * bary.z;
				o.ase_normal = patch[0].ase_normal * bary.x + patch[1].ase_normal * bary.y + patch[2].ase_normal * bary.z;
				
				#if defined(ASE_PHONG_TESSELLATION)
				float3 pp[3];
				for (int i = 0; i < 3; ++i)
					pp[i] = o.vertex.xyz - patch[i].ase_normal * (dot(o.vertex.xyz, patch[i].ase_normal) - dot(patch[i].vertex.xyz, patch[i].ase_normal));
				float phongStrength = _TessPhongStrength;
				o.vertex.xyz = phongStrength * (pp[0]*bary.x + pp[1]*bary.y + pp[2]*bary.z) + (1.0f-phongStrength) * o.vertex.xyz;
				#endif
				UNITY_TRANSFER_INSTANCE_ID(patch[0], o);
				return VertexFunction(o);
			}
			#else
			VertexOutput vert ( VertexInput v )
			{
				return VertexFunction( v );
			}
			#endif

			half4 frag(VertexOutput IN  ) : SV_TARGET
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( IN );

				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				float3 WorldPosition = IN.worldPos;
				#endif
				float4 ShadowCoords = float4( 0, 0, 0, 0 );

				#if defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
					#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
						ShadowCoords = IN.shadowCoord;
					#elif defined(MAIN_LIGHT_CALCULATE_SHADOWS)
						ShadowCoords = TransformWorldToShadowCoord( WorldPosition );
					#endif
				#endif

				float4 color426 = IsGammaSpace() ? float4(0,0,0,1) : float4(0,0,0,1);
				
				
				float3 Albedo = color426.rgb;
				float Alpha = _Opacity;
				float AlphaClipThreshold = 0.5;

				half4 color = half4( Albedo, Alpha );

				#ifdef _ALPHATEST_ON
					clip(Alpha - AlphaClipThreshold);
				#endif

				return color;
			}
			ENDHLSL
		}
		
	}
	/*ase_lod*/
	CustomEditor "TatoonEditorURPTransparent"
	Fallback "Hidden/InternalErrorShader"
	
}
/*ASEBEGIN
Version=18707
401;199;1205;571;7596.418;-726.8676;4.07813;True;False
Node;AmplifyShaderEditor.CommentaryNode;513;-2869.809,4399.596;Inherit;False;942.4075;253.7891;Comment;5;509;507;510;508;511;AdditionalLights;1,0.9753064,0,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;188;-1703.686,3571.258;Inherit;False;1174.282;632.0411;Comment;7;488;483;180;426;134;133;512;Master;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;113;-6002.787,3075.623;Inherit;False;2167.901;1060.778;normal;19;132;131;130;129;128;127;126;125;124;123;122;121;120;119;118;117;116;115;114;NormalMap;1,0.3058823,0.9274651,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;52;-2958.417,2899.626;Inherit;False;1224.731;538.1493;Comment;9;328;400;181;7;383;10;11;8;479;Attenuation and Ambient;0.9974991,1,0,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;50;-6021.98,1711.117;Inherit;False;2649.39;1146.548;;36;185;202;203;199;200;195;190;34;28;33;201;189;198;197;196;194;193;192;191;24;32;47;31;30;29;177;27;25;38;36;37;204;205;206;514;518;Rim;1,0.6396222,0,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;136;-5993.825,-533.1808;Inherit;False;3301.643;968.2485;Specular;41;174;173;172;171;170;169;168;167;166;165;164;163;162;161;160;159;158;157;156;155;154;153;152;151;150;149;148;147;146;145;144;143;142;141;140;139;138;137;515;520;521;Specular;0,1,0.07320952,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;48;-2875.871,3799.353;Inherit;False;814.8123;367.9126;Comment;4;175;3;1;2;N . L;1,0.9979696,0,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;187;-2888.299,1512.832;Inherit;False;2661.902;947.3845;Comment;23;186;176;221;86;42;83;82;112;81;58;184;57;55;56;331;135;405;406;408;476;495;505;496;Mix;0,0.3468349,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;88;-5994.515,-1665.921;Inherit;False;2909.998;819.7475;Albedo And Gradient;26;111;110;109;108;106;105;104;103;102;101;100;99;98;97;96;95;94;93;92;91;90;89;397;398;409;506;MainColor & Gradient;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;59;-6025.019,685.831;Inherit;False;2598.732;800.7354;Shadow;18;80;60;61;62;64;63;66;67;65;69;68;71;70;72;76;73;74;77;Shadow tex&color;0,0,0,1;0;0
Node;AmplifyShaderEditor.ScaleNode;141;-5675.535,-203.1597;Inherit;False;1;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;100;-5161.893,-1440.206;Inherit;False;Property;_ColorB;Color B;20;0;Create;True;0;0;False;0;False;0,0.1264467,1,0;0,0.1264467,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ToggleSwitchNode;150;-5243.298,-341.4636;Inherit;False;Property;_SpecularTextureViewProjection;Specular Texture View Projection;30;0;Create;True;0;0;False;0;False;0;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;86;-1290.954,1593.535;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;181;-1969.304,2953.191;Inherit;True;Ambient;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;96;-5279.625,-1033.833;Inherit;False;Property;_GradientPosition;Gradient Position;25;0;Create;False;0;0;False;0;False;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;91;-5946.003,-1245.367;Inherit;False;Constant;_Vector0;Vector 0;45;0;Create;True;0;0;False;0;False;0,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.WorldSpaceLightDirHlpNode;117;-5351.599,3342.346;Inherit;False;False;1;0;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.BreakToComponentsNode;98;-5286.536,-1188.546;Inherit;False;FLOAT2;1;0;FLOAT2;0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.RangedFloatNode;89;-5935.516,-1108.081;Inherit;False;Property;_GradientRotation;Gradient Rotation;26;0;Create;True;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;406;-1011.123,2166.771;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.TexturePropertyNode;104;-4355.648,-1073.342;Inherit;True;Property;_TextureDiffuse;Texture Diffuse;5;1;[NoScaleOffset];Create;True;0;0;False;0;False;None;None;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.RangedFloatNode;94;-5438.539,-950.8042;Inherit;False;Property;_GradientSize;Gradient Size;23;0;Create;True;0;0;False;0;False;0;1;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;101;-4895.763,-1203.439;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;99;-5165.283,-1615.921;Inherit;False;Property;_ColorA;Color A;21;0;Create;True;0;0;False;0;False;1,0,0,0;1,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;55;-2838.191,2063.605;Inherit;False;Property;_ShadowBlend;ShadowBlend;11;0;Create;True;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;130;-4544.474,3701.997;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;176;-1242.496,2342.441;Inherit;False;174;Specular;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.ToggleSwitchNode;203;-3776.793,1984.755;Inherit;False;Property;_UseRim;UseRim;12;0;Create;True;0;0;False;0;False;0;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ToggleSwitchNode;206;-4382.064,2118.015;Inherit;False;Property;_RimLightColor;Rim Light Color;38;0;Create;True;0;0;False;0;False;0;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;505;-465.2538,1731.578;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LightColorNode;8;-2846.418,2947.626;Inherit;False;0;3;COLOR;0;FLOAT3;1;FLOAT;2
Node;AmplifyShaderEditor.LightColorNode;153;-4448.339,-210.0672;Inherit;False;0;3;COLOR;0;FLOAT3;1;FLOAT;2
Node;AmplifyShaderEditor.OneMinusNode;161;-4575.961,244.1065;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;108;-4118.673,-1077.349;Inherit;True;Property;_TextureSample0;Texture Sample 0;0;0;Create;True;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.IndirectDiffuseLighting;11;-2904.157,3085.564;Inherit;False;Tangent;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;110;-3278.053,-1314.791;Inherit;True;Color;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;195;-5199.225,2651.835;Inherit;False;Property;_RimTextureRotation;Rim Texture Rotation;24;0;Create;True;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;31;-4633.656,1878.305;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;34;-4660.316,2088.067;Float;False;Property;_RimColor;Rim Color;0;0;Create;True;0;0;False;0;False;0,1,0.8758622,0;0,0,0,0;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;409;-4289.214,-1475.091;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;405;-1498.831,2205.853;Inherit;False;328;Atten;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;93;-5727.584,-1403.629;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;201;-5850.228,2565.114;Inherit;False;World;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleAddOpNode;521;-3935.37,-157.2548;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT3;0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SmoothstepOpNode;169;-4203.938,-18.49751;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;199;-5214.494,2227.081;Inherit;True;Property;_RimTexture;Rim Texture;18;1;[NoScaleOffset];Create;True;0;0;False;0;False;None;None;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.SaturateNode;32;-4313.722,1880.201;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;133;-1269.143,3844.674;Inherit;False;132;NormalMap;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.RotatorNode;156;-4844.298,-439.9517;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.NormalizeNode;123;-5048.504,3365.851;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;111;-3717.161,-982.9896;Inherit;False;Alpha;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;145;-5530.8,-203.1597;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.PosVertexDataNode;90;-5971.633,-1429.85;Inherit;True;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;120;-5718.684,3588.847;Inherit;True;Property;_Normal;Normal;3;2;[NoScaleOffset];[Normal];Create;True;0;0;False;0;False;-1;None;None;True;0;False;bump;Auto;True;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;97;-5029.835,-1026.04;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;400;-2440.522,3222.883;Inherit;True;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;171;-3658.063,-483.1807;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;508;-2380.068,4455.718;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;160;-4655.367,-291.0895;Inherit;False;Property;_SpecularColor;Specular Color;33;0;Create;True;0;0;False;0;False;1,0.9575656,0.75,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;184;-2689.111,1761.696;Inherit;False;175;NdotL;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;397;-4078.292,-1380.384;Inherit;False;181;Ambient;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;398;-3780.282,-1319.45;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TexturePropertyNode;114;-5952.902,3139.161;Inherit;True;Property;_Texture0;Texture 0;28;3;[HideInInspector];[NoScaleOffset];[Normal];Create;True;0;0;False;0;False;None;None;False;bump;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.DotProductOpNode;128;-4859.701,3216.643;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;509;-2595.151,4537.385;Inherit;False;506;MainTexture;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;115;-5952.787,3634.304;Inherit;False;Property;_NormalStrength;Normal Strength;6;0;Create;False;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;33;-4180.497,2210.604;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;202;-3913.954,2013.239;Inherit;False;3;3;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RadiansOpNode;92;-5718.39,-1104.291;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;488;-1169.457,4067.018;Inherit;False;Property;_Opacity;Opacity;41;0;Create;True;0;0;False;0;False;0.5;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.DotProductOpNode;127;-5088.738,3830.414;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;125;-4754.75,3550.235;Inherit;False;135;Result;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;193;-5499.564,2420.406;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;25;-5419.089,1953.887;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RotatorNode;95;-5536.314,-1190.581;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.LerpOp;102;-4620.67,-1256.017;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.FunctionNode;520;-4026.293,-30.12449;Inherit;False;SRP Additional Light;-1;;4;6c86746ad131a0a408ca599df5f40861;3,6,1,9,1,23,0;5;2;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;15;FLOAT3;0,0,0;False;14;FLOAT3;1,1,1;False;18;FLOAT;0.5;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ObjectScaleNode;116;-5612.645,3799.176;Inherit;False;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SamplerNode;72;-4592.025,1097.185;Inherit;True;Property;_tex1;tex1;13;0;Create;True;0;0;False;0;False;-1;None;None;True;0;False;gray;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;408;-1009.023,2325.471;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;172;-3398.92,-486.9495;Inherit;False;3;3;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ToggleSwitchNode;77;-3895.893,980.2164;Inherit;False;Property;_UseShadow;UseShadow;16;0;Create;True;0;0;False;0;False;0;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;331;-2695.038,1669.624;Inherit;False;328;Atten;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;200;-4568.787,2504.562;Inherit;True;Property;_RimTex;RimTex;25;0;Create;True;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;383;-2921.76,3313.992;Inherit;False;Property;_AttenuationPower;AttenuationPower;40;0;Create;True;0;0;False;0;False;1;1;0.01;50;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;142;-5319.325,-173.1437;Inherit;False;Property;_SpecularTextureRotation;Specular Texture Rotation;32;0;Create;True;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;73;-4571.643,860.2289;Inherit;False;Property;_ShadowColor;Shadow Color;8;0;Create;True;0;0;False;0;False;0,0,0,1;0.5377358,0.5377358,0.5377358,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RotatorNode;70;-4927.57,1074.292;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ToggleSwitchNode;173;-3160.313,-484.7285;Inherit;False;Property;_UseSpecular;UseSpecular;27;0;Create;True;0;0;False;0;False;0;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.DotProductOpNode;165;-4394.628,-22.18851;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;164;-4369.149,243.6614;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;80;-3666.843,981.9041;Inherit;True;ShadowColor;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.WorldSpaceLightDirHlpNode;121;-5432.029,3895.625;Inherit;False;False;1;0;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.AbsOpNode;479;-2601.883,3212.864;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;132;-4074.991,3609.848;Inherit;True;NormalMap;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;163;-4138.917,-206.7735;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;185;-3567.888,1980.287;Inherit;True;Rim;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;109;-3535.225,-1306.352;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ToggleSwitchNode;167;-3958.901,-323.5756;Inherit;False;Property;_SpecLightColor;Spec Light Color;34;0;Create;True;0;0;False;0;False;0;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;126;-4746.348,3828.683;Inherit;False;135;Result;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;103;-4589.453,-1449.57;Inherit;False;Property;_DiffuseColor;Diffuse Color;1;0;Create;True;0;0;False;0;False;1,1,1,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;42;-1538.765,1933.303;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;83;-1571.558,1598.742;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ScaleNode;191;-5542.082,2566.379;Inherit;False;1;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;158;-4572.35,-479.1776;Inherit;True;Property;_SpecularMap;Specular Map;29;1;[NoScaleOffset];Create;True;0;0;False;1;;False;-1;None;None;True;0;False;gray;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;186;-1242.855,2168.263;Inherit;False;185;Rim;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.ScaleNode;63;-5654.857,1162.355;Inherit;False;1;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;119;-5707.034,3146.879;Inherit;True;Property;_TextureSample1;Texture Sample 1;42;0;Create;True;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;True;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;60;-5984.513,1050.303;Inherit;False;Property;_ShadowTextureTiling;Shadow Texture Tiling;14;0;Create;True;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.WorldNormalVector;152;-4792.089,78.90245;Inherit;False;False;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.GetLocalVarNode;177;-5120.258,1863.581;Inherit;False;175;NdotL;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;57;-2477.043,2042.46;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;82;-1993.922,1824.869;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;166;-4236.134,-473.2517;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;135;-441.6295,1908.236;Inherit;True;Result;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ToggleSwitchNode;197;-5193.411,2466.044;Inherit;False;Property;_RimTextureViewProjection;Rim Texture View Projection;19;0;Create;True;0;0;False;0;False;0;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;66;-5439.563,1184.837;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RotatorNode;198;-4846.357,2493.084;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.WorldSpaceLightDirHlpNode;144;-5228.096,-91.16151;Inherit;False;False;1;0;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;24;-5796.728,2196.613;Float;False;Property;_RimSize;Rim Size;7;0;Create;True;0;0;False;0;False;0.5679239;0;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.RadiansOpNode;149;-5020.996,-200.8616;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;134;-1005.474,3793.798;Inherit;False;3;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;159;-4779.972,332.6176;Inherit;False;Property;_SpecularBlend;Specular Blend;37;0;Create;True;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;30;-4891.09,1953.887;Inherit;True;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;61;-5958.734,1314.104;Inherit;False;World;True;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;146;-5206.821,58.47347;Inherit;False;World;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;56;-2838.299,1974.783;Inherit;False;Property;_ShadowSize;Shadow Size;9;0;Create;True;0;0;False;0;False;1;0;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.LightColorNode;105;-4572.947,-1590.8;Inherit;False;0;3;COLOR;0;FLOAT3;1;FLOAT;2
Node;AmplifyShaderEditor.TextureCoordinatesNode;65;-5693.319,1002.072;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;174;-2944.521,-475.9826;Inherit;True;Specular;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ViewProjectionMatrixNode;189;-5871.241,2731.58;Inherit;False;0;1;FLOAT4x4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;76;-4248.699,1014.913;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;64;-5667.265,1260.111;Inherit;False;2;2;0;FLOAT4x4;0,0,0,0,0,1,0,0,0,0,1,0,0,0,0,1;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleAddOpNode;221;-672.6459,1917.772;Inherit;True;4;4;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LightAttenuation;7;-2841.17,3209.584;Inherit;False;0;1;FLOAT;0
Node;AmplifyShaderEditor.BlendNormalsNode;124;-5321.125,3618.05;Inherit;False;0;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ViewProjectionMatrixNode;139;-5946.168,42.14047;Inherit;False;0;1;FLOAT4x4;0
Node;AmplifyShaderEditor.SimpleAddOpNode;155;-4737.867,-91.08353;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleAddOpNode;476;-2478.078,1769.983;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RadiansOpNode;68;-5078.873,1292.965;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;138;-5949.691,-338.2537;Inherit;False;Property;_SpecularTextureTiling;Specular Texture Tiling;31;0;Create;True;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.BlendNormalsNode;122;-5254.833,3160.667;Inherit;False;0;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;190;-5848.249,2451.179;Inherit;False;Property;_RimTextureTiling;Rim Texture Tiling;22;0;Create;True;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;168;-3852.004,-481.8966;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SmoothstepOpNode;58;-2296.376,1956.751;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;27;-5259.089,1953.887;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;140;-5687.842,-101.6336;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT4x4;0,0,0,0,0,1,0,0,0,0,1,0,0,0,0,1;False;1;FLOAT3;0
Node;AmplifyShaderEditor.NormalizeNode;148;-4972.478,-91.17062;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ToggleSwitchNode;69;-5302.464,1066.589;Inherit;False;Property;_ShadowTextureViewProjection;Shadow Texture View Projection;13;0;Create;True;0;0;False;0;False;0;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ToggleSwitchNode;131;-4320.872,3612.839;Inherit;False;Property;_UseNormalMap;UseNormalMap;4;0;Create;True;0;0;False;0;False;0;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;28;-5195.089,2081.886;Float;False;Property;_RimBlend;Rim Blend;2;0;Create;True;0;0;False;0;False;0;0;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;143;-5539.697,-335.7626;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ObjectScaleNode;118;-5643.634,3351.618;Inherit;False;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.GetLocalVarNode;514;-4145.967,2465.066;Inherit;False;511;AdditionalLights;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;151;-4426.695,-100.2965;Inherit;False;Property;_SpecularLightIntensity;Specular Light Intensity;35;0;Create;True;0;0;False;0;False;1;1;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.ToggleSwitchNode;106;-4105.061,-1289.033;Inherit;False;Property;_UseGradient;Use Gradient;17;0;Create;True;0;0;False;0;False;0;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;154;-4872.282,235.2983;Inherit;False;Property;_SpecularSize;Specular Size;36;0;Create;True;0;0;False;0;False;0.005;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;192;-5554.213,2714.874;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT4x4;0,0,0,0,0,1,0,0,0,0,1,0,0,0,0,1;False;1;FLOAT3;0
Node;AmplifyShaderEditor.DotProductOpNode;3;-2475.869,3911.353;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;112;-1830.895,1876.778;Inherit;False;110;Color;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;129;-4550.873,3535.574;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;81;-2089.53,1597.821;Inherit;True;80;ShadowColor;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;506;-3708.847,-1091.114;Inherit;False;MainTexture;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;170;-3700.688,-354.0005;Inherit;False;175;NdotL;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.ViewProjectionMatrixNode;62;-5977.307,1228.583;Inherit;False;0;1;FLOAT4x4;0
Node;AmplifyShaderEditor.RadiansOpNode;196;-4982.279,2659.684;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;74;-4322.867,717.7863;Inherit;False;Constant;_ShadowColor1;Shadow Color1;5;0;Create;True;0;0;False;1;Header(SHADOWS);False;1,1,1,1;0.5377358,0.5377358,0.5377358,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;10;-2386.672,2946.718;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT3;0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;37;-5868.267,2029.523;Float;False;World;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RegisterLocalVarNode;175;-2279.458,3893.846;Inherit;True;NdotL;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;205;-4933.016,2239.282;Inherit;False;Property;_RimLightIntensity;Rim Light Intensity;39;0;Create;True;0;0;False;0;False;0;0;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.NormalizeNode;162;-4581.556,-89.18152;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.WorldNormalVector;36;-5916.267,1869.523;Inherit;False;False;1;0;FLOAT3;0,0,0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;194;-5394.802,2598.783;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.WorldSpaceLightDirHlpNode;2;-2858.671,4026.854;Inherit;False;True;1;0;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RegisterLocalVarNode;511;-2163.26,4474.443;Inherit;False;AdditionalLights;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;67;-5377.498,1309.334;Inherit;False;Property;_ShadowTextureRotation;Shadow Texture Rotation;15;0;Create;True;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.WorldNormalVector;507;-2819.809,4450.284;Inherit;False;False;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.TexturePropertyNode;71;-4932.15,858.623;Inherit;True;Property;_ShadowTexture;Shadow Texture;10;1;[NoScaleOffset];Create;True;0;0;True;0;False;None;None;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.RegisterLocalVarNode;496;-715.491,1742.712;Inherit;False;Opacity;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.NormalizeNode;157;-4584.156,78.29844;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.LightColorNode;47;-4818.297,2319.174;Inherit;False;0;3;COLOR;0;FLOAT3;1;FLOAT;2
Node;AmplifyShaderEditor.ColorNode;426;-1096.559,3611.739;Inherit;False;Constant;_Color0;Color 0;53;0;Create;True;0;0;False;0;False;0,0,0,1;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.NormalizeNode;147;-4974.004,62.94247;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;512;-1281.778,3945.995;Inherit;False;511;AdditionalLights;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.OneMinusNode;29;-5083.089,1953.887;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WorldNormalVector;1;-2858.871,3843.353;Inherit;False;True;1;0;FLOAT3;0,0,0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RegisterLocalVarNode;495;-704.0663,1592.295;Inherit;False;Shadows;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;328;-2044.275,3213.704;Inherit;True;Atten;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;510;-2628.827,4449.596;Inherit;False;SRP Additional Light;-1;;2;6c86746ad131a0a408ca599df5f40861;3,6,1,9,1,23,0;5;2;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;15;FLOAT3;0,0,0;False;14;FLOAT3;1,1,1;False;18;FLOAT;0.5;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;204;-4622.702,2271.126;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.WorldNormalVector;516;-5006.971,2350.372;Inherit;False;False;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.FunctionNode;517;-4695.471,2429.271;Inherit;False;SRP Additional Light;-1;;3;6c86746ad131a0a408ca599df5f40861;3,6,1,9,1,23,0;5;2;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;15;FLOAT3;0,0,0;False;14;FLOAT3;1,1,1;False;18;FLOAT;0.5;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleAddOpNode;518;-4456.697,2263.619;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT3;0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.WorldNormalVector;519;-4189.01,118.2232;Inherit;False;False;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.DotProductOpNode;38;-5612.268,1949.523;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;180;-1334.977,3745.246;Inherit;False;135;Result;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;137;-5943.825,-129.9065;Inherit;False;World;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.GetLocalVarNode;515;-3725.134,-254.9256;Inherit;False;511;AdditionalLights;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;485;1113.926,1555.918;Float;False;False;-1;2;UnityEditor.ShaderGraph.PBRMasterGUI;0;2;New Amplify Shader;94348b07e5e8bab40bd6c8a1e3df54cd;True;DepthOnly;0;3;DepthOnly;0;False;False;False;False;False;False;False;False;True;0;False;-1;True;0;False;-1;False;False;False;False;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;0;0;False;False;False;False;False;False;False;False;True;0;False;-1;False;True;False;False;False;False;0;False;-1;False;False;False;False;True;1;False;-1;False;False;True;1;LightMode=DepthOnly;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;482;1364.926,1481.918;Float;False;False;-1;2;UnityEditor.ShaderGraph.PBRMasterGUI;0;2;New Amplify Shader;94348b07e5e8bab40bd6c8a1e3df54cd;True;ExtraPrePass;0;0;ExtraPrePass;5;False;False;False;False;False;False;False;False;True;0;False;-1;True;0;False;-1;False;False;False;False;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;0;0;True;1;1;False;-1;0;False;-1;0;1;False;-1;0;False;-1;False;False;False;False;False;False;False;False;True;0;False;-1;True;True;True;True;True;0;False;-1;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;True;1;False;-1;True;3;False;-1;True;True;0;False;-1;0;False;-1;True;0;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;486;1113.926,1555.918;Float;False;False;-1;2;UnityEditor.ShaderGraph.PBRMasterGUI;0;2;New Amplify Shader;94348b07e5e8bab40bd6c8a1e3df54cd;True;Meta;0;4;Meta;0;False;False;False;False;False;False;False;False;True;0;False;-1;True;0;False;-1;False;False;False;False;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;0;0;False;False;False;False;False;False;False;False;False;True;2;False;-1;False;False;False;False;False;False;False;False;True;1;LightMode=Meta;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;484;1113.926,1555.918;Float;False;False;-1;2;UnityEditor.ShaderGraph.PBRMasterGUI;0;2;New Amplify Shader;94348b07e5e8bab40bd6c8a1e3df54cd;True;ShadowCaster;0;2;ShadowCaster;0;False;False;False;False;False;False;False;False;True;0;False;-1;True;0;False;-1;False;False;False;False;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;0;0;False;False;False;False;False;False;False;False;True;0;False;-1;False;False;False;False;False;False;True;1;False;-1;True;3;False;-1;False;True;1;LightMode=ShadowCaster;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;483;-790.9906,3708.976;Float;False;True;-1;2;TatoonEditorURPTransparent;0;2;TetraArts/Tatoon/URP/Tatoon_URP_Transparent;94348b07e5e8bab40bd6c8a1e3df54cd;True;Forward;0;1;Forward;17;False;False;False;False;False;False;False;False;True;0;False;-1;True;0;False;-1;False;False;False;False;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Transparent=RenderType;Queue=Transparent=Queue=0;True;2;0;True;1;5;False;-1;10;False;-1;1;1;False;-1;10;False;-1;False;False;False;False;False;False;False;False;False;True;True;True;True;True;0;False;-1;False;False;False;True;True;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;True;2;False;-1;True;0;False;-1;True;True;0;False;-1;0;False;-1;True;1;LightMode=UniversalForward;False;0;Hidden/InternalErrorShader;0;0;Standard;36;Workflow;1;Surface;1;  Refraction Model;0;  Blend;0;Two Sided;1;Fragment Normal Space,InvertActionOnDeselection;0;Transmission;0;  Transmission Shadow;0.5,False,-1;Translucency;0;  Translucency Strength;1,False,-1;  Normal Distortion;0.5,False,-1;  Scattering;2,False,-1;  Direct;0.9,False,-1;  Ambient;0.1,False,-1;  Shadow;0.5,False,-1;Cast Shadows;0;  Use Shadow Threshold;0;Receive Shadows;1;GPU Instancing;1;LOD CrossFade;0;Built-in Fog;1;_FinalColorxAlpha;1;Meta Pass;1;Override Baked GI;0;Extra Pre Pass;0;DOTS Instancing;0;Tessellation;0;  Phong;0;  Strength;0.5,False,-1;  Type;0;  Tess;16,False,-1;  Min;10,False,-1;  Max;25,False,-1;  Edge Length;16,False,-1;  Max Displacement;25,False,-1;Vertex Position,InvertActionOnDeselection;1;0;6;False;True;False;False;True;True;False;;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;487;1113.926,1555.918;Float;False;False;-1;2;UnityEditor.ShaderGraph.PBRMasterGUI;0;2;New Amplify Shader;94348b07e5e8bab40bd6c8a1e3df54cd;True;Universal2D;0;5;Universal2D;0;False;False;False;False;False;False;False;False;True;0;False;-1;True;0;False;-1;False;False;False;False;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;0;0;True;1;5;False;-1;10;False;-1;1;1;False;-1;10;False;-1;False;False;False;False;False;False;False;False;False;True;True;True;True;True;0;False;-1;False;False;False;False;True;2;False;-1;True;0;False;-1;True;True;0;False;-1;0;False;-1;True;1;LightMode=Universal2D;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
WireConnection;141;0;138;0
WireConnection;150;0;143;0
WireConnection;150;1;145;0
WireConnection;86;0;83;0
WireConnection;86;1;82;0
WireConnection;181;0;10;0
WireConnection;98;0;95;0
WireConnection;406;0;186;0
WireConnection;406;1;405;0
WireConnection;101;0;98;0
WireConnection;101;1;96;0
WireConnection;101;2;97;0
WireConnection;130;0;127;0
WireConnection;130;1;126;0
WireConnection;203;1;202;0
WireConnection;206;0;34;0
WireConnection;206;1;518;0
WireConnection;505;1;496;0
WireConnection;161;0;154;0
WireConnection;108;0;104;0
WireConnection;110;0;109;0
WireConnection;31;0;177;0
WireConnection;31;1;30;0
WireConnection;409;0;105;1
WireConnection;409;1;103;0
WireConnection;93;0;90;1
WireConnection;93;1;90;2
WireConnection;521;0;163;0
WireConnection;521;1;520;0
WireConnection;169;0;165;0
WireConnection;169;1;161;0
WireConnection;169;2;164;0
WireConnection;32;0;31;0
WireConnection;156;0;150;0
WireConnection;156;2;149;0
WireConnection;123;0;117;0
WireConnection;111;0;108;4
WireConnection;145;0;141;0
WireConnection;145;1;140;0
WireConnection;120;5;115;0
WireConnection;97;0;96;0
WireConnection;97;1;94;0
WireConnection;400;0;479;0
WireConnection;400;1;383;0
WireConnection;171;0;168;0
WireConnection;171;1;169;0
WireConnection;508;0;510;0
WireConnection;508;1;509;0
WireConnection;398;0;397;0
WireConnection;398;1;106;0
WireConnection;128;0;122;0
WireConnection;128;1;123;0
WireConnection;33;0;206;0
WireConnection;33;1;200;0
WireConnection;202;0;32;0
WireConnection;202;1;33;0
WireConnection;202;2;514;0
WireConnection;92;0;89;0
WireConnection;127;0;124;0
WireConnection;127;1;121;0
WireConnection;193;0;190;0
WireConnection;25;0;38;0
WireConnection;25;1;24;0
WireConnection;95;0;93;0
WireConnection;95;1;91;0
WireConnection;95;2;92;0
WireConnection;102;0;99;0
WireConnection;102;1;100;0
WireConnection;102;2;101;0
WireConnection;520;11;519;0
WireConnection;72;0;71;0
WireConnection;72;1;70;0
WireConnection;408;0;405;0
WireConnection;408;1;176;0
WireConnection;172;0;171;0
WireConnection;172;1;170;0
WireConnection;172;2;515;0
WireConnection;77;0;74;0
WireConnection;77;1;76;0
WireConnection;200;0;199;0
WireConnection;200;1;198;0
WireConnection;70;0;69;0
WireConnection;70;2;68;0
WireConnection;173;1;172;0
WireConnection;165;0;162;0
WireConnection;165;1;157;0
WireConnection;164;0;161;0
WireConnection;164;1;159;0
WireConnection;80;0;77;0
WireConnection;479;0;7;0
WireConnection;132;0;131;0
WireConnection;163;0;153;0
WireConnection;163;1;151;0
WireConnection;185;0;203;0
WireConnection;109;0;398;0
WireConnection;109;1;108;0
WireConnection;167;0;160;0
WireConnection;167;1;521;0
WireConnection;42;0;112;0
WireConnection;42;1;58;0
WireConnection;83;0;81;0
WireConnection;83;1;112;0
WireConnection;191;0;190;0
WireConnection;158;1;156;0
WireConnection;63;0;60;0
WireConnection;119;0;114;0
WireConnection;57;0;56;0
WireConnection;57;1;55;0
WireConnection;82;0;58;0
WireConnection;166;0;158;0
WireConnection;135;0;221;0
WireConnection;197;0;193;0
WireConnection;197;1;194;0
WireConnection;66;0;63;0
WireConnection;66;1;64;0
WireConnection;198;0;197;0
WireConnection;198;2;196;0
WireConnection;149;0;142;0
WireConnection;134;0;180;0
WireConnection;134;1;133;0
WireConnection;134;2;512;0
WireConnection;30;0;29;0
WireConnection;30;1;28;0
WireConnection;65;0;60;0
WireConnection;174;0;173;0
WireConnection;76;0;73;0
WireConnection;76;1;72;0
WireConnection;64;0;62;0
WireConnection;64;1;61;0
WireConnection;221;0;86;0
WireConnection;221;1;42;0
WireConnection;221;2;406;0
WireConnection;221;3;408;0
WireConnection;124;0;120;0
WireConnection;124;1;116;0
WireConnection;155;0;148;0
WireConnection;155;1;147;0
WireConnection;476;0;331;0
WireConnection;476;1;184;0
WireConnection;68;0;67;0
WireConnection;122;0;119;0
WireConnection;122;1;118;0
WireConnection;168;0;166;0
WireConnection;168;1;167;0
WireConnection;58;0;476;0
WireConnection;58;1;56;0
WireConnection;58;2;57;0
WireConnection;27;0;25;0
WireConnection;140;0;137;0
WireConnection;140;1;139;0
WireConnection;148;0;144;0
WireConnection;69;0;65;0
WireConnection;69;1;66;0
WireConnection;131;0;129;0
WireConnection;131;1;130;0
WireConnection;143;0;138;0
WireConnection;106;0;409;0
WireConnection;106;1;102;0
WireConnection;192;0;201;0
WireConnection;192;1;189;0
WireConnection;3;0;1;0
WireConnection;3;1;2;0
WireConnection;129;0;128;0
WireConnection;129;1;125;0
WireConnection;506;0;108;0
WireConnection;196;0;195;0
WireConnection;10;0;8;0
WireConnection;10;1;11;0
WireConnection;175;0;3;0
WireConnection;162;0;155;0
WireConnection;194;0;191;0
WireConnection;194;1;192;0
WireConnection;511;0;508;0
WireConnection;157;0;152;0
WireConnection;147;0;146;0
WireConnection;29;0;27;0
WireConnection;495;0;86;0
WireConnection;328;0;400;0
WireConnection;510;11;507;0
WireConnection;204;0;205;0
WireConnection;204;1;47;0
WireConnection;517;11;516;0
WireConnection;518;0;204;0
WireConnection;518;1;517;0
WireConnection;38;0;36;0
WireConnection;38;1;37;0
WireConnection;483;0;426;0
WireConnection;483;2;134;0
WireConnection;483;3;426;0
WireConnection;483;4;426;0
WireConnection;483;6;488;0
ASEEND*/
//CHKSM=C24FC99CCDD55FCD3F0AA59CA9D3C0B7EF19960A