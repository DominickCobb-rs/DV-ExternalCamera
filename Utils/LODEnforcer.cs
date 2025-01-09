using UnityEngine;

public class LODEnforcer : MonoBehaviour
{
    private TrainCar currentTrainCar;

    void Start()
    {
        PlayerManager.CarChanged += OnCarChanged;
    }

    void OnDisable()
    {
        if (currentTrainCar != null)
        {
            TrainPhysicsLod trainLod = currentTrainCar.GetComponent<TrainPhysicsLod>();
            if (trainLod != null)
            {
                trainLod.UnlockHighestLOD();
                trainLod.UpdateLod(null);
            }
        }
    }

    void OnDestroy()
    {
        PlayerManager.CarChanged -= OnCarChanged;
    }

    private void OnCarChanged(TrainCar newCar)
    {
        if (newCar != null)
        {
            currentTrainCar = newCar;
            EnsureHighLOD(currentTrainCar);
        }
        else if (currentTrainCar != null)
        {
            PreventLowLOD(currentTrainCar);
        }
    }

    private void EnsureHighLOD(TrainCar trainCar)
    {
        TrainPhysicsLod trainLod = trainCar.GetComponent<TrainPhysicsLod>();
        if (trainLod != null)
        {
            trainLod.LockHighestLOD();
        }

        if (!trainCar.IsInteriorLoaded)
        {
            trainCar.LoadInterior();
        }
    }

    private void PreventLowLOD(TrainCar trainCar)
    {
        TrainPhysicsLod trainLod = trainCar.GetComponent<TrainPhysicsLod>();
        if (trainLod != null)
        {
            trainLod.LockHighestLOD();
        }

        if (!trainCar.IsInteriorLoaded)
        {
            trainCar.LoadInterior();
        }
    }
}
