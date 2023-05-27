using UnityEngine;

namespace _Scripts
{
    [CreateAssetMenu(fileName = "GameAsset", menuName = "ScriptableObjects/GameAsset", order = 1)]
    public class GameAsset : ScriptableObject
    {
        #region Serialized Fields

        [SerializeField] private string assetName;
        [TextArea(3, 10)] [SerializeField] private string description;
        [SerializeField] private Difficulty difficulty;
        [SerializeField] private Genre genre;
        [SerializeField] private Keys keys;
        [SerializeField] private Orientation orientation;
        [SerializeField] private GameObject prefab;
        [SerializeField] private string version;

        #endregion

        #region Fields

        private int origin;

        #endregion

        #region GetSets

        public string Asset
        {
            get { return assetName; }
            set { assetName = value; }
        }

        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        public Difficulty Difficulty
        {
            get { return difficulty; }
            set { difficulty = value; }
        }

        public Genre Genre
        {
            get { return genre; }
            set { genre = value; }
        }

        public Keys Keys
        {
            get { return keys; }
            set { keys = value; }
        }

        public Orientation Orientation
        {
            get { return orientation; }
            set { orientation = value; }
        }

        public GameObject Prefab
        {
            get { return prefab; }
            set { prefab = value; }
        }

        public int Origin
        {
            get { return origin; }
            set { origin = value; }
        }

        #endregion
    }
}

