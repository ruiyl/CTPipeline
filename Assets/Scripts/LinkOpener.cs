using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts
{
	public class LinkOpener : MonoBehaviour, IPointerClickHandler
	{
		public void OnPointerClick(PointerEventData eventData)
		{
			TextMeshProUGUI text = GetComponent<TextMeshProUGUI>();
			int linkIndex = TMP_TextUtilities.FindIntersectingLink(text, eventData.position, null);
			if (linkIndex != -1)
			{
				TMP_LinkInfo link = text.textInfo.linkInfo[linkIndex];
				Application.OpenURL(link.GetLinkID());
			}
		}
	}
}