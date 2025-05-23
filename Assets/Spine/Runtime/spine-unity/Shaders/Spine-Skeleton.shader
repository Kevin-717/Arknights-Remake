// Shader "Spine/Skeleton" {
// 	Properties {
// 		_Cutoff ("Shadow alpha cutoff", Range(0,1)) = 0.1
// 		[NoScaleOffset] _MainTex ("Main Texture", 2D) = "black" {}
// 		[Toggle(_STRAIGHT_ALPHA_INPUT)] _StraightAlphaInput("Straight Alpha Texture", Int) = 0
// 		[HideInInspector] _StencilRef("Stencil Reference", Float) = 1.0
// 		[HideInInspector][Enum(UnityEngine.Rendering.CompareFunction)] _StencilComp("Stencil Comparison", Float) = 8 // Set to Always as default

// 		// Outline properties are drawn via custom editor.
// 		[HideInInspector] _OutlineWidth("Outline Width", Range(0,8)) = 3.0
// 		[HideInInspector] _OutlineColor("Outline Color", Color) = (1,1,0,1)
// 		[HideInInspector] _OutlineReferenceTexWidth("Reference Texture Width", Int) = 1024
// 		[HideInInspector] _ThresholdEnd("Outline Threshold", Range(0,1)) = 0.25
// 		[HideInInspector] _OutlineSmoothness("Outline Smoothness", Range(0,1)) = 1.0
// 		[HideInInspector][MaterialToggle(_USE8NEIGHBOURHOOD_ON)] _Use8Neighbourhood("Sample 8 Neighbours", Float) = 1
// 		[HideInInspector] _OutlineMipLevel("Outline Mip Level", Range(0,3)) = 0
// 	}

// 	SubShader {
// 		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "PreviewType"="Plane" }

// 		Fog { Mode Off }
// 		Cull Off
// 		ZWrite Off
// 		Blend One OneMinusSrcAlpha
// 		Lighting Off

// 		Stencil {
// 			Ref[_StencilRef]
// 			Comp[_StencilComp]
// 			Pass Keep
// 		}

// 		Pass {
// 			Name "Normal"

// 			CGPROGRAM
// 			#pragma shader_feature _ _STRAIGHT_ALPHA_INPUT
// 			#pragma vertex vert
// 			#pragma fragment frag
// 			#include "UnityCG.cginc"
// 			sampler2D _MainTex;

// 			struct VertexInput {
// 				float4 vertex : POSITION;
// 				float2 uv : TEXCOORD0;
// 				float4 vertexColor : COLOR;
// 			};

// 			struct VertexOutput {
// 				float4 pos : SV_POSITION;
// 				float2 uv : TEXCOORD0;
// 				float4 vertexColor : COLOR;
// 			};

// 			VertexOutput vert (VertexInput v) {
// 				VertexOutput o;
// 				o.pos = UnityObjectToClipPos(v.vertex);
// 				o.uv = v.uv;
// 				o.vertexColor = v.vertexColor;
// 				return o;
// 			}

// 			float4 frag (VertexOutput i) : SV_Target {
// 				float4 texColor = tex2D(_MainTex, i.uv);

// 				#if defined(_STRAIGHT_ALPHA_INPUT)
// 				texColor.rgb *= texColor.a;
// 				#endif

// 				return (texColor * i.vertexColor);
// 			}
// 			ENDCG
// 		}

// 		Pass {
// 			Name "Caster"
// 			Tags { "LightMode"="ShadowCaster" }
// 			Offset 1, 1
// 			ZWrite On
// 			ZTest LEqual

// 			Fog { Mode Off }
// 			Cull Off
// 			Lighting Off

// 			CGPROGRAM
// 			#pragma vertex vert
// 			#pragma fragment frag
// 			#pragma multi_compile_shadowcaster
// 			#pragma fragmentoption ARB_precision_hint_fastest
// 			#include "UnityCG.cginc"
// 			sampler2D _MainTex;
// 			fixed _Cutoff;

// 			struct VertexOutput {
// 				V2F_SHADOW_CASTER;
// 				float4 uvAndAlpha : TEXCOORD1;
// 			};

// 			VertexOutput vert (appdata_base v, float4 vertexColor : COLOR) {
// 				VertexOutput o;
// 				o.uvAndAlpha = v.texcoord;
// 				o.uvAndAlpha.a = vertexColor.a;
// 				TRANSFER_SHADOW_CASTER(o)
// 				return o;
// 			}

