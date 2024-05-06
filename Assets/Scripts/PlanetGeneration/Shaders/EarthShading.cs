using UnityEngine;

[CreateAssetMenu (menuName = "Celestial Body/Earth-Like/Earth Shading")]
public class EarthShading : PlanetShading {

	public EarthColours customizedCols;
	public EarthColours randomizedCols;

	[Header ("Shading Data")]
	public SimpleNoiseSettings detailWarpNoise;
	public SimpleNoiseSettings detailNoise;
	public SimpleNoiseSettings largeNoise;
	public SimpleNoiseSettings smallNoise;

	public override void SetTerrainProperties (Material material, Vector2 heightMinMax, float bodyScale) {

		material.SetVector ("heightMinMax", heightMinMax);
		material.SetFloat ("oceanLevel", oceanLevel);
		material.SetFloat ("bodyScale", bodyScale);

		if (randomize) {
			SetRandomColours (material);
			ApplyColours (material, randomizedCols);
		} else {
			ApplyColours (material, customizedCols);
		}
	}

	void ApplyColours (Material material, EarthColours colors) {
		material.SetColor ("_ShoreLow", colors.shoreColLow);
		material.SetColor ("_ShoreHigh", colors.shoreColHigh);

		material.SetColor ("_FlatLowA", colors.flatColLowA);
		material.SetColor ("_FlatHighA", colors.flatColHighA);

		material.SetColor ("_FlatLowB", colors.flatColLowB);
		material.SetColor ("_FlatHighB", colors.flatColHighB);

		material.SetColor ("_SteepLow", colors.steepLow);
		material.SetColor ("_SteepHigh", colors.steepHigh);
	}

	void SetRandomColours (Material material) {
		PRNG random = new PRNG (seed);
		//randomizedCols.shoreCol = ColorHelper.Random (random, 0.3f, 0.7f, 0.4f, 0.8f);
		randomizedCols.flatColLowA = ColorHelper.Random (random, 0.45f, 0.6f, 0.7f, 0.8f);
		randomizedCols.flatColHighA = ColorHelper.TweakHSV (
			randomizedCols.flatColLowA,
			random.SignedValue () * 0.2f,
			random.SignedValue () * 0.15f,
			random.Range (-0.25f, -0.2f)
		);

		randomizedCols.flatColLowB = ColorHelper.Random (random, 0.45f, 0.6f, 0.7f, 0.8f);
		randomizedCols.flatColHighB = ColorHelper.TweakHSV (
			randomizedCols.flatColLowB,
			random.SignedValue () * 0.2f,
			random.SignedValue () * 0.15f,
			random.Range (-0.25f, -0.2f)
		);

		randomizedCols.shoreColLow = ColorHelper.Random (random, 0.2f, 0.3f, 0.9f, 1);
		randomizedCols.shoreColHigh = ColorHelper.TweakHSV (
			randomizedCols.shoreColLow,
			random.SignedValue () * 0.2f,
			random.SignedValue () * 0.2f,
			random.Range (-0.3f, -0.2f)
		);

		randomizedCols.steepLow = ColorHelper.Random (random, 0.3f, 0.7f, 0.4f, 0.6f);
		randomizedCols.steepHigh = ColorHelper.TweakHSV (
			randomizedCols.steepLow,
			random.SignedValue () * 0.2f,
			random.SignedValue () * 0.2f,
			random.Range (-0.35f, -0.2f)
		);
	}

	protected override void SetShadingDataComputeProperties () {
		PRNG random = new PRNG (seed);
		detailNoise.SetComputeValues (shadingDataCompute, random, "_detail");
		detailWarpNoise.SetComputeValues (shadingDataCompute, random, "_detailWarp");
		largeNoise.SetComputeValues (shadingDataCompute, random, "_large");
		smallNoise.SetComputeValues (shadingDataCompute, random, "_small");
	}

	[System.Serializable]
	public struct EarthColours {
		public Color shoreColLow;
		public Color shoreColHigh;
		public Color flatColLowA;
		public Color flatColHighA;
		public Color flatColLowB;
		public Color flatColHighB;

		public Color steepLow;
		public Color steepHigh;
	}
}
