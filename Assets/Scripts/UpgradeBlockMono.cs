using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
	public class UpgradeBlockMono : BlockMono<UpgradeBlockLogic>
	{
		[SerializeField] protected GateMono inGate;
		[SerializeField] protected GateMono outGate;

		public GateMono InGate { get => inGate; }
		public GateMono OutGate { get => outGate; }

		protected override void OnAwake()
		{
			base.OnAwake();
			inGate.ClickEvent += OnGateClicked;
			outGate.ClickEvent += OnGateClicked;
			logic = new UpgradeBlockLogic(this);
		}
	}
}