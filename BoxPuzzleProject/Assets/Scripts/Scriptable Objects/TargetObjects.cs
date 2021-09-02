using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="TargetObject", menuName="ScriptableObjects/FieldObjects/Target", order = 3)]
public class TargetObjects : ScriptableObject {

	public List<Mesh> targetMeshes = new List<Mesh>();
	public List<Material> targetTex = new List<Material>();
	public List<Material> targetTexR = new List<Material>();
	public List<Mesh> starMeshes = new List<Mesh>();
	public List<Material> starTex = new List<Material>();
}
