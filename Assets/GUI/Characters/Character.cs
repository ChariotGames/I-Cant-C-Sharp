using UnityEngine;

namespace Scripts.Models
{
    /// <summary>
    /// Represents a package containing all relevant information
    /// of a single Minigame as a scriptable Object.
    /// </summary>
    [CreateAssetMenu(fileName = "Character", menuName = "ScriptableObjects/Character")]
    public class Character : ScriptableObject
    {
        [SerializeField] private string charName;
        [SerializeField] private Sprite preview;
        [SerializeField] private Sprite icon;

        /// <summary>
        /// The character's name for menu presentation.
        /// </summary>
        public string Name
        {
            get => charName;
        }

        /// <summary>
        /// The character's preview sprite for menu presentation.
        /// </summary>
        public Sprite Preview
        {
            get => preview;
        }

        /// <summary>
        /// The character's mini icon sprite for play presentation.
        /// </summary>
        public Sprite Icon
        {
            get => icon;
        }
    }
}
