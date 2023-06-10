using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Scripts.Games
{
    /// <summary>
    /// TODO: Provide a summary of your game here.
    /// To create an auto-generated summary template
    /// type 3 /// slash characters after you have
    /// written your class or method signature.
    /// 
    /// On Level 1, describe the difficulty levels.
    /// On Level 2, keep it short and concise.
    /// On Level 3, maybe one-liners are possible.
    /// </summary>
    public class BeatWalker : Game
    {
        /**
         * TODO: General Structure Ideas:
         * 
         * Try to keep an order of fields from most complex to primitive.
         * GameObject go;
         * struct point;
         * float num;
         * bool truthy;
         * 
         * Constants before variables maybe too.
         * const int TIME_PLANNED_FOR_THIS
         * int timeSpentOnThis
         * 
         * Also from most public to private. Valid for methods too.
         * public
         * internal
         * protected
         * private
         * 
         *  Then only probably by alphabet. If at all
         */

        #region Serialized Fields

        [SerializeField] private GameObject buttonPrefab;
        [SerializeField] private List<GameObject> distractionButtons;

        #endregion Serialized Fields

        #region Fields
        private float speed = 5f;
        private GameObject button;
        
        private Camera _mainCamera;
        
        private Bounds _cameraViewportBounds;
        private float width;
        private float instantiationDelay; //= .5f;
        private bool lose;
        private List<GameObject> instantiatedButtons = new List<GameObject>();

        #endregion Fields

        #region Built-Ins / MonoBehaviours

        private void Awake()
        {
            _mainCamera = Camera.main;
            _cameraViewportBounds = new Bounds(_mainCamera.transform.position, _mainCamera.ViewportToWorldPoint(new Vector3(1f, 1f, 0f)) - _mainCamera.ViewportToWorldPoint(Vector3.zero));
            width = _cameraViewportBounds.size.x;
            lose = false;
        }

        // Start is called before the first frame update
        void Start()
        {
            //TODO get the width of the parent of this game object
            //gameObject.transform.parent.transform.width

            StartCoroutine(InstantiateButtonsWithDelay());
        }
            

        // Update is called once per frame
        void Update()
        {
            instantiationDelay = Random.Range(0.5f, 2);
            /*
            foreach (GameObject b in instantiatedButtons)
            {
                b.transform.Translate(speed * Time.deltaTime * Vector3.left);
                
            }

            foreach (GameObject b in instantiatedButtons)
            {
                if (b.transform.position.x < -width / 2)
                {
                    Destroy(b);
                }
            }
            */

            for (int i = instantiatedButtons.Count - 1; i >= 0; i--)
            {
                GameObject b = instantiatedButtons[i];
                b.transform.Translate(speed * Time.deltaTime * Vector3.left);

                if (b.transform.position.x < -width / 2)
                {
                    Destroy(b);
                    instantiatedButtons.RemoveAt(i);
                }
            }
        }

        #endregion Built-Ins / MonoBehaviours

        #region GetSets / Properties

        
        #endregion GetSets / Properties

        #region Game Mechanics / Methods

       
        /// <summary>
        /// TODO: Provide a summary for the method
        /// </summary>
        /// <param name="param">List the parameters.</param>
        /// <returns>Specify what it returns, if it does so.</returns>

        

        private IEnumerator InstantiateButtonsWithDelay()
        {
            Vector3 spawnPos = new(transform.position.x + width / 2, transform.position.y, 0f);
            while (lose==false)
            {
                button = Instantiate(buttonPrefab, spawnPos, Quaternion.identity);

                instantiatedButtons.Add(button);

                yield return new WaitForSeconds(instantiationDelay);
            }
        }

        #endregion Game Mechanics / Methods

        #region Overarching Methods / Helpers

        

        #endregion Overarching Methods / Helpers
    }
}