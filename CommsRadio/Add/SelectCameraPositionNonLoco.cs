using System;

using DV;

using UnityEngine;

using CommsRadioAPI;
using DvMod.CustomCamera;

namespace CustomCamera.CommsRadio
{
    // this class detects what we're pointing at
    internal class SelectCameraPositionNonLoco : AStateBehaviour
    {
        private const float SIGNAL_RANGE = 100f;
        public static Settings settings = new Settings();
        private TrainCar trainCar;
        private int location;
        private string carID;

        public SelectCameraPositionNonLoco(TrainCar trainCar, string carID, int location = 0)
            : base(new CommsRadioState(
                titleText: "Location",
                contentText: CameraLocations.NonLocoLocationStrings[CameraLocations.NonLocoLocationInts[location]],
                actionText: "Accept",
                buttonBehaviour: ButtonBehaviourType.Override))
        {
            this.location = location;
            this.carID = carID;
            this.trainCar = trainCar;
        }

        public override AStateBehaviour OnAction(CommsRadioUtility utility, InputAction action)
        {
            switch (action)
            {
                case InputAction.Activate:
                    utility.PlaySound(VanillaSoundCommsRadio.Confirm);
                    Vector3 position;
                    Quaternion quaternion;
                    if (location == 4)
                    {
                        position = Main.customVector;
                        quaternion = Main.customQuaternion;
                    }
                    else
                    switch (trainCar.carLivery.parentType.name)
                    {
                        case "TrainCarType_BoxCar":
                        case "TrainCarType_Refrigerator":
                        case "TrainCarType_Stock":
                            position = CameraLocations.FlatCarOffsets[CameraLocations.NonLocoLocationInts[location]].Item1;
                            quaternion = CameraLocations.FlatCarOffsets[CameraLocations.NonLocoLocationInts[location]].Item2;
                            break;
                        case "TrainCarType_TankGas":
                        case "TrainCarType_TankChem":
                        case "TrainCarType_TankOil":
                            position = CameraLocations.TankerOffsets[CameraLocations.NonLocoLocationInts[location]].Item1;
                            quaternion = CameraLocations.TankerOffsets[CameraLocations.NonLocoLocationInts[location]].Item2;
                            break;
                        case "TrainCarType_Flatbed":
                        case "TrainCarType_FlatbedStakes":
                            position = CameraLocations.FlatCarOffsets[CameraLocations.NonLocoLocationInts[location]].Item1;
                            quaternion = CameraLocations.FlatCarOffsets[CameraLocations.NonLocoLocationInts[location]].Item2;
                            break;
                        case "TrainCarType_Hopper":
                            position = CameraLocations.HopperOffsets[CameraLocations.NonLocoLocationInts[location]].Item1;
                            quaternion = CameraLocations.HopperOffsets[CameraLocations.NonLocoLocationInts[location]].Item2;
                            break;
                        default:
                            position = CameraLocations.FlatCarOffsets[CameraLocations.NonLocoLocationInts[location]].Item1;
                            quaternion = CameraLocations.FlatCarOffsets[CameraLocations.NonLocoLocationInts[location]].Item2;
                            Debug.LogError($"Couldn't find camera offsets for car {trainCar.carLivery.parentType.name}");
                            break;
                    }

                    CustomCameraUtils.AttachCamera(trainCar, position, quaternion);
                    return new ConfirmPlacement();

                case InputAction.Up:
                    ++location;
                    if (location > 4)
                        location = 0;
                    return new SelectCameraPositionNonLoco(trainCar, carID, location);

                case InputAction.Down:
                    --location;
                    if (location < 0)
                        location = 4;
                    return new SelectCameraPositionNonLoco(trainCar, carID, location);

                default:
                    Debug.LogError("Main menu error: why are you here?");
                    throw new Exception($"Unexpected action: {action}");
            }

        }
    }
}