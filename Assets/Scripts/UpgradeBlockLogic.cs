using System.Collections;

namespace Assets.Scripts
{
	public class UpgradeBlockLogic : BlockLogic<UpgradeBlockMono>
	{
		private State state;
		private ItemMono currentItem;

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
					currentItem.Upgrade();
					PopItem(currentItem, monoRef.OutGate, monoRef.OutGate.GetOutPath());
					break;
			}
		}

		private void OnReceiveItem(ItemMono item)
		{
			currentItem = item;
			TakeItemIn(currentItem, monoRef.InGate);
			state = State.Wait;
		}

		public override void PreDestroyBlock()
		{
			base.PreDestroyBlock();
			currentItem.Destroy();
		}
	}
}