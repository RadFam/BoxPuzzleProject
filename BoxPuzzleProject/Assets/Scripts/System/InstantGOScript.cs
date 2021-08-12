using UnityEngine;

namespace GameControls
{
    public class InstantGOScript : MonoBehaviour
    {
        void Awake()
        {
			DontDestroyOnLoad(this.gameObject);
        }
    }
}