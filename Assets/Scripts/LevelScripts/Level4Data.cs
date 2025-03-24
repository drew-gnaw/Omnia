using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Level_4_1 : LevelData {
    public override string PlainName => "Level 4-1";
    public override string SceneName => "W-4-1";
    public override LevelData NextLevel => Get<Level_4_2>();
    public override LevelData PrevLevel => Get<Level_4_1>();
}

public class Level_4_2 : LevelData {
    public override string PlainName => "Level 4-2";
    public override string SceneName => "W-4-2";
    public override LevelData NextLevel => Get<Level_4_3>();
    public override LevelData PrevLevel => Get<Level_4_1>();
}
public class Level_4_3 : LevelData {
    public override string PlainName => "Level 4-3";
    public override string SceneName => "W-4-3";
    public override LevelData NextLevel => Get<Level_4_B>();
    public override LevelData PrevLevel => Get<Level_4_2>();
}
public class Level_4_B : LevelData {
    public override string PlainName => "Level 4-B";
    public override string SceneName => "W-4-B";
    public override LevelData NextLevel => Get<Level_4_B>();
    public override LevelData PrevLevel => Get<Level_4_S>();
}

public class Level_4_S : LevelData {
    public override string PlainName => "Level 4-S";
    public override string SceneName => "W-4-S";
    public override LevelData NextLevel => Get<Level_B_B>();
    public override LevelData PrevLevel => Get<Level_4_B>();
}
