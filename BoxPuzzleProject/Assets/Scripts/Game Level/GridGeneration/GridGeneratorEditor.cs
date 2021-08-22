using UnityEngine;
using UnityEditor;
using GeneratorControls;

[CustomEditor(typeof(GridGeneratorPool))]
public class GridGeneratorEditor : Editor 
{
	GridGeneratorPool gridGen;
	
	void Awake()
	{
		gridGen = (GridGeneratorPool)target;
	}

	public override void OnInspectorGUI()
	{
		if (GUILayout.Button("Generate base"))
		{
			gridGen.GenerateBaseGrid();
		}

		base.OnInspectorGUI();
	}
}
