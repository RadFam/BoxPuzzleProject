using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayControls;

namespace GeneratorControls
{
	public class GridGeneratorPool : MonoBehaviour 
	{
		public int widthSize;
		public int heightSize;

		[SerializeField]
		GridObject go;

		TargetController targetController;
		// Use this for initialization
		void Start () 
		{
			//Debug.Log("go object: " + go);
		}
			
		public void GenerateBaseGrid() // Just for brief - width and height are even numbers
		{
			foreach (Transform child in this.transform) {
     			GameObject.DestroyImmediate(child.gameObject, true);
 			}

			go = Resources.Load<GridObject>("Prefabs/GridObject");

			for (int i = 0; i < widthSize; ++i)
			{
				for (int j = 0; j < heightSize; ++j)
				{
					GridObject gg = Instantiate(go, new Vector3((i-widthSize/2.0f)*1, 0.5f, (j-heightSize/2.0f)*1), Quaternion.identity) as GridObject;
					gg.gameObject.transform.parent = this.transform;
					gg.throughNumber = i + j * widthSize;
				}
			}
		}

		public Dictionary<GameObject,GridObject> GetAllGridObjects()
		{
			Dictionary<GameObject,GridObject> allObjects = new Dictionary<GameObject,GridObject>();

			foreach (GridObject child in transform.GetComponentsInChildren<GridObject>()) 
			{
				allObjects.Add(child.gameObject, child);
    		}

			return allObjects;
		}
	}
}