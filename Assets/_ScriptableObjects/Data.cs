using UnityEngine;

namespace _Scripts
{
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Data", order = 2)]
    public class Data : ScriptableObject
    {
        #region Fields

        [SerializeField] private int lives = 3;

        #endregion

        #region GetSets

        public int Lives
        {
            get { return lives; }
            set { lives = value; }
        }

        #endregion
    }
}
