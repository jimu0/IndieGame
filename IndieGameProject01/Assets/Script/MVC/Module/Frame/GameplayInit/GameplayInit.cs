using System.Collections.Generic;
using Cinemachine;
using Script.MVC.Module.Class;
using UnityEngine;

namespace Script.MVC.Module.Frame.GameplayInit
{
    public class GameplayInit : MonoBehaviour
    {
        //public static Initialization Instance;

        private static GameplayInit instance;
        public static GameplayInit Instance => instance;// ??= new GameplayInit();

        public GameObject mainCamera;
        public GameObject basicTerrain;
        public GameObject playerUnit;
        public GameObject enemyUnit;
        private readonly GameObject[] gameUnit = new GameObject[16];
        public readonly Dictionary<GameObject, Gladiatus> DicPawns = new Dictionary<GameObject, Gladiatus>();
        private void Awake()
        {
            instance = gameObject.GetComponent<GameplayInit>();
            gameUnit[0] = playerUnit;
            for (int i = 1; i < gameUnit.Length; i++)
            {
                gameUnit[i] = enemyUnit ? enemyUnit : null;
            }
        }

        void Start()
        {
            
            if(basicTerrain) basicTerrain = Instantiate(basicTerrain);
            DicPawns.Clear(); 
        
            Transform playerUnitTsf = playerUnit.transform;
            Vector3 vector = new Vector3(0, 0, 0);
            for (int i = 0; i < 2; i++)
            {
                vector.x += 5;//!
                playerUnitTsf.position = vector;
                gameUnit[i]= Instantiate(gameUnit[i]);
                gameUnit[i].transform.position = playerUnitTsf.position;
                DicPawns.Add(gameUnit[i], gameUnit[i].GetComponent<Gladiatus>());
            }

            if (mainCamera)
            {
                mainCamera = Instantiate(mainCamera);
                Transform cmVcam0Tsf = mainCamera.transform.Find("cmVcam0");
                if (cmVcam0Tsf != null)
                {
                    CinemachineVirtualCamera virtualCamera = cmVcam0Tsf.GetComponent<CinemachineVirtualCamera>();
                    virtualCamera.Follow = gameUnit[0].transform;
                    virtualCamera.LookAt = gameUnit[0].transform;
                }
            }


        }

        // void Update()
        // {
        //
        // }

        
        public void BuildScenarios() { }
    }
}
