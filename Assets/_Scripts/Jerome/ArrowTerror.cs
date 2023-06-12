using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.Games
{
    public class ArrowTerror : BaseGame
    {
        #region Serialized Fields

        [SerializeField] [Range(0,10)] private int ammountEnemies = 0, ammountCheckpoints = 0, lives = 3; 
        [SerializeField] private GameObject player, goal, checkpoint, enemy, checkpointContainer, enemyContainer;

        #endregion Serialized Fields

        #region Fields

        private int checkpointsCollected = 0;

        #endregion Fields

        #region Built-Ins / MonoBehaviours

        void Start()
        {
            SpawnObjects(checkpoint, checkpointContainer, ammountCheckpoints);
            SpawnObjects(enemy, enemyContainer, ammountEnemies);
            SpawnObjects(goal, gameObject, 1);
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
            Type type = obj.GetComponent<ArrowObject>().type;

            switch (type)
            {
                case Type.CHECKPOINT:
                    Destroy(obj);
                    checkpointsCollected++;
                    break;
                case Type.ENEMY:
                    lives--;
                    break;
                case Type.GOAL:
                    if (checkpointsCollected == ammountCheckpoints)
                    {
                        base.Win();
                    }
                    break;
            }
        }

        #endregion Game Mechanics / Public Methods

        #region Overarching Methods / Private Helpers

        private void SpawnObjects(GameObject type, GameObject parent, int ammount)
        {
            for (int i = 0; i < ammount; i++)
            {
                GameObject obj = Instantiate(type, parent.transform);
                // TODO: Random positions
            }
        }

        #endregion Overarching Methods / Private Helpers
    }
}