using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
	public class GameSessionManager : MonoBehaviour
	{
		private bool initialised;

		private static GameSessionManager instance;

		private int numOfPlayer;
		private int p1CharID;
		private int p2CharID;

		private PlayMode selectedMode;

		public int NumOfPlayer { get => numOfPlayer; }
		public int P1CharID { get => p1CharID; }
		public int P2CharID { get => p2CharID; }

		public const int CHAR_NUMBER = 4;

		public enum PlayMode
		{
			Undefined = 0,
			Tutorial = 1,
			Arcade = 2,
		}

		public static GameSessionManager Instance
		{
			get
			{
				if (instance == null)
				{
					instance = FindObjectOfType<GameSessionManager>();
					if (instance == null)
					{
						instance = (new GameObject("GameSessionManager")).AddComponent<GameSessionManager>();
						DontDestroyOnLoad(instance.gameObject);
					}
				}
				if (!instance.initialised)
				{
					instance.Initialise();
				}
				return instance;
			}
		}

		private void Awake()
		{
			if (instance == null)
			{
				instance = this;
				DontDestroyOnLoad(gameObject);
			}
			else if (instance != this)
			{
				Destroy(instance);
				return;
			}
			if (!initialised)
			{
				Initialise();
			}
		}

		private void Initialise()
		{
			initialised = true;

			selectedMode = PlayMode.Undefined;
			numOfPlayer = 1;
			p1CharID = 0;
			p2CharID = 1;
		}

		public void SetNumPlayer(int n)
		{
			numOfPlayer = n;
		}

		public void SetPlayMode(PlayMode mode)
		{
			selectedMode = mode;
		}

		public void OffsetCharacterID(int playerID, int offset)
		{
			int index = 0;
			switch (playerID)
			{
				case 1:
					index = p1CharID;
					break;
				case 2:
					index = p2CharID;
					break;
			}
			index += offset;
			index %= CHAR_NUMBER;
			if (index < 0)
			{
				index += CHAR_NUMBER;
			}
			switch (playerID)
			{
				case 1:
					p1CharID = index;
					break;
				case 2:
					p2CharID = index;
					break;
			}
		}

		public void StartGame()
		{

		}

		public void ReturnToMainMenu()
		{

		}

		public void QuitGame()
		{
			Application.Quit();
		}
	}
}