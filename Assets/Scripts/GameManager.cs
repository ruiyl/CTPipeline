using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts
{
	public class GameManager : MonoBehaviour
	{
		[SerializeField] private UIManager uiManager;
		[SerializeField] private GameObject p1Go;
		[SerializeField] private GameObject p2Go;
		[SerializeField] private Mesh[] characterMeshList;
		[SerializeField] private TutorialManager tutorialManager;

		private GoalKeeper goalKeeper;

		public UnityAction<bool> PlanModeTrigger;

		private static bool isInPlanMode;

		public static bool IsInPlanMode { get => isInPlanMode; }		

		private void Awake()
		{
			switch (GameSessionManager.Instance.SelectedMode)
			{
				case GameSessionManager.PlayMode.Tutorial:
					goalKeeper = new TutorialGoalKeeper(uiManager, this);
					tutorialManager.SetTutorialMode(goalKeeper as TutorialGoalKeeper);
					break;
				case GameSessionManager.PlayMode.Arcade:
					goalKeeper = new ArcadeGoalKeeper(uiManager, this);
					break;
			}
			LoadPlayers();
		}

		private void LoadPlayers()
		{
			p1Go.GetComponent<MeshFilter>().sharedMesh = characterMeshList[GameSessionManager.Instance.P1CharID];
			if (GameSessionManager.Instance.NumOfPlayer == 2)
			{
				p2Go.GetComponent<MeshFilter>().sharedMesh = characterMeshList[GameSessionManager.Instance.P2CharID];
			}
			else
			{
				p2Go.SetActive(false);
				uiManager.DisableP2();
			}
		}

		public void StartGame()
		{

		}

		public void SubmitItem(ItemMono item)
		{
			goalKeeper.SubmitItem(item);
		}

		public void EnterPlanMode()
		{
			isInPlanMode = true;

			PlanModeTrigger?.Invoke(true);
		}

		public void ExitPlanMode()
		{
			isInPlanMode = false;

			PlanModeTrigger?.Invoke(false);
		}
	}

	public class Goal
	{
		public ItemData data;
		public int amount;

		public Goal(ItemData data, int amount)
		{
			if (amount <= 0)
			{
				throw new System.ArgumentOutOfRangeException("amount should be more than zero.");
			}
			this.data = data;
			this.amount = amount;
		}
	}

	public abstract class GoalKeeper
	{
		protected List<Goal> goals;
		protected GoalGenerator goalGenerator;
		protected UIManager uiManager;

		protected int score;

		public UnityAction<int> OnScoreAdded;
		public UnityAction OnAllGoalsComplete;

		protected const int PENALTY = -5;

		public GoalKeeper(UIManager uiManager, GameManager gameManager)
		{
			goals = new List<Goal>();
			this.uiManager = uiManager;
		}

		public void AddScore(int addingScore)
		{
			score += addingScore;
			uiManager.UpdateScore(score);

			OnScoreAdded?.Invoke(addingScore);
		}

		public virtual void SubmitItem(ItemMono item)
		{
			int correctGoalIndex = -1;
			for (int i = 0; i < goals.Count; i++)
			{
				if (item.ValueEqual(goals[i].data))
				{
					correctGoalIndex = i;
					break;
				}
			}
			if (correctGoalIndex >= 0)
			{
				AddScore(goals[correctGoalIndex].data.GetScore());
				goals[correctGoalIndex].amount--;
				if (goals[correctGoalIndex].amount <= 0)
				{
					goals.RemoveAt(correctGoalIndex);
				}
				uiManager.UpdateGoal(goals.ToArray());
			}
			else
			{
				AddScore(PENALTY);
			}
			if (goals.Count == 0)
			{
				OnAllGoalsComplete?.Invoke();
			}
		}

		public void GetNewGoals()
		{
			goals.AddRange(goalGenerator.GetNewGoals());
			uiManager.UpdateGoal(goals.ToArray());
		}
	}

	public class ArcadeGoalKeeper : GoalKeeper
	{
		public ArcadeGoalKeeper(UIManager uiManager, GameManager gameManager) : base(uiManager, gameManager)
		{
			goalGenerator = new ArcadeGoalGenerator();
		}

		public override void SubmitItem(ItemMono item)
		{
			base.SubmitItem(item);
			if (goals.Count == 0)
			{
				GetNewGoals();
			}
		}
	}

	public class TutorialGoalKeeper : GoalKeeper
	{
		public TutorialGoalGenerator GoalGenerator { get => goalGenerator as TutorialGoalGenerator; }

		public TutorialGoalKeeper(UIManager uiManager, GameManager gameManager) : base(uiManager, gameManager)
		{
			goalGenerator = new TutorialGoalGenerator();
		}

		public void SetGeneratorStep(TutorialManager.TutorialStep step)
		{
			(goalGenerator as TutorialGoalGenerator).SetStep(step);
		}
	}
}