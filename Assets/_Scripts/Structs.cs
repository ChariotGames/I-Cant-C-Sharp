using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Scripts
{
    [Serializable]
    public struct KeyMap
    {
        [SerializeField] private InputActionReference one, two, three, four;
        public InputActionReference One { get => one; set => one = value; }
        public InputActionReference Two { get => two; set => two = value; }
        public InputActionReference Three { get => three; set => three = value; }
        public InputActionReference Four { get => four; set => four = value; }
        public InputActionReference[] All { get => new[] { One, Two, Three, Four }; }
    }

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