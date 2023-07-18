using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Scripts.Controllers
{
    public class ScrollSelection : MonoBehaviour
    {

        #region Serialized Fields

            [SerializeField] private GameObject content;
            [SerializeField] private ScrollRect scrollRect;
            [SerializeField] private EventSystem eventSystem;

            #endregion Serialized Fields


        #region Fields

            private const int ROWCOUNT = 3;

        #endregion Fields


        #region Built-Ins / MonoBehaviours

        

        #endregion Built-Ins / MonoBehaviours


        #region Game Mechanics / Methods

        private void focusOnselected()
        {
            int childCount = content.transform.childCount;
            int gameIndex = getGameIndex(childCount);
            //Debug.Log("Game Index: " + gameIndex);
            int rownumber = gameIndex / ROWCOUNT;
            //Debug.Log("Row number: " + rownumber);
            float scrollPosition = (float) rownumber / (float) (childCount/ROWCOUNT);
            //Debug.Log("Scroll position: " + scrollPosition);
            scrollRect.verticalNormalizedPosition = 1 - scrollPosition;
        }

        private int getGameIndex(int childCount)
        {
            for (int i = 0; i < childCount; i++)
            {
                Transform child = content.transform.GetChild(i);
                if (child.GameObject().Equals(eventSystem.currentSelectedGameObject))
                {
                    return i;
                }
            }

            return -1;
        }

        #endregion Game Mechanics / Methods

        #region Overarching Methods / Helpers

            private void OnEnable()
            {
                MainMenuGame.selectionChanged += SelectionChanged;
            }

            private void SelectionChanged()
            {
                //Debug.Log("Selection changed");
                focusOnselected();
            }
            
            private void OnDisable()
            {
                MainMenuGame.selectionChanged -= SelectionChanged;
            }

        #endregion Overarching Methods / Helpers
    }
}
