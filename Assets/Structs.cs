using System;
using UnityEngine;

namespace Scripts.Models
{
    /// <summary>
    /// Contains the game's individual action descriptions for each key.
    /// </summary>
    [Serializable]
    public struct ActionNames
    {
        [SerializeField] private string one, two, three, four;
        public string One { get => one; }
        public string Two { get => two; }
        public string Three { get => three; }
        public string Four { get => four; }
        public string[] All { get => new[] { one, two, three, four }; }
    }

    /// <summary>
    /// Represents a collective structure holding action references a game can use.
    /// Makes button remapping kinda indepedant of a global manager.
    /// Might be obsolete by a better gobal built-in button map management?
    /// </summary>
    [Serializable]
    public struct KeyMap
    {
        [SerializeField] private Key one, two, three, four;

        public Key One { get => one; }
        public Key Two { get => two; }
        public Key Three { get => three; }
        public Key Four { get => four; }
        public Key[] All { get => new[] { one, two, three, four }; }
    }
}