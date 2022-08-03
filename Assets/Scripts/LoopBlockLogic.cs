using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts
{
	public class LoopBlockLogic : BlockLogic<LoopBlockMono>
	{
		private int loopNumberIndex;

		private static Dictionary<ItemMono, List<int>> itemLoopTracker = new Dictionary<ItemMono, List<int>>();
		private static List<int> availableLoopNumbers = new List<int>() { 2, 3 };

		public LoopBlockLogic(LoopBlockMono blockMono) : base(blockMono)
		{
			monoRef.InGate.OpenEvent += ReceiveItem;
			monoRef.LoopInGate.OpenEvent += OnItemLoop;
		}

		private void ReceiveItem(ItemMono item)
		{
			TakeItemIn(item, monoRef.InGate);
			if (!itemLoopTracker.ContainsKey(item))
			{
				itemLoopTracker[item] = new List<int>();
				
			}
			itemLoopTracker[item].Add(1);
			item.SetLoopValue(itemLoopTracker[item].ToArray());
			PopItem(item, monoRef.LoopOutGate, monoRef.LoopOutGate.GetConnectedPath());
		}

		private void OnItemLoop(ItemMono item)
		{
			TakeItemIn(item, monoRef.LoopInGate);
			if (!itemLoopTracker.ContainsKey(item))
			{
				Debug.LogError("Item not in tracker");
				return;
			}
			List<int> itemRecord = itemLoopTracker[item];
			if (itemRecord[itemRecord.Count - 1] == availableLoopNumbers[loopNumberIndex])
			{
				itemRecord.RemoveAt(itemRecord.Count - 1);
				item.SetLoopValue(itemRecord.ToArray());
				PopItem(item, monoRef.OutGate, monoRef.OutGate.GetConnectedPath());
			}
			else
			{
				itemRecord[itemRecord.Count - 1]++;
				item.SetLoopValue(itemRecord.ToArray());
				PopItem(item, monoRef.LoopOutGate, monoRef.LoopOutGate.GetConnectedPath());
			}
		}

		public override void OnClicked(PointerEventData eventData)
		{
			base.OnClicked(eventData);
			if (GameManager.IsInPlanMode)
			{
				loopNumberIndex = (loopNumberIndex + 1) % availableLoopNumbers.Count;
				monoRef.SetLoopCount(availableLoopNumbers[loopNumberIndex]);
			}
		}
	}
}