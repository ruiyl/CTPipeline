using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
	public class CharacterMono : MonoBehaviour
	{
		[SerializeField] private Rigidbody rb;
		[SerializeField] private Transform itemHolder;
		[SerializeField] private Transform itemDrop;
		[SerializeField] private float speed;
		[SerializeField] private float rotationMaxSpeed;
		[SerializeField] private Player playerNumber;

		private InputPoller input;

		private IPlayerInteractable interactableObj;
		private ItemMono holdItem;
		private Quaternion targetRot;

		public bool IsHolding { get => holdItem != null; }

		public enum Player
		{
			Player1,
			Player2,
		}

		private void Start()
		{
			input = new InputPoller(playerNumber);
		}

		public void SetPlayerNumber(Player player)
		{
			playerNumber = player;
		}

		private void Update()
		{
			bool handInput = input.GetInteractInput();

			if (handInput)
			{
				if (interactableObj != null)
				{
					if (IsInteractable(interactableObj))
					{
						interactableObj.Interact(this);
					}
				}
				else if (holdItem != null)
				{
					DropItem(itemDrop.position);
				}
			}
		}

		private void FixedUpdate()
		{
			Vector3 inputDir = input.GetDirectionInput();

			Vector3 offset = inputDir.normalized * speed;
			rb.velocity = offset;
			if (inputDir.magnitude > 0f)
			{
				targetRot = Quaternion.LookRotation(inputDir.normalized, Vector3.up);
				rb.rotation = Quaternion.RotateTowards(rb.rotation, targetRot, rotationMaxSpeed);
			}
		}

		private void OnTriggerEnter(Collider other)
		{
			if (interactableObj == null && other.TryGetComponent(out IPlayerInteractable item))
			{
				interactableObj = item;
			}
		}

		private void OnTriggerStay(Collider other)
		{
			if (interactableObj == null && other.TryGetComponent(out IPlayerInteractable item))
			{
				interactableObj = item;
			}
		}

		private void OnTriggerExit(Collider other)
		{
			if (interactableObj != null && other.TryGetComponent(out IPlayerInteractable interactable) && interactable == interactableObj)
			{
				interactableObj = null;
			}
		}

		public void HoldItem(ItemMono item)
		{
			holdItem = item;
			holdItem.MoveTo(itemHolder.position, itemHolder);
			if (interactableObj == (IPlayerInteractable)holdItem)
			{
				interactableObj = null;
			}
		}

		public ItemMono PopItem()
		{
			ItemMono returnItem = holdItem;
			DropItem();
			return returnItem;
		}

		private void DropItem()
		{
			holdItem.Drop();
			holdItem = null;
		}

		private void DropItem(Vector3 position)
		{
			holdItem.DropAt(position);
			holdItem = null;
		}

		private bool IsInteractable(IPlayerInteractable interactable)
		{
			return (interactable is ItemMono && !IsHolding) ||
				(interactable is BlockMono && IsHolding) ||
				(interactable is StockBlock && !IsHolding) ||
				(interactable is OutputBlock && IsHolding);
		}
	}
}