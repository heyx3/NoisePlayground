using System;
using System.Collections.Generic;
using UnityEngine;

namespace IslandGen
{
	public class TerrainGenerator : MonoBehaviour
	{
		public int Size = 1024;
		public float HorizontalScale = 1.0f, VerticalScale = 1.0f;
		public int Seed = 15623;
		public int ExtraThreads = 0;
		public OctaveInfo[] FBMOctaves;

		public bool ShouldSet = false;


		void Update()
		{
			if (ShouldSet)
			{
				ShouldSet = false;


				float[,] heightmap = HeightmapGenerator.GenerateHeightmap(Size, Size, FBMOctaves, Seed, ExtraThreads);

				TerrainData dat = new TerrainData();
				dat.heightmapResolution = Size;
				dat.SetHeights(0, 0, heightmap);
				dat.size = new Vector3(HorizontalScale, VerticalScale, HorizontalScale);
				

				//float[,,] alphaMap = new float[Size, Size, 1];
				//for (int y = 0; y < Size; ++y)
				//	for (int x = 0; x < Size; ++x)
				//		alphaMap[x, y, 0] = 1.0f;

				//dat.alphamapResolution = Size;
				//dat.alphamapLayers = 0;
				//dat.SetAlphamaps(0, 0, alphaMap);


				GameObject t = Terrain.CreateTerrainGameObject(dat);
			}
		}
	}
}