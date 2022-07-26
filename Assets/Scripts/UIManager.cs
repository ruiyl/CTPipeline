using System.Collections;
using UnityEngine;
using TMPro;

namespace Assets.Scripts
{
	public class UIManager : MonoBehaviour
	{
		[SerializeField] private GameManager gameManager;
		[SerializeField] private Camera planModeOverlayCamera;
		[SerializeField] private Canvas planModeOverlayCanvas;
		[SerializeField] private RectTransform planModeUI;
		[SerializeField] private RectTransform goalHolder;
		[SerializeField] private TextMeshProUGUI scoreText;
		[SerializeField] private GameObject playModeOnlyUIRoot;
		[SerializeField] private GameObject planModeOnlyUIRoot;

		private const string SCORE_TEXT = "SCORE: {0}";

		public void SetPlanMode(bool isOn)
		{
			if (isOn)
			{
				gameManager.EnterPlanMode();
				EnterPlanMode();
			}
			else
			{
				gameManager.ExitPlanMode();
				ExitPlanMode();
			}
		}

		public void EnterPlanMode()
		{
			planModeOverlayCamera.gameObject.SetActive(true);
			planModeOverlayCanvas.gameObject.SetActive(true);
			planModeUI.gameObject.SetActive(true);
			planModeOnlyUIRoot.gameObject.SetActive(true);
			playModeOnlyUIRoot.gameObject.SetActive(false);
		}

		public void ExitPlanMode()
		{
			planModeOverlayCamera.gameObject.SetActive(false);
			planModeOverlayCanvas.gameObject.SetActive(false);
			planModeUI.gameObject.SetActive(false);
			planModeOnlyUIRoot.gameObject.SetActive(false);
			playModeOnlyUIRoot.gameObject.SetActive(true);
		}

		public void UpdateScore(int score)
		{
			scoreText.text = string.Format(SCORE_TEXT, score);
		}

		public void UpdateGoal(Goal[] goals)
		{
			int n = Mathf.Max(goalHolder.childCount, goals.Length);
			for (int i = 0; i < n; i++)
			{
				RectTransform goalSlot;
				if (i >= goals.Length)
				{
					goalHolder.GetChild(i).gameObject.SetActive(false);
					continue;
				}
				else if (i >= goalHolder.childCount)
				{
					goalSlot = Instantiate(goalHolder.GetChild(goalHolder.childCount - 1) as RectTransform, goalHolder);
				}
				else
				{
					goalSlot = goalHolder.GetChild(i) as RectTransform;
				}
				goalSlot.gameObject.SetActive(true);
				var text = goalSlot.GetChild(1).GetComponent<TextMeshProUGUI>();
				text.text = $"'{goals[i].data}' * {goals[i].amount}";
			}
		}
	}
}