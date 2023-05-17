using UnityEngine;

namespace _Scripts.Games
{
    public class Game : MonoBehaviour
    {
        private int origin;
        private Difficulty difficulty = Difficulty.LVL1;

        #region Methods
        internal void Win(GameObject game)
        {
            SendMessageUpwards("WinCondition", game);
        }

        internal void Lose(GameObject game)
        {
            SendMessageUpwards("WinCondition", game);
        }

        internal void UpdateDifficulty(object[] paramArray)
        {
            SendMessageUpwards("SetDifficulty", paramArray);
        }

        #endregion

        #region GetSets

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

        #endregion
    }
}