using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayControls;

namespace GeneratorControls
{
	public class GridObject : MonoBehaviour 
	{
		public enum ObjectType{None, Field, Wall, Target, Box};
		public ObjectType objectType;
		public int objectSubType;

		[SerializeField]
		MeshFilter meshFilter;
		[SerializeField]
		MeshRenderer meshRenderer;

		public WallObjects wallObjects{get; private set;}
		public BoxObjects boxObjects{get; private set;}
		public TargetObjects targetObjects;//{get; private set;}


		// Use this for initialization
		void Start () 
		{
			//meshFilter = gameObject.AddComponent<MeshFilter>();
			//meshRenderer = gameObject.AddComponent<MeshRenderer>();
		}
		
		public void MakeChoose()
		{
			if (objectType == ObjectType.None)
			{
				meshFilter.mesh = null;
				meshRenderer.material = null;

				BoxController bc = gameObject.GetComponent<BoxController>();
				if (bc != null)
				{
					DestroyImmediate(bc as Object, true);
				}

				TargetController tc = gameObject.GetComponent<TargetController>();
				if (tc != null)
				{
					Vector3 coords = new Vector3(transform.position.x, 0.5f, transform.position.z);
					transform.position = coords;


					GameObject star = this.gameObject.transform.GetChild(0).gameObject;
					if (star != null)
					{
						DestroyImmediate(star as Object, true);
					}
					DestroyImmediate(tc as Object, true);
				}
			}
			
			if (objectType == ObjectType.Wall)
			{
				wallObjects = Resources.Load<WallObjects>("ScriptableObjects/WallObject");
				meshFilter.mesh = wallObjects.wallMeshes[objectSubType];
				meshRenderer.material = wallObjects.wallTex[objectSubType];
			}

			if (objectType == ObjectType.Box)
			{
				boxObjects = Resources.Load<BoxObjects>("ScriptableObjects/BoxObject");
				meshFilter.mesh = boxObjects.boxMeshes[objectSubType];
				meshRenderer.material = boxObjects.boxTex[objectSubType];
				gameObject.AddComponent(typeof(BoxController));
			}

			if (objectType == ObjectType.Target)
			{
				targetObjects = Resources.Load<TargetObjects>("ScriptableObjects/TargetObject");
				meshFilter.mesh = targetObjects.targetMeshes[objectSubType];
				meshRenderer.material = targetObjects.targetTex[objectSubType];
				gameObject.AddComponent(typeof(TargetController));
				gameObject.GetComponent<TargetController>().selfGrid = this;
				Vector3 coords = new Vector3(transform.position.x, 0.25f, transform.position.z);
				transform.position = coords;

				// Add star prefab to gameObject as a child component
				GameObject targetStar = Instantiate(Resources.Load("Prefabs/Star"), transform, false) as GameObject;
			}
		}
	}
}