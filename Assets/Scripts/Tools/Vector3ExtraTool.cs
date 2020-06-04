using UnityEngine;
using System.Collections;

public static class Vector3ExtraTool{

	//求水平距离
	public static float DistanceIgnoreYAxis(Vector3 start,Vector3 end)
	{
		Vector3 dis = end - start;
		dis.y = 0;
		return dis.magnitude;
	}
}
