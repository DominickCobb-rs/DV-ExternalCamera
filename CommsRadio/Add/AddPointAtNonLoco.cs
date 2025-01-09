using System;
using System.Diagnostics;
using CommsRadioAPI;
using DvMod.CustomCamera;

namespace CustomCamera.CommsRadio
{
	// This class inherits PurchasePointAtLocoState for the radio state
	internal class AddPointAtNonLoco : PointAtNonLocoState
	{

		private string carID;

		public AddPointAtNonLoco(TrainCar selectedCar, string carID)
			: base(selectedCar, carID)
		{
			this.carID = carID;
            UnityEngine.Debug.Log($"Pointed at {selectedCar.carLivery.parentType.name}");
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
					return new SelectCameraPositionNonLoco(selectedCar, carID);

				case InputAction.Up:
					return this;

				case InputAction.Down:
					return this;

				default:
					UnityEngine.Debug.Log("Camera add error: why are you here?");
					throw new Exception($"Unexpected action: {action}");
			}

		}
	}
}