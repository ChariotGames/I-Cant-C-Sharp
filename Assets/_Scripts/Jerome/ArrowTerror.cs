using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.Games
{
    public class ArrowTerror : Game
    {
        #region Serialized Fields

        [SerializeField] [Range(0,10)] private int ammountEnemies = 0, ammountCheckpoints = 0, lives = 3; 
        [SerializeField] private GameObject player, goal, checkpoint, enemy, checkpointContainer, enemyContainer;

        #endregion Serialized Fields

        #region Fields

        private int checkpointsCollected = 0;
        private List<GameObject> allObjects;
        #endregion Fields

        #region Built-Ins / MonoBehaviours

        void Start()
        {
            allObjects = new();
            allObjects.Add(player);
            SpawnObjects(checkpoint, checkpointContainer, ammountCheckpoints);
            SpawnObjects(enemy, enemyContainer, ammountEnemies);
            SpawnObjects(goal, gameObject, 1);
            
        }

        void Update()
        {
           
        }

        internal void UpdateEnemyPositions(Vector3 position)
        {
            for (int i = 0; i < enemyContainer.transform.childCount; i++)
            {
                ArrowObject obj = enemyContainer.transform.GetChild(i).GetComponent<ArrowObject>();
                obj.PlayerMoved(position);
            }
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
                    if (lives == 0)
                    {
                        base.Lose();
                    }
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
                allObjects.Add(obj);

                // TODO: Random positions
                //obj.transform.position = 
                Vector3 newPosition = new Vector3(UnityEngine.Random.Range(-8f, 8f), UnityEngine.Random.Range(-4.5f, 4.5f), 0);
                //for loop über alle objekte
                // if else ob die entfernung stimmt ok wenn nicht neuer vector (while loop?)
                foreach (GameObject element in allObjects)
                {
                    while (Vector3.Distance(element.transform.position, newPosition) < 2)
                    {
                        newPosition = new Vector3(UnityEngine.Random.Range(-8f, 8f), UnityEngine.Random.Range(-4.5f, 4.5f), 0);

                    }
                    obj.transform.position = newPosition; 
                }
               
            }
        }

        #endregion Overarching Methods / Private Helpers
    }
}