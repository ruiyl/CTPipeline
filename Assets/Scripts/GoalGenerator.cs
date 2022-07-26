using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
	public abstract class GoalGenerator
	{
		public abstract Goal[] GetNewGoals();
	}

	public class BasicGoalGenerator : GoalGenerator
	{
		private int step;

		public BasicGoalGenerator()
		{
			step = 0;
		}

		public override Goal[] GetNewGoals()
		{
			Goal[] goals;
			switch (step)
			{
				case 0:
					goals = new Goal[2] { new Goal(new ItemData(ItemData.ValueType.Letter, 1), 2), new Goal(new ItemData(ItemData.ValueType.Number, 2), 2) };
					break;
				case 1:
					goals = new Goal[2] { new Goal(new ItemData(new List<ItemData.ValueType>() { ItemData.ValueType.Letter, ItemData.ValueType.Number }, new List<int>() { 0, 0 }), 1),
					new Goal(new ItemData(new List<ItemData.ValueType>() { ItemData.ValueType.Letter, ItemData.ValueType.Number }, new List<int>() { 1, 1 }), 1)};
					break;
				case 2:
					goals = new Goal[2] { new Goal(new ItemData(new List<ItemData.ValueType>() { ItemData.ValueType.Letter, ItemData.ValueType.Number }, new List<int>() { 3, 1 }), 2),
					new Goal(new ItemData(new List<ItemData.ValueType>() { ItemData.ValueType.Letter, ItemData.ValueType.Number }, new List<int>() { 2, 3 }), 2)};
					break;
				default:
					goals = RandomGoals();
					break;
			}
			step++;
			return goals;
		}

		private Goal[] RandomGoals()
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
}