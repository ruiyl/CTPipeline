using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
	public class StartBlockLogic : BlockLogic<StartBlockMono>
	{
		public StartBlockLogic(StartBlockMono blockMono) : base(blockMono)
		{

		}

		public void OnReceiveItem(ItemMono item)
		{
			currentItem = item;
			PopItem(monoRef.OutGate, monoRef.OutGate.GetOutPath());
		}
	}
}