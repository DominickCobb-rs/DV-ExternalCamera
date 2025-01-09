using System;

using DV;

using UnityEngine;

using CommsRadioAPI;
using DvMod.CustomCamera;

namespace CustomCamera.CommsRadio
{
    // this class detects what we're pointing at
    internal class SelectCameraPositionLoco : AStateBehaviour
    {
        private const float SIGNAL_RANGE = 100f;
        public static Settings settings = new Settings();
        private TrainCar trainCar;
        private int location;
        private string carID;

        public SelectCameraPositionLoco(TrainCar trainCar, string carID, int location = 0)
            : base(new CommsRadioState(
                titleText: "Location",
                contentText: CameraLocations.LocationStrings[CameraLocations.LocationInts[location]],
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
                    if (location == 11)
                    {
                        position = Main.customVector;
                        quaternion = Main.customQuaternion;
                    }
                    else
                        switch (trainCar.carLivery.parentType.name)
                        {
                            case "TrainCarType_LocoS060":
                                position = CameraLocations.S060Offsets[CameraLocations.LocationInts[location]].Item1;
                                quaternion = CameraLocations.S060Offsets[CameraLocations.LocationInts[location]].Item2;
                                break;

                            case "TrainCarType_LocoS282A":
                            case "TrainCarType_LocoS282B":
                                position = CameraLocations.S282Offsets[CameraLocations.LocationInts[location]].Item1;
                                quaternion = CameraLocations.S282Offsets[CameraLocations.LocationInts[location]].Item2;
                                break;

                            case "TrainCarType_LocoDE6":
                                position = CameraLocations.DE6Offsets[CameraLocations.LocationInts[location]].Item1;
                                quaternion = CameraLocations.DE6Offsets[CameraLocations.LocationInts[location]].Item2;
                                break;

                            case "TrainCarType_LocoDH4":
                                position = CameraLocations.DH4Offsets[CameraLocations.LocationInts[location]].Item1;
                                quaternion = CameraLocations.DH4Offsets[CameraLocations.LocationInts[location]].Item2;
                                break;

                            case "TrainCarType_LocoDE2":
                                position = CameraLocations.DE2Offsets[CameraLocations.LocationInts[location]].Item1;
                                quaternion = CameraLocations.DE2Offsets[CameraLocations.LocationInts[location]].Item2;
                                break;

                            case "TrainCarType_LocoDM3":
                                position = CameraLocations.DM3Offsets[CameraLocations.LocationInts[location]].Item1;
                                quaternion = CameraLocations.DM3Offsets[CameraLocations.LocationInts[location]].Item2;
                                break;

                            default:
                                throw new Exception($"Unexpected loco: {trainCar.carLivery.parentType.name}");
                        }

                    if (CameraLocations.LocationInts[location] == CameraLocations.Locations.Cab_Controls ||
                        CameraLocations.LocationInts[location] == CameraLocations.Locations.Cab_Left ||
                        CameraLocations.LocationInts[location] == CameraLocations.Locations.Cab_Right)
                    {
                        CustomCameraUtils.AttachCamera(trainCar, position, quaternion, true);
                    }
                    else CustomCameraUtils.AttachCamera(trainCar, position, quaternion);
                    return new ConfirmPlacement();

                case InputAction.Up:
                    ++location;
                    if (location > 11)
                        location = 0;
                    return new SelectCameraPositionLoco(trainCar, carID, location);

                case InputAction.Down:
                    --location;
                    if (location < 0)
                        location = 11;
                    return new SelectCameraPositionLoco(trainCar, carID, location);

                default:
                    Debug.LogError("Main menu error: why are you here?");
                    throw new Exception($"Unexpected action: {action}");
            }

        }
    }
}