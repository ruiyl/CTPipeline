using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts
{
	public class TextFlowRunner : MonoBehaviour
	{
		[SerializeField] private Transform textRoot;
		[SerializeField] private GameObject nextBtn;
		[SerializeField] private UnityEvent<TextFlowRunner, int, int> OnFlowStep;

		private List<GameObject> textGoList;
		private int currentIndex;

		public int CurrentIndex { get => currentIndex; }

		private void Start()
		{
			textGoList = new List<GameObject>();
			foreach (Transform text in textRoot)
			{
				textGoList.Add(text.gameObject);
			}
			currentIndex = 0;
			OnFlowStep?.Invoke(this, currentIndex, textGoList.Count);
		}

		public void Next()
		{
			textGoList[currentIndex].SetActive(false);
			currentIndex++;
			if (currentIndex < textGoList.Count)
			{
				textGoList[currentIndex].SetActive(true);
			}
			OnFlowStep?.Invoke(this, currentIndex, textGoList.Count);
		}

		public void SetNextButtonState(bool visible)
		{
			nextBtn.SetActive(visible);
		}

		public void SetStep(int index, bool notify = false)
		{
			currentIndex = index;
			for (int i = 0; i < textGoList.Count; i++)
			{
				textGoList[i].SetActive(i == currentIndex);
			}
			if (notify)
			{
				OnFlowStep?.Invoke(this, currentIndex, textGoList.Count);
			}
		}
	}
}