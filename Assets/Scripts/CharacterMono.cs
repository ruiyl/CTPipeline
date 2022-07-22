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

		private IPlayerInteractable interactableObj;
		private ItemMono holdItem;
		private Quaternion targetRot;

		private void Update()
		{
			bool handInput = Input.GetKeyDown(KeyCode.LeftShift);

			if (handInput)
			{
				if (interactableObj != null)
				{
					interactableObj.Interact(this);
				}
				else if (holdItem != null)
				{
					DropItem(itemDrop.position);
				}
			}
		}

		private void FixedUpdate()
		{
			Vector3 inputDir = new Vector3((Input.GetKey(KeyCode.D) ? 1 : 0) + (Input.GetKey(KeyCode.A) ? -1 : 0), 0f,
				(Input.GetKey(KeyCode.W) ? 1 : 0) + (Input.GetKey(KeyCode.S) ? -1 : 0));

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
			if (other.TryGetComponent(out IPlayerInteractable item))
			{
				interactableObj = item;
			}
		}

		private void OnTriggerExit(Collider other)
		{
			if (other.TryGetComponent(out IPlayerInteractable interactable) && interactable == interactableObj)
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
	}
}