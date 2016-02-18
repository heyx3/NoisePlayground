using System;
using System.Collections.Generic;
using UnityEngine;


namespace PlanetGen
{
	public class HeightmapCubemap
	{
		public float[,] PosX { get; private set; }
		public float[,] PosY { get; private set; }
		public float[,] PosZ { get; private set; }
		public float[,] NegX { get; private set; }
		public float[,] NegY { get; private set; }
		public float[,] NegZ { get; private set; }

		public int PixelSize { get { return PosX.GetLength(0); } }
		public float TexelSize { get; private set; }


		public HeightmapCubemap(int texSize)
		{
			PosX = new float[texSize, texSize];
			PosY = new float[texSize, texSize];
			PosZ = new float[texSize, texSize];
			NegX = new float[texSize, texSize];
			NegY = new float[texSize, texSize];
			NegZ = new float[texSize, texSize];

			TexelSize = 1.0f / (float)texSize;
		}
		

		/// <summary>
		/// Gets the normalized cubemap lookup vector for accessing the positive X face on the given pixel.
		/// </summary>
		public Vector3 GetVectorForPosX(int x, int y)
		{
			return new Vector3(1.0f,
							   (y * TexelSize) * 2.0f - 1.0f,
							   (1.0f - (x * TexelSize)) * 2.0f - 1.0f).normalized;
		}
		/// <summary>
		/// Gets the normalized cubemap lookup vector for accessing the negative X face on the given pixel.
		/// </summary>
		public Vector3 GetVectorForNegX(int x, int y)
		{
			return new Vector3(-1.0f,
							   (y * TexelSize) * 2.0f - 1.0f,
							   (x * TexelSize) * 2.0f - 1.0f).normalized;
		}
		/// <summary>
		/// Gets the normalized cubemap lookup vector for accessing the positive Y face on the given pixel.
		/// </summary>
		public Vector3 GetVectorForPosY(int x, int y)
		{
			return new Vector3((x * TexelSize) * 2.0f - 1.0f,
							   1.0f,
							   (1.0f - (y * TexelSize)) * 2.0f - 1.0f).normalized;
		}
		/// <summary>
		/// Gets the normalized cubemap lookup vector for accessing the negative Y face on the given pixel.
		/// </summary>
		public Vector3 GetVectorForNegY(int x, int y)
		{
			return new Vector3((x * TexelSize) * 2.0f - 1.0f,
							   -1.0f,
							   (y * TexelSize) * 2.0f - 1.0f).normalized;
		}
		/// <summary>
		/// Gets the normalized cubemap lookup vector for accessing the positive Z face on the given pixel.
		/// </summary>
		public Vector3 GetVectorForPosZ(int x, int y)
		{
			return new Vector3((x * TexelSize) * 2.0f - 1.0f,
							   (y * TexelSize) * 2.0f - 1.0f,
							   1.0f).normalized;
		}
		/// <summary>
		/// Gets the normalized cubemap lookup vector for accessing the negative Z face on the given pixel.
		/// </summary>
		public Vector3 GetVectorForNegZ(int x, int y)
		{
			return new Vector3((1.0f - (x * TexelSize)) * 2.0f - 1.0f,
							   (y * TexelSize) * 2.0f - 1.0f,
							   -1.0f).normalized;
		}


		public Cubemap GenerateTex()
		{
			Cubemap c = new Cubemap(PixelSize, TextureFormat.RFloat, false);

			Color[] faceCols = new Color[PixelSize * PixelSize];
			FillWithFace(faceCols, c, CubemapFace.PositiveX);
			FillWithFace(faceCols, c, CubemapFace.NegativeX);
			FillWithFace(faceCols, c, CubemapFace.PositiveY);
			FillWithFace(faceCols, c, CubemapFace.NegativeY);
			FillWithFace(faceCols, c, CubemapFace.PositiveZ);
			FillWithFace(faceCols, c, CubemapFace.NegativeZ);

			c.Apply();
			return c;
		}
		private void FillWithFace(Color[] colsArray, Cubemap c, CubemapFace face)
		{
			float[,] faceArray = null;
			switch (face)
			{
				case CubemapFace.PositiveX:
					faceArray = PosX;
					break;
				case CubemapFace.PositiveY:
					faceArray = PosY;
					break;
				case CubemapFace.PositiveZ:
					faceArray = PosZ;
					break;
				case CubemapFace.NegativeX:
					faceArray = NegX;
					break;
				case CubemapFace.NegativeY:
					faceArray = NegY;
					break;
				case CubemapFace.NegativeZ:
					faceArray = NegZ;
					break;
			}

			for (int y = 0; y < faceArray.GetLength(1); ++y)
				for (int x = 0; x < faceArray.GetLength(0); ++x)
					colsArray[x + (y * PixelSize)] = new Color(faceArray[x, y], 0.0f, 0.0f);

			c.SetPixels(colsArray, face);
		}
	}
}