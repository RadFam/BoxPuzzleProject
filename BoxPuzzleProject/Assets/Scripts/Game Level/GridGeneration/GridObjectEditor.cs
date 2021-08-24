using UnityEngine;
using UnityEditor;
using GeneratorControls;

[CustomEditor(typeof(GridObject))]
public class GridObjectEditor : Editor {

	GridObject gridObject;
	void Awake () 
	{
		gridObject = (GridObject)target;
	}
	
	// Update is called once per frame
	public override void OnInspectorGUI() 
	{
		if (GUILayout.Button("Generate object"))
		{
			gridObject.MakeChoose();
		}

		base.OnInspectorGUI();	
	}
}
