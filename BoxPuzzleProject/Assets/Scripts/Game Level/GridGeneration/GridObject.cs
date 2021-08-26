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
		MeshFilter meshFilter;
		[SerializeField]
		MeshRenderer meshRenderer;

		WallObjects wallObjects;
		BoxObjects boxObjects;
		TargetObjects targetObjects;


		// Use this for initialization
		void Start () 
		{
			//meshFilter = gameObject.AddComponent<MeshFilter>();
			//meshRenderer = gameObject.AddComponent<MeshRenderer>();
		}
		
		public void MakeChoose()
		{
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
			}

			if (objectType == ObjectType.Target)
			{
				targetObjects = Resources.Load<TargetObjects>("ScriptableObjects/TargetObject");
				meshFilter.mesh = targetObjects.targetMeshes[objectSubType];
				meshRenderer.material = targetObjects.targetTex[objectSubType];
			}
		}
	}
}