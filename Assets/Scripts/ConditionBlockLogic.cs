using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
	public class ConditionBlockLogic : BlockLogic<ConditionBlockMono>
	{
		private ConditionResolver resolver;

		public ConditionBlockLogic(ConditionBlockMono blockMono) : base(blockMono)
		{
			monoRef.InGate.OpenEvent += ReceiveItem;
			resolver = new BinaryResolver(monoRef);
		}

		private void ReceiveItem(ItemMono item)
		{
			TakeItemIn(item, monoRef.InGate);
			if (resolver.CheckItem(item))
			{
				PopItem(item, monoRef.TrueOutGate, monoRef.TrueOutGate.GetOutPath());
			}
			else
			{
				PopItem(item, monoRef.FalseOutGate, monoRef.FalseOutGate.GetOutPath());
			}
		}

		public void OnInteract(CharacterMono player)
		{
			resolver.Trigger();
		}
	}

	abstract class ConditionResolver
	{
		protected ConditionBlockMono blockMono;

		public ConditionResolver(ConditionBlockMono blockMono)
		{
			this.blockMono = blockMono;
		}

		public abstract bool CheckItem(ItemMono item);
		public virtual void Trigger()
		{

		}
	}

	class BinaryResolver : ConditionResolver
	{
		private bool isSet;

		public BinaryResolver(ConditionBlockMono blockMono) : base(blockMono)
		{

		}

		public bool IsSet { get => isSet; }

		public override bool CheckItem(ItemMono item)
		{
			return isSet;
		}

		public override void Trigger()
		{
			isSet = !isSet;
			blockMono.SetLabelState(isSet);
		}
	}
}