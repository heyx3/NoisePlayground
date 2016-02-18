Shader "Planet/Surface"
{
    Properties
    {
		_ColorGradient("Color Gradient by Height", 2D) = "white" {}
		_Heightmap("Heightmap Cube Tex", CUBE) = "" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
				float3 localPos : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _ColorGradient;
			samplerCUBE _Heightmap;
            
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
                o.localPos = v.vertex.xyz;

                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
				fixed4 col = texCUBE(_Heightmap, i.localPos);
				//col = fixed4((i.localPos * 0.5 + 0.5), 1.0);
                return col;
            }
            ENDCG
        }
    }
}