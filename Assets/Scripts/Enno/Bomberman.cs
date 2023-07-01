using System;
using System.Collections;
using System.Collections.Generic;
using Scripts._Input;
using Scripts.Models;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Games
{
    /// <summary>
    /// Bomberman, player spawns on a small Square, bombs spawn aswell and the player has to hide from them.
    /// player can move in 4 directions, black tiles block the bomb explosion path and player movement.
    /// Win & Lose condition: player hiding from the bomb or not.
    /// lvl 1: 2 bombs & explosionindicators
    /// lvl 2: 2 bombs no indicators anymore
    /// lvl 3: 3-4 bombs and possibly a bigger Square.
    /// </summary>
    public class Bomberman : BaseGame
    {

        #region Serialized Fields

        [SerializeField] private GameObject owner_ref, bomb_ref, player_ref, fieldPrefab;
        

        #endregion Serialized Fields

        #region Fields

        Square[,] playingField;
        private int fieldDimension = 5;
        private int[] playerPos = new int[2];
        
        public struct Square
        {
            public GameObject obj;
            public squareState state;

            public Square(GameObject obj, squareState state)
            {
                this.obj = obj;
                this.state = state;
            }

            public enum squareState
            {
                normal,
                blocked,
                inBombPath
            }
        }

        #endregion Fields

        #region Built-Ins / MonoBehaviours
        
        void Start()
        {
            SetDifficultyVariables();
            player_ref.transform.SetParent(playingField[playerPos[0],playerPos[1]].obj.transform);
            player_ref.SetActive(true);

        }
        
        void Update()
        {
            player_ref.transform.position = player_ref.transform.parent.transform.position;
        }

        #region inputs
        
        
        private void OnEnable()
        {
            
            InputHandler.ButtonNorth += moveNorth;
            InputHandler.ButtonEast += moveEast;
            InputHandler.ButtonSouth += moveSouth;
            InputHandler.ButtonWest += moveWest;
        }

        private void OnDisable()
        {
            InputHandler.ButtonNorth -= moveNorth;
            InputHandler.ButtonEast -= moveEast;
            InputHandler.ButtonSouth -= moveSouth;
            InputHandler.ButtonWest -= moveWest;
        }
        private void moveNorth()
        {
            if (!(playerPos[1] - 1 < 0) && playingField[playerPos[0],playerPos[1]-1].state != Square.squareState.blocked)
            {
                --playerPos[1];
                player_ref.transform.SetParent(playingField[playerPos[0],playerPos[1]].obj.transform);
            }
        }

        private void moveEast()
        {
            if (!(playerPos[0] + 1 >= fieldDimension) && playingField[playerPos[0]+1,playerPos[1]].state != Square.squareState.blocked)
            {
                ++playerPos[0];
                player_ref.transform.SetParent(playingField[playerPos[0],playerPos[1]].obj.transform);
            }
        }

        private void moveSouth()
        {
            
            if (!(playerPos[1] + 1 >= fieldDimension) && playingField[playerPos[0],playerPos[1]+1].state != Square.squareState.blocked)
            {
                ++playerPos[1];
                player_ref.transform.SetParent(playingField[playerPos[0],playerPos[1]].obj.transform);
            }
        }

        private void moveWest()
        {
            if (!(playerPos[0] - 1 < 0) && playingField[playerPos[0]-1,playerPos[1]].state != Square.squareState.blocked)
            {
                --playerPos[0];
                player_ref.transform.SetParent(playingField[playerPos[0],playerPos[1]].obj.transform);
            }
        }
        #endregion inputs
        
        #endregion Built-Ins / MonoBehaviours

        #region Game Mechanics / Methods

        public void SetDifficultyVariables()
        {
            switch (Difficulty)
            {
                case Difficulty.EASY:
                    break;
                case Difficulty.MEDIUM:
                    break;
                case Difficulty.HARD:
                    fieldDimension = 7;
                    break;
            }
            playingField = SetupPlayingField(fieldDimension);
            playerPos[0] = fieldDimension / 2;
            playerPos[1] = fieldDimension / 2;
        }

        public Square[,] SetupPlayingField(int dimension)
        {
            Square[,] squares = new Square[dimension,dimension];
            owner_ref.transform.GetChild(0).transform.GetComponent<GridLayoutGroup>().constraintCount = dimension;
            
            for (int i = 0; i < dimension; ++i)
            {
                for (int j = 0; j < dimension; j++)
                {
                    if (i % 2 == 1 && j % 2 == 1)
                    {
                        squares[j , i] = new Square(Instantiate(fieldPrefab, owner_ref.transform.GetChild(0).transform), Square.squareState.blocked);
                        squares[j , i].obj.GetComponent<SpriteRenderer>().color = Color.gray;
                    }
                    else
                    {
                        squares[j , i] = new Square(Instantiate(fieldPrefab, owner_ref.transform.GetChild(0).transform), Square.squareState.normal);
                    }
                }
            }
            return squares;
        }

        
        #endregion Game Mechanics / Methods

        #region Overarching Methods / Helpers

        // TODO: Put your helper methods here.

        #endregion Overarching Methods / Helpers
    }
}