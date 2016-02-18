using System;
using UnityEngine;
using Assert = UnityEngine.Assertions.Assert;


namespace CityGen
{
	/// <summary>
	/// Each borough is a skewed rectangular grid.
	/// Grid lines are roads and spaces between lines are buildings/parks.
	/// </summary>
	public struct BoroughLayout
	{
		/// <summary>
		/// A single area that a building might be located in.
		/// </summary>
		public struct BuildingArea
		{
			/// <summary>
			/// If false, this area is an empty space, e.x. a park.
			/// </summary>
			bool IsBuilding;

			/// <summary>
			/// The region of space this area occupies.
			/// </summary>
			SkewedBounds Bounds;
		}

		

		/// <summary>
		/// The space between adjacent buildings are roads.
		/// </summary>
		public BuildingArea[,] BuildingAreas;

		
		private struct Road
		{
			float Width;
			float T;
			Road(float width, float t) { Width = width; T = t; }
		}

		public void Generate(SkewedBounds bounds, float minRoadWidth, float maxRoadWidth,
							 int minBuildings, int maxBuildings,
							 int seed = 12345)
		{
			System.Random r = new System.Random(seed);

			Vector2 normalMinY = (bounds.Corners[1, 0] - bounds.Corners[0, 0]).normalized.GetPerp1(),
					normalMinX = (bounds.Corners[0, 1] - bounds.Corners[0, 0]).normalized.GetPerp1(),
					normalMaxY = (bounds.Corners[1, 1] - bounds.Corners[0, 1]).normalized.GetPerp1(),
					normalMaxX = (bounds.Corners[1, 1] - bounds.Corners[1, 0]).normalized.GetPerp1();
			Assert.IsTrue(normalMinY.y < 0.0f, "Flip normal min y!");
			Assert.IsTrue(normalMinX.x < 0.0f, "Flip normal min x!");
			Assert.IsTrue(normalMaxY.y > 0.0f, "Flip normal max y!");
			Assert.IsTrue(normalMaxX.x > 0.0f, "Flip normal max x!");

			int nBuildingsX = Mathf.RoundToInt(Mathf.Lerp((float)minBuildings, (float)maxBuildings, (float)r.NextDouble())),
				nBuildingsY = Mathf.RoundToInt(Mathf.Lerp((float)minBuildings, (float)maxBuildings, (float)r.NextDouble()));

			//Generate road data for vertical and horizontal roads.
			//TODO: Finish.
			Road[] vertRoads = new Road[nBuildingsX + 1],
				   horzRoads = new Road[nBuildingsY + 1];
			for (int i = 0; i < vertRoads.Length; ++i)
			{
				
			}
			for (int i = 0; i < horzRoads.Length; ++i)
			{

			}
		}
	}
}