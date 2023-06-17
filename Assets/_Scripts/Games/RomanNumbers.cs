using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace _Scripts.Games
{
    public class RomanNumbers : BaseGame
    {
        #region Serialized Fields

            [SerializeField] private TMP_Text task;
            [SerializeField] private int timeout;

        #endregion Serialized Fields

        #region Fields



        #endregion Fields

        #region Built-Ins / MonoBehaviours

        void Start()
        {
            StartCoroutine(GameCoroutine());
        }

        #endregion Built-Ins / MonoBehaviours

        #region GetSets / Properties



        #endregion GetSets / Properties

        #region Game Mechanics / Methods


            private IEnumerator GameCoroutine()
            {
                yield return new WaitForSeconds(1);
                while (true)
                {
                    
                }
            }


        #endregion Game Mechanics / Methods

        #region Overarching Methods / Helpers

        private string decimalToRoman(int value)
            {
                if ((value < 1) || (value >= 4999)) { return ""; }
                string res = "";

                while (value >= 1000) { value -= 1000; res += "M"; }
                if (value >= 900) { value -= 900; res += "CM"; }

                while (value >= 500) { value -= 500; res += "D"; }
                if (value >= 400) { value -= 400; res += "CD"; }

                while (value >= 100) { value -= 100; res += "C"; }
                if (value >= 90) { value -= 90; res += "XC"; }

                while (value >= 50) { value -= 50; res += "L"; }
                if (value >= 40) { value -= 40; res += "XL"; }

                while (value >= 10) { value -= 10; res += "X"; }
                if (value >= 9) { value -= 9; res += "IX"; }

                while (value >= 5) { value -= 5; res += "V"; }
                if (value >= 4) { value -= 4; res += "IV"; }

                while (value >= 1) { value -= 1; res += "I"; }

                return res;
            }

        #endregion Overarching Methods / Helpers
    }
}