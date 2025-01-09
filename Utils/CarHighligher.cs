using System;

using DV;

using UnityEngine;

using CommsRadioAPI;
using DvMod.CustomCamera;


namespace CustomCamera
{
	internal class CarHighlighter
	{
		private static readonly Vector3 HIGHLIGHT_BOUNDS_EXTENSION = new Vector3(0.25f, 0.8f, 0f);

		private Transform signalOrigin;
		private int trainCarMask;

		internal TrainCar selectedCar = new();

		private GameObject highlighter = new();

		/*-----------------------------------------------------------------------------------------------------------------------*/

		#region HIGHLIGHTER FUNCTIONS

		// Accepts 2 arguments: Car to highlight, comms radio deleter state
		public void InitHighlighter(TrainCar selectedCar, CommsRadioCarDeleter carDeleter)
		{
			this.selectedCar = selectedCar;
			highlighter = carDeleter.trainHighlighter;
			highlighter.SetActive(false);
			highlighter.transform.SetParent(null);
		}

		// Accepts 3 arguments: CommsRadioUtility, AStateBehaviour, material type bool
		public void StartHighlighter(CommsRadioUtility utility, AStateBehaviour? previous, bool isValid)
		{
			MeshRenderer highlighterRenderer = highlighter.GetComponentInChildren<MeshRenderer>(true);
			if (isValid)
			{
				highlighterRenderer.material = utility.GetMaterial(VanillaMaterial.Valid);
			}
			else
			{
				highlighterRenderer.material = utility.GetMaterial(VanillaMaterial.Invalid);
			}

			highlighter.transform.localScale = selectedCar.Bounds.size + HIGHLIGHT_BOUNDS_EXTENSION;
			Vector3 b = selectedCar.transform.up * (highlighter.transform.localScale.y / 2f);
			Vector3 b2 = selectedCar.transform.forward * selectedCar.Bounds.center.z;
			Vector3 position = selectedCar.transform.position + b + b2;

			highlighter.transform.SetPositionAndRotation(position, selectedCar.transform.rotation);
			highlighter.SetActive(true);
			highlighter.transform.SetParent(selectedCar.transform, true);
		}

		public void StopHighlighter(CommsRadioUtility utility, AStateBehaviour? next)
		{
			highlighter.SetActive(false);
			highlighter.transform.SetParent(null);
		}

		#endregion

		/*-----------------------------------------------------------------------------------------------------------------------*/

		#region COMPONENT STEALERS

		public CommsRadioCarDeleter RefreshCarDeleterComponent()
		{
			ICommsRadioMode? commsRadioMode = ControllerAPI.GetVanillaMode(VanillaMode.Clear);
			if (commsRadioMode is null)
			{
				Main.DebugLog("Could not find CommsRadioCarDeleter");
				throw new NullReferenceException();
			}
			CommsRadioCarDeleter carDeleter = (CommsRadioCarDeleter)commsRadioMode;
			

			return carDeleter;
		}

		public Transform RefreshSignalOrigin()
		{
			CommsRadioCarDeleter carDeleter = RefreshCarDeleterComponent();
			signalOrigin = carDeleter.signalOrigin;
			return signalOrigin;
		}

		public int RefreshTrainCarMask()
		{
			trainCarMask = LayerMask.GetMask(new string[]
			{
			"Train_Big_Collider"
			});

			return trainCarMask;
		}

		#endregion

		/*-----------------------------------------------------------------------------------------------------------------------*/

	}
}