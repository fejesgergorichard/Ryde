Shader "GGNT/AlwaysVisible"
{
	Properties
	{ }

	SubShader
	{
		Tags { "Queue" = "Geometry" }

		ColorMask 0
		ZWrite Off

		Stencil 
		{
			Ref 1
			Comp always
			Pass replace
		}

		CGPROGRAM
		#pragma surface surf Lambert

		struct Input {
			float3 worldPos;
		};

		void surf(Input IN, inout SurfaceOutput o)
		{
			o.Albedo = fixed4(0, 0, 0, 0);
		}

		ENDCG
	}
	Fallback "Diffuse"
}
