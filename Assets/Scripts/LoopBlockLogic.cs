using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
	public class LoopBlockLogic : BlockLogic<LoopBlockMono>
	{
		private int loopCount;

		private static Dictionary<ItemMono, List<int>> itemLoopTracker = new Dictionary<ItemMono, List<int>>();

		public LoopBlockLogic(LoopBlockMono blockMono) : base(blockMono)
		{
			monoRef.InGate.OpenEvent += ReceiveItem;
			monoRef.LoopInGate.OpenEvent += OnItemLoop;
		}

		public void SetLoopCount(int value)
		{
			loopCount = value;
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
			PopItem(item, monoRef.LoopOutGate, monoRef.LoopOutGate.GetOutPath());
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
			if (itemRecord[itemRecord.Count - 1] == loopCount)
			{
				itemRecord.RemoveAt(itemRecord.Count - 1);
				item.SetLoopValue(itemRecord.ToArray());
				PopItem(item, monoRef.OutGate, monoRef.OutGate.GetOutPath());
			}
			else
			{
				itemRecord[itemRecord.Count - 1]++;
				item.SetLoopValue(itemRecord.ToArray());
				PopItem(item, monoRef.LoopOutGate, monoRef.LoopOutGate.GetOutPath());
			}
		}
	}
}