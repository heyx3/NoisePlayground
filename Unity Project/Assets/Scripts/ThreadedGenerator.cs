using System;
using System.Collections.Generic;
using System.Threading;


/// <summary>
/// Automatically splits up the task of generating an array of something
/// across any number of threads, including the one it is created on.
/// </summary>
[Serializable]
public abstract class ThreadedGenerator<DatType>
{
	private static Thread RunThread(int min, int max, DatType[] array,
									ThreadedGenerator<DatType> gen)
	{
		Thread t = new Thread(() =>
		{
			for (int i = min; i <= max; ++i)
				array[i] = gen.CalculateAt(i);
		});
		t.Start();
		return t;
	}
	private static Thread RunThread(int minY, int maxY, DatType[,] array,
									ThreadedGenerator<DatType> gen)
	{
		Thread t = new Thread(() =>
		{
			for (int y = minY; y <= maxY; ++y)
				for (int x = 0; x < array.GetLength(0); ++x)
					array[x, y] = gen.CalculateAt(new Vector2i(x, y));
		});
		t.Start();
		return t;
	}
	private static Thread RunThread(int minZ, int maxZ, DatType[,,] array,
									ThreadedGenerator<DatType> gen)
	{
		Thread t = new Thread(() =>
		{
			for (int z = minZ; z <= maxZ; ++z)
				for (int y = 0; y < array.GetLength(1); ++y)
					for (int x = 0; x < array.GetLength(0); ++x)
						array[x, y, z] = gen.CalculateAt(x, y, z);
		});
		t.Start();
		return t;
	}


	public int NThreads;


	public ThreadedGenerator(int nThreads) { NThreads = nThreads; }


	public void Generate(DatType[] array)
	{
		int perThread = array.Length / NThreads;
		List<Thread> threads = new List<Thread>();

		for (int i = 0; i < NThreads - 1; ++i)
		{
			threads.Add(RunThread(i * perThread,
								  ((i + 1) * perThread) - 1,
								  array, this));
		}
		//Generate the remaining values on this thread.
		int min = (NThreads - 1) * perThread,
			max = array.Length - 1;
		for (int i = min; i <= max; ++i)
			array[i] = CalculateAt(i);

		//Wait for the other threads to finish too.
		for (int i = 0; i < threads.Count; ++i)
			threads[i].Join();
	}
	public void Generate(DatType[,] array)
	{
		int perThread = array.GetLength(1) / NThreads;
		List<Thread> threads = new List<Thread>();

		for (int i = 0; i < NThreads - 1; ++i)
		{
			threads.Add(RunThread(i * perThread,
								  ((i + 1) * perThread) - 1,
								  array, this));
		}
		//Generate the remaining values on this thread.
		int min = (NThreads - 1) * perThread,
			max = array.GetLength(1) - 1;
		for (int y = min; y <= max; ++y)
			for (int x = 0; x < array.GetLength(0); ++x)
				array[x, y] = CalculateAt(new Vector2i(x, y));

		//Wait for the other threads to finish too.
		for (int i = 0; i < threads.Count; ++i)
			threads[i].Join();
	}
	public void Generate(DatType[,,] array)
	{
		int perThread = array.GetLength(2) / NThreads;
		List<Thread> threads = new List<Thread>();

		for (int i = 0; i < NThreads - 1; ++i)
		{
			threads.Add(RunThread(i * perThread,
								  ((i + 1) * perThread) - 1,
								  array, this));
		}
		//Generate the remaining values on this thread.
		int min = (NThreads - 1) * perThread,
			max = array.GetLength(1) - 1;
		for (int z = min; z <= max; ++z)
			for (int y = 0; y < array.GetLength(1); ++y)
				for (int x = 0; x < array.GetLength(0); ++x)
					array[x, y, z] = CalculateAt(x, y, z);

		//Wait for the other threads to finish too.
		for (int i = 0; i < threads.Count; ++i)
			threads[i].Join();
	}

	public abstract DatType CalculateAt(int pos);
	public abstract DatType CalculateAt(Vector2i pos);
	public abstract DatType CalculateAt(int posX, int posY, int posZ);
}