using UnityEngine;

namespace _Scripts
{
    [CreateAssetMenu(fileName = "Game", menuName = "ScriptableObjects/Game", order = 1)]
    public class Game : ScriptableObject
    {
        #region Fields

        [SerializeField] private GameObject prefab;
        [SerializeField] private string fullName;
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

