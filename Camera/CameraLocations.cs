using System;
using System.Collections.Generic;
using UnityEngine;

public class CameraLocations
{
    public enum Locations
    {
        Roof_Forward,
        Roof_Backward,
        Left_Backward,
        Left_Forward,
        Right_Backward,
        Right_Forward,
        Head_Knuckle,
        Rear_Knuckle,
        Cab_Left,
        Cab_Right,
        Cab_Controls,
        Custom
    }

    public enum NonLocoLocations
    {
        Top_Forward,
        Top_Backward,
        Head_Knuckle,
        Rear_Knuckle,
        Custom
    }

    public enum Vehicles
    {
        S060,
        S282, // Just attach cameras to the loco, forget the tender
        DE4,
        DE6,
        DE2,
        DM3
    }

    // Loco enum to strings
    public static readonly Dictionary<Locations, string> LocationStrings = new Dictionary<Locations, string>
    {
        { Locations.Roof_Forward, "Roof Forward" },
        { Locations.Roof_Backward, "Roof Backward" },
        { Locations.Left_Backward, "Left Backward" },
        { Locations.Left_Forward, "Left Forward" },
        { Locations.Right_Backward, "Right Backward" },
        { Locations.Right_Forward, "Right Forward" },
        { Locations.Head_Knuckle, "Head Knuckle" },
        { Locations.Rear_Knuckle, "Rear Knuckle" },
        { Locations.Cab_Left, "Cab Left" },
        { Locations.Cab_Right, "Cab Right" },
        { Locations.Cab_Controls, "Dash/Controls" },
        { Locations.Custom, "Custom"}
    };
    // Non-loco enum to strings
    public static readonly Dictionary<NonLocoLocations, string> NonLocoLocationStrings = new Dictionary<NonLocoLocations, string>
    {
        { NonLocoLocations.Top_Forward, "Top Forward" },
        { NonLocoLocations.Top_Backward, "Top Backward" },
        { NonLocoLocations.Head_Knuckle, "Head Knuckle" },
        { NonLocoLocations.Rear_Knuckle, "Rear Knuckle" },
        { NonLocoLocations.Custom, "Custom" },
    };

    public static readonly Dictionary<int, Locations> LocationInts = new Dictionary<int, Locations>
    {
        {0, Locations.Roof_Forward },
        {1, Locations.Roof_Backward },
        {2, Locations.Left_Backward },
        {3, Locations.Left_Forward },
        {4, Locations.Right_Backward},
        {5, Locations.Right_Forward},
        {6, Locations.Head_Knuckle},
        {7, Locations.Rear_Knuckle},
        {8, Locations.Cab_Left},
        {9, Locations.Cab_Right},
        {10, Locations.Cab_Controls},
        {11, Locations.Custom}
    };

    public static readonly Dictionary<int, NonLocoLocations> NonLocoLocationInts = new Dictionary<int, NonLocoLocations>
    {
        {0, NonLocoLocations.Top_Forward },
        {1, NonLocoLocations.Top_Backward },
        {2, NonLocoLocations.Head_Knuckle },
        {3, NonLocoLocations.Rear_Knuckle },
        {4, NonLocoLocations.Custom}
    };

    public static readonly Dictionary<Locations, (Vector3, Quaternion)> S282Offsets = new Dictionary<Locations, (Vector3, Quaternion)>
    {
        {Locations.Roof_Forward, (new Vector3(0, 5, 0), Quaternion.Euler(20, 0, 0)) },
        {Locations.Roof_Backward, (new Vector3(0, 5, 0), Quaternion.Euler(20, 180, 0)) },
        {Locations.Left_Backward, (new Vector3(-2, 1, 4), Quaternion.Euler(0, 170, 0)) },
        {Locations.Left_Forward, (new Vector3(-2, 1, 0), Quaternion.Euler(0, 10, 0)) },
        {Locations.Right_Backward, (new Vector3(2, 1, 4), Quaternion.Euler(0, -170, 0)) },
        {Locations.Right_Forward, (new Vector3(2, 1, 0), Quaternion.Euler(0, 170, 0)) },
        {Locations.Head_Knuckle, (new Vector3(0, 5, 12.25f), Quaternion.Euler(90, 0, 0)) },
        {Locations.Rear_Knuckle, (new Vector3(0, 5, -9.6f), Quaternion.Euler(90, 180, 0)) },
        {Locations.Cab_Left, (new Vector3(-1.4f, 3.5f, -1.6f), Quaternion.Euler(30, 40, 0)) },
        {Locations.Cab_Right, (new Vector3(1.4f, 3.5f, -1.6f), Quaternion.Euler(30, -40, 0)) },
        {Locations.Cab_Controls, (new Vector3(1, 2.5f, -1.6f), Quaternion.Euler(0, -15, 0)) }
    };

