using Puzzle;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DinkyBossFightManager;

public abstract class Wave : Enum<Wave>
{
    public abstract bool WaveEndCondition(int tanksRemaining);
    public abstract List<TankInfo> ActiveTanks(DinkyBossFightTanks tanks);
    public abstract Wave NextWave();

    public static readonly IEnumerable<Wave> WaveValues = Values;
    public static Wave Get<T>() where T : Wave => ParseFromType(typeof(T));
    public static Wave Parse(Type type) => ParseFromType(type);
}

public class DialogueWave : Wave {
    public override List<TankInfo> ActiveTanks(DinkyBossFightTanks tanks) => new();

    public override bool WaveEndCondition(int tanksRemaining) => true;
    public override Wave NextWave() => Get<Wave1>();
}

public class Wave1 : Wave {
    public override List<TankInfo> ActiveTanks(DinkyBossFightTanks tanks) =>
        new() { new TankInfo(tanks.armadilloTank, 0f) };
    public override bool WaveEndCondition(int tanksRemaining) => tanksRemaining == 0;
    public override Wave NextWave() => Get<Wave2>();
}

public class Wave2 : Wave {
    public override List<TankInfo> ActiveTanks(DinkyBossFightTanks tanks) =>
        new() { new TankInfo(tanks.birdTank, 2f), new TankInfo(tanks.crabTank, 0f) };

    public override bool WaveEndCondition(int tanksRemaining) => tanksRemaining == 1;
    public override Wave NextWave() => Get<Wave3>();
}

public class Wave3 : Wave {
    public override List<TankInfo> ActiveTanks(DinkyBossFightTanks tanks) =>
        new() { new TankInfo(tanks.sundewTank, 1f), new TankInfo(tanks.armadilloTank, 5f) };

    public override bool WaveEndCondition(int tanksRemaining) => tanksRemaining == 2;
    public override Wave NextWave() => Get<Wave4>();

}

public class Wave4 : Wave {
    public override List<TankInfo> ActiveTanks(DinkyBossFightTanks tanks) =>
        new() { new TankInfo(tanks.birdTank, 3f), new TankInfo(tanks.crabTank, 5f) };
    public override bool WaveEndCondition(int tanksRemaining) => tanksRemaining == 1;
    public override Wave NextWave() => Get<Wave5>();

}

public class Wave5 : Wave {
    public override List<TankInfo> ActiveTanks(DinkyBossFightTanks tanks) =>
        new() {
            new TankInfo(tanks.sundewTank, 0f),
            new TankInfo(tanks.armadilloTank, 3f),
            new TankInfo(tanks.crabTank, 5f)
        };
    public override bool WaveEndCondition(int tanksRemaining) => tanksRemaining == 0;
    public override Wave NextWave() => Get<Wave6>();
}
public class Wave6 : Wave {
    public override List<TankInfo> ActiveTanks(DinkyBossFightTanks tanks) =>
        new() {
            new TankInfo(tanks.armadilloTank, 0f),
            new TankInfo(tanks.birdTank, 0f),
            new TankInfo(tanks.sundewTank, 0f),
            new TankInfo(tanks.crabTank, 0f)
        };

    public override bool WaveEndCondition(int tanksRemaining) => tanksRemaining == 0;
    public override Wave NextWave() => Get<EndScene>();
}

public class EndScene : Wave {
    public override List<TankInfo> ActiveTanks(DinkyBossFightTanks tanks) => new();
    public override bool WaveEndCondition(int tanksRemaining) => tanksRemaining == 0;
    public override Wave NextWave() => Get<EndScene>();
}

