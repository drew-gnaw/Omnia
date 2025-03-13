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

public class Level_1_1 : LevelData {
    public override string PlainName => "Level 1-1";
    public override string SceneName => "W-1-1";
    public override LevelData NextLevel => Get<PuzzleTestScene>();
    public override LevelData PrevLevel => Get<MainScene>();
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