    public static readonly Dictionary<Locations, (Vector3, Quaternion)> S060Offsets = new Dictionary<Locations, (Vector3, Quaternion)>
    {
        {Locations.Roof_Forward, (new Vector3(0, 5, -3), Quaternion.Euler(20, 0, 0)) },
        {Locations.Roof_Backward, (new Vector3(0, 5, -2.5f), Quaternion.Euler(20, 180, 0)) },
        {Locations.Left_Backward, (new Vector3(-1.6f, 1, 3), Quaternion.Euler(0, 170, 0)) },
        {Locations.Left_Forward, (new Vector3(-1.6f, 1, -3), Quaternion.Euler(0, 10, 0)) },
        {Locations.Right_Backward, (new Vector3(1.6f, 1, 3), Quaternion.Euler(0, -170, 0)) },
        {Locations.Right_Forward, (new Vector3(1.6f, 1, -3), Quaternion.Euler(0, -10, 0)) },
        {Locations.Head_Knuckle, (new Vector3(0, 4, 4.5f), Quaternion.Euler(90, 0, 0)) },
        {Locations.Rear_Knuckle, (new Vector3(0, 4, -4.5f), Quaternion.Euler(90, -180, 0)) },
        {Locations.Cab_Left, (new Vector3(-1, 3.7f, -3.4f), Quaternion.Euler(33, 35, 0)) },
        {Locations.Cab_Right, (new Vector3(1, 3.7f, -3.4f), Quaternion.Euler(33, -35, 0)) },
        {Locations.Cab_Controls, (new Vector3(.7f, 3, -3.4f), Quaternion.Euler(20, -10, 0)) }
    };

    public static readonly Dictionary<Locations, (Vector3, Quaternion)> DE2Offsets = new Dictionary<Locations, (Vector3, Quaternion)>
    {
        {Locations.Roof_Forward, (new Vector3(0, 4, .9f), Quaternion.Euler(20, 0, 0)) },
        {Locations.Roof_Backward, (new Vector3(0, 4, -1.2f), Quaternion.Euler(20, 180, 0)) },
        {Locations.Left_Backward, (new Vector3(-1.8f, 1, -1.2f), Quaternion.Euler(0, 180, 0)) },
        {Locations.Left_Forward, (new Vector3(-2f, 1, 1), Quaternion.Euler(0, 0, 0)) },
        {Locations.Right_Backward, (new Vector3(1.7f, 1, -1.2f), Quaternion.Euler(0, 180, 0)) },
        {Locations.Right_Forward, (new Vector3(2f, 1, 1), Quaternion.Euler(0, 0, 0)) },
        {Locations.Head_Knuckle, (new Vector3(0, 4, 3.8f), Quaternion.Euler(90, 0, 0)) },
        {Locations.Rear_Knuckle, (new Vector3(0, 4, -3.8f), Quaternion.Euler(90, 180, 0)) },
        {Locations.Cab_Left, (new Vector3(-1, 3.5f, -1.3f), Quaternion.Euler(33, 50, 0)) },
        {Locations.Cab_Right, (new Vector3(1, 3.5f, -1.3f), Quaternion.Euler(33, -50, 0)) },
        {Locations.Cab_Controls, (new Vector3(0.4f, 2.9f, -.45f), Quaternion.Euler(34f, 0, 0)) }
    };

