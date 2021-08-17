using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayControls
{
    public class BoxMoverManager : MonoBehaviour
    {
		public static BoxMoverManager inst;

		public int walkDistance {get; private set;}
		public float moveSpeed {get; private set;}

		public Dictionary<GameObject, BoxController> boxesPool;
		public Dictionary<GameObject, TargetController> targetsPool;

		FieldManager fieldManager;
        
        void Awake()
		{
			if (inst == null)
			{
				inst = this;
			}
			else
			{
				Destroy(this.gameObject);
			}

			moveSpeed = 4.0f;
			walkDistance = 1;

			boxesPool = new Dictionary<GameObject, BoxController>();
			targetsPool = new Dictionary<GameObject, TargetController>();
		}

		void Start()
		{
			fieldManager = GetComponent<FieldManager>();
		}

		public void CorrectPosition(ref GameObject go)
		{

		}

		public void AddBoxToPool(GameObject go, BoxController bc)
		{
			CorrectPosition(ref go);
			boxesPool.Add(go, bc);
		}

		public void AddTargetToPool(GameObject go, TargetController tc)
		{
			CorrectPosition(ref go);
			targetsPool.Add(go, tc);

			LevelManager.inst.lvlMaxScore += 1;
		}

		public bool CanPlayerMoveFurther(Vector2Int dir, Vector3 coords)
		{
			return true;
		}

		public bool CanBoxMoveFurther(Vector2Int dir, Vector3 coords)
		{
			return true;
		}

		public void GetTarget()
		{
			LevelManager.inst.ChangeLevelScore(1);
		}

		public void LeaveTarget()
		{
			LevelManager.inst.ChangeLevelScore(-1);
		}
    }

	class SaveData
	{
		public Vector3 player {get; private set;}
		public GameObject box {get; private set;}
		public GameObject target {get; private set;}
		public int score {get; private set;}
		public SaveData(Vector3 pl, GameObject bx, GameObject tgt, int val)
		{
			player = pl;
			box = bx;
			target = tgt;
			score = val;
		}
	}
}