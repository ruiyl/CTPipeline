using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
	public abstract class GoalGenerator
	{
		public abstract Goal[] GetNewGoals();
	}

	public class ArcadeGoalGenerator : GoalGenerator
	{
		public override Goal[] GetNewGoals()
		{
			int goalCount = Random.Range(1, 4);
			Goal[] goals = new Goal[goalCount];
			for (int i = 0; i < goalCount; i++)
			{
				int length = Random.Range(1, 4);
				List<ItemData.ValueType> types = new List<ItemData.ValueType>();
				List<int> indices = new List<int>();
				for (int j = 0; j < length; j++)
				{
					types.Add((ItemData.ValueType)Random.Range(0, 2));
					indices.Add(Random.Range(0, 5));
				}
				ItemData goalData = new ItemData(types, indices);
				goals[i] = new Goal(goalData, Random.Range(1, 4));
			}
			return goals;
		}
	}

	public class TutorialGoalGenerator : GoalGenerator
	{
		private TutorialManager.TutorialStep step;

		public void SetStep(TutorialManager.TutorialStep value)
		{
			step = value;
		}

		public override Goal[] GetNewGoals()
		{
			Goal[] goals = null;
			switch (step)
			{
				case TutorialManager.TutorialStep.Control:
					goals = new Goal[1] { new Goal(new ItemData(ItemData.ValueType.Letter, 0), 2) };
					break;
				case TutorialManager.TutorialStep.PlanMode:
					goals = new Goal[2] { new Goal(new ItemData(ItemData.ValueType.Letter, 1), 2), new Goal(new ItemData(ItemData.ValueType.Number, 2), 1) };
					break;
				case TutorialManager.TutorialStep.MergeBlock:
					goals = new Goal[2] { new Goal(new ItemData(new List<ItemData.ValueType>() { ItemData.ValueType.Letter, ItemData.ValueType.Number }, new List<int>() { 0, 0 }), 1),
					new Goal(new ItemData(new List<ItemData.ValueType>() { ItemData.ValueType.Letter, ItemData.ValueType.Number }, new List<int>() { 1, 1 }), 1)};
					break;
				case TutorialManager.TutorialStep.AdvanceBlock:
					goals = new Goal[2] { new Goal(new ItemData(new List<ItemData.ValueType>() { ItemData.ValueType.Letter, ItemData.ValueType.Number }, new List<int>() { 3, 1 }), 2),
					new Goal(new ItemData(new List<ItemData.ValueType>() { ItemData.ValueType.Letter, ItemData.ValueType.Number }, new List<int>() { 2, 3 }), 2)};
					break;
			}
			step++;
			return goals;
		}
	}
}