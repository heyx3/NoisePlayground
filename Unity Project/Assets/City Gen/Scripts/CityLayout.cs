using System;
using UnityEngine;


namespace CityGen
{
	/// <summary>
	/// The city is split into a grid of cells.
	/// Each cell's points are randomly offset so that it isn't completely uniform.
	/// </summary>
	public struct CityLayout
	{
		/// <summary>
		/// The points defining the bounds of each cell.
		/// Roughly resembles a square grid but with some offset for each point.
		/// </summary>
		public Vector2[,] CellPoints;


		public int Width { get { return CellPoints.GetLength(0); } }
		public int Height { get { return CellPoints.GetLength(1); } }


		public CityLayout(int nCellsHorizontal, int nCellsVertical)
		{
			CellPoints = new Vector2[nCellsHorizontal, nCellsVertical];
		}


		public void Generate(float maxPointVariance, int seed = 12345)
		{
			System.Random rnd = new System.Random(seed);

			for (int y = 0; y < Height; ++y)
			{
				for (int x = 0; x < Width; ++x)
				{
					CellPoints[x, y] = new Vector2((float)x, (float)y);
					CellPoints[x, y] += new Vector2(maxPointVariance * (float)(-1.0f + (rnd.NextDouble() * 2.0f)),
													maxPointVariance * (float)(-1.0f + (rnd.NextDouble() * 2.0f)));
				}
			}
		}
	}
}