// 			float4 frag (VertexOutput i) : SV_Target {
// 				fixed4 texcol = tex2D(_MainTex, i.uvAndAlpha.xy);
// 				clip(texcol.a * i.uvAndAlpha.a - _Cutoff);
// 				SHADOW_CASTER_FRAGMENT(i)
// 			}
// 			ENDCG
// 		}
// 	}
// 	CustomEditor "SpineShaderWithOutlineGUI"
// }
Shader "Spine/Skeleton" {
	Properties {
		_Cutoff ("Shadow alpha cutoff", Range(0,1)) = 0.1
		[NoScaleOffset] _MainTex ("Main Texture", 2D) = "black" {}
		[Toggle(_STRAIGHT_ALPHA_INPUT)] _StraightAlphaInput("Straight Alpha Texture", Int) = 0
		[HideInInspector] _StencilRef("Stencil Reference", Float) = 1.0
		[HideInInspector][Enum(UnityEngine.Rendering.CompareFunction)] _StencilComp("Stencil Comparison", Float) = 8 // Set to Always as default

		// Outline properties are drawn via custom editor.
		[HideInInspector] _OutlineWidth("Outline Width", Range(0,8)) = 3.0
		[HideInInspector] _OutlineColor("Outline Color", Color) = (1,1,0,1)
		[HideInInspector] _OutlineReferenceTexWidth("Reference Texture Width", Int) = 1024
		[HideInInspector] _ThresholdEnd("Outline Threshold", Range(0,1)) = 0.25
		[HideInInspector] _OutlineSmoothness("Outline Smoothness", Range(0,1)) = 1.0
		[HideInInspector][MaterialToggle(_USE8NEIGHBOURHOOD_ON)] _Use8Neighbourhood("Sample 8 Neighbours", Float) = 1
		[HideInInspector] _OutlineMipLevel("Outline Mip Level", Range(0,3)) = 0
        _angle ("Angle", Float) = 60
	}

	SubShader {
		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "PreviewType"="Plane" }

		Fog { Mode Off }
		Cull Off
		ZWrite Off
		Blend One OneMinusSrcAlpha
		Lighting Off

		Stencil {
			Ref[_StencilRef]
			Comp[_StencilComp]
			Pass Keep
		}

		Pass {
			Name "Normal"

			CGPROGRAM
			#pragma shader_feature _ _STRAIGHT_ALPHA_INPUT
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			sampler2D _MainTex;
            float4 _MainTex_ST;
            float _angle;
			struct VertexInput {
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float4 vertexColor : COLOR;
			};

			struct VertexOutput {
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
				float4 vertexColor : COLOR;
			};

			VertexOutput vert (VertexInput v) {
				VertexOutput o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				o.vertexColor = v.vertexColor;
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);

                //在 MVP 变换之后再进行旋转操作,并修改顶点的 Z 值(深度)
                //弧度
                fixed radian = _angle / 180 * 3.14159;
                fixed cosTheta = cos(radian);
                fixed sinTheta = sin(radian);

                //旋转中心点(测试用的四边形, 正常的 spine 做的模型脚下旋转的点就是(0,0), 可以省去下面这一步已经旋转完成后的 +center 操作)
                // half2 center = half2(0, -0.5);
                // v.vertex.zy -= center;

                half z = v.vertex.z * cosTheta - v.vertex.y * sinTheta;
                half y = v.vertex.z * sinTheta + v.vertex.y * cosTheta;
                v.vertex = half4(v.vertex.x, y, z, v.vertex.w);

                // v.vertex.zy += center;

                float4 verticalClipPos = UnityObjectToClipPos(v.vertex);
                o.pos.z = verticalClipPos.z / verticalClipPos.w * o.pos.w;
				return o;
			}

			float4 frag (VertexOutput i) : SV_Target {
				float4 texColor = tex2D(_MainTex, i.uv);

				#if defined(_STRAIGHT_ALPHA_INPUT)
				texColor.rgb *= texColor.a;
				#endif

				return (texColor * i.vertexColor);
			}
			ENDCG
		}

		Pass {
			Name "Caster"
			Tags { "LightMode"="ShadowCaster" }
			Offset 1, 1
			ZWrite On
			ZTest LEqual

			Fog { Mode Off }
			Cull Off
			Lighting Off

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_shadowcaster
			#pragma fragmentoption ARB_precision_hint_fastest
			#include "UnityCG.cginc"
			sampler2D _MainTex;
			fixed _Cutoff;

			struct VertexOutput {
				V2F_SHADOW_CASTER;
				float4 uvAndAlpha : TEXCOORD1;
			};

			VertexOutput vert (appdata_base v, float4 vertexColor : COLOR) {
				VertexOutput o;
				o.uvAndAlpha = v.texcoord;
				o.uvAndAlpha.a = vertexColor.a;
				TRANSFER_SHADOW_CASTER(o)
				return o;
			}

			float4 frag (VertexOutput i) : SV_Target {
				fixed4 texcol = tex2D(_MainTex, i.uvAndAlpha.xy);
				clip(texcol.a * i.uvAndAlpha.a - _Cutoff);
				SHADOW_CASTER_FRAGMENT(i)
			}
			ENDCG
		}
	}
	CustomEditor "SpineShaderWithOutlineGUI"
}
