using UnityEngine;

namespace _Scripts
{
    [CreateAssetMenu(fileName = "Settings", menuName = "ScriptableObjects/Settings", order = 2)]
    public class Settings : ScriptableObject
    {
        #region Fields

        [SerializeField] private byte lives = 3, players = 1;
<<<<<<< Updated upstream
        [SerializeField] private GameMode mode = GameMode.SINGLE;
        [SerializeField] private GameAsset selectedGame;
=======
        [SerializeField] private Mode mode = Mode.SINGLE;
        [SerializeField] private GameAsset selectedGame = null;
        [SerializeField] private List<GameAsset> games = new();
>>>>>>> Stashed changes

        #endregion

        #region GetSets

        public byte Lives
        {
            get { return lives; }
            set { lives = value; }
        }
        public byte Players
        {
            get { return players; }
            set { players = value; }
        }

        public Mode Mode
        {
            get { return mode; }
            set { mode = value; }
        }

        public GameAsset SelectedGame
        {
            get { return selectedGame; }
            set { selectedGame = value; }
        }

        #endregion
    }
}
