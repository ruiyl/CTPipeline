using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

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
		[SerializeField] private GameObject p2Control;
		[SerializeField] private Toggle planModeToggle;
		[SerializeField] private GameObject[] blockIconLocks;

		private const string SCORE_TEXT = "SCORE: {0}";
		private const string GOAL_TEXT = "{0} PIECES OF\n'{1}'";

		public void DisableP2()
		{
			p2Control.SetActive(false);
		}

		public void SetPlanModeButton(bool enable, bool interactable)
		{
			planModeToggle.gameObject.SetActive(enable);
			planModeToggle.interactable = interactable;
		}

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

		public void SetBlockIconLockState(bool isLocked, int index = -1)
		{
			if (index < 0)
			{
				for (int i = 0; i < blockIconLocks.Length; i++)
				{
					blockIconLocks[i].SetActive(isLocked);
				}
			}
			else
			{
				blockIconLocks[index].SetActive(isLocked);
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
				text.text = string.Format(GOAL_TEXT, goals[i].amount, goals[i].data);
			}
		}
	}
}