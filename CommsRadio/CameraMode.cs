using CommsRadioAPI;
using DV;
using UnityEngine;

namespace CustomCamera.CommsRadio;

internal static class CameraMode
{
	public static void Create()
	{
		CommsRadioMode.Create(new MainMenu(), Color.gray, (mode) => mode is CommsRadioCrewVehicle);
	}
}