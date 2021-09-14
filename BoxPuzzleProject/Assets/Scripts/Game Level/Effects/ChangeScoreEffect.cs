using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PlayControls
{
    public class ChangeScoreEffect : MonoBehaviour
    {
		[SerializeField]
		Text changeScore;
		[SerializeField]
		float alphaSpeed;
		[SerializeField]
		float moveSpeed;
        // Use this for initialization
        public void SetEffect(int changeValue)
		{
			changeScore.gameObject.SetActive(true);
			changeScore.gameObject.transform.localPosition = new Vector3(100, 0, 0);
			changeScore.color = new Color(1, 1, 1, 1);
			if (changeValue > 0)
			{
				changeScore.text = "+" + changeValue.ToString();
				StartCoroutine(UpMove());
			}
			if (changeValue < 0)
			{
				changeScore.text = changeValue.ToString();
				StartCoroutine(DownMove());
			}
		}

		IEnumerator UpMove()
		{
			float alpha = 1.0f;
			float yPos = 0.0f;
			while (alpha > 0)
			{
				alpha -= alphaSpeed;
				yPos += alphaSpeed;
				changeScore.gameObject.transform.localPosition = new Vector3(100.0f, yPos, 0.0f);
				changeScore.color = new Color(1.0f, 1.0f, 1.0f, alpha);
				yield return new WaitForEndOfFrame();
			}
			changeScore.gameObject.transform.localPosition = new Vector3(100.0f, 0, 0.0f);
			changeScore.gameObject.SetActive(false);
			yield return null;
		}

		IEnumerator DownMove()
		{
			float alpha = 1.0f;
			float yPos = 0.0f;
			while (alpha > 0)
			{
				alpha -= alphaSpeed;
				yPos -= alphaSpeed;
				changeScore.gameObject.transform.localPosition = new Vector3(100.0f, yPos, 0.0f);
				changeScore.color = new Color(1.0f, 1.0f, 1.0f, alpha);
				yield return new WaitForEndOfFrame();
			}
			changeScore.gameObject.transform.localPosition = new Vector3(100.0f, 0, 0.0f);
			changeScore.gameObject.SetActive(false);
			yield return null;
		}
    }
}