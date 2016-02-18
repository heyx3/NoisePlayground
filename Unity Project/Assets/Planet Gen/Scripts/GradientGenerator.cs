using System;
using System.Collections.Generic;
using UnityEngine;


namespace PlanetGen
{
	[RequireComponent(typeof(MeshRenderer))]
	public class GradientGenerator : MonoBehaviour
	{
		public Gradient GradientToUse = new Gradient();
		public int Resolution = 512;
		public TextureFormat TexFormat = TextureFormat.RGB24;
		public string MaterialGradientVarName = "_ColorGradient";

		[NonSerialized]
		Texture2D GradientTex = null;


		void Start()
		{
			if (!SystemInfo.SupportsTextureFormat(TexFormat))
			{
				Debug.LogError("This platform doesn't support the texture format " + TexFormat);
				return;
			}

			Color[] cols = new Color[Resolution];
			for (int i = 0; i < Resolution; ++i)
			{
				float t = (float)i / (float)(Resolution - 1);
				cols[i] = GradientToUse.Evaluate(t);
			}

			GradientTex = new Texture2D(Resolution, 1, TexFormat, false);
			GradientTex.SetPixels(cols);
			GradientTex.Apply();

			GetComponent<MeshRenderer>().material.SetTexture(MaterialGradientVarName, GradientTex);
		}

		void OnValidate()
		{
			if (!SystemInfo.SupportsTextureFormat(TexFormat))
			{
				Debug.LogWarning("This platform doesn't support the texture format " + TexFormat);
				return;
			}
		}
	}
}