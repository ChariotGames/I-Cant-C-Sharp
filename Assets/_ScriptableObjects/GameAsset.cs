using UnityEngine;
using UnityEngine.UI;

namespace _Scripts
{
    [CreateAssetMenu(fileName = "GameAsset", menuName = "ScriptableObjects/GameAsset", order = 1)]
    public class GameAsset : ScriptableObject
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
        [SerializeField] private Image screenshot;

        #endregion Serialized Fields

        #region GetSets
        public AssetID AssetID
        {
            get { return assetID; }
        }

        public Complexity Complexity
        {
            get { return complexity; }
        }

        public string Description
        {
            get { return description; }
        }

        public Difficulty Difficulty
        {
            get { return difficulty; }
            set { difficulty = value; }
        }

        public Genre Genre
        {
            get { return genre; }
        }

        public Keys Keys
        {
            get { return keys; }
        }

        public Orientation Orientation
        {
            get { return orientation; }
        }

        public GameObject Prefab
        {
            get { return prefab; }
        }

        public Image Screenshot
        {
            get { return screenshot; }
        }

        #endregion GetSets
    }
}

