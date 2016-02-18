using System;
using System.Collections.Generic;
using UnityEngine;


namespace PlanetGen
{
	[RequireComponent(typeof(MeshRenderer))]
	public class HeightGenerator : MonoBehaviour
	{
		public int NThreads = 4;
		public int Resolution = 256;
		public string MaterialCubemapVarName = "_Heightmap";

		public int NOctaves = 3;
		public float InitialScale = 10.0f;
		public float Persistence = 0.5f;
		public float Seed = 32.12312f;

		[NonSerialized]
		public Cubemap GeneratedCubemap = null;


		void Start()
		{
			if (!Mathf.IsPowerOfTwo(Resolution))
			{
				Debug.LogError("'Resolution' field must be a power of 2!");
				return;
			}

			GeneratedCubemap = Generate(Resolution).GenerateTex();
			GetComponent<MeshRenderer>().material.SetTexture(MaterialCubemapVarName, GeneratedCubemap);
		}

		
		private HeightmapCubemap Generate(int texSize)
		{
			HeightmapCubemap c = new HeightmapCubemap(texSize);
			ThreadedHeightmapGen worker = new ThreadedHeightmapGen(this, c);

			worker.CurrentFace = CubemapFace.PositiveX;
			worker.Generate(c.PosX);

			worker.CurrentFace = CubemapFace.PositiveY;
			worker.Generate(c.PosY);

			worker.CurrentFace = CubemapFace.PositiveZ;
			worker.Generate(c.PosZ);

			worker.CurrentFace = CubemapFace.NegativeX;
			worker.Generate(c.NegX);

			worker.CurrentFace = CubemapFace.NegativeY;
			worker.Generate(c.NegY);

			worker.CurrentFace = CubemapFace.NegativeZ;
			worker.Generate(c.NegZ);

			return c;
		}

		/// <summary>
		/// Does the work for generating noise.
		/// </summary>
		private class ThreadedHeightmapGen : ThreadedGenerator<float>
		{
			public CubemapFace CurrentFace;

			private HeightmapCubemap hc;
			private HeightGenerator gen;


			public ThreadedHeightmapGen(HeightGenerator _gen, HeightmapCubemap _hc)
				: base(_gen.NThreads)
			{
				gen = _gen;
				hc = _hc;
			}


			//Only need the 2D generator.
			public override float CalculateAt(Vector2i pos)
			{
				//Get the cubemap lookup vector.
				Vector3 posV = Vector3.zero;
				switch (CurrentFace)
				{
					case CubemapFace.PositiveX:
						posV = hc.GetVectorForPosX(pos.x, pos.y);
						break;
					case CubemapFace.NegativeX:
						posV = hc.GetVectorForNegX(pos.x, pos.y);
						break;
					case CubemapFace.PositiveY:
						posV = hc.GetVectorForPosY(pos.x, pos.y);
						break;
					case CubemapFace.NegativeY:
						posV = hc.GetVectorForNegY(pos.x, pos.y);
						break;
					case CubemapFace.PositiveZ:
						posV = hc.GetVectorForPosZ(pos.x, pos.y);
						break;
					case CubemapFace.NegativeZ:
						posV = hc.GetVectorForNegZ(pos.x, pos.y);
						break;
				}


				//Generate 3D noise using that vector as the seed.
				float weight = 0.5f;
				float scale = gen.InitialScale;
				float total = 0.0f;
				for (int i = 0; i < gen.NOctaves; ++i)
				{
					total += weight * (NoiseAlgos3D.SmootherNoise(posV * scale));

					weight *= gen.Persistence;
					scale /= gen.Persistence;
				}
				return total;
			}

			public override float CalculateAt(int pos)
			{
 				throw new NotImplementedException();
			}
			public override float CalculateAt(int posX, int posY, int posZ)
			{
				throw new NotImplementedException();
			}

		}
	}
}