using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Assets.Scripts
{
	public class BlockImgUI : MonoBehaviour, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
	{
		public UnityEvent<PointerEventData, GameObject> PointerExitEvent;
		public UnityEvent<GameObject> BeginDragEvent;
		public UnityEvent<PointerEventData, GameObject> DragEvent;
		public UnityEvent EndDragEvent;

		public void OnBeginDrag(PointerEventData eventData)
		{
			BeginDragEvent.Invoke(gameObject);
		}

		public void OnDrag(PointerEventData eventData)
		{
			DragEvent.Invoke(eventData, gameObject);
		}

		public void OnEndDrag(PointerEventData eventData)
		{
			EndDragEvent.Invoke();
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			PointerExitEvent.Invoke(eventData, gameObject);
		}
	}
}