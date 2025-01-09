using System;

using DV;
using CommsRadioAPI;
using System.Diagnostics;
using DvMod.CustomCamera;

namespace CustomCamera.CommsRadio
{
	internal class CameraAdd : AStateBehaviour
	{
		public CameraAdd()
			: base(new CommsRadioState(
				titleText: "Add Camera",
				contentText: "Add a camera?",
				buttonBehaviour: ButtonBehaviourType.Override))
		{

		}

		public override AStateBehaviour OnAction(CommsRadioUtility utility, InputAction action)
		{
			switch (action)
			{
				case InputAction.Activate:
					utility.PlaySound(VanillaSoundCommsRadio.Warning);
					return new AddPointAtNothing();
				
				case InputAction.Up:
				  	return new CameraRemove();

				case InputAction.Down:
					return new CameraRemove();
				
				default:
					Main.DebugLog("Camera add error: why are you here?");
					throw new Exception($"Unexpected action: {action}");
			}
		}
	}
}