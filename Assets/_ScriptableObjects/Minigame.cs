using UnityEngine;

namespace _Scripts
{
    [CreateAssetMenu(fileName = "Minigame", menuName = "ScriptableObjects/Minigame", order = 1)]
    public class Minigame : ScriptableObject
    {
        #region Serialized Fields

        [SerializeField] private AssetID assetID;
        [TextArea(3, 10)] [SerializeField] private string description;
        [SerializeField] private Complexity complexity;
        [SerializeField] private Difficulty difficulty;
        [SerializeField] private Genre genre;
        [SerializeField] private Keys keysFirst, keysSecond;
        [SerializeField] private KeyMap keysMain, keysAux;
        [SerializeField] private Orientation orientation;
        [SerializeField] private GameObject prefab;
        [SerializeField] private Sprite preview;

        #endregion Serialized Fields

        #region GetSets
        public AssetID AssetID
        {
            get => assetID;
        }

        public Complexity Complexity
        {
            get => complexity;
        }

        public string Description
        {
            get => description;
        }

        public Difficulty Difficulty
        {
            get => difficulty;
            set => difficulty = (Difficulty)Mathf.Clamp((int)value, (int)Difficulty.EASY, (int)Difficulty.HARD);
        }

        public Genre Genre
        {
            get => genre;
        }

        public Keys KeysFirst
        {
            get => keysFirst;
        }

        public Keys KeysSecond
        {
            get => keysSecond;
        }

        public KeyMap KeysMain
        {
            get => keysMain;
        }

        public KeyMap KeysAux
        {
            get => keysAux;
        }

        public Orientation Orientation
        {
            get => orientation;
        }

        public GameObject Prefab
        {
            get => prefab;
        }

        public Sprite Preview
        {
            get => preview;
        }

        #endregion GetSets
    }
}

