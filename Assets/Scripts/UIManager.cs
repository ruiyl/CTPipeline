using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
	public class UIManager : MonoBehaviour
	{
		[SerializeField] private GameManager gameManager;
		[SerializeField] private Camera planModeOverlayCamera;
		[SerializeField] private Canvas planModeOverlayCanvas;
		[SerializeField] private RectTransform planModeUI;

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
		}

		public void ExitPlanMode()
		{
			planModeOverlayCamera.gameObject.SetActive(false);
			planModeOverlayCanvas.gameObject.SetActive(false);
			planModeUI.gameObject.SetActive(false);
		}
	}
}