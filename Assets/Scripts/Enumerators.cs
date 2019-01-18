﻿public enum ItemQuality
{
    Study,
    Ordinary,
    Superior,
    Masterpiece
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

public enum ItemType
{
    None, OneStone, TwoStone, ThreeStone, FourStone,
    FiveStone, GoldPotion, AmethystPotion, StudyItem, OrdinaryItem,
    SuperiorItem, MasterPieceItem, StudyAdd, OrdinaryAdd, SuperiorAdd,
    MasterpieceAdd
}

/// <summary>
/// Enum for game's state.
/// </summary>
public enum GameState { MainMenu, Ingame, Tetris, Pause, Inventory, GameOver }
/// <summary>
/// Enum for room types.
/// </summary>
public enum RoomType { Start, Item, BothSide, Gold, Amethyst, Boss, Normal }
/// <summary>
/// Enum for room's sprite types.
/// </summary>
public enum RoomSpriteType { Start, Item, BothSide, Gold, Amethyst, Boss, Normal1, Normal2, Nomal3, Normal4, Current }