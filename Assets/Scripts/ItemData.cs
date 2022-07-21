using UnityEngine;

namespace Assets.Scripts
{
	[CreateAssetMenu(fileName = "New Item Data", menuName = "Scriptable Object/Item Data")]
	public class ItemData : ScriptableObject
	{
		public ItemType type;
		public int level;

		public enum ItemType
		{
			A,
			B,
			C,
		}
	}
}