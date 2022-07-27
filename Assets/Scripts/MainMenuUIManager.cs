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

		private void Start()
		{
			UpdateCharacterDemo();
		}

		public void OnSetNumPlayer(int n)
		{
			GameSessionManager.Instance.SetNumPlayer(n);
			UpdateCharacterDemo();
		}

		public void OnSetPlayerMode(int mode)
		{
			GameSessionManager.Instance.SetPlayMode((GameSessionManager.PlayMode)mode);
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
	}
}