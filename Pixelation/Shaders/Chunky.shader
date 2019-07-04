Shader "Hidden/Custom/Chunky"
{
	HLSLINCLUDE

		#include "Packages/com.unity.postprocessing/PostProcessing/Shaders/StdLib.hlsl"
		TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);
		TEXTURE2D_SAMPLER2D(_SprTex, sampler_SprTex);

		float4 _Color = float4(1, 1, 1, 1);
		float2 BlockCount;
		float2 BlockSize;

		float4 Frag(VaryingsDefault i) : SV_Target
		{
			float2 blockPos = floor(i.texcoord * BlockCount);
			float2 blockCenter = blockPos * BlockSize + BlockSize * 0.5;

			// (2)
			float4 del = float4(1, 1, 1, 1) - _Color;

			// (3)
			float4 tex = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, blockCenter) - del;
			float grayscale = dot(tex.rgb, float3(0.3, 0.59, 0.11));
			grayscale = clamp(grayscale, 0.0, 1.0);

			// (4)
			float dx = floor(grayscale * 16.0);

			// (5)
			float2 sprPos = i.texcoord;
			sprPos -= blockPos * BlockSize;
			sprPos.x /= 16;
			sprPos *= BlockCount;
			sprPos.x += 1.0 / 16.0 * dx;

			// (6)
			float4 tex2 = SAMPLE_TEXTURE2D(_SprTex, sampler_SprTex, sprPos);
			return tex2;
		}

	ENDHLSL

	SubShader
	{
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			HLSLPROGRAM
				#pragma vertex VertDefault
				#pragma fragment Frag
			ENDHLSL
		}
	}
}
