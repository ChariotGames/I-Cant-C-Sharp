using UnityEngine;
using UnityEngine.InputSystem;

namespace Scripts.Games
{
    public class KonamiButton : BasePressElement
    {
        #region Fields

        [SerializeField] private KonamiCode _game;

        #endregion Fields

        void Awake()
        {
            
        }

        public override void ButtonPressed(InputAction.CallbackContext ctx)
        {
            _game.CheckInput(ctx.action.ToString());
        }
    }
}