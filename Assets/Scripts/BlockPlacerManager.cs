using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Assets.Scripts
{
	[System.Serializable]
	public struct BlockSpawnPair
	{
		public GameObject blockPrefab;
		public GameObject blockImgObj;
	}

	public class BlockPlacerManager : MonoBehaviour
	{
		[SerializeField] private Camera mainCamera;
		[SerializeField] private List<BlockSpawnPair> spawnPairs;

		private GameObject draggedImg;
		private bool hasExitedDraggedImg;
		private GameObject draggedBlock;
		private Vector3 draggOffset;
		//private bool isRemoving;
		private GateConnection currentConnectionData;

		public UnityAction<BlockMono> BlockPlaced;
		public UnityAction<PipelinePathMono> ConnectionCreated;

		private struct GateConnection
		{
			public BlockLogic logic1;
			public GateMono gate1;
			public BlockLogic logic2;
			public GateMono gate2;
		}

		public void NotifyPointerInTrash(bool isInside)
		{
			//isRemoving = isInside;
		}

		private void RemoveDraggedBlock()
		{
			Destroy(draggedBlock);
		}

		public void OnPointerExitBlockImg(PointerEventData eventData, GameObject imgObj)
		{
			if (imgObj == draggedImg)
			{
				hasExitedDraggedImg = true;
				GameObject blockPrefab = null;
				foreach (var pair in spawnPairs)
				{
					if (pair.blockImgObj == draggedImg)
					{
						blockPrefab = pair.blockPrefab;
						break;
					}
				}
				if (blockPrefab != null)
				{
					SpawnBlock(blockPrefab);
					draggOffset = Vector3.zero;
					DragBlock(eventData);
				}
			}
		}

		public void NotifyBeginDragFromImg(GameObject draggedObj)
		{
			draggedImg = draggedObj;
			hasExitedDraggedImg = false;
		}

		public void NotifyBeginDragFromBlock(PointerEventData eventData, GameObject draggedObj)
		{
			//draggedBlock = draggedObj;
			//DragBlock(eventData, true);
		}

		public void NotifyDrag(PointerEventData eventData, GameObject draggedObj)
		{
			bool draggingFromImg = draggedObj == draggedImg && hasExitedDraggedImg;
			//bool draggingBlock = draggedObj == draggedBlock;
			if (draggingFromImg)// || draggingBlock)
			{
				DragBlock(eventData);
			}
		}

		public void NotifyEndDrag()
		{
			BlockPlaced?.Invoke(draggedBlock.GetComponent<BlockMono>());
			draggedImg = null;
			draggedBlock = null;
			//if (isRemoving)
			//{
			//	RemoveDraggedBlock();
			//}
		}

		private void OnBlockDoubleClicked(BlockMono block)
		{
			block.DestroyBlock();
		}

		public void DragBlock(PointerEventData eventData, bool setOffset = false)
		{
			Ray pointerRay = mainCamera.ScreenPointToRay(eventData.position);
			if (Physics.Raycast(pointerRay, out RaycastHit hit, float.MaxValue, LayerMask.GetMask("PlacablePlane")))
			{
				if (setOffset)
				{
					draggOffset = draggedBlock.transform.position - hit.point;
				}
				else
				{
					draggedBlock.transform.position = hit.point + draggOffset;
				}
			}
		}

		public void SpawnBlock(GameObject blockPrefab)
		{
			draggedBlock = Instantiate(blockPrefab);
			//Movable movableComp = draggedBlock.GetComponent<Movable>();
			//movableComp.BeginDragEvent.AddListener(NotifyBeginDragFromBlock);
			//movableComp.DragEvent.AddListener(NotifyDrag);
			//movableComp.EndDragEvent.AddListener(NotifyEndDrag);
			BlockMono block = draggedBlock.GetComponent<BlockMono>();
			block.GateClickEvent += OnGateClicked;
			block.BlockDClickEvent += OnBlockDoubleClicked;
		}

		private void OnGateClicked(BlockLogic logic, GateMono gate)
		{
			RegisterGateConnection(logic, gate);
		}

		private void RegisterGateConnection(BlockLogic logic, GateMono gate)
		{
			if (currentConnectionData.logic1 == null && currentConnectionData.gate1 == null)
			{
				currentConnectionData.logic1 = logic;
				currentConnectionData.gate1 = gate;

				gate.SetConnectState(true);
			}
			else
			{
				currentConnectionData.logic2 = logic;
				currentConnectionData.gate2 = gate;

				CreateBlockConnection(currentConnectionData);
				ClearGateConnection(ref currentConnectionData);
			}
		}

		private void ClearGateConnection(ref GateConnection connection)
		{
			connection.logic1 = null;
			connection.gate1 = null;
			connection.logic2 = null;
			connection.gate2 = null;
		}

		private void CreateBlockConnection(GateConnection connection)
		{
			bool isConnectionValid = connection.logic1.CheckConnectionValid(connection.gate1, connection.logic2, connection.gate2) &&
				connection.logic2.CheckConnectionValid(connection.gate2, connection.logic1, connection.gate1);
			if (isConnectionValid)
			{
				PipelinePathMono path = GateMono.Connect(connection.gate1, connection.gate2);
				ConnectionCreated?.Invoke(path);
			}
			connection.gate1.SetConnectState(isConnectionValid);
			connection.gate2.SetConnectState(isConnectionValid);
		}
	}
}