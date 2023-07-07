using System.Collections;
using System.Collections.Generic;
using Scripts.Models;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

namespace Scripts.Games
{
    public class ColoredTrafficLight : BaseGame
    {
        
        /// <summary>
        /// Colored Traffic lights
        ///Description
        ///    traffic lights with 2 colors appear for a few seconds. player has to memorize them, then they turn black.
        ///    then after a short delay differently colored traffic lights appear and on the middle one a selector appears, player has to select one that has the same colors as one from the start
        ///
        ///    LVL0: 1 traffic light at the start, then 3. only 1 traffic light is correct.
        ///    LVL1: 2 at the start, then 5. only 1 traffic light is correct.
        ///    LVL2: 3 at the start, then 7. 1 traffic lights are correct.
        ///
        ///Win Condition
        ///move the selector to one of the correct traffic lights and let the time run out
        ///Lose Condition
        ///time runs out and a wrong traffic light has been selected
        ///
        ///Default Input
        ///Any 2 buttons to resemble left & right: Dpad left right, face left right, shoulderbuttons, trigger.
        ///    Genre / Type
        ///    memory


        /// </summary>
        #region Serialized Fields
        
        [SerializeField] private GameObject trafficLightPrefab, owner_ref,selector_ref;
        [SerializeField] private List<GameObject> trafficLights;
        [SerializeField] private GameObject simonNot_ref;
        [SerializeField] private GameObject simonOk_ref;
        [SerializeField] private int delaySecondWave = 5;
        [SerializeField] private int timeToSelectLight = 5;

        #endregion Serialized Fields

        #region Fields

        private List<trafficLight> correctColors;
        private List<trafficLight> secondWaveColors;
        private int correctLightAmount = 1;
        private int trafficLightAmount = 3;
        private int selectorIndex;
        private bool gameVariant;
        
        private struct trafficLight
        {
            public Colors colorTop;
            public Colors colorBottom;

            public trafficLight(trafficLight light)
            {
                this.colorTop = light.colorTop;
                this.colorBottom = light.colorBottom;
            }
        }
        
        #endregion Fields

        #region Built-Ins / MonoBehaviours
        void Start()
        {
            StartCoroutine(GameCoroutine());
        }
        
        private IEnumerator GameCoroutine()
        {
            while (true)
            {
                Setup();
                FirstWave();
                yield return new WaitForSeconds(delaySecondWave);
                SecondWave();
                yield return new WaitForSeconds(timeToSelectLight);
                DisableInputs();
                ShowResults();
                yield return new WaitForSeconds(2);
                CheckSelector();
                Reset();
            }
            
        }

        private void Setup()
        {
            //decides which variant of coloredtrafficlights to run
            if (Random.Range(0, 2) == 1) gameVariant = true;
            else gameVariant = false;
            
            //setup
            SetDifficultyVariables();
            SpawnTrafficLights(trafficLightAmount);
        }

        private void FirstWave()
        {
            //set correct color combinations
            correctColors = GenLightColors(correctLightAmount, null);
            SetLightColors(trafficLights, correctColors, true);
        }

        private void SecondWave()
        {
            //Mix correct colors with random generated ones and set them.
            secondWaveColors = MixCorrectColorsIntoWrongOnes(
                GenLightColors(trafficLightAmount, correctColors), correctColors);
            SetLightColors(trafficLights, secondWaveColors, false);
            selector_ref.transform.SetParent(trafficLights[selectorIndex].transform);
            selector_ref.transform.position = selector_ref.transform.parent.transform.position;
            selector_ref.transform.Translate(0,-1.2f,0);
            selector_ref.SetActive(true);
            EnableInputs();
        }

        private void Reset()
        {
            GameObject selector = Instantiate(selector_ref, owner_ref.transform);
            selector_ref = selector;
            //Destroy(owner_ref.transform.GetChild(0));
            GameObject horizontalLayout = owner_ref.transform.GetChild(0).gameObject;
            int count = horizontalLayout.transform.childCount;
            horizontalLayout.transform.DetachChildren();
            for (int i = 0; i < count; ++i)
            {
                Destroy(trafficLights[i]);
            }
            
            
            
            /*for (int i = owner_ref.transform.GetChild(0).transform.childCount-1; i > 0; i--)
            {
                GameObject obj = owner_ref.transform.GetChild(0).transform.GetChild(i).gameObject;
                for (int j = obj.transform.childCount-1; j > 0; j--)
                {
                    Destroy(obj.transform.GetChild(i).gameObject);
                }
                Destroy(obj);
            }*/

            
        }
        

        #region Inputs
        private void EnableInputs()
        {
            _keys.One.Input.action.performed += ButtonPressR;
            _keys.Two.Input.action.performed += ButtonPressL;
        }

        private void DisableInputs()
        {
            _keys.One.Input.action.performed -= ButtonPressR;
            _keys.Two.Input.action.performed -= ButtonPressL;
        }

        private void OnDisable()
        {
            _keys.One.Input.action.performed -= ButtonPressR;
            _keys.Two.Input.action.performed -= ButtonPressL;
        }
        
        public void ButtonPressL(InputAction.CallbackContext ctx)
        {
            if (selectorIndex != 0)
            {
                --selectorIndex;
                selector_ref.transform.SetParent(trafficLights[selectorIndex].transform);
                selector_ref.transform.position = selector_ref.transform.parent.transform.position;
                selector_ref.transform.Translate(0,-1.2f,0);
            }
        }
        
        public void ButtonPressR(InputAction.CallbackContext ctx)
        {
            if (selectorIndex != trafficLights.Count-1)
            {
                ++selectorIndex;
                selector_ref.transform.SetParent(trafficLights[selectorIndex].transform);
                selector_ref.transform.position = selector_ref.transform.parent.transform.position;
                selector_ref.transform.Translate(0,-1.2f,0);
            }
        }
        #endregion Inputs
        

