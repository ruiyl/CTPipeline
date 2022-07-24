using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
	public class ItemMono : MonoBehaviour, IPlayerInteractable
	{
		[SerializeField] private ItemData data;
		[SerializeField] private Transform valueTextPivot;
		[SerializeField] private TMPro.TextMeshPro valueText;

		private Rigidbody rb;
		private Collider col;
		private PipelinePathMono pipelinePath;
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
			col.enabled = false;
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
			this.pipelinePath.GetOn(this);
			currentDistance = 0f;
		}

		public void StopTravelling()
		{
			pipelinePath.GetOff(this);
			pipelinePath = null;
		}

		private void FixedUpdate()
		{
			if (pipelinePath != null)
			{
				currentDistance += Time.deltaTime * PIPELINE_SPEED;
				rb.MovePosition(pipelinePath.GetPointAt(currentDistance));
			}
			valueTextPivot.rotation = Quaternion.LookRotation(Vector3.forward, Vector3.up);
		}

		private void Update()
		{
			if (pipelinePath != null && currentDistance >= pipelinePath.GetLength())
			{
				GateMono endGate = pipelinePath.EndGate;
				StopTravelling();
				endGate.PutItemIn(this);
			}
		}

		public void Interact(CharacterMono character)
		{
			if (pipelinePath == null)
			{
				character.HoldItem(this);
			}
		}

		public void SetData(ItemData data)
		{
			this.data = data;
			UpdateValue();
		}

		public void Upgrade()
		{
			data.IncreaseIndices();
			UpdateValue();
		}

		public void UpdateValue()
		{
			valueText.text = data.GetValue();
		}

		public void Destroy()
		{
			Destroy(gameObject);
		}

		public bool ValueEqual(ItemData data)
		{
			return ItemData.ValueEqual(this.data, data);
		}

		public static ItemMono MergeItem(ItemMono lhs, ItemMono rhs)
		{
			ItemData newData = ItemData.ConcatItem(lhs.data, rhs.data);
			rhs.Destroy();
			lhs.SetData(newData);

			return lhs;
		}
	}
}