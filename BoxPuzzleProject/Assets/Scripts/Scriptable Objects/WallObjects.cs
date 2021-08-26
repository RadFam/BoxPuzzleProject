using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="WallObject", menuName="ScriptableObjects/FieldObjects/Wall", order = 2)]
public class WallObjects : ScriptableObject 
{
	public List<Mesh> wallMeshes = new List<Mesh>();
	public List<Material> wallTex = new List<Material>();
}
