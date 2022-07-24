namespace Assets.Scripts
{
	public abstract class BlockLogic
	{
		public virtual void Update()
		{

		}

		public virtual bool CheckConnectionValid(GateMono gate, BlockLogic otherBlock, GateMono otherGate)
		{
			if (otherGate == gate)
			{
				return false;
			}
			if (gate.Direction == otherGate.Direction)
			{
				return false;
			}
			return true;
		}
	}

	public abstract class BlockLogic<T> : BlockLogic where T : BlockMono
	{
		protected T monoRef;
		protected ItemMono currentItem;

		public BlockLogic(T blockMono)
		{
			monoRef = blockMono;
		}

		public void TakeItemIn(GateMono inGate)
		{
			currentItem.MoveTo(inGate.GetHidePosition());
			currentItem.Hide();
		}

		public void PopItem(GateMono outGate, PipelinePathMono outPath = null)
		{
			currentItem.Show();
			if (outPath != null)
			{
				currentItem.MoveTo(outGate.GetShowPosition());
				currentItem.StartTravelling(outPath);
			}
			else
			{
				currentItem.DropAt(outGate.GetFrontPosition());
			}
			currentItem = null;
		}
	}
}
