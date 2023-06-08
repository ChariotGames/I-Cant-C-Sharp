using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.Games
{
    public class ArrowTerror : Game
    {
        #region Serialized Fields
        [SerializeField] private int ammountOfEnemys, ammountCheckpoints, lives = 3; 
        [SerializeField] private GameObject tempPlayer, tempGoal, tempCheckpoint, tempEnemy;
        [SerializeField] private GameObject[] Checkpoints, Enemys;



        #endregion Serialized Fields

        #region Fields
        private GameObject player, goal, checkpoint, enemy;


        #endregion Fields

        #region Built-Ins / MonoBehaviours

        void Start()
        {
            Checkpoints = spawnObjekts(checkpoint, ammountCheckpoints);
            Enemys = spawnObjekts(enemy, ammountOfEnemys);

            //player = Instantiate(tempPlayer);
            //goal = Instantiate(tempGoal);
        }

     

        void Update()
        {
           
        }

        #endregion Built-Ins / MonoBehaviours

        #region GetSets / Properties



        #endregion GetSets / Properties

        #region Game Mechanics / Public Methods

        public void PlayerTouched(GameObject obj)
        {
            if (obj.name.Contains("Checkpoint"))
            {
                removeCheckpoint(obj);
            }
            if (obj.name.Contains("Enemy"))
            {
                lives--;
            }
            if (obj.name.Contains("Goal"))
            {
                if (Checkpoints.Length == 0)
                {
                    base.Win();
                }
            }
        }

       

        public void TemplateMethod(bool param)
        {
            
        }

        #endregion Game Mechanics / Public Methods

        #region Overarching Methods / Private Helpers

        private GameObject[] spawnObjekts(GameObject obj, int ammount)
        {
            GameObject[] objects = new GameObject[ammount];

            for (int i = 0; i < ammount; i++)
            {
                GameObject o = Instantiate(obj);
                objects[i] = o;
            }
            return objects;
        }

        private void removeCheckpoint(GameObject obj)
        {
            for (int i = 0; i < Checkpoints.Length; i++)
            {
                if (obj == Checkpoints[i])
                {
                    Destroy(Checkpoints[i]);
                    Checkpoints[i] = null;
                }
            }
        }

        #endregion Overarching Methods / Private Helpers
    }
}