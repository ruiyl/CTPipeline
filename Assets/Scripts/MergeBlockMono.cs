using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
	public class MergeBlockMono : BlockMono<MergeBlockLogic>
	{
		[SerializeField] private GateMono leftInGate;
		[SerializeField] private GateMono rightInGate;
		[SerializeField] private GateMono outGate;

		public GateMono LeftInGate { get => leftInGate; }
		public GateMono RightInGate { get => rightInGate; }
		public GateMono OutGate { get => outGate; }

		protected override void CreateLogic()
		{
			logic = new MergeBlockLogic(this);
		}
	}
}