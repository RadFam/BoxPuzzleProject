using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="TargetObject", menuName="ScriptableObjects/GameResourses", order = 4)]
public class TargetObjects : ScriptableObject {

	public List<Mesh> targetMeshes = new List<Mesh>();
	public List<Material> targetTex = new List<Material>();
}
