using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts
{
	public class LoopBlockMono : BlockMono<LoopBlockLogic>
	{
		[SerializeField] private GateMono inGate;
		[SerializeField] private GateMono outGate;
		[SerializeField] private GateMono loopInGate;
		[SerializeField] private GateMono loopOutGate;
		[SerializeField] private TMPro.TextMeshPro loopCountText;

		public GateMono InGate { get => inGate; }
		public GateMono OutGate { get => outGate; }
		public GateMono LoopInGate { get => loopInGate; }
		public GateMono LoopOutGate { get => loopOutGate; }

		protected override void CreateLogic()
		{
			logic = new LoopBlockLogic(this);
		}

		public void SetLoopCount(int value)
		{
			loopCountText.text = value.ToString();
		}
	}
}