using System;
using System.Collections.Generic;

public abstract class LevelData : Enum<LevelData> {
    public abstract string PlainName { get; }
    public abstract string SceneName { get; }
    public abstract LevelData NextLevel { get; }
    public abstract LevelData PrevLevel { get; }

    public static LevelData Get<T>() where T : LevelData => ParseFromType(typeof(T));
    public static LevelData Parse(Type type) => ParseFromType(type);
    public static readonly Dictionary<string, LevelData> SceneToLevelMap = new();

    static LevelData() {
        InitializeSceneToLevelMap();
    }

    private static void InitializeSceneToLevelMap() {
        foreach (LevelData item in Values) {
            SceneToLevelMap[item.SceneName] = item;
        }
    }
}

public class Title : LevelData {
    public override string PlainName => "Title";
    public override string SceneName => "1_Title";
    public override LevelData NextLevel => Get<Opening>();
    public override LevelData PrevLevel => Get<Title>();
}

public class Opening : LevelData {
    public override string PlainName => "Opening";
    public override string SceneName => "2_Opening";
    public override LevelData NextLevel => Get<Birthday>();
    public override LevelData PrevLevel => Get<Title>();
}

public class Birthday : LevelData {
    public override string PlainName => "Birthday";
    public override string SceneName => "3_Birthday";
    public override LevelData NextLevel => Get<Tutorial>();
    public override LevelData PrevLevel => Get<Opening>();
}

public class Tutorial : LevelData {
    public override string PlainName => "Tutorial";
    public override string SceneName => "4_Tutorial";
    public override LevelData NextLevel => Get<City>();
    public override LevelData PrevLevel => Get<Birthday>();
}

public class City : LevelData {
    public override string PlainName => "City";
    public override string SceneName => "5_City";
    public override LevelData NextLevel => Get<ArmadilloRoom>();
    public override LevelData PrevLevel => Get<Tutorial>();
}

public class ArmadilloRoom : LevelData {
    public override string PlainName => "Armadillo Room";
    public override string SceneName => "6_ArmadilloRoom";
    public override LevelData NextLevel => Get<UncleRoom>();
    public override LevelData PrevLevel => Get<City>();
}

public class UncleRoom : LevelData {
    public override string PlainName => "Uncle Room";
    public override string SceneName => "7_UncleRoom";
    public override LevelData NextLevel => Get<DinkyWasted>();
    public override LevelData PrevLevel => Get<ArmadilloRoom>();
}

public class DinkyWasted : LevelData {
    public override string PlainName => "Dinky Wasted";
    public override string SceneName => "8_DinkyWasted";
    public override LevelData NextLevel => Get<City2>();
    public override LevelData PrevLevel => Get<UncleRoom>();
}

public class City2 : LevelData {
    public override string PlainName => "City 2";
    public override string SceneName => "9_City2";
    public override LevelData NextLevel => Get<ArmadilloRoom2>();
    public override LevelData PrevLevel => Get<DinkyWasted>();
}

public class ArmadilloRoom2 : LevelData {
    public override string PlainName => "Armadillo Room 2";
    public override string SceneName => "10_ArmadilloRoom2";
    public override LevelData NextLevel => Get<Level_1_2>();
    public override LevelData PrevLevel => Get<City2>();
}

public class Level_1_1 : LevelData {
    public override string PlainName => "Level 1-1";
    public override string SceneName => "W-1-1";
    public override LevelData NextLevel => Get<Level_1_2>();
    public override LevelData PrevLevel => Get<MainScene>();
}
public class Level_1_2 : LevelData {
    public override string PlainName => "Level 1-2";
    public override string SceneName => "W-1-2";
    public override LevelData NextLevel => Get<Level_1_3>();
    public override LevelData PrevLevel => Get<Level_1_1>();
}

public class Level_1_3 : LevelData {
    public override string PlainName => "Level 1-3";
    public override string SceneName => "W-1-3";
    public override LevelData NextLevel => Get<Level_1_B>();
    public override LevelData PrevLevel => Get<Level_1_2>();
}
public class Level_1_B : LevelData {
    public override string PlainName => "Level 1-B";
    public override string SceneName => "W-1-B";
    public override LevelData NextLevel => Get<PuzzleTestScene>();
    public override LevelData PrevLevel => Get<Level_1_3>();
}
public class Level_1_S : LevelData {
    public override string PlainName => "Level 1-S";
    public override string SceneName => "W-1-S";
    public override LevelData NextLevel => Get<Level_1_B>();
    public override LevelData PrevLevel => Get<Level_1_B>();
}

public class MainScene : LevelData {
    public override string PlainName => "Test Scene";
    public override string SceneName => "MainScene";
    public override LevelData NextLevel => Get<Level_1_1>();
    public override LevelData PrevLevel => Get<MainScene>();
}

public class PuzzleTestScene: LevelData {
    public override string PlainName => "Puzzle Test Scene";
    public override string SceneName => "Puzzle1";
    public override LevelData NextLevel => Get<MainScene>();
    public override LevelData PrevLevel => Get<MainScene>();
}
