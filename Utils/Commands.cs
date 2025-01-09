using CommandTerminal;
using DvMod.CustomCamera;
using UnityEngine;
using System.Reflection;

namespace CustomCamera
{
    public static class DebugCommands
    {
        [RegisterCommand("cc.update.vector", Help = "Edits position for custom camera attachment", MinArgCount = 3, MaxArgCount = 3)]
        private static void UpdateVector(CommandArg[] args)
        {
            if (args.Length == 3 && EachArgIsFloat(args))
            {
                Main.customVector = new Vector3(args[0].Float, args[1].Float, args[2].Float);
                Debug.Log($"Set Vector3 to: {Main.customVector}");
            }
            else
            {
                Debug.Log("Invalid arguments. Usage: cc.update.vector <x(float)> <y(float)> <z(float)>");
            }
            return;
        }

        [RegisterCommand("cc.update.rotation", Help = "Edits position for custom camera attachment", MinArgCount = 3, MaxArgCount = 3)]
        private static void UpdateRotation(CommandArg[] args)
        {
            if (args.Length == 3 && EachArgIsFloat(args))
            {
                Main.customQuaternion = Quaternion.Euler(args[0].Float, args[1].Float, args[2].Float);
                Debug.Log($"Set Vector3 to: {Main.customQuaternion}");
            }
            else
            {
                Debug.Log("Invalid arguments. Usage: cc.update.rotation <x(float)> <y(float)> <z(float)>");
            }
            return;
        }

        [RegisterCommand("cc.dump.capture", Help = "Dumps the capture object's components and their values")]
        private static void DumpCapture(CommandArg[] args)
        {
            DumpComponents(CustomCameraUtils.newCameraObject);
        }

        private static bool EachArgIsFloat(CommandArg[] args)
        {
            foreach (CommandArg arg in args)
            {
                try
                {
                    if (arg.Float is float)
                    {
                        continue;
                    }
                }
                catch
                {
                    return false;
                }
            }
            return true;
        }

        public static void DumpComponents(GameObject source)
        {
            if(source==null)
            {
                Debug.LogError("cc.dump.capture source is null");
                return;
            };
            Component[] components = source.GetComponents<Component>();
            foreach (Component component in components)
            {
                if (component is Transform)
                    continue;
                Debug.Log($"Dumping {component.GetType()}");
                DumpComponentValues(component);
                Debug.Log("\n\n");
            }
        }

        public static void DumpComponentValues(Component original)
        {
            System.Type type = original.GetType();
            BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Default;

            // Copy all fields
            FieldInfo[] fields = type.GetFields(flags);
            foreach (FieldInfo field in fields)
            {
                Debug.Log($"Field {field}: {field.GetValue(original)}");
            }

            // Copy all properties
            PropertyInfo[] properties = type.GetProperties(flags);
            foreach (PropertyInfo property in properties)
            {
                if (property.CanWrite && property.CanRead)
                {
                    Debug.Log($"Property{property}: {property.GetValue(original, null)}");
                }
            }
        }
    }
}