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
		public UnityAction<BlockMono> BlockDestroyedEvent;

		private const int DOUBLE_CLICK = 2;

		protected virtual void OnClicked(PointerEventData eventData)
		{

		}

		public void OnPointerClick(PointerEventData eventData)
		{
			OnClicked(eventData);
			if (eventData.clickCount == DOUBLE_CLICK && GameManager.IsInPlanMode)
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
				gate.SetBlock(this);
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
			BlockDestroyedEvent?.Invoke(this);
		}

		protected override void OnClicked(PointerEventData eventData)
		{
			base.OnClicked(eventData);
			logic.OnClicked(eventData);
		}
	}
}