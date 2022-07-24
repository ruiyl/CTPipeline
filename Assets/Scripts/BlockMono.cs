using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Assets.Scripts
{
	public abstract class BlockMono : MonoBehaviour, IPointerClickHandler
	{
		[SerializeField] protected List<GateMono> gateList;

		public UnityAction<BlockLogic, GateMono> GateClickEvent;
		public UnityAction<BlockMono> BlockDClickEvent;

		private const int DOUBLE_CLICK = 2;

		public void OnPointerClick(PointerEventData eventData)
		{
			if (eventData.clickCount == DOUBLE_CLICK)
			{
				BlockDClickEvent?.Invoke(this);
			}
		}

		public virtual void DestroyBlock()
		{
			foreach (GateMono gate in gateList)
			{
				GateMono.DisconnectAll(gate);
			}
			Destroy(gameObject);
		}
	}

	public abstract class BlockMono<T> : BlockMono where T : BlockLogic
	{
		protected T logic;

		private void Awake()
		{
			CreateLogic();
			OnAwake();
		}

		protected abstract void CreateLogic();

		protected virtual void OnAwake()
		{
			foreach (GateMono gate in gateList)
			{
				gate.ClickEvent += OnGateClicked;
			}
		}

		private void Update()
		{
			logic.Update();
		}

		protected void OnGateClicked(GateMono gate)
		{
			GateClickEvent?.Invoke(logic, gate);
		}

		public override void DestroyBlock()
		{
			logic.PreDestroyBlock();
			base.DestroyBlock();
		}
	}
}