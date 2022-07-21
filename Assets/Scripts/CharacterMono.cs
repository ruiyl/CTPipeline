using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
	public class CharacterMono : MonoBehaviour
	{
		[SerializeField] private Rigidbody rb;
		[SerializeField] private float speed;
		[SerializeField] private float rotationMaxSpeed;

		private Quaternion targetRot;

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

		}
	}
}