using System.Collections.Generic;
using Script.MVC.Module.Class;
using UnityEngine;

namespace Script.MVC.Module.Frame
{
    public class Initialization : MonoBehaviour
    {
        public static Initialization Instance;


        public GameObject BasicTerrain;
        public GameObject PlayerUnit;
        public GameObject EnemyUnit;
        private GameObject[] GameUnit = new GameObject[16];
        public Dictionary<GameObject, Gladiatus> Dic_Gladiatus = new Dictionary<GameObject, Gladiatus>();
        private void Awake()
        {
            Instance = gameObject.GetComponent<Initialization>();
            GameUnit[0] = PlayerUnit;
            for (int i = 1; i < GameUnit.Length; i++)
            {
                GameUnit[i] = EnemyUnit;
            }
        }

        void Start()
        {
            BasicTerrain = Instantiate(BasicTerrain);
            Dic_Gladiatus.Clear(); 
        
            Transform transform = PlayerUnit.transform;//��
            Vector3 vector = new Vector3(0, 0, 0);//��
            for (int i = 0; i < 2; i++)//GameUnit.Length
            {
                vector.x += 5;//!
                transform.position = vector;//��
                GameUnit[i]= Instantiate(GameUnit[i]);
                GameUnit[i].transform.position = transform.position;//��
                Dic_Gladiatus.Add(GameUnit[i], GameUnit[i].GetComponent<Gladiatus>());
            }
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        //��������
        public void BuildScenarios() { }
    }
}
