using UnityEngine;

namespace _Scripts
{
    [CreateAssetMenu(fileName = "GameAsset", menuName = "ScriptableObjects/GameAsset", order = 1)]
    public class GameAsset : ScriptableObject
    {
        #region Fields

        [SerializeField] private GameObject prefab;
        [SerializeField] private string fullName;
        [TextArea] [SerializeField] private string description;
        [SerializeField] private int origin;
        [SerializeField] private Difficulty difficulty;
        [SerializeField] private Genre genre;
        [SerializeField] private Orientation orientation;
        [SerializeField] private Keys keys;

        #endregion

        #region GetSets

        public GameObject Prefab
        {
            get { return prefab; }
            set { prefab = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public int Origin
        {
            get { return origin; }
            set { origin = value; }
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

        public Orientation Orientation
        {
            get { return orientation; }
            set { orientation = value; }
        }

        public Keys Keys
        {
            get { return keys; }
            set { keys = value; }
        }

        #endregion
    }
}

