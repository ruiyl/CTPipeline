using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Assets.Scripts
{
	[Serializable]
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
		private GameObject prefabToSpawn;
		private Vector3 draggOffset;

		private GateConnection currentConnectionData;

		private Dictionary<Type, int> blockCounts;
		private List<BlockMono> spawnedBlocks;
		private List<ItemMono> spawnedItems;

		public UnityAction<BlockMono> BlockPlaced;
		public UnityAction<PipelinePathMono> ConnectionCreated;

		private struct GateConnection
		{
			public BlockLogic logic1;
			public GateMono gate1;
			public BlockLogic logic2;
			public GateMono gate2;
		}

		private void Awake()
		{
			blockCounts = new Dictionary<Type, int>();
			spawnedBlocks = new List<BlockMono>();
			spawnedItems = new List<ItemMono>();
		}

		private void RemoveDraggedBlock()
		{
			Destroy(draggedBlock);
		}

		public void OnPointerExitBlockImg(PointerEventData eventData, GameObject imgObj)
		{
			if (imgObj == draggedImg && draggedBlock == null)
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
					prefabToSpawn = blockPrefab;
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

		}

		public void NotifyDrag(PointerEventData eventData, GameObject draggedObj)
		{
			bool draggingFromImg = (draggedObj == draggedImg) && hasExitedDraggedImg;
			if (draggingFromImg)
			{
				DragBlock(eventData);
			}
		}

		public void NotifyEndDrag()
		{
			if (draggedBlock)
			{
				BlockPlaced?.Invoke(draggedBlock.GetComponent<BlockMono>());
			}
			draggedImg = null;
			draggedBlock = null;
			prefabToSpawn = null;
		}

		private void OnBlockDoubleClicked(BlockMono block)
		{
			block.DestroyBlock();
		}

		public void OnSpawnedItem(ItemMono item)
		{
			spawnedItems.Add(item);
		}

		public void DragBlock(PointerEventData eventData, bool setOffset = false)
		{
			Ray pointerRay = mainCamera.ScreenPointToRay(eventData.position);
			if (Physics.Raycast(pointerRay, out RaycastHit hit, float.MaxValue, LayerMask.GetMask("PlacablePlane")))
			{
				if (prefabToSpawn != null)
				{
					SpawnBlock(prefabToSpawn);
					draggOffset = Vector3.zero;
					prefabToSpawn = null;
				}
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

		private void SpawnBlock(GameObject blockPrefab)
		{
			draggedBlock = Instantiate(blockPrefab, new Vector3(0f, -10f, 0f), Quaternion.identity);
			BlockMono block = draggedBlock.GetComponent<BlockMono>();
			block.GateClickEvent += OnGateClicked;
			block.BlockDClickEvent += OnBlockDoubleClicked;
			block.BlockDestroyedEvent += OnBlockDestroyed;

			if (!blockCounts.ContainsKey(block.GetType()))
			{
				blockCounts[block.GetType()] = 0;
			}
			blockCounts[block.GetType()]++;
			spawnedBlocks.Add(block);
		}

		private void OnBlockDestroyed(BlockMono block)
		{
			blockCounts[block.GetType()]--;
			spawnedBlocks.Remove(block);
		}

		public int GetBlockCount(Type blockType)
		{
			if (blockCounts.TryGetValue(blockType, out int count))
			{
				return count;
			}
			return 0;
		}

		public void DestroyAllBlock()
		{
			for (int i = spawnedBlocks.Count - 1; i >= 0; i--)
			{
				spawnedBlocks[i].DestroyBlock();
			}
			spawnedBlocks.Clear();
			blockCounts.Clear();

			for (int i = spawnedItems.Count - 1; i >= 0; i--)
			{
				if (spawnedItems[i] != null)
				{
					spawnedItems[i].Destroy();
				}
			}
			spawnedItems.Clear();
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