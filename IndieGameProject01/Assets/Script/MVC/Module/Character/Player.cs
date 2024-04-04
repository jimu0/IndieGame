namespace Script.MVC.Module.Character
{
    public class Player : Class.Character
    {
        public PlayerController BP_PlayerController;
        private void Awake()
        {
            GameMode._Player = this;
        }
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
            SetPosInt(transform.position);

        }


    }
}
