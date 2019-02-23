public enum EnemyDebuffCase
{
    Fire,
    Ice,
    Stun,
    Blind,
    Charm,
    END_POINTER
};

public enum DebuffState
{
    Off,
    On,
    Immune,
}

public enum NumeratedDir
{
    Left = -1,
    Right = 1
};

public enum PlayerDebuffCase
{

}
public enum ItemQuality
{
    Study,
    Ordinary,
    Superior,
    Masterpiece,
	Gold,
	None
}

public enum AddonType
{
    Prop,
    Matter,
    Component,
    Theory
}

public enum LifeStoneType
{
    Normal=1,
    Gold=2,
    Amethyst=3
}

public enum ItemSpawnType
{
	LifeStone, GoldPotion, Item, Addon, LifeStoneFrame
}

public enum ItemType
{
    None, OneStone, TwoStone, ThreeStone, FourStone,
    FiveStone, GoldPotion, AmethystPotion, StudyItem, OrdinaryItem,
    SuperiorItem, MasterPieceItem, StudyAdd, OrdinaryAdd, SuperiorAdd,
    MasterpieceAdd, LifeStoneFrame
}

public enum PlayerState { Idle, Walk, Run, GoingUp, GoingDown, Rope, Attack }

/// <summary>
/// Enum for game's state.
/// </summary>
public enum GameState { MainMenu, Ingame, Tetris, Portal, Inventory, Pause, GameOver }
/// <summary>
/// Enum for room types.
/// </summary>
public enum RoomType { Start, Item, BothSide, Gold, Amethyst, Boss, Normal }
/// <summary>
/// Enum for room's sprite types.
/// </summary>
public enum RoomSpriteType { Item, BothSide, Gold, Amethyst, Boss, Normal1, Normal2, Nomal3, Normal4 }