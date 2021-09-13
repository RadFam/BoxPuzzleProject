using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PlayControls
{
    public class EndLevelEffect : MonoBehaviour
    {

		[SerializeField]
		GameObject winText;
		[SerializeField]
		GameObject bonusText;
		[SerializeField]
		ParticleSystem starExplode;

		bool bonusTextShow;
        
		public void StartWinEffect(bool bonus=false)
		{
			winText.gameObject.SetActive(true);
			starExplode.gameObject.SetActive(true);
			bonusTextShow = bonus;
			StartCoroutine(StageOne());
		}
		
		IEnumerator StageOne()
		{
			starExplode.Play();
			float delta = 0.8f;
			while (delta < 1.2f)
			{
				winText.transform.localScale = new Vector3(delta, delta, 1.0f);
				delta += 0.05f;
				yield return new WaitForEndOfFrame();
			}

			yield return StageTwo();

			starExplode.gameObject.SetActive(false);
			winText.transform.localScale = new Vector3(0.8f, 0.8f, 1.0f);
			winText.gameObject.SetActive(false);
			LevelManager.inst.EndPlayScene();
		}

		IEnumerator StageTwo()
		{
			if (bonusTextShow)
			{
				bonusText.gameObject.SetActive(true);
				string message = "Уровень пройден за " + LevelManager.inst.fixedTime.ToString() + " секунд\nВы получаете " + LevelManager.inst.bonusSteps.ToString() + " дополнительных шага назад";
				bonusText.GetComponent<Text>().text = message;
			}

			Vector3 intScale = new Vector3(1.2f, 1.2f, 1.2f);
			Vector3 tgtScale = new Vector3(1.0f, 1.0f, 1.0f);
			Quaternion startRot = winText.transform.rotation;
			Quaternion firstRot = Quaternion.Euler(new Vector3(0, 0, 15));
			Quaternion secondRot = Quaternion.Euler(new Vector3(0, 0, -15));
			float time = 1.0f;
			float elapcedTime = 0.0f;
			while (elapcedTime < time)
			{
				elapcedTime += Time.deltaTime;
				winText.transform.localScale = Vector3.Slerp(intScale, tgtScale, elapcedTime / time);
				winText.transform.rotation = Quaternion.Slerp(startRot, firstRot, elapcedTime / time);
				yield return new WaitForEndOfFrame();
			}
			time*=2;
			elapcedTime = 0.0f;
			while (elapcedTime < time)
			{
				elapcedTime += Time.deltaTime;
				winText.transform.rotation = Quaternion.Slerp(firstRot, secondRot, elapcedTime / time);
				yield return new WaitForEndOfFrame();
			}
			time/=2;
			elapcedTime = 0.0f;
			while (elapcedTime < time)
			{
				elapcedTime += Time.deltaTime;
				winText.transform.rotation = Quaternion.Slerp(secondRot, startRot, elapcedTime / time);
				yield return new WaitForEndOfFrame();
			}

			yield return new WaitForEndOfFrame();
		}
    }
}