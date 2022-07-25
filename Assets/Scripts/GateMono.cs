using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Assets.Scripts
{
	public class GateMono : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
	{
		public UnityAction<ItemMono> OpenEvent;
		public UnityAction<GateMono> ClickEvent;

		[SerializeField] private GateType direction;
		[SerializeField] private TMPro.TextMeshPro labelText;

		private List<PipelinePathMono> connectedPath;

		private bool pointerIn;
		private bool connectedState;

		public GateType Direction { get => direction; }

		private const float MULTICLICK_INTERVAL = 0.3f;
		private const int SINGLE_CLICK = 1;
		private const int DOUBLE_CLICK = 2;

		public enum GateType
		{
			In,
			Out,
		}

		private void Awake()
		{
			connectedPath = new List<PipelinePathMono>();
		}

		public void PutItemIn(ItemMono item)
		{
			OpenEvent?.Invoke(item);
		}

		public Vector3 GetHidePosition()
		{
			return transform.position;
		}

		public Vector3 GetShowPosition()
		{
			return transform.position;
		}

		public Vector3 GetConnectPosition()
		{
			return transform.position;
		}

		public Vector3 GetFrontPosition()
		{
			return transform.position + transform.forward;
		}

		public PipelinePathMono GetOutPath(int index = 0)
		{
			return connectedPath.Count > index ? connectedPath[index] : null;
		}

		public static void Connect(GateMono gate1, GateMono gate2)
		{
			GameObject pathObj = new GameObject("ConnectPath");
			PipelinePathMono pathComponent = pathObj.AddComponent<PipelinePathMono>();

			GateMono startGate = gate1.Direction == GateType.Out ? gate1 : gate2;
			GateMono endGate = gate1.Direction == GateType.In ? gate1 : gate2;
			pathComponent.CreatePath(startGate, endGate);

			gate1.connectedPath.Add(pathComponent);
			gate2.connectedPath.Add(pathComponent);
		}

		public static void DisconnectAll(GateMono endPoint)
		{
			for (int i = endPoint.connectedPath.Count - 1; i >= 0; i--)
			{
				PipelinePathMono path = endPoint.connectedPath[i];
				GateMono otherEnd = endPoint == path.StartGate ? path.EndGate : path.StartGate;
				endPoint.connectedPath.RemoveAt(i);
				otherEnd.connectedPath.Remove(path);
				otherEnd.SetConnectState(false);
				path.Destroy();
			}
			endPoint.SetConnectState(false);
		}

		private void FireClickEvent()
		{
			ClickEvent?.Invoke(this);
		}

		public void OnPointerClick(PointerEventData eventData)
		{
			if (!GameManager.IsInPlanMode)
			{
				return;
			}
			if (eventData.clickCount == SINGLE_CLICK)
			{
				Invoke(nameof(FireClickEvent), MULTICLICK_INTERVAL);
			}
			else if (eventData.clickCount == DOUBLE_CLICK)
			{
				CancelInvoke(nameof(FireClickEvent));
				DisconnectAll(this);
			}
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
			if (!GameManager.IsInPlanMode)
			{
				return;
			}
			pointerIn = true;
			UpdateLabelColour();
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			if (!GameManager.IsInPlanMode)
			{
				return;
			}
			pointerIn = false;
			UpdateLabelColour();
		}

		public void SetConnectState(bool isConnected)
		{
			connectedState = isConnected;
			UpdateLabelColour();
		}

		private void UpdateLabelColour()
		{
			if (pointerIn)
			{
				labelText.color = Color.yellow;
			}
			else if (connectedState)
			{
				labelText.color = Color.green;
			}
			else
			{
				labelText.color = Color.white;
			}
		}
	}
}