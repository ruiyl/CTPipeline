using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
	public class InputPoller
	{
		public enum KeyCodeInput
		{
			Up,
			Down,
			Left,
			Right,
			Interact,
		}

		private Dictionary<KeyCodeInput, KeyCode> keymap;

		public InputPoller(CharacterMono.Player player)
		{
			keymap = GenerateKeyMap(player);
		}

		public Vector3 GetDirectionInput()
		{
			if (GameManager.IsInPlanMode)
			{
				return Vector3.zero;
			}
			Vector3 inputDir = new Vector3(
				(Input.GetKey(keymap[KeyCodeInput.Right]) ? 1 : 0) + (Input.GetKey(keymap[KeyCodeInput.Left]) ? -1 : 0),
				0f,
				(Input.GetKey(keymap[KeyCodeInput.Up]) ? 1 : 0) + (Input.GetKey(keymap[KeyCodeInput.Down]) ? -1 : 0));
			return inputDir;
		}

		public bool GetInteractInput()
		{
			bool handInput = Input.GetKeyDown(keymap[KeyCodeInput.Interact]);
			return handInput && !GameManager.IsInPlanMode;
		}

		private static Dictionary<KeyCodeInput, KeyCode> GenerateKeyMap(CharacterMono.Player player)
		{
			Dictionary<KeyCodeInput, KeyCode> keymap = new Dictionary<KeyCodeInput, KeyCode>();
			switch (player)
			{
				case CharacterMono.Player.Player1:
					keymap[KeyCodeInput.Up] = KeyCode.W;
					keymap[KeyCodeInput.Down] = KeyCode.S;
					keymap[KeyCodeInput.Left] = KeyCode.A;
					keymap[KeyCodeInput.Right] = KeyCode.D;
					keymap[KeyCodeInput.Interact] = KeyCode.LeftShift;
					break;
				case CharacterMono.Player.Player2:
					keymap[KeyCodeInput.Up] = KeyCode.I;
					keymap[KeyCodeInput.Down] = KeyCode.K;
					keymap[KeyCodeInput.Left] = KeyCode.J;
					keymap[KeyCodeInput.Right] = KeyCode.L;
					keymap[KeyCodeInput.Interact] = KeyCode.RightShift;
					break;
			}
			return keymap;
		}
	}
}