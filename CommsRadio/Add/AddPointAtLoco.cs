using System;

using CommsRadioAPI;
using DvMod.CustomCamera;

namespace CustomCamera.CommsRadio
{
	// This class inherits PurchasePointAtLocoState for the radio state
	internal class AddPointAtLoco : PointAtLocoState
	{

		private string carID;

		public AddPointAtLoco(TrainCar selectedCar, string carID)
			: base(selectedCar, carID)
		{
			this.carID = carID;
		}

		public override AStateBehaviour OnAction(CommsRadioUtility utility, InputAction action)
		{
			switch (action)
			{
				case InputAction.Activate:
					utility.PlaySound(VanillaSoundCommsRadio.Confirm);
					if (selectedCar.carLivery == null)
					{
						Main.DebugLog("Car livery is null");
						return this;
					}
					return new SelectCameraPositionLoco(selectedCar, carID);

				case InputAction.Up:
					return this;

				case InputAction.Down:
					return this;

				default:
					Main.DebugLog("Camera add error: why are you here?");
					throw new Exception($"Unexpected action: {action}");
			}

		}
	}
}