using System.Collections;

namespace Assets.Scripts
{
	public class UpgradeBlockLogic : BlockLogic<UpgradeBlockMono>
	{
		private State state;

		private enum State
		{
			Idle,
			Wait,
			Busy,
		}

		public UpgradeBlockLogic(UpgradeBlockMono blockMono) : base(blockMono)
		{
			monoRef.InGate.OpenEvent += OnReceiveItem;
			state = State.Idle;
		}

		public override void Update()
		{
			base.Update();
			switch (state)
			{
				case State.Idle:
					break;
				case State.Wait:
					state = State.Busy;
					break;
				case State.Busy:
					state = State.Idle;
					PopItem(monoRef.OutGate, monoRef.OutGate.GetOutPath());
					currentItem = null;
					break;
			}
		}

		private void OnReceiveItem(ItemMono item)
		{
			currentItem = item;
			TakeItemIn(monoRef.InGate);
			state = State.Wait;
		}
	}
}