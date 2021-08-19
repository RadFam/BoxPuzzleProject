using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayControls
{
    public abstract class FieldManager : MonoBehaviour
    {

        public abstract Vector3 GetAdaptedCoordinates(Vector3 initCoords);
		public abstract bool IsPassible(Vector3 coords);
		public abstract bool IsTarget(Vector3 coords);
    }
}