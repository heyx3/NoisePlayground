using UnityEngine;


namespace CityGen
{
	/// <summary>
	/// A region of space defined by four corners, roughly similar to a rectangle. 
	/// </summary>
	public struct SkewedBounds
	{
		/// <summary>
		/// The four corners of this borough, in order along each axis.
		/// </summary>
		public Vector2[,] Corners;

		public SkewedBounds(Vector2 minXY, Vector2 minXMaxY, Vector2 maxXMinY, Vector2 maxXY)
		{
			Corners = new Vector2[1, 1];
			Corners[0, 0] = minXY;
			Corners[0, 1] = minXMaxY;
			Corners[1, 0] = maxXMinY;
			Corners[1, 1] = maxXY;
		}
	}
}