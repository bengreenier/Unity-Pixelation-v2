Shader "Hidden/Custom/Pixelation"
{
	HLSLINCLUDE

#include "Packages/com.unity.postprocessing/PostProcessing/Shaders/StdLib.hlsl"
		TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);

	float2 BlockCount;
	float2 BlockSize;

	float4 Frag(VaryingsDefault i) : SV_Target
	{
		float2 blockPos = floor(i.texcoord * BlockCount);
		float2 blockCenter = blockPos * BlockSize + BlockSize * 0.5;

		float4 tex = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, blockCenter);
		return tex;
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
