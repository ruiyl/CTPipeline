using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
	public class OutputBlock : MonoBehaviour, IPlayerInteractable
	{
		[SerializeField] private GameManager gameManager;

		public void Interact(CharacterMono character)
		{
			ItemMono item = character.PopItem();
			gameManager.SubmitItem(item);
			item.Destroy();
		}
	}
}