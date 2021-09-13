using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PlayControls
{
    public class AddBackStepEffect : MonoBehaviour
    {
		[SerializeField]
		Text addStepText;
		[SerializeField]
		ParticleSystem starExplode;
        // Use this for initialization
        public void StartAddbackStepEffects()
		{
			addStepText.gameObject.SetActive(true);
			starExplode.gameObject.SetActive(true);
			StartCoroutine(StageOne());
		}

		IEnumerator StageOne()
		{
			Color texC = addStepText.color;
			float yCrd = 0.0f;
			float alpha = 1.0f;
			float upInitSpeed = 1.0f;

			starExplode.Play();

			while (alpha > 0.0f)
			{
				addStepText.color = new Color(texC.r, texC.g, texC.b, alpha);
				addStepText.transform.localPosition = new Vector3(0.0f, yCrd, 0.0f);

				yCrd += 1.5f * upInitSpeed;

				alpha -= 0.05f;
				yield return new WaitForEndOfFrame();
			}

			addStepText.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);

			addStepText.gameObject.SetActive(false);
			starExplode.gameObject.SetActive(false);
			yield return null;
		}
    }
}