using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GeneratorControls
{
	public class GridObject : MonoBehaviour 
	{
		public enum ObjectType{Field, Wall, Target, Box};
		public ObjectType objectType;
		public int objectSubType;

		[SerializeField]
		GameObject wallPrefab;
		[SerializeField]
		GameObject boxPrefab;

		// Use this for initialization
		void Start () {
			
		}
		
		public void MakeChoose()
		{
			if (objectType == ObjectType.Wall)
			{
				Instantiate(wallPrefab, new Vector3(0,0,0), Quaternion.identity, gameObject.transform);
			}

			if (objectType == ObjectType.Box)
			{
				Instantiate(boxPrefab, new Vector3(0,0,0), Quaternion.identity, gameObject.transform);
			}
		}
	}
}