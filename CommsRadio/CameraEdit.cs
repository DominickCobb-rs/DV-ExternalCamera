/*
using System;

using DV;
using CommsRadioAPI;
using System.Diagnostics;

namespace ExternalCamera.CommsRadio
{
	internal class CameraEdit : AStateBehaviour
	{
		public CameraEdit()
			: base(new CommsRadioState(
				titleText: "Add Camera",
				contentText: "Add a camera?",
				buttonBehaviour: ButtonBehaviourType.Regular))
		{

		}

		public override AStateBehaviour OnAction(CommsRadioUtility utility, InputAction action)
		{
			switch (action)
			{
				case InputAction.Activate:
					utility.PlaySound(VanillaSoundCommsRadio.Warning);
					return new EditPointAtNothing();

				case InputAction.Up:
					return new CameraAdd();

				case InputAction.Down:
					return new CameraRemove();

				default:
					Debug.LogError("Main menu error: why are you here?");
					throw new Exception($"Unexpected action: {action}");
			}
		}
	}
}
*/