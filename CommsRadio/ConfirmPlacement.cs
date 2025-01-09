using System;

using DV;

using CommsRadioAPI;

namespace CustomCamera.CommsRadio
{
	internal class ConfirmPlacement : AStateBehaviour
	{
		public ConfirmPlacement()
			: base(new CommsRadioState(
				titleText: "Done!",
				contentText: "Placed camera",
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
			return new MainMenu();
		}
	}
}