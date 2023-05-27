using System.Collections.Generic;
using UnityEngine;

namespace _Scripts
{
    [CreateAssetMenu(fileName = "GameAsset", menuName = "ScriptableObjects/GameAsset", order = 1)]
    public class GameAsset : ScriptableObject
    {
        #region Serialized Fields

        [SerializeField] private AssetID assetID;
        [TextArea(3, 10)] [SerializeField] private string description;
        [SerializeField] private Difficulty difficulty;
        [SerializeField] private Genre genre;
        [SerializeField] private Keys keys;
        [SerializeField] private Orientation orientation;
        //[SerializeField] private Shape shape;
        [SerializeField] private GameObject prefab;

        #endregion Serialized Fields

        #region GetSets
        public AssetID AssetID
        {
            get { return assetID; }
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

        #endregion GetSets
    }
}

