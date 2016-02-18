using System;
using System.Threading;
using System.Collections.Generic;
using UnityEngine;


namespace IslandGen
{
	/// <summary>
	/// Information about the island generation algorithm.
	/// </summary>
	[Serializable]
	public class OctaveInfo
	{
		public float Weight = 0.5f,
					 Scale = 128.0f,
					 Power = 1.0f;
	}


	public static class HeightmapGenerator
	{
		public static float[,] GenerateHeightmap(int width, int height,
												 OctaveInfo[] octaves, int seed,
												 int extraThreads)
		{
			float[,] map = new float[width, height];
			
			//Do the work on multiple threads, including this one.

			Thread[] threads = new Thread[extraThreads];

			int nLinesPerThread = height / (extraThreads + 1);

			for (int i = 0; i < extraThreads; ++i)
			{
				threads[i] = CreateThread(map, octaves, seed,
										  nLinesPerThread * i,
										  (nLinesPerThread * (i + 1)) - 1);
				threads[i].Start();
			}
			GenerateRange(map, octaves, seed, nLinesPerThread * extraThreads, height - 1);

			//Wait for all other threads to finish.
			for (int i = 0; i < extraThreads; ++i)
				threads[i].Join();

			return map;
		}
		private static Thread CreateThread(float[,] heightmap, OctaveInfo[] octaves, int seed, int firstY, int lastY)
		{
			return new Thread(() => GenerateRange(heightmap, octaves, seed, firstY, lastY));
		}
		private static void GenerateRange(float[,] heightmap, OctaveInfo[] octaves, int seed, int firstY, int lastY)
		{
			Vector3 posF = new Vector3(0.0f, 0.0f, (float)seed);
			for (Vector2i posI = new Vector2i(0, firstY); posI.y <= lastY; ++posI.y)
			{
				posF.y = (float)posI.y;
				for (posI.x = 0; posI.x < heightmap.GetLength(0); ++posI.x)
				{
					posF.x = (float)posI.x;

					heightmap[posI.x, posI.y] = 0.0f;
					for (int i = 0; i < octaves.Length; ++i)
					{
						float val = NoiseAlgos3D.SmoothNoise(posF / octaves[i].Scale);
						val = Mathf.Pow(val, octaves[i].Power);
						val *= octaves[i].Weight;

						heightmap[posI.x, posI.y] += val;
					}
				}
			}
		}
	}
}