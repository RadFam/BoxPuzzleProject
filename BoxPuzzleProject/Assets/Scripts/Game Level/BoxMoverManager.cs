using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

		[SerializeField]
		FieldManager fieldManager;
		Stack<SaveData> savedSteps;

		public PlayerController currPlayer;
		GameObject currBox;
		GameObject currTarget;
		GameObject lastTarget;
		GameObject currTargetPR;
		GameObject lastTargetPR;
		int currScore;
        
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

			moveSpeed = 0.5f;
			walkDistance = 1;

			boxesPool = new Dictionary<GameObject, BoxController>();
			targetsPool = new Dictionary<GameObject, TargetController>();
			savedSteps = new Stack<SaveData>();

			currBox = null;
			currTarget = null;
			lastTarget = null;
			currTargetPR = null;
			lastTargetPR = null;
		}

		void Start()
		{
		}

		public void CorrectPosition(GameObject go)
		{
			go.transform.position = fieldManager.GetAdaptedCoordinates(go.transform.position);

		}

		public void AddBoxToPool(GameObject go, BoxController bc)
		{
			boxesPool.Add(go, bc);
		}

		public void AddTargetToPool(GameObject go, TargetController tc)
		{
			//CorrectPosition(go);
			targetsPool.Add(go, tc);

			LevelManager.inst.lvlMaxScore += 1;
		}

		public bool CanPlayerMoveFurther(Vector2Int dir, Vector3 coords, out int push)
		{
			Vector3 nextCoords = coords + new Vector3Int(dir.x, 0, dir.y) * walkDistance;
			//Debug.Log("Next coords: " + nextCoords + "  dictCount: " + boxesPool.Count);

			GameObject box = null;
			foreach (KeyValuePair<GameObject, BoxController> item in boxesPool)
			{
				//Debug.Log("box item: " + item);
				if (item.Key.transform.position.x == nextCoords.x && item.Key.transform.position.z == nextCoords.z)
				{
					box = item.Key;
					break;
				}
			}
			currBox = box;

			PreReachDestination(dir, coords);
			MakeSaveStep(); // (!!!!) HERE WE SAVE STEP DATA (!!!!)

			if (box != null)
			{
				bool check = CanBoxMoveFurther(dir, box.transform.position);
				if (check)
				{
					boxesPool[box].moveVector = dir;
					boxesPool[box].StartMove();
				}
				push = 1;
				return check;
			}

			push = 0;
			return fieldManager.IsPassible(nextCoords); 
		}

		public bool CanBoxMoveFurther(Vector2Int dir, Vector3 coords)
		{
			Vector3 nextCoords = coords + new Vector3Int(dir.x, 0, dir.y) * walkDistance;

			foreach (KeyValuePair<GameObject, BoxController> item in boxesPool)
			{
				if (item.Key.transform.position.x == nextCoords.x && item.Key.transform.position.z == nextCoords.z)
				{
					return false;
				}
			}

			return fieldManager.IsPassible(nextCoords);
		}

		void PreReachDestination(Vector2Int dir, Vector3 coords)
		{
			Vector3 nextCoords = coords + new Vector3Int(dir.x, 0, dir.y) * 2 * walkDistance;
			bool res =  fieldManager.IsTarget(nextCoords);
			//Debug.Log("PreReach res: " + res);
			lastTargetPR = currTargetPR;
            if (res)
            {
                foreach (KeyValuePair<GameObject, TargetController> item in targetsPool)
                {
                    if (item.Key.transform.position.x == nextCoords.x && item.Key.transform.position.z == nextCoords.z)
                    {
                        currTargetPR = item.Key;
                        break;
                    }
                }
            }
			else
			{
				currTargetPR = null;
			}
		}

		public bool ReachDestination(Vector3 coords)
		{
			bool res =  fieldManager.IsTarget(coords);

			lastTarget = currTarget;
            if (res)
            {
                foreach (KeyValuePair<GameObject, TargetController> item in targetsPool)
                {
                    if (item.Key.transform.position.x == coords.x && item.Key.transform.position.z == coords.z)
                    {
                        currTarget = item.Key;
                        break;
                    }
                }
            }
			else
			{
				currTarget = null;
			}

			return res;
		}
		public void GetTarget()
		{
			targetsPool[currTarget].OnReachTarget(); // need lastTarget(!)
			LevelManager.inst.ChangeLevelScore(1);
			currScore++;
		}

		public void LeaveTarget()
		{
			targetsPool[lastTarget].OnDeReachTarget();
			LevelManager.inst.ChangeLevelScore(-1);
			currScore--;
		}

		public void SwapTargets()
		{
			targetsPool[lastTarget].OnDeReachTarget();
			targetsPool[currTarget].OnReachTarget();
		}

		//
		//
		// ................................
		// WE NEED TO MAKE SOME INVESTIGATION ABOUNT BINDING REFERENCES OF OBJECTS WITH DICTIONARY(!!!!)
		public void MakeSaveStep()
		{
			//Debug.Log("SAVE STAGE");
			bool stat_1 = false;
			bool stat_2 = false;
			if (currBox != null)
			{
				//Debug.Log("Saved box: " + currBox.transform.position);
			}
			if (lastTargetPR != null)
			{
				stat_1 = targetsPool[lastTargetPR].reachStatus;
			}
			if (currTargetPR != null)
			{
				stat_2 = targetsPool[currTargetPR].reachStatus;
			}
			SaveData dat = new SaveData(currPlayer.transform.position, currBox, lastTargetPR, stat_1, currTargetPR, stat_2, LevelManager.inst.lvlScore);
			savedSteps.Push(dat);
		}

		public void RestorePrevoiusStep()
		{
			if (savedSteps.Count == 0)
			{
				return;
			}

			SaveData dat = savedSteps.Pop();

			currPlayer.transform.position = dat.player;
            if (dat.boxPos != null)
            {
                foreach (KeyValuePair<GameObject, Vector3> items in dat.boxPos)
                {
                    items.Key.transform.position = items.Value;
                }	
            }
			
			if (dat.oldTgt != null) // USE TARGET.SetStaus(bool newStat)
			{
				foreach (KeyValuePair<GameObject, bool> items in dat.oldTgt)
                {
                    if (targetsPool[items.Key].reachStatus && !items.Value)
					{
						targetsPool[items.Key].OnDeReachTarget();
					}
					if (!targetsPool[items.Key].reachStatus && items.Value)
					{
						targetsPool[items.Key].OnReachTarget();
					}
                }	
			}

			if (dat.newTgt != null)
			{
				foreach (KeyValuePair<GameObject, bool> items in dat.newTgt)
                {
                    if (targetsPool[items.Key].reachStatus && !items.Value)
					{
						targetsPool[items.Key].OnDeReachTarget();
					}
					if (!targetsPool[items.Key].reachStatus && items.Value)
					{
						targetsPool[items.Key].OnReachTarget();
					}
                }	
			}
			LevelManager.inst.ChangeLevelScore(dat.score - currScore);
			currScore = dat.score;
		}

		public Dictionary<string, object> FullSaveData(int sceneNum)
		{
			Dictionary<string, object> toSaveData = new Dictionary<string, object>();
			toSaveData.Add("SceneNum", (object)sceneNum);
			toSaveData.Add("SceneScore", (object)currScore);

			List<Dictionary<int, bool>> targetDat = new List<Dictionary<int, bool>>();
			foreach(KeyValuePair<GameObject, TargetController> item in targetsPool)
			{
				int num = item.Value.GetID();
				bool state = item.Value.reachStatus;
				targetDat.Add(new Dictionary<int, bool>{{num, state}});
			}
			toSaveData.Add("Targets", (object)targetDat);

			List<Dictionary<int, List<double>>> boxesDat = new List<Dictionary<int, List<double>>>();
			foreach(KeyValuePair<GameObject, BoxController> item in boxesPool)
			{
				int num = item.Value.GetID();
				List<double> lst = ToFullSaveData.SerializeVector(item.Value.transform.position);
				boxesDat.Add(new Dictionary<int, List<double>>{{num, lst}});
			}
			toSaveData.Add("Boxes", (object)boxesDat);

			toSaveData.Add("Player", (object)ToFullSaveData.SerializeVector(currPlayer.gameObject.transform.position));

			return toSaveData;
		}

		public void FullLoadData(Dictionary<string, object> toLoadData)
		{
			currScore = (int)toLoadData["SceneScore"];
			// Set score to LevelManager (!!!)

			currPlayer.gameObject.transform.position = ToFullSaveData.DeserializeVector((List<double>)toLoadData["Player"]);
			
			List<Dictionary<int, bool>> targetDat = (List<Dictionary<int, bool>>)toLoadData["Targets"];
			foreach (Dictionary<int, bool> item in targetDat)
			{
				foreach(KeyValuePair<int, bool> pair in item)
				{
					foreach (KeyValuePair<GameObject, TargetController> item2 in targetsPool)
					{
						if (item2.Value.GetID() == pair.Key)
						{
							targetsPool[item2.Key].SetStaus(pair.Value);
						}
					}
				}
			}

			List<Dictionary<int, List<double>>> boxesDat = (List<Dictionary<int, List<double>>>)toLoadData["Boxes"];
			foreach (Dictionary<int, List<double>> item in boxesDat)
			{
				foreach(KeyValuePair<int, List<double>> pair in item)
				{
					foreach (KeyValuePair<GameObject, BoxController> item2 in boxesPool)
					{
						if (item2.Value.GetID() == pair.Key)
						{
							targetsPool[item2.Key].transform.position = ToFullSaveData.DeserializeVector(pair.Value);
						}
					}
				}
			}
		}
    }

	class SaveData
	{

		public Vector3 player {get; private set;}
		
		public Dictionary<GameObject, Vector3> boxPos{get; private set;}
		public Dictionary<GameObject, bool> oldTgt{get; private set;}
		public Dictionary<GameObject, bool> newTgt{get; private set;}
		public int score {get; private set;}
		
		public SaveData(Vector3 pl, GameObject bx, GameObject tgt, bool val_1, GameObject tgt_n, bool val_2, int val)
		{
			boxPos = new Dictionary<GameObject, Vector3>();
			oldTgt = new Dictionary<GameObject, bool>();
			newTgt = new Dictionary<GameObject, bool>();
			
			player = pl;
			if (bx != null)
			{
				Vector3 pos = bx.transform.position;
				boxPos.Add(bx, pos);
			}
			else
			{
				boxPos = null;
			}
			
			if (tgt != null)
			{
				oldTgt.Add(tgt, val_1);
			}
			else
			{
				oldTgt = null;
			}

			if (tgt_n != null)
			{
				newTgt.Add(tgt_n, val_2);
			}
			else
			{
				newTgt = null;
			}

			score = val;
		}
	}

	class ToFullSaveData
	{
		public static List<double> SerializeVector(Vector3 vct)
		{
			List<double> list = new List<double>{0,0,0};
			list[0] = (double)vct.x;
			list[1] = (double)vct.y;
			list[2] = (double)vct.z;
			return list;
		}

		public static Vector3 DeserializeVector(List<double> lst)
		{
			Vector3 ans = new Vector3((float)lst[0], (float)lst[1], (float)lst[2]);
			return ans;
		}
	}
}