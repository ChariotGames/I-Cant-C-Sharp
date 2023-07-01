using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Scripts.Games
{
    /// <summary>
    /// A row of buttons move from right to left with predefined spacing in between that define the rhythm.
    /// A Square in the middle defines the area where the buttons needs to be pressed when entering the Square.
    /// 
    /// Easy: Button comes in rhythm.
    /// Medium: Buttons randomly and suddenly disappear before reaching the Square.
    /// Hard: Other buttons appear that need to be ignored.
    /// </summary>
    public class BeatWalker : BaseGame
    {
        #region Serialized Fields

        [SerializeField] private GameObject buttonPrefab;
        [SerializeField] private List<GameObject> distractionButtons;

        #endregion Serialized Fields

        #region Fields

        private GameObject button;
        private Camera _mainCamera;
        
        private Bounds _cameraViewportBounds;
        private float _speed = 5f, _width, _instantiationDelay; //= .5f;
        private bool _lose;
        private List<GameObject> _instantiatedButtons = new();

        #endregion Fields

        #region Built-Ins / MonoBehaviours

        private void Awake()
        {
            _mainCamera = Camera.main;
            _cameraViewportBounds = new Bounds(_mainCamera.transform.position, _mainCamera.ViewportToWorldPoint(new Vector3(1f, 1f, 0f)) - _mainCamera.ViewportToWorldPoint(Vector3.zero));
            _width = _cameraViewportBounds.size.x;
            _lose = false;
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
            _instantiationDelay = Random.Range(0.5f, 2);
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

            for (int i = _instantiatedButtons.Count - 1; i >= 0; i--)
            {
                GameObject b = _instantiatedButtons[i];
                b.transform.Translate(_speed * Time.deltaTime * Vector3.left);

                if (b.transform.position.x < -_width / 2)
                {
                    Destroy(b);
                    _instantiatedButtons.RemoveAt(i);
                }
            }
        }

        #endregion Built-Ins / MonoBehaviours

        #region GetSets / Properties


        #endregion GetSets / Properties

        #region Game Mechanics / Methods



        #endregion Game Mechanics / Methods

        #region Overarching Methods / Helpers

        private IEnumerator InstantiateButtonsWithDelay()
        {
            Vector3 spawnPos = new(transform.position.x + _width / 2, transform.position.y, 0f);
            while (_lose == false)
            {
                button = Instantiate(buttonPrefab, spawnPos, Quaternion.identity);

                _instantiatedButtons.Add(button);

                yield return new WaitForSeconds(_instantiationDelay);
            }
        }

        #endregion Overarching Methods / Helpers
    }
}