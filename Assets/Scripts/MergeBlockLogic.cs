using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
	public class MergeBlockLogic : BlockLogic<MergeBlockMono>
	{
		private Queue<ItemMono> leftGateQueue;
		private Queue<ItemMono> rightGateQueue;

		public MergeBlockLogic(MergeBlockMono blockMono) : base(blockMono)
		{
			leftGateQueue = new Queue<ItemMono>();
			rightGateQueue = new Queue<ItemMono>();

			monoRef.LeftInGate.OpenEvent += ReceiveLeftItem;
			monoRef.RightInGate.OpenEvent += ReceiveRightItem;
		}

		public override void Update()
		{
			base.Update();
			if (leftGateQueue.Count > 0 && rightGateQueue.Count > 0)
			{
				ItemMono leftItem = leftGateQueue.Dequeue();
				ItemMono rightItem = rightGateQueue.Dequeue();
				TakeItemIn(leftItem, monoRef.LeftInGate);
				TakeItemIn(rightItem, monoRef.RightInGate);
				ItemMono newItem = ItemMono.MergeItem(leftItem, rightItem);
				PopItem(newItem, monoRef.OutGate, monoRef.OutGate.GetConnectedPath());
			}
		}

		private void ReceiveLeftItem(ItemMono item)
		{
			leftGateQueue.Enqueue(item);
		}

		private void ReceiveRightItem(ItemMono item)
		{
			rightGateQueue.Enqueue(item);
		}

		public override void PreDestroyBlock()
		{
			base.PreDestroyBlock();
			while (leftGateQueue.Count > 0)
			{
				leftGateQueue.Dequeue().Destroy();
			}
			while (rightGateQueue.Count > 0)
			{
				rightGateQueue.Dequeue().Destroy();
			}
		}
	}
}