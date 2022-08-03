using System.Collections;

namespace Assets.Scripts
{
	public class UpgradeBlockLogic : BlockLogic<UpgradeBlockMono>
	{
		private ItemMono currentItem;

		public UpgradeBlockLogic(UpgradeBlockMono blockMono) : base(blockMono)
		{
			monoRef.InGate.OpenEvent += OnReceiveItem;
		}

		private void OnReceiveItem(ItemMono item)
		{
			currentItem = item;
			TakeItemIn(currentItem, monoRef.InGate);
			currentItem.Upgrade();
			PopItem(currentItem, monoRef.OutGate, monoRef.OutGate.GetConnectedPath());
		}
	}
}