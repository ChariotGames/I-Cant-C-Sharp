
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Represents an abstract structure of a Key.
/// It contains the input event bound to it.
/// Also the string related to the input font icon.
/// </summary>
[CreateAssetMenu(fileName = "Key", menuName = "ScriptableObjects/Key")]
public class Key : ScriptableObject
{
    #region Serialized Fields

    [SerializeField] private InputActionReference input;
    [SerializeField] private string icon;

    #endregion Serialized Fields

    #region GetSets

    public InputActionReference Input { get => input; }
    public string Icon { get => icon; }

    #endregion GetSets
}
