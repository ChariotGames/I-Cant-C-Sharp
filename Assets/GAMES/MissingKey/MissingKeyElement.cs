using UnityEngine.InputSystem;

namespace Scripts.Games
{
    public class MissingKeyElement : BasePressElement
    {
        #region Fields

        private MissingKey _game;

        #endregion Fields

        void Awake()
        {
            _game = (MissingKey)parent;
        }

        public override void ButtonPressed(InputAction.CallbackContext ctx)
        {
            gameObject.SetActive(false);
        }
    }
}