using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
	public class ItemMono : MonoBehaviour, IPlayerInteractable
	{
		[SerializeField] private ItemData itemData;

		private Rigidbody rb;
		private Collider col;
		private PipelinePathMono pipelinePath;
		private bool isTravelling;
		private float currentDistance;

		private const float PIPELINE_SPEED = 3f;

		private void Awake()
		{
			rb = GetComponent<Rigidbody>();
			col = GetComponent<Collider>();
		}

		public void Hide()
		{
			gameObject.SetActive(false);
		}

		public void Show()
		{
			gameObject.SetActive(true);
		}

		public void MoveTo(Vector3 pos, Transform parent = null)
		{
			rb.isKinematic = true;
			rb.useGravity = false;
			transform.position = pos;
			if (parent != null)
			{
				transform.SetParent(parent);
			}
		}

		public void Drop()
		{
			rb.isKinematic = false;
			rb.useGravity = true;
			col.enabled = true;
			transform.SetParent(null);
		}

		public void DropAt(Vector3 pos)
		{
			transform.position = pos;
			Drop();
		}

		public void StartTravelling(PipelinePathMono pipelinePath)
		{
			this.pipelinePath = pipelinePath;
			isTravelling = true;
			currentDistance = 0f;
		}

		public void StopTravelling()
		{
			pipelinePath = null;
			isTravelling = false;
		}

		private void FixedUpdate()
		{
			if (isTravelling)
			{
				currentDistance += Time.deltaTime * PIPELINE_SPEED;
				rb.MovePosition(pipelinePath.GetPointAt(currentDistance));
			}
		}

		private void OnTriggerEnter(Collider other)
		{
			if (isTravelling && other.TryGetComponent(out GateMono gate) && gate == pipelinePath.EndGate)
			{
				StopTravelling();
				gate.PutItemIn(this);
			}
		}

		public void Interact(CharacterMono character)
		{
			if (!isTravelling)
			{
				character.HoldItem(this);
				col.enabled = false;
			}
		}

		public void Destroy()
		{
			Destroy(gameObject);
		}
	}
}