using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GeneratorControls;

namespace PlayControls
{
    public class RealFieldManager : FieldManager
    {
		[SerializeField]
		GridGeneratorPool gridGeneratorPool;
		Dictionary<GameObject, GridObject> dataFromGrid;

		int deltaXField;
		int deltaZField;
        // Use this for initialization
        void Start()
        {
			dataFromGrid = gridGeneratorPool.GetAllGridObjects();
			deltaXField = gridGeneratorPool.widthSize;
			deltaZField = gridGeneratorPool.heightSize;

			Debug.Log("dataFromGrid: " + dataFromGrid);
        }

		public GameObject GetObjectByCoord(Vector3 coords)
		{
			GameObject go = null;

			foreach(KeyValuePair<GameObject, GridObject> pr in dataFromGrid)
			{
				if (pr.Key.transform.position.x == coords.x && pr.Key.transform.position.z == coords.z)
				{
					go = pr.Key;
				}
			}
			return go;
		}

        public override Vector3 GetAdaptedCoordinates(Vector3 initCoords)
		{
			float dist = 100.0f;
			float distMin = 100.0f;
			Vector3 pos = new Vector3(0,0,0);
			
			foreach(KeyValuePair<GameObject, GridObject> pr in dataFromGrid)
			{
				dist = Vector3.Distance(initCoords, pr.Key.transform.position);
				if (dist < distMin)
				{
					distMin = dist;
					pos = pr.Key.transform.position;
				}
			}

			return new Vector3(pos.x, initCoords.y, pos.z);
		}

		public override bool IsPassible(Vector3 coords)
		{
			//Debug.Log("Is passible coords: " + coords);
			Vector3 realPos = GetAdaptedCoordinates(coords);
			//Debug.Log("Real coords: " + realPos);

			if ((realPos.x >= -1*deltaXField/2 && realPos.x <= deltaXField/2) && (realPos.z >= -1*deltaZField/2 && realPos.z <= deltaZField/2))
			{
				GameObject go = GetObjectByCoord(realPos);
				//Debug.Log("go: " + go + " " + go.transform.position);
				if (dataFromGrid[go].objectType == GridObject.ObjectType.Wall)
				{
					return false;
				}
			}

			return true;
		}

		public override bool IsTarget(Vector3 coords)
		{
			Vector3 realPos = GetAdaptedCoordinates(coords);

			if ((realPos.x >= -1*deltaXField/2 && realPos.x <= deltaXField/2) && (realPos.z >= -1*deltaZField/2 && realPos.z <= deltaZField/2))
			{
				GameObject go = GetObjectByCoord(realPos);
				if (dataFromGrid[go].objectType == GridObject.ObjectType.Target)
				{
					return true;
				}
			}

			return false;
		}
    }
}