// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "TetraArts/Tatoon/URP/Outline_URP"
{
	Properties
	{
		_OutlineSize("OutlineSize", Range( 0 , 0.5)) = 0.07
		[Toggle]_UseOutline("UseOutline", Float) = 0
		[HDR]_OutlineColor2("OutlineColor2", Color) = (2,1.913725,0,0)
		[HDR]_OutlineColor1("OutlineColor1", Color) = (1,0.5094871,0,1)
		_Speed("Speed", Range( -1 , 1)) = -0.1
		_PowerColor1("PowerColor1", Float) = 1
		_PowerColor2("PowerColor2", Float) = 1
		[NoScaleOffset]_Texture("Texture", 2D) = "white" {}
		_NoiseTextureScale("NoiseTextureScale", Float) = 2
		[Toggle]_UseOutlineFire("UseOutlineFire", Float) = 0
		_AlphaClip("AlphaClip", Float) = 0.5
		[HDR]_OutlineColor("Color 0", Color) = (0,0,0,0)

	}

	SubShader
	{
		LOD 0

		
		Tags { "RenderPipeline"="UniversalPipeline" "RenderType"="Opaque" "Queue"="Geometry" }
		
		Cull Back
		HLSLINCLUDE
		#pragma target 3.0
		ENDHLSL

		
		Pass
		{
			Name "Forward"
			Tags { "LightMode"="UniversalForward" }
			
			Blend One Zero , One Zero
			ZWrite On
			ZTest LEqual
			Offset 0 , 0
			ColorMask RGBA
			

			HLSLPROGRAM
			#pragma multi_compile_instancing
			#define _ALPHATEST_ON 1
			#define ASE_SRP_VERSION 999999

			#pragma prefer_hlslcc gles
			#pragma exclude_renderers d3d11_9x

			#pragma vertex vert
			#pragma fragment frag


			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/UnityInstancing.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"

			

			struct VertexInput
			{
				float4 vertex : POSITION;
				float3 ase_normal : NORMAL;
				
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 clipPos : SV_POSITION;
				#ifdef ASE_FOG
				float fogFactor : TEXCOORD0;
				#endif
				float4 ase_texcoord1 : TEXCOORD1;
				float4 ase_texcoord2 : TEXCOORD2;
				float4 ase_texcoord3 : TEXCOORD3;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			sampler2D _Texture;
			CBUFFER_START( UnityPerMaterial )
			float _UseOutline;
			float _OutlineSize;
			float _UseOutlineFire;
			float4 _OutlineColor;
			float4 _OutlineColor1;
			float _PowerColor1;
			float _Speed;
			float _NoiseTextureScale;
			float _PowerColor2;
			float4 _OutlineColor2;
			float _AlphaClip;
			CBUFFER_END


			
			VertexOutput vert ( VertexInput v  )
			{
				VertexOutput o = (VertexOutput)0;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

				float3 ase_worldPos = mul(GetObjectToWorldMatrix(), v.vertex).xyz;
				o.ase_texcoord1.xyz = ase_worldPos;
				float3 ase_worldNormal = TransformObjectToWorldNormal(v.ase_normal);
				o.ase_texcoord2.xyz = ase_worldNormal;
				float4 ase_clipPos = TransformObjectToHClip((v.vertex).xyz);
				float4 screenPos = ComputeScreenPos(ase_clipPos);
				o.ase_texcoord3 = screenPos;
				
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord1.w = 0;
				o.ase_texcoord2.w = 0;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.vertex.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif
				float3 vertexValue = (( _UseOutline )?( ( _OutlineSize * v.ase_normal ) ):( float3( 0,0,0 ) ));
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					v.vertex.xyz = vertexValue;
				#else
					v.vertex.xyz += vertexValue;
				#endif
				v.ase_normal = v.ase_normal;

				o.clipPos = TransformObjectToHClip( v.vertex.xyz );
				#ifdef ASE_FOG
				o.fogFactor = ComputeFogFactor( o.clipPos.z );
				#endif
				return o;
			}

			half4 frag ( VertexOutput IN  ) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( IN );

				float3 ase_worldPos = IN.ase_texcoord1.xyz;
				float3 ase_worldViewDir = ( _WorldSpaceCameraPos.xyz - ase_worldPos );
				ase_worldViewDir = SafeNormalize( ase_worldViewDir );
				float3 ase_worldNormal = IN.ase_texcoord2.xyz;
				float dotResult90 = dot( ase_worldViewDir , ase_worldNormal );
				float4 temp_cast_0 = (pow( abs( dotResult90 ) , _PowerColor1 )).xxxx;
				float4 screenPos = IN.ase_texcoord3;
				float4 ase_screenPosNorm = screenPos / screenPos.w;
				ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
				float2 panner174 = ( 1.0 * _Time.y * ( float2( 0,1 ) * _Speed ) + ase_screenPosNorm.xy);
				float4 Noise124 = tex2D( _Texture, (panner174*_NoiseTextureScale + 0.0) );
				float4 clampResult102 = clamp( ( ( temp_cast_0 - Noise124 ) * 5.0 ) , float4( 0,0,0,0 ) , float4( 1,1,1,1 ) );
				float4 temp_cast_2 = (pow( abs( dotResult90 ) , _PowerColor2 )).xxxx;
				float4 clampResult103 = clamp( ( 10.0 * ( temp_cast_2 - Noise124 ) ) , float4( 0,0,0,0 ) , float4( 1,1,1,1 ) );
				float4 OutlineResult111 = ( ( _OutlineColor1 * clampResult102 ) + ( ( clampResult103 - clampResult102 ) * _OutlineColor2 ) );
				
				float4 NoiseAlpha110 = clampResult103;
				
				float3 BakedAlbedo = 0;
				float3 BakedEmission = 0;
				float3 Color = (( _UseOutlineFire )?( OutlineResult111 ):( _OutlineColor )).rgb;
				float Alpha = (( _UseOutlineFire )?( NoiseAlpha110 ):( float4( 1,1,1,1 ) )).r;
				float AlphaClipThreshold = _AlphaClip;

				#ifdef _ALPHATEST_ON
					clip( Alpha - AlphaClipThreshold );
				#endif

				#ifdef ASE_FOG
					Color = MixFog( Color, IN.fogFactor );
				#endif

				#ifdef LOD_FADE_CROSSFADE
					LODDitheringTransition( IN.clipPos.xyz, unity_LODFade.x );
				#endif

				return half4( Color, Alpha );
			}

			ENDHLSL
		}

		
		Pass
		{
			
			Name "ShadowCaster"
			Tags { "LightMode"="ShadowCaster" }

			ZWrite On
			ZTest LEqual

			HLSLPROGRAM
			#pragma multi_compile_instancing
			#define _ALPHATEST_ON 1
			#define ASE_SRP_VERSION 999999

			#pragma prefer_hlslcc gles
			#pragma exclude_renderers d3d11_9x

			#pragma vertex ShadowPassVertex
			#pragma fragment ShadowPassFragment


			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"

			

			struct VertexInput
			{
				float4 vertex : POSITION;
				float3 ase_normal : NORMAL;
				
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 clipPos : SV_POSITION;
				float4 ase_texcoord7 : TEXCOORD7;
				float4 ase_texcoord8 : TEXCOORD8;
				float4 ase_texcoord9 : TEXCOORD9;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			sampler2D _Texture;
			CBUFFER_START( UnityPerMaterial )
			float _UseOutline;
			float _OutlineSize;
			float _UseOutlineFire;
			float4 _OutlineColor;
			float4 _OutlineColor1;
			float _PowerColor1;
			float _Speed;
			float _NoiseTextureScale;
			float _PowerColor2;
			float4 _OutlineColor2;
			float _AlphaClip;
			CBUFFER_END


			
			float3 _LightDirection;

			VertexOutput ShadowPassVertex( VertexInput v )
			{
				VertexOutput o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );

				float3 ase_worldPos = mul(GetObjectToWorldMatrix(), v.vertex).xyz;
				o.ase_texcoord7.xyz = ase_worldPos;
				float3 ase_worldNormal = TransformObjectToWorldNormal(v.ase_normal);
				o.ase_texcoord8.xyz = ase_worldNormal;
				float4 ase_clipPos = TransformObjectToHClip((v.vertex).xyz);
				float4 screenPos = ComputeScreenPos(ase_clipPos);
				o.ase_texcoord9 = screenPos;
				
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord7.w = 0;
				o.ase_texcoord8.w = 0;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.vertex.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif
				float3 vertexValue = (( _UseOutline )?( ( _OutlineSize * v.ase_normal ) ):( float3( 0,0,0 ) ));
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					v.vertex.xyz = vertexValue;
				#else
					v.vertex.xyz += vertexValue;
				#endif

				v.ase_normal = v.ase_normal;

				float3 positionWS = TransformObjectToWorld( v.vertex.xyz );
				float3 normalWS = TransformObjectToWorldDir( v.ase_normal );

				float4 clipPos = TransformWorldToHClip( ApplyShadowBias( positionWS, normalWS, _LightDirection ) );

				#if UNITY_REVERSED_Z
					clipPos.z = min(clipPos.z, clipPos.w * UNITY_NEAR_CLIP_VALUE);
				#else
					clipPos.z = max(clipPos.z, clipPos.w * UNITY_NEAR_CLIP_VALUE);
				#endif
				o.clipPos = clipPos;

				return o;
			}

			half4 ShadowPassFragment(VertexOutput IN  ) : SV_TARGET
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( IN );

				float3 ase_worldPos = IN.ase_texcoord7.xyz;
				float3 ase_worldViewDir = ( _WorldSpaceCameraPos.xyz - ase_worldPos );
				ase_worldViewDir = SafeNormalize( ase_worldViewDir );
				float3 ase_worldNormal = IN.ase_texcoord8.xyz;
				float dotResult90 = dot( ase_worldViewDir , ase_worldNormal );
				float4 temp_cast_0 = (pow( abs( dotResult90 ) , _PowerColor2 )).xxxx;
				float4 screenPos = IN.ase_texcoord9;
				float4 ase_screenPosNorm = screenPos / screenPos.w;
				ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
				float2 panner174 = ( 1.0 * _Time.y * ( float2( 0,1 ) * _Speed ) + ase_screenPosNorm.xy);
				float4 Noise124 = tex2D( _Texture, (panner174*_NoiseTextureScale + 0.0) );
				float4 clampResult103 = clamp( ( 10.0 * ( temp_cast_0 - Noise124 ) ) , float4( 0,0,0,0 ) , float4( 1,1,1,1 ) );
				float4 NoiseAlpha110 = clampResult103;
				
				float Alpha = (( _UseOutlineFire )?( NoiseAlpha110 ):( float4( 1,1,1,1 ) )).r;
				float AlphaClipThreshold = _AlphaClip;

				#ifdef _ALPHATEST_ON
					clip(Alpha - AlphaClipThreshold);
				#endif

				#ifdef LOD_FADE_CROSSFADE
					LODDitheringTransition( IN.clipPos.xyz, unity_LODFade.x );
				#endif
				return 0;
			}

			ENDHLSL
		}

		
		Pass
		{
			
			Name "DepthOnly"
			Tags { "LightMode"="DepthOnly" }

			ZWrite On
			ColorMask 0

			HLSLPROGRAM
			#pragma multi_compile_instancing
			#define _ALPHATEST_ON 1
			#define ASE_SRP_VERSION 999999

			#pragma prefer_hlslcc gles
			#pragma exclude_renderers d3d11_9x

			#pragma vertex vert
			#pragma fragment frag


			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"

			

			struct VertexInput
			{
				float4 vertex : POSITION;
				float3 ase_normal : NORMAL;
				
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 clipPos : SV_POSITION;
				float4 ase_texcoord : TEXCOORD0;
				float4 ase_texcoord1 : TEXCOORD1;
				float4 ase_texcoord2 : TEXCOORD2;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			sampler2D _Texture;
			CBUFFER_START( UnityPerMaterial )
			float _UseOutline;
			float _OutlineSize;
			float _UseOutlineFire;
			float4 _OutlineColor;
			float4 _OutlineColor1;
			float _PowerColor1;
			float _Speed;
			float _NoiseTextureScale;
			float _PowerColor2;
			float4 _OutlineColor2;
			float _AlphaClip;
			CBUFFER_END


			
			VertexOutput vert( VertexInput v  )
			{
				VertexOutput o = (VertexOutput)0;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

				float3 ase_worldPos = mul(GetObjectToWorldMatrix(), v.vertex).xyz;
				o.ase_texcoord.xyz = ase_worldPos;
				float3 ase_worldNormal = TransformObjectToWorldNormal(v.ase_normal);
				o.ase_texcoord1.xyz = ase_worldNormal;
				float4 ase_clipPos = TransformObjectToHClip((v.vertex).xyz);
				float4 screenPos = ComputeScreenPos(ase_clipPos);
				o.ase_texcoord2 = screenPos;
				
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord.w = 0;
				o.ase_texcoord1.w = 0;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.vertex.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif
				float3 vertexValue = (( _UseOutline )?( ( _OutlineSize * v.ase_normal ) ):( float3( 0,0,0 ) ));
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					v.vertex.xyz = vertexValue;
				#else
					v.vertex.xyz += vertexValue;
				#endif

				v.ase_normal = v.ase_normal;

				o.clipPos = TransformObjectToHClip(v.vertex.xyz);
				return o;
			}

			half4 frag(VertexOutput IN  ) : SV_TARGET
			{
				UNITY_SETUP_INSTANCE_ID(IN);
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( IN );

				float3 ase_worldPos = IN.ase_texcoord.xyz;
				float3 ase_worldViewDir = ( _WorldSpaceCameraPos.xyz - ase_worldPos );
				ase_worldViewDir = SafeNormalize( ase_worldViewDir );
				float3 ase_worldNormal = IN.ase_texcoord1.xyz;
				float dotResult90 = dot( ase_worldViewDir , ase_worldNormal );
				float4 temp_cast_0 = (pow( abs( dotResult90 ) , _PowerColor2 )).xxxx;
				float4 screenPos = IN.ase_texcoord2;
				float4 ase_screenPosNorm = screenPos / screenPos.w;
				ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
				float2 panner174 = ( 1.0 * _Time.y * ( float2( 0,1 ) * _Speed ) + ase_screenPosNorm.xy);
				float4 Noise124 = tex2D( _Texture, (panner174*_NoiseTextureScale + 0.0) );
				float4 clampResult103 = clamp( ( 10.0 * ( temp_cast_0 - Noise124 ) ) , float4( 0,0,0,0 ) , float4( 1,1,1,1 ) );
				float4 NoiseAlpha110 = clampResult103;
				
				float Alpha = (( _UseOutlineFire )?( NoiseAlpha110 ):( float4( 1,1,1,1 ) )).r;
				float AlphaClipThreshold = _AlphaClip;

				#ifdef _ALPHATEST_ON
					clip(Alpha - AlphaClipThreshold);
				#endif

				#ifdef LOD_FADE_CROSSFADE
					LODDitheringTransition( IN.clipPos.xyz, unity_LODFade.x );
				#endif
				return 0;
			}
			ENDHLSL
		}

	
	}
	CustomEditor "TatoonOutlineEditorURP"
	Fallback "Hidden/InternalErrorShader"
	
}
/*ASEBEGIN
Version=17700
0;73;1190;509;5116.243;384.9896;3.606953;True;False
Node;AmplifyShaderEditor.CommentaryNode;112;-4112.959,1032.21;Inherit;False;1805.441;600.5363;OutlineColor;10;124;123;121;122;120;174;186;118;113;185;OutlineTextureNoise;0,0.240608,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;113;-4047.42,1506.774;Inherit;False;Property;_Speed;Speed;4;0;Create;True;0;0;False;0;-0.1;0;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;185;-3937.936,1376.898;Inherit;False;Constant;_Vector0;Vector 0;13;0;Create;True;0;0;False;0;0,1;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;186;-3768.584,1383.57;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ScreenPosInputsNode;118;-3821.054,1191.791;Float;False;0;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;120;-3386.326,1497.954;Inherit;False;Property;_NoiseTextureScale;NoiseTextureScale;8;0;Create;True;0;0;False;0;2;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;174;-3416.235,1258.026;Inherit;True;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.CommentaryNode;87;-3652.847,-238.4931;Inherit;False;3150.787;993.6045;Comment;27;109;111;105;104;110;106;102;103;101;99;100;98;96;95;93;91;90;89;88;130;152;153;187;188;189;190;191;FireOutline;1,0,0.1493015,1;0;0
Node;AmplifyShaderEditor.TexturePropertyNode;121;-3141.559,1086.139;Inherit;True;Property;_Texture;Texture;7;1;[NoScaleOffset];Create;False;0;0;False;0;e28dc97a9541e3642a48c0e3886688c5;e28dc97a9541e3642a48c0e3886688c5;False;white;Auto;Texture2D;-1;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.ScaleAndOffsetNode;122;-3131.507,1347.957;Inherit;True;3;0;FLOAT2;0,0;False;1;FLOAT;1;False;2;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;89;-3583.165,136.6497;Inherit;False;World;True;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.WorldNormalVector;88;-3602.847,305.2449;Inherit;False;False;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SamplerNode;123;-2853.827,1235.019;Inherit;True;Property;_NoiseTexture;NoiseTexture;43;0;Create;True;0;0;False;0;-1;e28dc97a9541e3642a48c0e3886688c5;e28dc97a9541e3642a48c0e3886688c5;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DotProductOpNode;90;-3346.568,253.8638;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;124;-2554.651,1241.738;Inherit;True;Noise;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.AbsOpNode;190;-3114.487,276.13;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;95;-3272.925,468.3964;Inherit;False;Property;_PowerColor2;PowerColor2;6;0;Create;True;0;0;False;0;1;0.1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;153;-2850.665,630.696;Inherit;False;124;Noise;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.PowerNode;187;-2903.947,387.6862;Inherit;True;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;191;-2489.245,341.5677;Inherit;False;Constant;_Float1;Float 0;47;0;Create;True;0;0;False;0;10;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;152;-2589.802,415.399;Inherit;True;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;100;-2229.305,397.3023;Inherit;True;2;2;0;FLOAT;0;False;1;COLOR;5,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;165;-1824.949,1002.957;Inherit;False;1567.494;987.2643;Comment;11;143;132;163;135;162;133;138;86;127;85;75;Master;1,1,1,1;0;0
Node;AmplifyShaderEditor.ClampOpNode;103;-1964.221,396.9201;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;1,1,1,1;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;75;-1370.98,1499.181;Inherit;False;Property;_OutlineSize;OutlineSize;0;0;Create;True;0;0;False;0;0.07;1;0;0.5;0;1;FLOAT;0
Node;AmplifyShaderEditor.NormalVertexDataNode;85;-1279.21,1590.989;Inherit;False;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;110;-1322.981,75.47678;Inherit;True;NoiseAlpha;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;127;-1107.195,1343.45;Inherit;False;110;NoiseAlpha;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;86;-1041.041,1537.02;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ToggleSwitchNode;162;-857.4556,1320.737;Inherit;False;Property;_UseOutlineFire;UseOutlineFire;13;0;Create;False;0;0;False;0;0;2;0;COLOR;1,1,1,1;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;98;-2492.214,277.3818;Inherit;False;Constant;_Float0;Float 0;47;0;Create;True;0;0;False;0;5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;106;-1478.896,316.5459;Inherit;True;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.PowerNode;188;-2901.401,33.34535;Inherit;True;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ToggleSwitchNode;135;-858.0349,1212.668;Inherit;False;Property;_UseOutlineFire;UseOutlineFire;9;0;Create;True;0;0;False;0;0;2;0;COLOR;1,1,1,1;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;105;-1461.9,-144.08;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.AbsOpNode;189;-3121.195,165.0084;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;93;-2847.154,280.2213;Inherit;False;124;Noise;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;163;-1143.455,1044.737;Inherit;False;Property;_OutlineColor;Color 0;11;1;[HDR];Create;False;0;0;False;0;0,0,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleSubtractOpNode;96;-2556.779,68.26131;Inherit;True;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;138;-797.9579,1418.516;Inherit;False;Property;_AlphaClip;AlphaClip;10;0;Create;True;0;0;False;0;0.5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;104;-1474.103,549.0734;Inherit;False;Property;_OutlineColor2;OutlineColor2;2;1;[HDR];Create;False;0;0;False;0;2,1.913725,0,0;0,0.9740796,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ToggleSwitchNode;133;-848.799,1504.841;Inherit;False;Property;_UseOutline;UseOutline;1;0;Create;True;0;0;False;0;0;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;111;-689.2649,229.4673;Inherit;True;OutlineResult;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;130;-1217.713,339.266;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0.3382353,0.3382353,0.3382353,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;101;-1958.416,-154.213;Inherit;False;Property;_OutlineColor1;OutlineColor1;3;1;[HDR];Create;True;0;0;False;0;1,0.5094871,0,1;0,0.9740796,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ClampOpNode;102;-1977.892,66.11574;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;1,1,1,1;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;109;-931.0155,234.8593;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;132;-1117.484,1252.173;Inherit;False;111;OutlineResult;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;99;-2249.332,74.19441;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;5;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;91;-3278.34,24.511;Inherit;False;Property;_PowerColor1;PowerColor1;5;0;Create;True;0;0;False;0;1;1.158531;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;144;557.4736,875.3408;Float;False;False;-1;2;UnityEditor.ShaderGraph.PBRMasterGUI;0;3;New Amplify Shader;2992e84f91cbeb14eab234972e07ea9d;True;ShadowCaster;0;1;ShadowCaster;0;False;False;False;True;0;False;-1;False;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;2;0;False;False;False;False;False;False;True;1;False;-1;True;3;False;-1;False;True;1;LightMode=ShadowCaster;False;0;Hidden/InternalErrorShader;0;0;Standard;0;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;146;557.4736,875.3408;Float;False;False;-1;2;UnityEditor.ShaderGraph.PBRMasterGUI;0;3;New Amplify Shader;2992e84f91cbeb14eab234972e07ea9d;True;Meta;0;3;Meta;0;False;False;False;True;0;False;-1;False;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;2;0;False;False;False;True;2;False;-1;False;False;False;False;False;True;1;LightMode=Meta;False;0;Hidden/InternalErrorShader;0;0;Standard;0;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;143;-513.3614,1304.291;Float;False;True;-1;2;TatoonOutlineEditorURP;0;3;TetraArts/Tatoon/URP/Outline_URP;2992e84f91cbeb14eab234972e07ea9d;True;Forward;0;0;Forward;7;False;False;False;True;0;False;-1;False;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;2;0;True;1;1;False;-1;0;False;-1;1;1;False;-1;0;False;-1;False;False;False;True;True;True;True;True;0;False;-1;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;True;1;False;-1;True;3;False;-1;True;True;0;False;-1;0;False;-1;True;1;LightMode=UniversalForward;False;0;Hidden/InternalErrorShader;0;0;Standard;10;Surface;0;  Blend;0;Two Sided;1;Cast Shadows;1;Receive Shadows;1;GPU Instancing;1;LOD CrossFade;0;Built-in Fog;0;Meta Pass;0;Vertex Position,InvertActionOnDeselection;1;0;4;True;True;True;False;False;;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;145;557.4736,875.3408;Float;False;False;-1;2;UnityEditor.ShaderGraph.PBRMasterGUI;0;3;New Amplify Shader;2992e84f91cbeb14eab234972e07ea9d;True;DepthOnly;0;2;DepthOnly;0;False;False;False;True;0;False;-1;False;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;2;0;False;False;False;False;True;False;False;False;False;0;False;-1;False;True;1;False;-1;False;False;True;1;LightMode=DepthOnly;False;0;Hidden/InternalErrorShader;0;0;Standard;0;0
WireConnection;186;0;185;0
WireConnection;186;1;113;0
WireConnection;174;0;118;0
WireConnection;174;2;186;0
WireConnection;122;0;174;0
WireConnection;122;1;120;0
WireConnection;123;0;121;0
WireConnection;123;1;122;0
WireConnection;90;0;89;0
WireConnection;90;1;88;0
WireConnection;124;0;123;0
WireConnection;190;0;90;0
WireConnection;187;0;190;0
WireConnection;187;1;95;0
WireConnection;152;0;187;0
WireConnection;152;1;153;0
WireConnection;100;0;191;0
WireConnection;100;1;152;0
WireConnection;103;0;100;0
WireConnection;110;0;103;0
WireConnection;86;0;75;0
WireConnection;86;1;85;0
WireConnection;162;1;127;0
WireConnection;106;0;103;0
WireConnection;106;1;102;0
WireConnection;188;0;189;0
WireConnection;188;1;91;0
WireConnection;135;0;163;0
WireConnection;135;1;132;0
WireConnection;105;0;101;0
WireConnection;105;1;102;0
WireConnection;189;0;90;0
WireConnection;96;0;188;0
WireConnection;96;1;93;0
WireConnection;133;1;86;0
WireConnection;111;0;109;0
WireConnection;130;0;106;0
WireConnection;130;1;104;0
WireConnection;102;0;99;0
WireConnection;109;0;105;0
WireConnection;109;1;130;0
WireConnection;99;0;96;0
WireConnection;99;1;98;0
WireConnection;143;2;135;0
WireConnection;143;3;162;0
WireConnection;143;4;138;0
WireConnection;143;5;133;0
ASEEND*/
//CHKSM=E1E616378DDD68A138EB4E3E35EFBDCEDAA91AFC