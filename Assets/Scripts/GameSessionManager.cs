﻿using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts
{
	public class GameSessionManager : MonoBehaviour
	{
		[SerializeField] private Image fadeOverlay;
		[SerializeField] private GameObject confirmOverlay;
		[SerializeField] private GameObject quitPanel;
		[SerializeField] private GameObject returnPanel;

		private bool initialised;

		private int numOfPlayer;
		private int p1CharID;
		private int p2CharID;

		private PlayMode selectedMode;
		private TutorialManager.TutorialStep tutorialStep;

		public int NumOfPlayer { get => numOfPlayer; }
		public int P1CharID { get => p1CharID; }
		public int P2CharID { get => p2CharID; }
		public PlayMode SelectedMode { get => selectedMode; }
		public TutorialManager.TutorialStep TutorialStep { get => tutorialStep; }

		private static GameSessionManager instance;

		public const int CHAR_NUMBER = 4;

		private const float FADE_DURATION = 0.25f;

		public enum PlayMode
		{
			Undefined = 0,
			Tutorial = 1,
			Arcade = 2,
		}

		private enum SceneIndex
		{
			MainMenu = 0,
			Game = 1,
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
				Destroy(gameObject);
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
			tutorialStep = TutorialManager.TutorialStep.Undefined;
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

		public void SetTutorialLevel(TutorialManager.TutorialStep step)
		{
			tutorialStep = step;
		}

		public void SaveClearedTutorial(int value)
		{
			PlayerPrefs.SetInt($"ClearedTutorial{value}", 1);
		}

		public bool[] GetClearedTutorial()
		{
			bool[] clearedStates = new bool[2];
			for (int i = 0; i < clearedStates.Length; i++)
			{
				string key = $"ClearedTutorial{i}";
				clearedStates[i] = PlayerPrefs.HasKey(key) && (PlayerPrefs.GetInt(key) == 1);
			}
			return clearedStates;
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
			fadeOverlay.gameObject.SetActive(true);
			StartCoroutine(FadeTask(true, () => LoadScene((int)SceneIndex.Game)));
		}

		public void ReturnToMainMenu()
		{
			selectedMode = PlayMode.Undefined;
			tutorialStep = TutorialManager.TutorialStep.Undefined;
			fadeOverlay.gameObject.SetActive(true);
			StartCoroutine(FadeTask(true, () => LoadScene((int)SceneIndex.MainMenu)));
		}

		private IEnumerator FadeTask(bool fadeOut, UnityAction onFinishFade)
		{
			Color curCol = fadeOverlay.color;
			fadeOverlay.color = new Color(curCol.r, curCol.g, curCol.b, fadeOut ? 0f : 1f);
			Color tarCol = new Color(curCol.r, curCol.g, curCol.b, fadeOut ? 1f : 0f);
			curCol = fadeOverlay.color;
			float t = 0f;
			while (t < FADE_DURATION)
			{
				yield return null;
				t += Time.deltaTime;
				fadeOverlay.color = Color.Lerp(curCol, tarCol, t / FADE_DURATION);
			}
			onFinishFade?.Invoke();
		}

		private void LoadScene(int sceneIndex)
		{
			AsyncOperation loadOp = SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Single);
			loadOp.completed += EnterScene;
		}

		private void EnterScene(AsyncOperation loadOp)
		{
			StartCoroutine(FadeTask(false, () => fadeOverlay.gameObject.SetActive(false)));
		}

		public void RequestQuit()
		{
			confirmOverlay.SetActive(true);
			quitPanel.SetActive(true);
		}

		public void RequestReturn()
		{
			confirmOverlay.SetActive(true);
			returnPanel.SetActive(true);
		}

		public void QuitGame()
		{
			Application.Quit();
		}
	}
}