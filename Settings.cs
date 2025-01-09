using UnityEngine;
using UnityModManagerNet;

namespace DvMod.CustomCamera
{
    public class Settings : UnityModManager.ModSettings, IDrawable
    {
        [Draw("Enable logging")]
        public bool isLoggingEnabled = false;

        [Header("Custom Position")]
        [Draw("Custom position X")] public float customVectorX = 0f;
        [Draw("Custom position Y")] public float customVectorY = 0f;
        [Draw("Custom position Z")] public float customVectorZ = 0f;

        [Header("Custom Rotation")]

        [Draw("Custom rotation X")] public float customRotationX = 0f;
        [Draw("Custom rotation Y")] public float customRotationY = 0f;
        [Draw("Custom rotation Z")] public float customRotationZ = 0f;

        public readonly string? version = Main.modEntry?.Info.Version;

        public override void Save(UnityModManager.ModEntry entry)
        {
            Save(this, entry);
        }

        public void OnChange()
        {

        }

        public void Draw()
        {
            this.Draw(Main.modEntry);
        }
    }
}