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
        // Use this for initialization
        void Start()
        {
			dataFromGrid = gridGeneratorPool.GetAllGridObjects();
        }

        public override Vector3 GetAdaptedCoordinates(Vector3 initCoords)
		{
			return new Vector3(0,0,0);
		}

		public override bool IsPassible(Vector3 coords)
		{
			return true;
		}

		public override bool IsTarget(Vector3 coords)
		{
			return true;
		}
    }
}