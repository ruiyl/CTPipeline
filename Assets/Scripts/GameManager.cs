using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
	public class GameManager : MonoBehaviour
	{
		[SerializeField] private List<ItemData> goalItemDatas;

		private int score;

		private static bool isInPlanMode;

		public static bool IsInPlanMode { get => isInPlanMode; }

		private void Awake()
		{
			goalItemDatas = new List<ItemData>();
		}

		public void SubmitItem(ItemMono item)
		{
			bool correct = false;
			ItemData correctGoal = null;
			foreach (ItemData itemData in goalItemDatas)
			{
				if (item.ValueEqual(itemData))
				{
					correct = true;
					correctGoal = itemData;
					break;
				}
			}
			if (correct)
			{
				AddScore(GetScore(correctGoal));
			}
		}

		private void AddScore(int addingScore)
		{
			score += addingScore;
		}

		private int GetScore(ItemData goal)
		{
			return 0;
		}

		public void EnterPlanMode()
		{
			isInPlanMode = true;
		}

		public void ExitPlanMode()
		{
			isInPlanMode = false;
		}
	}
}