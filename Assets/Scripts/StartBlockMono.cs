using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
	public class StartBlockMono : BlockMono<StartBlockLogic>
	{
		[SerializeField] protected GateMono outGate;

		public GateMono OutGate { get => outGate; }

		protected override void OnAwake()
		{
			base.OnAwake();
			outGate.ClickEvent += OnGateClicked;
			logic = new StartBlockLogic(this);
		}

		public void ReceiveItem(ItemMono item)
		{
			logic.OnReceiveItem(item);
		}
	}
}