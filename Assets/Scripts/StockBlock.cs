using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts
{
	public class StockBlock : MonoBehaviour, IPlayerInteractable
	{
		[SerializeField] private ItemMono itemPrefab;
		[SerializeField] private UnityEvent<ItemMono> SpawnedItem;

		public void Interact(CharacterMono character)
		{
			ItemMono newItem = Instantiate(itemPrefab);
			newItem.Interact(character);
			SpawnedItem?.Invoke(newItem);
		}
	}
}