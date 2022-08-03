using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
	public class MainMenuUIManager : MonoBehaviour
	{
		[SerializeField] private GameObject p1CharDemoPanel;
		[SerializeField] private GameObject p2CharDemoPanel;
		[SerializeField] private Transform p1CharRoot;
		[SerializeField] private Transform p2CharRoot;
		[SerializeField] private GameObject[] tutorialClearedLabels; 

		private void Start()
		{
			bool[] clearedTutorialStates = GameSessionManager.Instance.GetClearedTutorial();
			for (int i = 0; i < clearedTutorialStates.Length; i++)
			{
				tutorialClearedLabels[i].SetActive(clearedTutorialStates[i]);
			}
			UpdateCharacterDemo();
		}

		public void OnSetNumPlayer(int n)
		{
			GameSessionManager.Instance.SetNumPlayer(n);
			UpdateCharacterDemo();
		}

		public void OnSetPlayerMode(int mode)
		{
			GameSessionManager.PlayMode playMode = (GameSessionManager.PlayMode)mode;
			GameSessionManager.Instance.SetPlayMode(playMode);
			if (playMode != GameSessionManager.PlayMode.Tutorial)
			{
				GameSessionManager.Instance.StartGame();
			}
		}

		public void OnSetTutorialLevel(int level)
		{
			switch (level)
			{
				case 0:
					GameSessionManager.Instance.SetTutorialLevel(TutorialManager.TutorialStep.Intro);
					break;
				case 1:
					GameSessionManager.Instance.SetTutorialLevel(TutorialManager.TutorialStep.LoopBlock);
					break;
			}
			GameSessionManager.Instance.StartGame();
		}

		public void OnP1ChangeChar(int direction)
		{
			GameSessionManager.Instance.OffsetCharacterID(1, direction);
			UpdateCharacterDemo();
		}

		public void OnP2ChangeChar(int direction)
		{
			GameSessionManager.Instance.OffsetCharacterID(2, direction);
			UpdateCharacterDemo();
		}

		public void UpdateCharacterDemo()
		{
			p2CharDemoPanel.SetActive(GameSessionManager.Instance.NumOfPlayer >= 2);
			for (int i = 0; i < p1CharRoot.childCount; i++)
			{
				p1CharRoot.GetChild(i).gameObject.SetActive(i == GameSessionManager.Instance.P1CharID);
				p2CharRoot.GetChild(i).gameObject.SetActive(i == GameSessionManager.Instance.P2CharID);
			}
		}

		public void OnQuitButton()
		{
			GameSessionManager.Instance.RequestQuit();
		}
	}
}