using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
	public class ConditionBlockMono : BlockMono<ConditionBlockLogic>, IPlayerInteractable
	{
		[SerializeField] private GateMono inGate;
		[SerializeField] private GateMono trueOutGate;
		[SerializeField] private GateMono falseOutGate;
		[SerializeField] private Transform icon;

		public GateMono InGate { get => inGate; }
		public GateMono TrueOutGate { get => trueOutGate; }
		public GateMono FalseOutGate { get => falseOutGate; }

		protected override void CreateLogic()
		{
			logic = new ConditionBlockLogic(this);
		}

		public void Interact(CharacterMono character)
		{
			logic.OnInteract(character);
		}

		public void SetLabelState(bool value)
		{
			icon.rotation = Quaternion.Euler(270f, 0f, value ? 45f : 315f);
		}
	}
}