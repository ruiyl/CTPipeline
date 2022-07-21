using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Assets.Scripts
{
	public class Movable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
	{
		public UnityEvent<PointerEventData, GameObject> BeginDragEvent;
		public UnityEvent<PointerEventData, GameObject> DragEvent;
		public UnityEvent EndDragEvent;

		public void OnBeginDrag(PointerEventData eventData)
		{
			BeginDragEvent.Invoke(eventData, gameObject);
		}

		public void OnDrag(PointerEventData eventData)
		{
			DragEvent.Invoke(eventData, gameObject);
		}

		public void OnEndDrag(PointerEventData eventData)
		{
			EndDragEvent.Invoke();
		}
	}
}