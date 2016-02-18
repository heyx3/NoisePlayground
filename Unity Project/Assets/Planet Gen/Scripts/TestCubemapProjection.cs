using System;
using UnityEngine;

namespace PlanetGen
{
	[RequireComponent(typeof(MeshRenderer))]
	public class TestCubemapProjection : MonoBehaviour
	{
		public Texture2D PosXFace, PosYFace, PosZFace,
						 NegXFace, NegYFace, NegZFace;

		[NonSerialized]
		public Cubemap CubeMap;


		void Start()
		{
			CubeMap = new Cubemap(PosXFace.width, TextureFormat.RGBAFloat, false);
			
			Color[] cols = PosXFace.GetPixels();
			CubeMap.SetPixels(cols, CubemapFace.PositiveX);

			cols = PosYFace.GetPixels();
			CubeMap.SetPixels(cols, CubemapFace.PositiveY);

			cols = PosZFace.GetPixels();
			CubeMap.SetPixels(cols, CubemapFace.PositiveZ);

			cols = NegXFace.GetPixels();
			CubeMap.SetPixels(cols, CubemapFace.NegativeX);

			cols = NegYFace.GetPixels();
			CubeMap.SetPixels(cols, CubemapFace.NegativeY);

			cols = NegZFace.GetPixels();
			CubeMap.SetPixels(cols, CubemapFace.NegativeZ);

			CubeMap.Apply();

			GetComponent<MeshRenderer>().material.SetTexture("_Heightmap", CubeMap);
		}
	}
}