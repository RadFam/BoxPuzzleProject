using UnityEngine;

[CreateAssetMenu(fileName="DefaultSettings", menuName="ScriptableObjects/GameResourses/Settings", order = 1)]
public class DefaultSettings : ScriptableObject {

	public int defaultScore;
	public int defaultLevel;
	public float defaultMusic;
	public float defaultEffects;
}
