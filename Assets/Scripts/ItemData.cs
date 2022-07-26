using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
	[System.Serializable]
	public class ItemData
	{
		[SerializeField] private List<ValueType> valueTypes;
		[SerializeField] private List<int> valueIndices;

		private static readonly char[][] valueMap = new char[2][]
		{
			new char[] { 'A', 'B', 'C', 'D' },
			new char[] { '1', '2', '3', '4' },
		};

		public enum ValueType
		{
			Letter = 0,
			Number = 1,
		}

		public ItemData(ValueType type, int index)
		{
			valueTypes = new List<ValueType>() { type };
			valueIndices = new List<int>() { index };
		}

		public ItemData(List<ValueType> types, List<int> indices)
		{
			valueTypes = types;
			valueIndices = indices;
		}

		public void IncreaseIndices()
		{
			for (int i = 0; i < valueIndices.Count; i++)
			{
				valueIndices[i] = Mathf.Clamp(valueIndices[i] + 1, 0, valueMap[(int)valueTypes[i]].Length - 1);
			}
		}

		public static ItemData ConcatItem(ItemData lhs, ItemData rhs)
		{
			List<ValueType> types = new List<ValueType>();
			List<int> indices = new List<int>();

			types.AddRange(lhs.valueTypes);
			types.AddRange(rhs.valueTypes);
			indices.AddRange(lhs.valueIndices);
			indices.AddRange(rhs.valueIndices);

			return new ItemData(types, indices);
		}

		public string GetValue()
		{
			List<char> values = new List<char>();
			if (valueTypes.Count != valueIndices.Count)
			{
				return string.Empty;
			}
			for (int i = 0; i < valueTypes.Count; i++)
			{
				values.Add(valueMap[(int)valueTypes[i]][valueIndices[i]]);
			}
			return string.Join(string.Empty, values);
		}

		public static bool ValueEqual(ItemData lhs, ItemData rhs)
		{
			return System.Linq.Enumerable.SequenceEqual(lhs.valueTypes, rhs.valueTypes) &&
				System.Linq.Enumerable.SequenceEqual(lhs.valueIndices, rhs.valueIndices);
		}

		public override string ToString()
		{
			return GetValue();
		}

		public int GetScore()
		{
			int sumIndices = 0;
			foreach (int index in valueIndices)
			{
				sumIndices += index + 1;
			}
			return sumIndices * valueIndices.Count;
		}
	}
}