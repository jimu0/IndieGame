using System;
using Script.MVC.Controller.PawnController;
using Script.MVC.Module.Class;

namespace Script.MVC.Module.Frame
{
    public class GameMode : CoreFrame
    {
        //private Vector2Int KMK_v;
        public Class.Character _Player;
        public PlayerController _PlayerController;
        public Actor _GridMap;
        private void Awake()
        {
            KMK_SizeInt = (int)Math.Round(KMK_Size, MidpointRounding.AwayFromZero);

        
            _Player.GameMode = this;
            GridMap GridMap = (GridMap)Instantiate(_GridMap);
            GridMap._Player = Instantiate(_Player);

        }

        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
            GetKMK_View(_Player.PosInt);
        }


    }
}
