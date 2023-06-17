using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using _Scripts._Input;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Scripts.Games
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
        //[SerializeField] private Action inputL;
        //[SerializeField] private Action inputR;
        

        #endregion Serialized Fields

        #region Fields

        private List<trafficLight> correctColors;
        private List<trafficLight> secondWaveColors;
        private int correctLightAmount = 1;
        private int totalLightAmount = 3;
        private int selectorIndex;
        
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
            
            //setup
            SetDifficultyVariables();
            SpawnTrafficLights(totalLightAmount);
             
            //set correct color combinations
            correctColors = GenLightColors(totalLightAmount, null);
            SetLightColors(trafficLights, correctColors);
            
            
            Invoke(nameof(SecondWave), 8);
            
        }

        private void SecondWave()
        {
            //Mix correct colors with random generated ones and set them.
            secondWaveColors = MixCorrectColorsIntoWrongOnes(
                GenLightColors(totalLightAmount, correctColors), correctColors, correctLightAmount);
            SetLightColors(trafficLights, secondWaveColors);
            
            Invoke(nameof(CheckSelector), 7);
        }
        
        private void OnEnable()
        {
            //spawn trafficlights
            
            //selector_ref.transform.SetParent(trafficLights[totalLightAmount/2].transform);
            InputHandler.ArrowLeft += ButtonPressL;
            InputHandler.ArrowRight += ButtonPressR;
        }
        
        private void OnDisable()
        {
            InputHandler.ArrowLeft -= ButtonPressL;
            InputHandler.ArrowRight -= ButtonPressR;
        }

        #endregion Built-Ins / MonoBehaviours

        #region GetSets / Properties
        
        

        #endregion GetSets / Properties

        #region Game Mechanics / Methods
        
        
        //sets variables & spawns traffic lights
        public void SetDifficultyVariables()
        {
            switch (Difficulty)
            {
                case Difficulty.EASY:
                    break;
                case Difficulty.MEDIUM:
                    totalLightAmount = 5;
                    correctLightAmount = 2;
                    break;
                case Difficulty.HARD:
                    totalLightAmount = 7;
                    correctLightAmount = 3;
                    break;
            }
            selectorIndex = totalLightAmount / 2;
        }

        public void SpawnTrafficLights(int amountToSpawn )
        {
            for (int i = -amountToSpawn/2; i <= amountToSpawn/2 ; i++)
            {
                GameObject trafficLight = Instantiate(trafficLightPrefab, owner_ref.transform);
                trafficLight.transform.Translate(i*1.5f, 0, 0);
                trafficLights.Add(trafficLight);
            }
        }
        
        //colors
        private List<trafficLight> MixCorrectColorsIntoWrongOnes( List<trafficLight> colorList, List<trafficLight> colorsToMixInto, int amountToMixInto )
        {
            
            List<trafficLight> newList = new List<trafficLight>(colorList);
            List<int> indicesToIgnore = new List<int>();
            for (int i = 0; i < amountToMixInto; i++)
            {
                int tmp = Random.Range(0, colorsToMixInto.Count);
                if (!indicesToIgnore.Contains(tmp))
                {
                    indicesToIgnore.Add(tmp);
                    newList[Random.Range(0, newList.Count)] = new trafficLight(colorsToMixInto[tmp]);
                }
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

        private void SetLightColors(List<GameObject> lights, List<trafficLight> colors)
        {
            for (int i = 0; i < lights.Count; i++)
            {
                
                lights[i].transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().color = GetColorFromColors(colors[i].colorTop);
                lights[i].transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().color = GetColorFromColors(colors[i].colorBottom);
            }
        }
        
        // selectorCheck
        private void CheckSelector()
        {
            
            //a.Any(a => b.Contains(a))
            if (correctColors.Contains(secondWaveColors[selectorIndex]))
            {
                base.Win();
            }
            else
            {
                base.Lose();
            }
        }
        
        //Inputs
        public void ButtonPressL()
        {
            
            //print(trafficLights.IndexOf(selector_ref.transform.parent.gameObject)-1);
            //.transform.SetParent(trafficLights[trafficLights.IndexOf(selector_ref.transform.parent.gameObject)-1].transform);
            if (selectorIndex != 0)
            {
                selector_ref.transform.Translate(1.5f,0,0);
                --selectorIndex;
            }
        }
        
        public void ButtonPressR()
        {
            
            //print(trafficLights.IndexOf(selector_ref.transform.parent.gameObject)+1);
            //selector_ref.transform.SetParent(trafficLights[trafficLights.IndexOf(selector_ref.transform.parent.gameObject)+1].transform);
            if (selectorIndex != trafficLights.Count-1)
            {
                selector_ref.transform.Translate(-1.5f,0,0);
                ++selectorIndex;
            }
        }

        #endregion Game Mechanics / Methods

        #region Overarching Methods / Helpers

        private trafficLight GenRandomColorCombination()
        {
            trafficLight generatedColors = new trafficLight();

            generatedColors.colorTop = (Colors)Random.Range(1, 6);
            generatedColors.colorBottom = (Colors)Random.Range(1, 6);
            
            return generatedColors;
        }

        private Color GetColorFromColors(Colors color)
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
        }

        #endregion Overarching Methods / Helpers
    }
}