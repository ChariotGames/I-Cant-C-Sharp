using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Games
{
    public class MoveIt : BaseGame
    {
        [SerializeField] private GameObject obstacle, goal, boulder, container;

        public GameObject player;

        private bool active = false;
        private Vector3 offset;
        private List<Vector3> map = new List<Vector3>();
        private List<Vector3> layout;
        private int winCounter = 0;
        private int loseCounter = 0;

        // Start is called before the first frame update
        void Start()
        {
            active = true;
            offset = boulder.transform.position - player.transform.position;

            for (int i = -3; i <= 3; i++)
            {
                for (int j = -3; j <= 3; j++)
                {
                    map.Add(new Vector3(i, j, 0));
                }
            }
            CreateLayout();
        }

        private void CreateLayout()
        {
            layout = map;

            GameObject obstacle1 = Instantiate(obstacle, gameObject.transform.position, Quaternion.identity, container.transform);
            GameObject obstacle2 = Instantiate(obstacle, gameObject.transform.position, Quaternion.identity, container.transform);
            GameObject obstacle3 = Instantiate(obstacle, gameObject.transform.position, Quaternion.identity, container.transform);
            obstacle1.SetActive(true);
            obstacle2.SetActive(true);
            obstacle3.SetActive(true);

            SetObject(goal);
            SetObject(player);
            SetObject(boulder);
            SetObject(obstacle1);
            SetObject(obstacle2);
            SetObject(obstacle3);
            
        }

        private void SetObject(GameObject obj)
        {
            int chance = Random.Range(0, layout.Count);
            obj.transform.position = layout[chance];
            layout.RemoveAt(chance);
        }

        private void Move()
        {
            RaycastHit2D hitR = Physics2D.Raycast(player.transform.position, Vector2.right, 0.6f),
                hitL = Physics2D.Raycast(player.transform.position, Vector2.left, 0.6f),
                hitU = Physics2D.Raycast(player.transform.position, Vector2.up, 0.6f),
                hitD = Physics2D.Raycast(player.transform.position, Vector2.down, 0.6f);

            Vector3 moveDirection = (player.transform.position - boulder.transform.position).normalized;
            Vector3 newPos = moveDirection;

            boulder.GetComponent<Rigidbody2D>().MovePosition(player.transform.position * 0.9f);
            //boulder.transform.position = player.transform.position + offset;   
        }

        public void CheckWinCondition()
        {
            if (boulder.transform.position == goal.transform.position)
            {


            }

            if (winCounter == 25)
            {
                Debug.Log("You passed this quest my son. Now go forth into the world and prove them that you are a real hero of the people! Aka get some Pizza.");
                active = false;
                base.Win();
            }
            if (loseCounter == 3)
            {
                Debug.Log("Snake? SNAKE? SNAAAAAACKE!!!");
                active = false;
                base.Lose();
            }
        }

        // Update is called once per frame
        void Update()
        {
            Move();
        }
    }
}
