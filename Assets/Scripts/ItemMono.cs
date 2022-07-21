using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
	public class ItemMono : MonoBehaviour
	{
		[SerializeField] private ItemData itemData;

		private PipelinePathMono pipelinePath;
		private bool isTravelling;
		private float currentDistance;

		private const float PIPELINE_SPEED = 3f;

		public void Hide()
		{
			gameObject.SetActive(false);
		}

		public void Show()
		{
			gameObject.SetActive(true);
		}

		public void MoveTo(Vector3 pos)
		{
			transform.position = pos;
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

		private void Update()
		{
			if (isTravelling)
			{
				currentDistance += Time.deltaTime * PIPELINE_SPEED;
				MoveTo(pipelinePath.GetPointAt(currentDistance));
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
	}
}