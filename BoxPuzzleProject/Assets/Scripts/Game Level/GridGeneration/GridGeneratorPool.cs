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
		// Use this for initialization
		void Start () 
		{
			//Debug.Log("go object: " + go);
		}
			
		public void GenerateBaseGrid() // Just for brief - width and height are even numbers
		{
			foreach (Transform child in this.transform) {
     			GameObject.Destroy(child.gameObject);
 			}

			go = Resources.Load<GridObject>("Prefabs/GridObject");
			Debug.Log("go object: " + go);

			for (int i = 0; i < widthSize; ++i)
			{
				for (int j = 0; j < heightSize; ++j)
				{
					GridObject gg = Instantiate(go, new Vector3((i-widthSize/2)*BoxMoverManager.inst.walkDistance, 0.5f, (j-heightSize/2)*BoxMoverManager.inst.walkDistance), Quaternion.identity);
					gg.gameObject.transform.parent = this.transform;
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