
Shader "Celestial/Earth"
{

	Properties
	{
		[Header(Flat Terrain)]
		_ShoreLow("Shore Low", Color) = (0,0,0,1)
		_ShoreHigh("Shore High", Color) = (0,0,0,1)
		_FlatLowA("Flat Low A", Color) = (0,0,0,1)
		_FlatHighA("Flat High A", Color) = (0,0,0,1)

		_FlatLowB("Flat Low B", Color) = (0,0,0,1)
		_FlatHighB("Flat High B", Color) = (0,0,0,1)

		_FlatColBlend("Colour Blend", Range(0,3)) = 1.5
		_FlatColBlendNoise("Blend Noise", Range(0,1)) = 0.3
		_ShoreHeight("Shore Height", Range(0,0.2)) = 0.05
		_ShoreBlend("Shore Blend", Range(0,0.2)) = 0.03
		_MaxFlatHeight("Max Flat Height", Range(0,1)) = 0.5

		[Header(Steep Terrain)]
		_SteepLow("Steep Colour Low", Color) = (0,0,0,1)
		_SteepHigh("Steep Colour High", Color) = (0,0,0,1)
		_SteepBands("Steep Bands", Range(1, 20)) = 8
		_SteepBandStrength("Band Strength", Range(-1,1)) = 0.5

		[Header(Flat to Steep Transition)]
		_SteepnessThreshold("Steep Threshold", Range(0,1)) = 0.5
		_FlatToSteepBlend("Flat to Steep Blend", Range(0,0.3)) = 0.1
		_FlatToSteepNoise("Flat to Steep Noise", Range(0,0.2)) = 0.1

		[Header(Snowy Poles)]
		[Toggle()]
      _UseSnowyPoles("Use Poles", float) = 0
		_SnowCol("Snow Colour", Color) = (1,1,1,1)
		_SnowLongitude("Snow Longitude", Range(0,1)) = 0.8
		_SnowBlend("Snow Blend", Range(0, 0.2)) = 0.1
		_SnowSpecular("Snow Specular", Range(0,1)) = 1
		_SnowHighlight("Snow Highlight", Range(1,2)) = 1.2
		_SnowNoiseA("Snow Noise A", Range(0,10)) = 5
		_SnowNoiseB("Snow Noise B", Range(0,10)) = 4

		[Header(Noise)]
		[NoScaleOffset] _NoiseTex ("Noise Texture", 2D) = "white" {}
		_NoiseScale("Noise Scale", Float) = 1
		_NoiseScale2("Noise Scale2", Float) = 1

		[Header(Other)]
		_FresnelCol("Fresnel Colour", Color) = (1,1,1,1)
		_FresnelStrengthNear("Fresnel Strength Min", float) = 2
		_FresnelStrengthFar("Fresnel Strength Max", float) = 5
		_FresnelPow("Fresnel Power", float) = 2
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0



		_TestParams ("Test Params", Vector) = (0,0,0,0)

	}
	SubShader
	{

		Tags { "RenderType"="Opaque" "RenderPipeline" = "UniversalRenderPipeline" }
		LOD 200

		Pass{
		HLSLPROGRAM

		#pragma vertex vert
		#pragma fragment frag
		#pragma enable_d3d11_debug_symbols

		//#pragma surface URPSurfaceShader URP/Lit fullforwardshadows
		// Physically based Standard lighting model, and enable shadows on all light types


		#pragma target 5.0
		//#pragma only_renderers d3d11
		#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
		
		#include "../Includes/Triplanar.cginc"
		#include "../Includes/Math.cginc"

            struct appdata_full
            {
                float4 vertex : POSITION;
                float4 tangent : TANGENT;
                float3 normal : NORMAL;
                float4 texcoord : TEXCOORD0;
                float4 texcoord1 : TEXCOORD1;
                float4 texcoord2 : TEXCOORD2;
                float4 texcoord3 : TEXCOORD3;
                half4 color : COLOR;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };



		float4 _TestParams;
		float4 _FresnelCol;
		float _FresnelStrengthNear;
		float _FresnelStrengthFar;
		float _FresnelPow;
		float bodyScale;

		struct Input
		{
			float2 uv_MainTex;
			float3 worldPos;
			float4 terrainData;
			float3 vertPos;
			float3 normal;
			float4 tangent;
			float fresnel;
		};

		void vert (inout appdata_full v, out Input o)
		{
			o = (Input)0;
			//UNITY_INITIALIZE_OUTPUT(Input, o);
			o.vertPos = v.vertex;
			o.normal = v.normal;
			o.terrainData = v.texcoord;
			o.tangent = v.tangent;

			// Fresnel (fade out when close to body)
			float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
			float3 bodyWorldCentre = mul(unity_ObjectToWorld, float4(0, 0, 0, 1)).xyz;
			float camRadiiFromSurface = (length(bodyWorldCentre - _WorldSpaceCameraPos.xyz) - bodyScale) / bodyScale;
			float fresnelT = smoothstep(0,1,camRadiiFromSurface);
			float3 viewDir = normalize(worldPos - _WorldSpaceCameraPos.xyz);
			float3 normWorld = normalize(mul(unity_ObjectToWorld, float4(v.normal,0)));
			float fresStrength = lerp(_FresnelStrengthNear, _FresnelStrengthFar, fresnelT);
			o.fresnel = saturate(fresStrength * pow(1 + dot(viewDir, normWorld), _FresnelPow));
		}

		// Flat terrain:
		float4 _ShoreLow;
		float4 _ShoreHigh;

		float4 _FlatLowA;
		float4 _FlatHighA;
		float4 _FlatLowB;
		float4 _FlatHighB;

		float _FlatColBlend;
		float _FlatColBlendNoise;
		float _ShoreHeight;
		float _ShoreBlend;
		float _MaxFlatHeight;

		// Steep terrain
		float4 _SteepLow;
		float4 _SteepHigh;
		float _SteepBands;
		float _SteepBandStrength;

		// Flat to steep transition
		float _SteepnessThreshold;
		float _FlatToSteepBlend;
		float _FlatToSteepNoise;

		// Snowy poles
		float _UseSnowyPoles;
		float3 _SnowCol;
		float _SnowLongitude;
		float _SnowBlend;
		float _SnowSpecular;
		float _SnowHighlight;
		float _SnowNoiseA;
		float _SnowNoiseB;

		// Other:
		float _Glossiness;
		float _Metallic;

		sampler2D _NoiseTex;
		sampler2D _SnowNormal;
		float _NoiseScale;
		float _NoiseScale2;


		// Height data:
		float2 heightMinMax;
		float oceanLevel;

		struct SurfaceOutput
		{
			half3 Albedo;  // diffuse color
			half3 Normal;  // tangent space normal, if written
			half3 Emission;
			half Specular;  // specular power in 0..1 range
			half Gloss;    // specular intensity
			half Alpha;    // alpha for transparencies
			float Metallic;
		};

		half4 LightingNoLighting(SurfaceOutput s, half3 lightDir, half atten)
		{
			half4 c;
			c.rgb = s.Albedo * 0.8;
			c.a = s.Alpha;
			return c;
		}

		void surf (Input IN, inout SurfaceOutput o)
		{

			// Calculate steepness: 0 where totally flat, 1 at max steepness
			float3 sphereNormal = normalize(IN.vertPos);
			float steepness = 1 - dot (sphereNormal, IN.normal);
			steepness = remap01(steepness, 0, 0.65);

			// Calculate heights
			float terrainHeight = length(IN.vertPos);
			float shoreHeight = lerp(heightMinMax.x, 1, oceanLevel);
			float aboveShoreHeight01 = remap01(terrainHeight, shoreHeight, heightMinMax.y);
			float flatHeight01 = remap01(aboveShoreHeight01, 0, _MaxFlatHeight);

			// Sample noise texture at two different scales
			float4 texNoise = triplanar(IN.vertPos, IN.normal, _NoiseScale, _NoiseTex);
			float4 texNoise2 = triplanar(IN.vertPos, IN.normal, _NoiseScale2, _NoiseTex);

			// Flat terrain colour A and B
			float flatColBlendWeight = Blend(0, _FlatColBlend, (flatHeight01-.5) + (texNoise.b - 0.5) * _FlatColBlendNoise);
			float3 flatTerrainColA = lerp(_FlatLowA, _FlatHighA, flatColBlendWeight);
			flatTerrainColA = lerp(flatTerrainColA, (_FlatLowA + _FlatHighA) / 2, texNoise.a);
			float3 flatTerrainColB = lerp(_FlatLowB, _FlatHighB, flatColBlendWeight);
			flatTerrainColB = lerp(flatTerrainColB, (_FlatLowB + _FlatHighB) / 2, texNoise.a);

			// Biome
			float biomeWeight = Blend(_TestParams.x, _TestParams.y,IN.terrainData.x);
			biomeWeight = Blend(0, _TestParams.z, IN.vertPos.x + IN.terrainData.x * _TestParams.x + IN.terrainData.y * _TestParams.y);
			float3 flatTerrainCol = lerp(flatTerrainColA, flatTerrainColB, biomeWeight);

			// Shore
			float shoreBlendWeight = 1-Blend(_ShoreHeight, _ShoreBlend, flatHeight01);
			float4 shoreCol = lerp(_ShoreLow, _ShoreHigh, remap01(aboveShoreHeight01, 0, _ShoreHeight));
			shoreCol = lerp(shoreCol, (_ShoreLow + _ShoreHigh) / 2, texNoise.g);
			flatTerrainCol = lerp(flatTerrainCol, shoreCol, shoreBlendWeight);

			// Steep terrain colour
			float3 sphereTangent = normalize(float3(-sphereNormal.z, 0, sphereNormal.x));
			float3 normalTangent = normalize(IN.normal - sphereNormal * dot(IN.normal, sphereNormal));
			float banding = dot(sphereTangent, normalTangent) * .5 + .5;
			banding = (int)(banding * (_SteepBands + 1)) / _SteepBands;
			banding = (abs(banding - 0.5) * 2 - 0.5) * _SteepBandStrength;
			float3 steepTerrainCol = lerp(_SteepLow, _SteepHigh, aboveShoreHeight01 + banding);

			// Flat to steep colour transition
			float flatBlendNoise = (texNoise2.r - 0.5) * _FlatToSteepNoise;
			float flatStrength = 1 - Blend(_SteepnessThreshold + flatBlendNoise, _FlatToSteepBlend, steepness);
			float flatHeightFalloff = 1 - Blend(_MaxFlatHeight + flatBlendNoise, _FlatToSteepBlend, aboveShoreHeight01);
			flatStrength *= flatHeightFalloff;

			// Snowy poles
			float3 snowCol = 0;
			float snowWeight = 0;
			float snowLineNoise = IN.terrainData.y * _SnowNoiseA * 0.01 + (texNoise.b-0.5) * _SnowNoiseB * 0.01;
			snowWeight = Blend(_SnowLongitude, _SnowBlend, abs(IN.vertPos.y + snowLineNoise)) * _UseSnowyPoles;
			float snowSpeckle = 1 - texNoise2.g * 0.5 * 0.1;
			snowCol = _SnowCol * lerp (1, _SnowHighlight, aboveShoreHeight01 + banding) * snowSpeckle;

			// Set surface colour
			float3 compositeCol = lerp(steepTerrainCol, flatTerrainCol, flatStrength);
			compositeCol = lerp(compositeCol, snowCol, snowWeight);
			compositeCol = lerp(compositeCol, _FresnelCol, IN.fresnel);
			o.Albedo = compositeCol;

			// Glossiness
			float glossiness = dot(o.Albedo, 1) / 3 * _Glossiness;
			glossiness = max(glossiness, snowWeight * _SnowSpecular);
			o.Gloss = glossiness;
			o.Metallic = _Metallic;
		}
		    struct Attributes
            {
                // The positionOS variable contains the vertex positions in object
                // space.
                float4 positionOS   : POSITION;                 
            };
		    struct Varyings
            {
                // The positions in this struct must have the SV_POSITION semantic.
                float4 positionHCS  : SV_POSITION;
            };   
		    Varyings vert(Attributes IN)
            {
                // Declaring the output object (OUT) with the Varyings struct.
                Varyings OUT;
                // The TransformObjectToHClip function transforms vertex positions
                // from object space to homogenous space
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                // Returning the output.
                return OUT;
            }
		    half4 frag() : SV_Target
            {
                // Defining the color variable and returning it.
                half4 customColor;
                customColor = half4(0, .5, 0, 1);
                return customColor;
            }
		ENDHLSL
		}
	}
}
