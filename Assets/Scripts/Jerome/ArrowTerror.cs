using Scripts.Models;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Games
{
    public class ArrowTerror : BaseGame
    {
        #region Serialized Fields

        [SerializeField] [Range(0,10)] private int ammountEnemies = 0, ammountCheckpoints = 0, lives = 3; 
        [SerializeField] private GameObject player, goal, checkpoint, enemy, checkpointContainer, enemyContainer, Container;

        #endregion Serialized Fields

        #region Fields

        private int checkpointsCollected = 0;
        private int winCounter = 0;
        private List<GameObject> allObjects;
        #endregion Fields

        #region Built-Ins / MonoBehaviours

        private void Awake()
        {
            base.SetUp();
            player.GetComponent<ArrowPlayer>().stick = keys.One.Input;
        }

        void Start()
        {
            allObjects = new();
            allObjects.Add(player);
            SpawnObjects(checkpoint, checkpointContainer, ammountCheckpoints);
            SpawnObjects(enemy, enemyContainer, ammountEnemies);
            SpawnObjects(goal, Container, 1);
            
        }

        private void Restart()
        {
            allObjects.Add(player);
            SpawnObjects(checkpoint, checkpointContainer, ammountCheckpoints);
            SpawnObjects(enemy, enemyContainer, ammountEnemies);
            SpawnObjects(goal, Container, 1);
        }

        void Update()
        {
           if (winCounter == 5)
            {
                winCounter = 0;
                base.Win();
            }
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
            ElementType type = obj.GetComponent<ArrowObject>().type;

            switch (type)
            {
                case ElementType.CHECKPOINT:
                    Destroy(obj);
                    checkpointsCollected++;
                    break;
                case ElementType.ENEMY:
                        
                        base.Lose();
                    break;
                case ElementType.GOAL:
                    if (checkpointsCollected == ammountCheckpoints)
                    {
                        checkpointsCollected = 0;
                        winCounter++;
                        DestroyObjects();
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

                Vector3 newPosition = new Vector3(UnityEngine.Random.Range(-8f, 8f), UnityEngine.Random.Range(-4.5f, 4.5f), 0);
     
                foreach (GameObject element in allObjects)
                {
                    if (element.GetComponent<ArrowPlayer>() != null) continue;
                   
                    float minDistance = 0;
                    switch (element.GetComponent<ArrowObject>().type)
                    {
                        case ElementType.CHECKPOINT:
                            minDistance = 6;
                            break;

                        case ElementType.ENEMY:
                            minDistance = 8;
                            break;

                        case ElementType.GOAL:
                            minDistance = 6;
                            break;
                    }
                    //Debug.Log(Vector3.Distance(element.transform.position, newPosition));
                    while (Vector3.Distance(element.transform.position, newPosition) < minDistance)
                    {
                        newPosition = new Vector3(UnityEngine.Random.Range(-8f, 8f), UnityEngine.Random.Range(-4.5f, 4.5f), 0);
                    }
                    obj.transform.position = newPosition; 
                }
            }
        }

        private void DestroyObjects()
        {
            for (int i = 0; i < checkpointContainer.transform.childCount; i++)
            {
                Destroy(checkpointContainer.transform.GetChild(i).gameObject);
            }

            for (int i = 0; i < enemyContainer.transform.childCount; i++)
            {
                Destroy(enemyContainer.transform.GetChild(i).gameObject);
            }

            for (int i = 0; i < Container.transform.childCount; i++)
            {
                Destroy(Container.transform.GetChild(i).gameObject);
            }
            allObjects.Clear();

            Invoke(nameof(Restart), 3);
        }

        #endregion Overarching Methods / Private Helpers
    }
}