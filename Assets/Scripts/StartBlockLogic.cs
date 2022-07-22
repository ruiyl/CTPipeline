using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
	public class StartBlockLogic : BlockLogic<StartBlockMono>
	{
		public StartBlockLogic(StartBlockMono blockMono) : base(blockMono)
		{

		}

		public void ReceiveItem(ItemMono item)
		{
			currentItem = item;
			PopItem(monoRef.OutGate, monoRef.OutGate.GetOutPath());
		}

		public void OnInteractedByPlayer(CharacterMono player)
		{
			ReceiveItem(player.PopItem());
		}
	}
}