    public static readonly Dictionary<Locations, (Vector3, Quaternion)> DH4Offsets = new Dictionary<Locations, (Vector3, Quaternion)>
    {
        {Locations.Roof_Forward, (new Vector3(0, 4, 1), Quaternion.Euler(20, 0, 0)) },
        {Locations.Roof_Backward, (new Vector3(0, 4, -2), Quaternion.Euler(20, 180, 0)) },
        {Locations.Left_Backward, (new Vector3(-1.7f, 1.2f, 2f), Quaternion.Euler(0, 170, 0)) },
        {Locations.Left_Forward, (new Vector3(-1.7f, 1.2f, -2f), Quaternion.Euler(0, 10, 0)) },
        {Locations.Right_Backward, (new Vector3(1.7f, 1.2f, 2f), Quaternion.Euler(0, -170, 0)) },
        {Locations.Right_Forward, (new Vector3(1.7f, 1.2f, -2f), Quaternion.Euler(0, -10, 0)) },
        {Locations.Head_Knuckle, (new Vector3(0, 4, 6.4f), Quaternion.Euler(90, 0, 0)) },
        {Locations.Rear_Knuckle, (new Vector3(0, 4, -6.4f), Quaternion.Euler(90, 180, 0)) },
        {Locations.Cab_Left, (new Vector3(-1, 4, -1.9f), Quaternion.Euler(30, 40, 0)) },
        {Locations.Cab_Right, (new Vector3(1, 4, -1.9f), Quaternion.Euler(30, -40, 0)) },
        {Locations.Cab_Controls, (new Vector3(.7f, 3.5f, -1.1f), Quaternion.Euler(30, 0, 0)) }
    };

    public static readonly Dictionary<Locations, (Vector3, Quaternion)> DE6Offsets = new Dictionary<Locations, (Vector3, Quaternion)>
    {
        {Locations.Roof_Forward, (new Vector3(0, 5, 5f), Quaternion.Euler(20, 0, 0)) },
        {Locations.Roof_Backward, (new Vector3(0, 5, -6f), Quaternion.Euler(20, 180, 0)) },
        {Locations.Left_Backward, (new Vector3(-1.8f, 1, -1.2f), Quaternion.Euler(0, 180, 0)) },
        {Locations.Left_Forward, (new Vector3(-2f, 1, 1), Quaternion.Euler(0, 0, 0)) },
        {Locations.Right_Backward, (new Vector3(1.7f, 1, -1.2f), Quaternion.Euler(0, 180, 0)) },
        {Locations.Right_Forward, (new Vector3(2f, 1, 1), Quaternion.Euler(0, 0, 0)) },
        {Locations.Head_Knuckle, (new Vector3(0, 4, 9.5f), Quaternion.Euler(90, 0, 0)) },
        {Locations.Rear_Knuckle, (new Vector3(0, 4, -9.5f), Quaternion.Euler(90, 180, 0)) },
        {Locations.Cab_Left, (new Vector3(-1.2f, 3.5f, 5.1f), Quaternion.Euler(33, 50, 0)) },
        {Locations.Cab_Right, (new Vector3(1.2f, 3.5f, 5.1f), Quaternion.Euler(33, -50, 0)) },
        {Locations.Cab_Controls, (new Vector3(1.48f, 2.64f, 5.75f), Quaternion.Euler(0, -60, 0)) }
    };

    public static readonly Dictionary<Locations, (Vector3, Quaternion)> DM3Offsets = new Dictionary<Locations, (Vector3, Quaternion)>
    {
        {Locations.Roof_Forward, (new Vector3(0, 3.8f, -1.4f), Quaternion.Euler(20, 0, 0)) },
        {Locations.Roof_Backward, (new Vector3(0, 4, -3.5f), Quaternion.Euler(20, 180, 0)) },
        {Locations.Left_Backward, (new Vector3(-1.4f, .6f, 2.95f), Quaternion.Euler(0, 170, 0)) },
        {Locations.Left_Forward, (new Vector3(-1.4f, .6f, -2.95f), Quaternion.Euler(0, 10, 0)) },
        {Locations.Right_Backward, (new Vector3(1.4f, .6f, 2.95f), Quaternion.Euler(0, -170, 0)) },
        {Locations.Right_Forward, (new Vector3(1.4f, .6f, -2.95f), Quaternion.Euler(0, -10, 0)) },
        {Locations.Head_Knuckle, (new Vector3(0, 0, 4.25f), Quaternion.Euler(90, 0, 0)) },
        {Locations.Rear_Knuckle, (new Vector3(0, 4, -4.25f), Quaternion.Euler(90, 180, 0)) },
        {Locations.Cab_Left, (new Vector3(-1, 3.6f, -3.5f), Quaternion.Euler(30, 40, 0)) },
        {Locations.Cab_Right, (new Vector3(1, 3.6f, -3.5f), Quaternion.Euler(30, -40, 0)) },
        {Locations.Cab_Controls, (new Vector3(.6f, 3.3f, -2.8f), Quaternion.Euler(20, -10, 0)) }
    };

