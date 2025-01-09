using System;

using DV;

using UnityEngine;

using CommsRadioAPI;
using DvMod.CustomCamera;

namespace CustomCamera.CommsRadio
{
    // this class detects what we're pointing at
    internal class AddPointAtNothing : AStateBehaviour
    {
        private const float SIGNAL_RANGE = 100f;

        private Transform signalOrigin;
        private int trainCarMask;
        private CarHighlighter highlighter;

        private string carID;

        public AddPointAtNothing()
            : base(new CommsRadioState(
                titleText: "Add",
                contentText: "Aim at the car you wish to attach a camera to.",
                actionText: "Cancel",
                buttonBehaviour: ButtonBehaviourType.Override))
        {
            highlighter = new CarHighlighter();
        }

        public override void OnEnter(CommsRadioUtility utility, AStateBehaviour? previous)
        {
            base.OnEnter(utility, previous);
            // Steal some components from other radio modes
            refreshSignalOriginAndTrainCarMask();
        }

        public override AStateBehaviour OnAction(CommsRadioUtility utility, InputAction action)
        {
            if (action != InputAction.Activate)
            {
                throw new ArgumentException();
            }
            utility.PlaySound(VanillaSoundCommsRadio.Cancel);
            return new CameraAdd();
        }

        private void refreshSignalOriginAndTrainCarMask()
        {
            trainCarMask = highlighter.RefreshTrainCarMask();
            signalOrigin = highlighter.RefreshSignalOrigin();
        }

        // Detecting what we're looking at
        public override AStateBehaviour OnUpdate(CommsRadioUtility utility)
        {
            while (signalOrigin is null)
            {
                Main.DebugLog("signalOrigin is null for some reason");
                refreshSignalOriginAndTrainCarMask();
            }

            RaycastHit hit;

            // If we're not pointing at anything
            if (!Physics.Raycast(signalOrigin.position, signalOrigin.forward, out hit, SIGNAL_RANGE, trainCarMask))
            {
                return this;
            }

            // Try to get the car we're pointing at
            TrainCar selectedCar = TrainCar.Resolve(hit.transform.root);

            if (selectedCar is null)
            {
                return this;
            }
            bool isLoco = selectedCar.IsLoco;
            carID = selectedCar.ID;
            if (isLoco || Equals(selectedCar.carLivery.parentType.name,"TrainCarType_S282B"))
            {
                utility.PlaySound(VanillaSoundCommsRadio.HoverOver);
                return new AddPointAtLoco(selectedCar, carID);
            }
            else
            {
                utility.PlaySound(VanillaSoundCommsRadio.HoverOver);
                return new AddPointAtNonLoco(selectedCar, carID);
            }
        }
    }
}