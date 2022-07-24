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

		public virtual void PreDestroyBlock()
		{

		}
	}

	public abstract class BlockLogic<T> : BlockLogic where T : BlockMono
	{
		protected T monoRef;

		public BlockLogic(T blockMono)
		{
			monoRef = blockMono;
		}

		public void TakeItemIn(ItemMono item, GateMono inGate)
		{
			item.MoveTo(inGate.GetHidePosition());
			item.Hide();
		}

		public void PopItem(ItemMono item, GateMono outGate, PipelinePathMono outPath = null)
		{
			item.Show();
			if (outPath != null)
			{
				item.MoveTo(outGate.GetShowPosition());
				item.StartTravelling(outPath);
			}
			else
			{
				item.DropAt(outGate.GetFrontPosition());
			}
		}
	}
}
