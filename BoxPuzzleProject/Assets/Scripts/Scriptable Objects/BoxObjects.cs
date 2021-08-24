using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="BoxObject", menuName="ScriptableObjects/GameResourses", order = 3)]
public class BoxObjects : ScriptableObject {

	public List<Mesh> boxMeshes = new List<Mesh>();
	public List<Material> boxTex = new List<Material>();
}
