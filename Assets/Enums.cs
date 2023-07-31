using System;

namespace Scripts.Models
{
    #region Enums

    public enum AnimType { Win, Lose, ScoreUp, ScoreDown, Fill };

    /// <summary>
    /// Represents a color key to set and check.
    /// </summary>
    public enum Colors
    {
        NONE, BLUE, RED, YELLOW, GREEN, PURPLE, ANY
    }

    /// <summary>
    /// Represents a game's Difficulty setting.
    /// Tutorial is effectively "no difficulty" - meant to learn the game.
    /// </summary>
    public enum Difficulty
    {
        TUTORIAL, EASY, MEDIUM, HARD, VARYING
    }

    /// <summary>
    /// Represents a game modifier seleftion, that can change a games functionality.
    /// </summary>
    public enum Modifier
    {
        NONE, NORMAL, REVERSE, DOUBLE, EXPONENTIAL, MIRROR, NEGATIVE, TIME, ROTATION, SPEED, ALTERNATE, BEFORE, ROULETTE, OK
    }

    /// <summary>
    /// Represents a game element's type.
    /// Used for ArrowTerror firstly and mainly.
    /// </summary>
    public enum ElementType
    {
        NONE, PLAYER, ENEMY, CHECKPOINT, GOAL
    }

    public enum SceneNr
    {
        MainMenu, PlayField, GameOver
    }

    public enum SpawnSide
    {
        Center, Side
    }

    #endregion

    #region Flags

    /// <summary>
    /// Represents genres a game may fall in.
    /// So similar games aren't picked to play together.
    /// Team decided, too similar games, might impair playability.
    /// </summary>
    [Flags]
    public enum Genre
    {
        NONE = 0, MEMORY = 1, REACTION = 2, COGNITIVE = 4, RHYTHM = 8
    }

    #endregion
}