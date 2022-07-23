using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
	public class StockBlock : MonoBehaviour, IPlayerInteractable
	{
		[SerializeField] private ItemMono itemPrefab;

		public void Interact(CharacterMono character)
		{
			ItemMono newItem = Instantiate(itemPrefab);
			newItem.Interact(character);
		}
	}
}