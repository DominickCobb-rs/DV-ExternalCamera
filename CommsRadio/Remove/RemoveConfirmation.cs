using System;

using DV;

using CommsRadioAPI;

namespace CustomCamera.CommsRadio
{
	internal class RemoveConfirmation : AStateBehaviour
	{
		public RemoveConfirmation()
			: base(new CommsRadioState(
				titleText: "Done!",
				contentText: "Removed camera",
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