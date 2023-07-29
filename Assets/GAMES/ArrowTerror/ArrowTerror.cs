using Scripts.Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Games
{
    public class ArrowTerror : BaseGame
    {
        #region Serialized Fields

        //[SerializeField] [Range(0, 10)] private int ammountEnemies = 2, ammountCheckpoints = 2, lives = 3;
        [Space]
        [Header("Game Specific Stuff")]
        [SerializeField] private AudioSource sound;
        [SerializeField] private AudioClip[] clips;
        [SerializeField] private GameObject border, player, goal, checkpoint, enemy, checkpointContainer, enemyContainer, Container;

        #endregion Serialized Fields

        #region Fields

        private int checkpointsCollected = 0;
        private int winCounter = 0;
        private int ammountEnemies = 2, ammountCheckpoints = 2, lives = 3;
        private List<GameObject> allObjects;
        private bool invul = false;
        private float _time = 0;
        private bool timerEnded;
        #endregion Fields

        #region Built-Ins / MonoBehaviours

        private void Awake()
        {
            //base.SetUp();
            player.GetComponent<ArrowPlayer>().stick = _keys.One.Input;
            Vector2[] borders = border.GetComponent<EdgeCollider2D>().points;
            borders[0] = new Vector2(_playarea.xMin, _playarea.yMin);
            borders[1] = new Vector2(_playarea.xMin, _playarea.yMax);
            borders[2] = new Vector2(_playarea.xMax, _playarea.yMax);
            borders[3] = new Vector2(_playarea.xMax, _playarea.yMin);
            borders[4] = new Vector2(_playarea.xMin, _playarea.yMin);
            for (int i = 0; i < borders.Length; i++)
            {
                border.GetComponent<EdgeCollider2D>().points = borders;
            }
            
        }

        IEnumerator Start()
        {
            yield return StartCoroutine(AnimateInstruction());

            allObjects = new();
            allObjects.Add(player);
            invul = true;
            Invoke(nameof(switchState), 1);


            if (Difficulty == Models.Difficulty.EASY)
            {
                ammountCheckpoints = UnityEngine.Random.Range(1, 2);
            }
            if (Difficulty == Models.Difficulty.HARD)
            {
                ammountEnemies = UnityEngine.Random.Range(3, 5);
            }
            if (Difficulty != Models.Difficulty.EASY)
            {
                ammountCheckpoints = UnityEngine.Random.Range(2, 4);
                SpawnObjects(enemy, enemyContainer, ammountEnemies);
            }

            SpawnObjects(checkpoint, checkpointContainer, ammountCheckpoints);
            SpawnObjects(goal, Container, 1);
            base.RunTimer(30.0f);
        }

        private void Restart()
        {
            allObjects.Add(player);
            invul = true;
            Invoke(nameof(switchState), 2);
            if (Difficulty == Models.Difficulty.EASY)
            {
                ammountCheckpoints = UnityEngine.Random.Range(1, 2);
            }
            if (Difficulty == Models.Difficulty.HARD)
            {
                ammountEnemies = UnityEngine.Random.Range(3, 5);
            }
            if (Difficulty != Models.Difficulty.EASY)
            {
                ammountCheckpoints = UnityEngine.Random.Range(2, 5);
                SpawnObjects(enemy, enemyContainer, ammountEnemies);
            }

            SpawnObjects(checkpoint, checkpointContainer, ammountCheckpoints);
            SpawnObjects(goal, Container, 1);
            base.RunTimer(30.0f);
        }

        void Update()
        {
           if (winCounter == 3)
            {
                winCounter = 0;
                
                base.Harder();
                base.Win();
            }

           if (checkpointsCollected == ammountCheckpoints)
            {
                Container.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().enabled = false;
                Container.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.SetActive(true);
            }
            //Debug.Log(base._playarea.Contains(player.transform.position));

            _time += Time.deltaTime;
            if (_time >= 30.0f)
            {
                Debug.Log("Time is up!");
                timerEnded = true;
                base.Easier();
                base.Lose();
                checkpointsCollected = 0;
                _time = 0;
                DestroyObjects();
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

        private void switchState()
        {
            invul = false;
        }

        public void PlayerTouched(GameObject obj)
        {
            ElementType type = obj.GetComponent<ArrowObject>().type;

            switch (type)
            {
                case ElementType.CHECKPOINT:
                    sound.time = 0.8f;
                    sound.clip = clips[Random.Range(0, clips.Length)];
                    sound.Play();
                    Destroy(obj);
                    checkpointsCollected++;
                    break;
                case ElementType.ENEMY:
                    if (invul)
                    {
                        // Do nothing.
                    }
                    else
                    {
                        invul = true;
                        Invoke(nameof(switchState), 2);
                        base.AnimateFail(player.transform, 1, 1);
                        base.Easier();
                        base.Lose();
                    } 
                    break;
                case ElementType.GOAL:
                    if (checkpointsCollected == ammountCheckpoints)
                    {
                        checkpointsCollected = 0;
                        _time = 0;
                        winCounter++;
                        base.AnimateSuccess(player.transform, 1, 3);
                        base.ScoreUp();
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

                Vector3 newPosition = new Vector3(UnityEngine.Random.Range(_playarea.xMin, _playarea.xMax), UnityEngine.Random.Range( _playarea.yMin, _playarea.yMax), 0);

                obj.transform.localPosition = newPosition;
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