        #endregion Built-Ins / MonoBehaviours

        #region Game Mechanics / Methods
        
        
        //sets variables & spawns traffic lights
        public void SetDifficultyVariables()
        {
            if (gameVariant)
            {
                switch (Difficulty)
                {
                    case Difficulty.EASY:
                        correctLightAmount = 1;
                        trafficLightAmount = 3;
                        break;
                    case Difficulty.MEDIUM:
                        correctLightAmount = 1;
                        trafficLightAmount = 4;
                        break;
                    case Difficulty.HARD:
                        correctLightAmount = 1;
                        trafficLightAmount = 5;
                        break;
                }
            }
            else
            {
                switch (Difficulty)
                {
                    case Difficulty.EASY:
                        trafficLightAmount = 3;
                        correctLightAmount = 1;
                        break;
                    case Difficulty.MEDIUM:
                        trafficLightAmount = 3;
                        correctLightAmount = 2;
                        break;
                    case Difficulty.HARD:
                        trafficLightAmount = 3;
                        correctLightAmount = 3;
                        break;
                }
            }
            
            selectorIndex = trafficLightAmount / 2;
        }

        public void SpawnTrafficLights(int amountToSpawn )
        {
            trafficLights = new List<GameObject>();
            for (int i = 0; i < amountToSpawn ; i++)
            {
                GameObject trafficLight = Instantiate(trafficLightPrefab, owner_ref.transform.GetChild(0).transform);
                trafficLights.Add(trafficLight);
            }
        }

        // selectorCheck
        private void CheckSelector()
        {
            if (correctColors.Contains(secondWaveColors[selectorIndex]))
            {
                base.Win();
            }
            else
            {
                base.Lose();
            }
        }

        private void ShowResults()
        {
            for (int i = 0; i < trafficLights.Count; i++)
            {
                if (correctColors.Contains(secondWaveColors[i]))
                {
                    Instantiate(simonOk_ref, trafficLights[i].transform);
                    trafficLights[i].transform.GetChild(trafficLights[i].transform.childCount-1).gameObject.SetActive(true);
                }
                else
                {
                    Instantiate(simonNot_ref, trafficLights[i].transform);
                    trafficLights[i].transform.GetChild(trafficLights[i].transform.childCount-1).gameObject.SetActive(true);
                }
            }
        }
        

        #endregion Game Mechanics / Methods

        #region Overarching Methods / Helpers
        
        private List<trafficLight> MixCorrectColorsIntoWrongOnes( List<trafficLight> colorList, List<trafficLight> colorsToMixInto)
        {
            
            List<trafficLight> newList = new List<trafficLight>(colorList);
            List<int> indicesToIgnore = new List<int>();
                int tmp = Random.Range(0, colorsToMixInto.Count);
                if (!indicesToIgnore.Contains(tmp))
                {
                    indicesToIgnore.Add(tmp);
                    newList[Random.Range(0, newList.Count)] = new trafficLight(colorsToMixInto[tmp]);
                }
            return newList;
        }

        //generates colors & ignores duplicates as well as all colors in colorsToIgnore if its not null
        private List<trafficLight> GenLightColors(int amount, List<trafficLight> colorsToIgnore)
        {
            trafficLight tmpColors= new trafficLight();
            List<trafficLight> genColors = new List<trafficLight>();
            for (int i = 0; i < amount; i++)
            {
                tmpColors = GenRandomColorCombination();
                if (colorsToIgnore == null)
                {
                    if (genColors.Contains(tmpColors))
                    {
                        i--;
                        continue;
                    }
                }
                else
                {
                    if (genColors.Contains(tmpColors) || colorsToIgnore.Contains(tmpColors))
                    {
                        i--;
                        continue;
                    }
                }
                
                genColors.Add(tmpColors);
            }
            
            return genColors;
        }

        private void SetLightColors(List<GameObject> lights, List<trafficLight> colors, bool firstWave)
        {
            int start = 0;
            int end = colors.Count;
            int colorI = 0;
            
            
            
            if (firstWave && (gameVariant || Difficulty == Difficulty.EASY))
            {
                start++;
                end++;
                
                if (Difficulty == Difficulty.HARD)
                {
                    start++;
                    end++;
                }
                
            }

            for (int i = start; i < end; i++)
            {
                for (int j = 1; j < 7; j++)
                {
                    lights[i].transform.GetChild(j).gameObject.SetActive((false));
                }
                lights[i].transform.GetChild((int)colors[colorI].colorTop).gameObject.SetActive((true));
                lights[i].transform.GetChild((int)colors[colorI].colorBottom+3).gameObject.SetActive((true));
                colorI++;
            }
        }

        private trafficLight GenRandomColorCombination()
        {
            trafficLight generatedColors = new trafficLight();

            generatedColors.colorTop = (Colors)Random.Range(1, 4);
            generatedColors.colorBottom = (Colors)Random.Range(1, 4);
            
            return generatedColors;
        }

        /*private Color GetColorFromColors(Colors color)
        {
            Color col = new Color();
            switch(color)
            {
                case Colors.BLUE:
                    col = Color.blue;
                    break;
                case Colors.RED:
                    col = Color.red;
                    break;
                case Colors.GREEN:
                    col = Color.green;
                    break;
                case Colors.YELLOW:
                    col = Color.yellow;
                    break;
                case Colors.PURPLE:
                    col = Color.magenta;
                    break;
            }
            return col;
        }*/

        #endregion Overarching Methods / Helpers
    }
}