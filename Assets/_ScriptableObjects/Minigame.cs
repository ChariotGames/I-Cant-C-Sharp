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
        [SerializeField] private Keys keys;
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
            set => difficulty = value;
        }

        public Genre Genre
        {
            get => genre;
        }

        public Keys Keys
        {
            get => keys;
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

