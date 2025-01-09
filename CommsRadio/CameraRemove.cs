using System;

using DV;
using CommsRadioAPI;
using DvMod.CustomCamera;

namespace CustomCamera.CommsRadio
{
	internal class CameraRemove : AStateBehaviour
	{
		public CameraRemove()
			: base(new CommsRadioState(
				titleText: "Remove Camera",
				contentText: "Remove the current camera?",
				buttonBehaviour: ButtonBehaviourType.Override))
		{

		}

		public override AStateBehaviour OnAction(CommsRadioUtility utility, InputAction action)
		{
			
			switch (action)
			{
				case InputAction.Activate:
					utility.PlaySound(VanillaSoundCommsRadio.Warning);
					CustomCameraUtils.ClearCamera();
					return new RemoveConfirmation();

				case InputAction.Up:
					return new CameraAdd();

				case InputAction.Down:
					return new CameraAdd();

				default:
					Main.DebugLog("Main menu error: why are you here?");
					throw new Exception($"Unexpected action: {action}");
			}
		}
	}
}