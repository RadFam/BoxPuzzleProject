using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="BoxObject", menuName="ScriptableObjects/FieldObjects/Box", order = 1)]
public class BoxObjects : ScriptableObject {

	public List<Mesh> boxMeshes = new List<Mesh>();
	public List<Material> boxTex = new List<Material>();
}
