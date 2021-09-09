using UnityEngine;

[CreateAssetMenu(fileName="SoundResources", menuName="ScriptableObjects/GameResourses/Sounds", order = 2)]
public class SoundResources : ScriptableObject 
{
	public AudioClip menuMusic;
    public AudioClip gameMusic;
    
    public AudioClip winEffect;
    public AudioClip stompEffect;
    public AudioClip dragEffect;
}
