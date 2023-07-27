using UnityEngine;

namespace Scripts.Models
{
    /// <summary>
    /// Represents a package containing all relevant information
    /// of a single Minigame as a scriptable Object.
    /// </summary>
    [CreateAssetMenu(fileName = "Minigame", menuName = "ScriptableObjects/Minigame")]
    public class Minigame : ScriptableObject
    {
        #region Serialized Fields
        [Header("General Values")]
        //[SerializeField] private AssetID assetID;
        [SerializeField] [TextArea(3, 10)] private string description;
        //[SerializeField] private Complexity complexity;
        [SerializeField] private Difficulty difficulty;
        [SerializeField] private Genre genre;
        //[SerializeField] private Keys keysFirst, keysSecond;
        [Space]
        [Header("Input Related")]
        [SerializeField] protected ActionNames actionNames;
        [SerializeField] private KeyMap keysLeft, keysRight;
        //[SerializeField] private Orientation orientation;
        [Header("Play Related Stuff")]
        [SerializeField] private GameObject prefab;
        [SerializeField] private Sprite preview;
        [SerializeField] private string instructionText;

        #endregion Serialized Fields

        #region Fields

        private KeyMap selectedKeys;

        #endregion

        #region GetSets

        /// <summary>
        /// The game's unique ID.
        /// </summary>
        //public AssetID AssetID
        //{
        //    get => assetID;
        //}

        /// <summary>
        /// The game's complexity.
        /// </summary>
        //public Complexity Complexity
        //{
        //    get => complexity;
        //}

        /// <summary>
        /// The game's description.
        /// </summary>
        public string Description
        {
            get => description;
        }

        /// <summary>
        /// The game's difficulty.
        /// </summary>
        public Difficulty Difficulty
        {
            get => difficulty;
            set => difficulty = (Difficulty)Mathf.Clamp((int)value, (int)Difficulty.EASY, (int)Difficulty.HARD);
        }

        /// <summary>
        /// The game's genre.
        /// </summary>
        public Genre Genre
        {
            get => genre;
        }

        /// <summary>
        /// The game's main keys flags.
        /// </summary>
        //public Keys KeysFirst
        //{
        //    get => keysFirst;
        //}

        /// <summary>
        /// The game's secondary keys flags.
        /// </summary>
        //public Keys KeysSecond
        //{
        //    get => keysSecond;
        //}

        /// <summary>
        /// The game's main key map, used for (Up and) Left positions.
        /// </summary>
        public KeyMap KeysLeft
        {
            get => keysLeft;
        }

        /// <summary>
        /// The game's main key map, used for (Down and) Right positions.
        /// </summary>
        public KeyMap KeysRight
        {
            get => keysRight;
        }

        /// <summary>
        /// Actions associated with this particular game.
        /// </summary>
        public ActionNames ActionNames
        {
            get => actionNames;
            set => actionNames = value;
        }

        /// <summary>
        /// The game's orientation
        /// </summary>
        //public Orientation Orientation
        //{
        //    get => orientation;
        //}

        /// <summary>
        /// The game's actual content game object - the prefab.
        /// </summary>
        public GameObject Prefab
        {
            get => prefab;
        }

        /// <summary>
        /// The game's preview sprite for menu presentation.
        /// </summary>
        public Sprite Preview
        {
            get => preview;
        }

        /// <summary>
        /// The keys selected for load by GameManager.
        /// </summary>
        public KeyMap SelectedKeys
        {
            get => selectedKeys;
            set => selectedKeys = value;
        }

        /// <summary>
        /// The text to display at game start.
        /// </summary>
        public string InstructionText
        {
            get => instructionText;
        }

        #endregion GetSets
    }
}

