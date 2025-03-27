using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DinkyBossFightManager : MonoBehaviour
{

    [SerializeField] private DinkyBossFightTanks dinkyBossfightTanks;
    [Serializable]
    public class DinkyBossFightTanks {
        public const int MAX_TANK_COUNT = 4;
        public Tank armadilloTank;
        public Tank sundewTank;
        public Tank crabTank;
        public Tank birdTank;

#nullable enable
        public Tank GetRandomInactiveTank() {
            List<Tank> tanks = new() { armadilloTank, sundewTank, crabTank, birdTank };
            List<Tank> inactiveTanks = tanks.FindAll(t => t.IsInactive());
            if (inactiveTanks.Count == 0) {
                Debug.LogWarning("Something went pretty wrong to have you call get random when no tanks are active");
                return armadilloTank;
            }

            return inactiveTanks[UnityEngine.Random.Range(0, inactiveTanks.Count)];
        }
    }
    public struct TankInfo {
        public Tank Tank { get; }
        public float ActivationTime { get; }

        public TankInfo(Tank tank, float activationTime) {
            Tank = tank;
            ActivationTime = activationTime;
        }
    }

    private int currentProgress = 0;
    private List<Tank> activeTanks = new();
    private Wave currentWave = Wave.Get<DialogueWave>();
    public event Action<int>? ProgressChanged;
    public int MaxProgress => Wave.WaveValues.Aggregate(0, (acc, wave) => acc + wave.ActiveTanks(dinkyBossfightTanks).Count);

    private void OnEnable() {
       Tank.TankDeactivated += IncrementProgress;
        Tank.TankActivated += HandleActiveTank;
    }

    private void OnDisable() {
        Tank.TankDeactivated -= IncrementProgress;
        Tank.TankActivated -= HandleActiveTank;
    }

    private void HandleActiveTank(Tank tank) {
        activeTanks.Add(tank);
    }

    private void IncrementProgress(Tank tank) {
        currentProgress += 1;
        activeTanks.Remove(tank);
        ProgressChanged?.Invoke(currentProgress);

        if (currentWave.WaveEndCondition(activeTanks.Count)) {
            StartWave(currentWave.NextWave());
        }
    }

    private void StartWave(Wave newWave) {
        currentWave = newWave;
        StartCoroutine(ActivateTanks(newWave.ActiveTanks(dinkyBossfightTanks)));
    }

    private IEnumerator ActivateTanks(List<TankInfo> tankInfos) {
        foreach (var tankInfo in tankInfos) {
            yield return new WaitForSeconds(tankInfo.ActivationTime);
            if (activeTanks.Count == DinkyBossFightTanks.MAX_TANK_COUNT) {
                Debug.LogWarning("Something went pretty wrong to try and activate a tank when all tanks are active");
                currentProgress += 1; //Attempt to escape softlock condition
                continue;
            }

            if (tankInfo.Tank.IsInactive()) {
                tankInfo.Tank.Activate();
            } else {
                dinkyBossfightTanks.GetRandomInactiveTank().Activate();
            }
        }
    }
}
