using PathCreation;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
	public class PipelinePathMono : MonoBehaviour
	{
		private GateMono startGate;
		private GateMono endGate;
		private VertexPath path;

		private List<ItemMono> itemsOnPath;

		public GateMono StartGate { get => startGate; }
		public GateMono EndGate { get => endGate; }

		private void Awake()
		{
			itemsOnPath = new List<ItemMono>();
		}

		public void CreatePath(GateMono start, GateMono end)
		{
			BezierPath bezierPath = new BezierPath(new Vector3[] { start.GetConnectPosition(), end.GetConnectPosition() }, false, PathSpace.xyz);
			bezierPath.SetPoint(1, start.GetFrontPosition());
			bezierPath.SetPoint(2, end.GetFrontPosition());

			PathCreator pathCreator = gameObject.AddComponent<PathCreator>();
			pathCreator.bezierPath = bezierPath;

			LineRenderer line = gameObject.AddComponent<LineRenderer>();
			line.positionCount = pathCreator.path.NumPoints;
			line.SetPositions(pathCreator.path.localPoints);
			line.startWidth = 0.1f;
			line.endWidth = 0.1f;
			// TODO: Make it a prefab

			startGate = start;
			endGate = end;
			path = pathCreator.path;
		}

		public float GetLength()
		{
			return path.length;
		}

		public Vector3 GetPointAt(float distance)
		{
			return path.GetPointAtDistance(distance, EndOfPathInstruction.Stop);
		}

		public void GetOn(ItemMono item)
		{
			itemsOnPath.Add(item);
		}

		public void GetOff(ItemMono item)
		{
			itemsOnPath.Remove(item);
		}

		public void Destroy()
		{
			foreach (ItemMono item in itemsOnPath)
			{
				item.Destroy();
			}
			Destroy(gameObject);
		}
	}
}