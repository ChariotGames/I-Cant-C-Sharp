using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Scripts.Models
{
    /// <summary>
    /// Represents a collective structure holding action references a game can use.
    /// Makes button remapping kinda indepedant of a global manager.
    /// Might be obsolete by a better gobal built-in button map management?
    /// </summary>
    [Serializable]
    public struct KeyMap
    {
        [SerializeField] private Key one, two, three, four;

        public Key One { get => one; set => one = value; }
        public Key Two { get => two; set => two = value; }
        public Key Three { get => three; set => three = value; }
        public Key Four { get => four; set => four = value; }
        public Key[] All { get => new[] { one, two, three, four }; }
    }

    /// <summary>
    /// Represents a collection of Transforms for game spawning positions.
    /// Reduces code complexity and calculations accross games.
    /// </summary>
    [Serializable]
    public struct SpawnPoints
    {
        [SerializeField] private Transform up, down, left, right, center;

        public Transform Up { get => up; set => up = value; }
        public Transform Down { get => down; set => down = value; }
        public Transform Left { get => left; set => left = value; }
        public Transform Right { get => right; set => right = value; }
        public Transform Center { get => center; set => center = value; }
        public Transform[] All { get => new[] { up, down, left, right, center }; }
    }
}