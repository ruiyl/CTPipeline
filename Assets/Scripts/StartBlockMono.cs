using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
	public class StartBlockMono : BlockMono<StartBlockLogic>, IPlayerInteractable
	{
		[SerializeField] protected GateMono outGate;

		public GateMono OutGate { get => outGate; }

		protected override void OnAwake()
		{
			base.OnAwake();
			outGate.ClickEvent += OnGateClicked;
			logic = new StartBlockLogic(this);
		}

		public void Interact(CharacterMono character)
		{
			logic.OnInteractedByPlayer(character);
		}
	}
}