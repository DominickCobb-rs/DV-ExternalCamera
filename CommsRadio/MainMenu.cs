using System;

using DV;

using CommsRadioAPI;

namespace CustomCamera.CommsRadio
{
	internal class MainMenu : AStateBehaviour
	{
		public MainMenu()
			: base(new CommsRadioState(
				titleText: "Cameras",
				contentText: "Manage Cameras.",
				buttonBehaviour: ButtonBehaviourType.Regular))
		{

		}

		public override AStateBehaviour OnAction(CommsRadioUtility utility, InputAction action)
		{
			if (action != InputAction.Activate)
			{
				throw new ArgumentException();
			}

			utility.PlaySound(VanillaSoundCommsRadio.Confirm);
			return new CameraAdd();
		}
	}
}