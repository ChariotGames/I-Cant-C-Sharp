using System;

namespace _Scripts
{
    #region Enums

    public enum AssetID
    {
        NONE = 0,
        G00_AutoRun = 23553,
        G01_MagicCircle = 16897,
        G02_MissingKey = 32011,
        G03_SameAsBefore = 20227,
        G04_Simon = 11952,
        G05_ArrowTerror = 19961,
        G06_RapidReflex = 27261,
        G07_NumbersAndSmileys = 30998,
        G08_FallingObstacles = 11042,
        G09_UnnamedGame = 28674,
        G10_UnnamedGame = 15108,
        G11_UnnamedGame = 12293,
        G12_UnnamedGame = 18439,
        G13_UnnamedGame = 21513,
        G14_UnnamedGame = 26890,
        G15_UnnamedGame = 32267,
        G16_UnnamedGame = 16140,
        G17_UnnamedGame = 22287,
        G18_UnnamedGame = 22548,
        G19_UnnamedGame = 14357,
        G20_UnnamedGame = 28439,
        G21_UnnamedGame = 25627,
        G22_UnnamedGame = 23838,
        G23_UnnamedGame = 20512,
        G24_UnnamedGame = 23845,
        G25_UnnamedGame = 10022,
        G26_UnnamedGame = 29740,
        G27_UnnamedGame = 31276,
        G28_UnnamedGame = 17709,
        G29_UnnamedGame = 27950,
        G30_UnnamedGame = 24624,
        G31_UnnamedGame = 29494,
        G32_UnnamedGame = 13879,
        G33_UnnamedGame = 24120,
        G34_UnnamedGame = 16186,
        G35_UnnamedGame = 27198,
        G36_UnnamedGame = 25150,
        G37_UnnamedGame = 12612,
        G38_UnnamedGame = 25418,
        G39_UnnamedGame = 22091,
        G40_UnnamedGame = 16205,
        G41_UnnamedGame = 11597,
        G42_UnnamedGame = 30549,
        G43_UnnamedGame = 29527,
        G44_UnnamedGame = 19034,
        G45_UnnamedGame = 15450,
        G46_UnnamedGame = 24412,
        G47_UnnamedGame = 31326,
        G48_UnnamedGame = 19550,
        G49_UnnamedGame = 13919,
        G50_UnnamedGame = 27233,
        G51_UnnamedGame = 15971,
        G52_UnnamedGame = 12900,
        G53_UnnamedGame = 17260,
        G54_UnnamedGame = 26479,
        G55_UnnamedGame = 32625,
        G56_UnnamedGame = 20346,
        G57_UnnamedGame = 18299,
        G58_UnnamedGame = 14460,
        G59_UnnamedGame = 23167,
        G60_UnnamedGame = 16521,
        G61_UnnamedGame = 10638,
        G62_UnnamedGame = 12690,
        G63_UnnamedGame = 12182,
        G64_UnnamedGame = 16022,
        G65_UnnamedGame = 16534,
        G66_UnnamedGame = 21144,
        G67_UnnamedGame = 16795,
        G68_UnnamedGame = 26268,
        G69_UnnamedGame = 19357,
        G70_UnnamedGame = 30886,
        G71_UnnamedGame = 25769,
        G72_UnnamedGame = 16557,
        G73_UnnamedGame = 10926,
        G74_UnnamedGame = 15281,
        G75_UnnamedGame = 13491,
        G76_UnnamedGame = 17852,
        G77_UnnamedGame = 13502,
        G78_UnnamedGame = 32446,
        G79_UnnamedGame = 19908,
        G80_UnnamedGame = 14532,
        G81_UnnamedGame = 18381,
        G82_UnnamedGame = 32718,
        G83_UnnamedGame = 18384,
        G84_UnnamedGame = 23763,
        G85_UnnamedGame = 11221,
        G86_UnnamedGame = 10715,
        G87_UnnamedGame = 24795,
        G88_UnnamedGame = 13020,
        G89_UnnamedGame = 30685,
        G90_UnnamedGame = 24798,
        G91_UnnamedGame = 15584,
        G92_UnnamedGame = 24039,
        G93_UnnamedGame = 21735,
        G94_UnnamedGame = 26091,
        G95_UnnamedGame = 17899,
        G96_UnnamedGame = 17652,
        G97_UnnamedGame = 18684,
        G98_UnnamedGame = 27134,
        G99_UnnamedGame = 31743,
    }

    public enum Colors
    {
        NONE, BLUE, RED, YELLOW, GREEN, PURPLE, ANY
    }

    public enum Complexity
    {
        NONE, SOLO, MIDDLE, MIX
    }

    public enum Difficulty
    {
        LVL1, LVL2, LVL3, LVL4, LVL5, LVL6, LVL7, LVL8, LVL9
    }

    public enum Direction
    {
        NONE = -1, UP, DOWN, LEFT, RIGHT, CENTER
    }

    public enum Mode
    {
        NONE, ENDLESS, SINGLE, MIXED, TUTORIAL
    }

    public enum Modifier
    {
        NONE, NORMAL, REVERSE, DOUBLE, EXPONENTIAL, MIRROR, NEGATIVE, TIME, ROTATION, SPEED, ALTERNATE, BEFORE, ROULETTE, OK
    }

    public enum Orientation
    {
        NONE, HORIZONTAL, VERTICAL, QUARTER, FULLSCREEN
    }

    public enum Type
    {
        NONE, PLAYER, ENEMY, CHECKPOINT, GOAL
    }

    #endregion

    #region Flags

    [Flags]
    public enum Genre
    {
        NONE = 0, MEMORY = 1, REACTION = 2, COGNITIVE = 4, RHYTHM = 8
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