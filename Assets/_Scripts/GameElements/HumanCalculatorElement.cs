using _Scripts.Games;
using TMPro;
using UnityEngine.InputSystem;

namespace _Scripts.GameElements
{
    public class HumanCalculatorElement : BasePressElement
    {
       
            #region Fields

            private HumanCalculator _game;
            private TextMeshPro _textMeshPro;

            #endregion Fields

            private void Awake()
            {
                _game = (HumanCalculator)parent;
                _textMeshPro = GetComponent<TextMeshPro>();
            }

            public override void ButtonPressed(InputAction.CallbackContext ctx)
            {
                _game.CheckAnswer(_textMeshPro.text);
                _game.GenerateNewEquation();
            }
        
    }
}