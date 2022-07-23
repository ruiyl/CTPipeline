using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
	public class OutputBlock : MonoBehaviour, IPlayerInteractable
	{
		public void Interact(CharacterMono character)
		{
			ItemMono item = character.PopItem();
			// TODO: Check score
			item.Destroy();
		}
	}
}