using System.Collections.Generic;
using UnityEngine;

namespace _Scripts
{
    [CreateAssetMenu(fileName = "Settings", menuName = "ScriptableObjects/Settings", order = 2)]
    public class Settings : ScriptableObject
    {
        #region Fields

        [SerializeField] private byte lives = 3, players = 1;
        [SerializeField] private GameMode mode = GameMode.SINGLE;
        [SerializeField] private GameAsset selectedGame;
        [SerializeField] private List<GameAsset> games;

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

        public GameMode Mode
        {
            get { return mode; }
            set { mode = value; }
        }

        public GameAsset SelectedGame
        {
            get { return selectedGame; }
            set { selectedGame = value; }
        }

        public List<GameAsset> Games
        {
            get { return games; }
            set { games = value; }
        }

        #endregion
    }
}