    public static readonly Dictionary<NonLocoLocations, (Vector3, Quaternion)> TankerOffsets = new Dictionary<NonLocoLocations, (Vector3, Quaternion)>
    {
        {NonLocoLocations.Top_Forward, (new Vector3(0, 4, 4), Quaternion.Euler(20, 0, 0)) },
        {NonLocoLocations.Top_Backward, (new Vector3(0, 4, -4), Quaternion.Euler(20, 180, 0)) },
        {NonLocoLocations.Head_Knuckle, (new Vector3(0, 4, 7.5f), Quaternion.Euler(90, 0, 0)) },
        {NonLocoLocations.Rear_Knuckle, (new Vector3(0, 4, -7.5f), Quaternion.Euler(90, 180, 0)) },
    };

    public static readonly Dictionary<NonLocoLocations, (Vector3, Quaternion)> FlatCarOffsets = new Dictionary<NonLocoLocations, (Vector3, Quaternion)>
    {
        {NonLocoLocations.Top_Forward, (new Vector3(0, 5.5f, 4), Quaternion.Euler(20, 0, 0)) },
        {NonLocoLocations.Top_Backward, (new Vector3(0, 5.5f, -4), Quaternion.Euler(20, 180, 0)) },
        {NonLocoLocations.Head_Knuckle, (new Vector3(0, 4, 9), Quaternion.Euler(90, 0, 0)) },
        {NonLocoLocations.Rear_Knuckle, (new Vector3(0, 4, -9), Quaternion.Euler(90, 180, 0)) },
    };

    public static readonly Dictionary<NonLocoLocations, (Vector3, Quaternion)> BoxCarOffsets = new Dictionary<NonLocoLocations, (Vector3, Quaternion)>
    {
        {NonLocoLocations.Top_Forward, (new Vector3(0, 4, 4), Quaternion.Euler(20, 0, 0)) },
        {NonLocoLocations.Top_Backward, (new Vector3(0, 4, -4), Quaternion.Euler(20, 180, 0)) },
        {NonLocoLocations.Head_Knuckle, (new Vector3(0, 4, 7), Quaternion.Euler(90, 0, 0)) },
        {NonLocoLocations.Rear_Knuckle, (new Vector3(0, 4, -7), Quaternion.Euler(90, 180, 0)) },
    };

    public static readonly Dictionary<NonLocoLocations, (Vector3, Quaternion)> HopperOffsets = new Dictionary<NonLocoLocations, (Vector3, Quaternion)>
    {
        {NonLocoLocations.Top_Forward, (new Vector3(0, 5.5f, 4), Quaternion.Euler(20, 0, 0)) },
        {NonLocoLocations.Top_Backward, (new Vector3(0, 5.5f, -4), Quaternion.Euler(20, 180, 0)) },
        {NonLocoLocations.Head_Knuckle, (new Vector3(0, 4, 9.25f), Quaternion.Euler(90, 0, 0)) },
        {NonLocoLocations.Rear_Knuckle, (new Vector3(0, 4, -9.25f), Quaternion.Euler(90, 180, 0)) },
    };

    public static readonly Dictionary<string, string> TrainCarNames = new Dictionary<string, string>
    {
        {"TrainCarType_LocoS060","S060"},
        {"TrainCarType_LocoS282A","S282"},
        {"TrainCarType_S282B","S282"},
        {"TrainCarType_LocoDE6","DE6"},
        {"TrainCarType_LocoDH4","DH4"},
        {"TrainCarType_LocoDE2","DE2"},
        {"TrainCarType_LocoDM3","DM3"},
        {"TrainCarType_Refrigerator","Refrigerator"},
        {"TrainCarType_BoxCar","Box car"},
        {"TrainCarType_Stock","Stock car"},
        {"TrainCarType_TankGas","Tanker"},
        {"TrainCarType_TankGasChem","Tanker"},
        {"TrainCarType_TankGasOil","Tanker"},
        {"TrainCarType_Flatbed","Flatbed"},
        {"TrainCarType_FlatbedStakes","Flatbed"},
        {"TrainCarType_Hopper","Hopper"}
    };

    public static string GetContextText(TrainCar trainCar)
    {
        try
        {
            return $"Add camera to {TrainCarNames[trainCar.carLivery.parentType.name]}?";
        }
        catch
        {
            Debug.Log($"No traincarname for {trainCar.carLivery.parentType.name}");
            return "Add camera to selected car?";
        }
    }
    // TODO: Add AutoRack offsets
}