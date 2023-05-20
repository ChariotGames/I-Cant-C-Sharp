using System;

namespace _Scripts
{
    #region Enums

    public enum Simon
    {
        BLUE, RED, YELLOW, GREEN, EMPTY, DOUBLE, NONE, OK
    }

    public enum Difficulty
    {
        LVL1, LVL2, LVL3, LVL4, LVL5, LVL6, LVL7, LVL8, LVL9
    }

    public enum Direction
    {
        NONE = -1, UP = 0, DOWN = 1, LEFT = 2, RIGHT = 3, CENTER = 4
    }

    #endregion

    #region Flags

    [Flags]
    public enum Genre
    {
        NONE = 0, MEMORY = 1, REACTION = 2, COGNITIVE = 4, RHYTHM = 8
    }

    [Flags]
    public enum Orientation
    {
        NONE = 0, HORIZONTAL = 1, VERTICAL = 2, QUARTER = 4, FULLSCREEN = 8
    }

    [Flags]
    public enum Keys
    {
        NONE = 0,

        RightStick = 1,
        LeftStick = 2,

        UpArrow = 4,
        DownArrow = 8,
        LeftArrow = 16,
        RightArrow = 32,

        NorthButton = 64,
        EastButton = 128,
        SouthButton = 256,
        WestButton = 1024,

        RightShoulder = 2048,
        LeftShoulder = 4096,

        RightTrigger = 8192,
        LeftTrigger = 16384,

        RStickButton = 32768,
        LStickButton = 65536
    }

    #endregion
}