using _Scripts.Games;
using UnityEngine.InputSystem;

namespace _Scripts.GameElements
{
    public class ButtonHeroElement : BasePressElement
    {
        #region Fields

        private ButtonHero _game;

        #endregion Fields

        private void Awake()
        {
            _game = (ButtonHero)parent;
        }

        public override void ButtonPressed(InputAction.CallbackContext ctx)
        {
            gameObject.SetActive(false);
            _game.ResetTimer2();
        }
    }
}
