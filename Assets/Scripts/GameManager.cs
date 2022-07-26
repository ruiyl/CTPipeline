using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
	public class GameManager : MonoBehaviour
	{
		[SerializeField] private List<Goal> goals;
		[SerializeField] private UIManager uiManager;

		private GoalGenerator goalGenerator;
		private int score;

		private static bool isInPlanMode;

		public static bool IsInPlanMode { get => isInPlanMode; }

		public const int PENALTY = -5;

		private void Awake()
		{
			goals = new List<Goal>();
			goalGenerator = new BasicGoalGenerator();
		}

		public void StartGate()
		{
			goals.AddRange(goalGenerator.GetNewGoals());
			uiManager.UpdateGoal(goals.ToArray());
		}

		public void SubmitItem(ItemMono item)
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
				goals.AddRange(goalGenerator.GetNewGoals());
				uiManager.UpdateGoal(goals.ToArray());
			}
		}

		private void AddScore(int addingScore)
		{
			score += addingScore;
			uiManager.UpdateScore(score);
		}

		public void EnterPlanMode()
		{
			isInPlanMode = true;
		}

		public void ExitPlanMode()
		{
			isInPlanMode = false;
		}

		public void QuitGame()
		{
			Application.Quit();
		}
	}

	public class Goal
	{
		public ItemData data;
		public int amount;

		public Goal(ItemData data, int amount)
		{
			this.data = data;
			this.amount = amount;
		}
	}
}