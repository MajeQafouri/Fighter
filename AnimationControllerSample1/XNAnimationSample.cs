using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Design;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Net;
using XNAnimation;
using XNAnimation.Controllers;
using XNAnimation.Effects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using AnimationControllerSample1;
using PickingSample;
using BoundingVolumeRendering;
using Keys = Microsoft.Xna.Framework.Input.Keys;
using ButtonState = Microsoft.Xna.Framework.Input.ButtonState;
// Add ProjectMercury.dll to references
// Then set your project to use it



namespace AnimationControllerSample1
{
    public enum FighterState
    {
        Run = 0,
        Shoot,
        Aim,
        Idle,
    };

    public enum ObjectType
    {
        HealthBox=0,
        GunBox,
        Enemy
    };

    public enum CollisionType
    {
        Enemy=0,
        Box,
        Trees
    };

    public enum EnemyState
    {
        Walk = 0,
        Jump,
        Rear,
        Bite1,
        TakeDamage,
        Idle,
        Die,
        Stomp,
    };
   
    public class XNAnimationSample : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont spriteFont;
        Texture2D myCrossHair;

        // Skinned Models Name List
        static readonly string[] ModelFilenamesWithSkinnedModel = new string[]
        {
           
            "Enemy1",
            "Enemy2"
            
        };

        #region Fields


        Cursor cursor;
     

        
       // Camera
        static Vector3 cameraPosition;
        //static  float camera3View = 35f;
        public Vector3 cameraOffset = new Vector3(0, 50, -95);  //35:y  z:-65   x:0
        static Vector3 cameraTarget = new Vector3(0, -20, 0);
        private Matrix world = Matrix.CreateTranslation(new Vector3(0,0, 0));
        public Matrix [] enemyworld =new Matrix[20];
        Matrix cameraView;
        Matrix cameraProjection;

        //Mouse
        public MouseState mymouse, mymouseDown,mymouseUp;
        static float mouseYposition ;
        static float mouseXposition ;
       
        //Collision Chech
        static bool collision = false;
        static bool status = false;
        CollisionCheck mycheckCollision = new CollisionCheck();


        //Jumping Control
        static bool start=false;

        //KeyBoard
        static KeyboardState perviousKey, currentKey, keyboard;
        private float angle = 0f;
       
        //Models
        Model  RightWall,LeftWall,tree
            , HealthBox, FeshangBox, MyGround, MyGround2, EndWall, Tank, Door, 
            ground2, myStone,WaterTower,koh,Truck1,SandBag,TankMG,Tanker1,bomb;
        Vector3 mymodelposition = new Vector3(50, 20, 250);
      
        
        //Fighter
        static FighterState myFighterState;
        public int Gun ;
        int time1;
        public int firstcol, lastcol, resultcol;
        bool backControl=false;
        
        
        //Bomb
        public bool BombFind = false;

        //field  type ham ezafe mikonim masalan type:enemy,Box
        public struct WorldObject
        {
            public Vector3 position;
            public Model model;
            public Vector3 lastPosition;
            public float Scale;
            public SkinnedModel skinnedModel;
            public AnimationController animationController;
            public int health ;
            public EnemyState myEnemyState;
            public ObjectType Type;
            public bool Visible;
            public void backup()
            {

                collision = true;
            }
        }

        public  WorldObject fighter = new WorldObject();
        public  WorldObject[] Mymodels = new WorldObject[5];
        public WorldObject[] Stone = new WorldObject[12];
        public  WorldObject[] myBoxs = new WorldObject[9];
        public WorldObject[] mytrees = new WorldObject[28];
        public WorldObject[] Enemies = new WorldObject[10];
        public WorldObject[] MyWaterTower = new WorldObject[3];
      
        public WorldObject[] sandBag = new WorldObject[5];
        public WorldObject Tanker = new WorldObject();
        public WorldObject[] Tank_MG = new WorldObject[12];
        public WorldObject Truck = new WorldObject();
        public WorldObject Mybomb = new WorldObject();

        public WorldObject myDoor = new WorldObject();

        #endregion


        Matrix[][] modelAbsoluteBoneTransforms = new Matrix[10][];
        Matrix[] modelWorldTransforms = new Matrix[10];
        public bool drawBoundingSphere = true;

        //Audio
        Song mysong, myendsong;
        SoundEffect mySoundEffect, mySoundEffect1, mySoundEffect2, mySoundEffect3, mySoundEffect4;

        //BOXS
        float HealthBoxRotation = 0;
           

        //End Game
        bool endofgame = false;
        Texture2D myendsplash, GameOver, myStartSplash, myendsplash2;
        Rectangle dynamicRec;
        Rectangle staticRec,staticRec2;
        bool marhale1 = false;
        bool marhale2 = false;
        bool firstUse = false;
        
        //Border
        Texture2D myBorder;
        Color myColor = Color.White;

     

        //Stone
        float sr = 0;
        int EnemyCounter = 10;
        int calculateEnemycounter = 0;

        //Time
        int elapseTimeSecend, elapseTimeMinute;
        //labels
        Texture2D mylabel;
        Vector2 labelpos = new Vector2(0, 600);
        public XNAnimationSample()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferMultiSampling = true;
            graphics.PreferredBackBufferWidth = 900;
            graphics.PreferredBackBufferHeight = 700;
            Window.Title = "Majid Qafouri";
           // graphics.IsFullScreen = true;
            Content.RootDirectory = "Content";

           
        }

        protected override void Initialize()
        {

            //Mahal Shoro Harekat Enemy Az Ghesmat Zir Moshakas Mishavad.
            // modelWorldTransforms[1] = Matrix.CreateTranslation(new Vector3(0, 0, 100));
        
            cursor = new Cursor(this);
            Components.Add(cursor);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            
            spriteBatch = new SpriteBatch(GraphicsDevice);
            spriteFont = Content.Load<SpriteFont>("Fonts/Arial");



            //myEffect = Content.Load<ParticleEffect>("BasicExplosion");
            //myEffect.LoadContent(Content);
            //myEffect.Initialise();
            //myRenderer.LoadContent(Content);



            mylabel = Content.Load<Texture2D>("Model2/baner");


            //Models Load
            
           
            RightWall = Content.Load<Model>("walltext");
            LeftWall = Content.Load<Model>("walltext");
            EndWall = Content.Load<Model>("Model2/WallF");
       
            tree = Content.Load<Model>("Model2/tree");
            HealthBox = Content.Load<Model>("Model2/Blood2");
            FeshangBox = Content.Load<Model>("Model2/feshang");
            Door = Content.Load<Model>("Models/Door2");
            MyGround = Content.Load<Model>("Model2/GND");
            MyGround2 = Content.Load<Model>("Marhale2/GND2");
            Tank = Content.Load<Model>("Model2/Tank");
            myendsplash = Content.Load<Texture2D>("Splash/endofgame");
            GameOver = Content.Load<Texture2D>("Splash/GameOver");
            myStartSplash = Content.Load<Texture2D>("Splash/Splash");
            myStone = Content.Load<Model>("Model3/stone");

            //Marhale2 Objects
            

            
            WaterTower = Content.Load<Model>("Marhale2/WaterTowwer");
            koh = Content.Load<Model>("Marhale2/koh");
            TankMG = Content.Load<Model>("Marhale2/Tank_MG");
            Truck1 = Content.Load<Model>("Marhale2/Truck");
            SandBag = Content.Load<Model>("Marhale2/sandBag");
            Tanker1 = Content.Load<Model>("Marhale2/Tanker");
            bomb = Content.Load<Model>("Marhale2/Bomb");
            myendsplash2 = Content.Load<Texture2D>("Splash/endofgame2");


            //Bomb
            Mybomb.model = bomb;
            Mybomb.position = new Vector3(110, 5, 2580); //2580 x:110
            Mybomb.Scale = 1;
            Mybomb.Visible = true;

           //Stone
            Stone[0].model = myStone;
            Stone[0].position = new Vector3(-30, 8, 9500);

            Stone[1].model = myStone;
            Stone[1].position = new Vector3(-150, 8, 9900);

            Stone[2].model = myStone;
            Stone[2].position = new Vector3(-100, 8, 9100);

            Stone[3].model = myStone;
            Stone[3].position = new Vector3(-60, 8, 8100);

            Stone[4].model = myStone;
            Stone[4].position = new Vector3(-180, 8, 5600);

            Stone[5].model = myStone;
            Stone[5].position = new Vector3(-130, 8, 6200);

            Stone[6].model = myStone;
            Stone[6].position = new Vector3(-80, 8, 4300);

            Stone[7].model = myStone;
            Stone[7].position = new Vector3(0, 8, 5000);

            Stone[8].model = myStone;
            Stone[8].position = new Vector3(-270, 8, 4500);

            Stone[9].model = myStone;
            Stone[9].position = new Vector3(-150, 8, 5100);

            Stone[10].model = myStone;
            Stone[10].position = new Vector3(20, 8, 5500);

            Stone[11].model = myStone;
            Stone[11].position = new Vector3(-280, 8, 6300);

            //Tank
            for (int i = 0; i < Tank_MG.Length; i++)
            {
                Tank_MG[i].model = TankMG;
                Tank_MG[i].Scale = 1;
            }

            Tank_MG[0].position = new Vector3(-280, 0, 800);
            Tank_MG[1].position = new Vector3(-280, 0, 1700);
            Tank_MG[2].position = new Vector3(-280, 0, 1850);
            Tank_MG[3].position = new Vector3(100, 0, 3600);
            Tank_MG[4].position = new Vector3(100, 0, 3700);
            Tank_MG[5].position = new Vector3(100, 0, 3800);
            Tank_MG[6].position = new Vector3(-30, 0, 3600);
            Tank_MG[7].position = new Vector3(-30, 0, 3700);
            Tank_MG[8].position = new Vector3(-30, 0, 3800);
            Tank_MG[9].position = new Vector3(-180, 0, 3600);
            Tank_MG[10].position = new Vector3(-180, 0, 3700);
            Tank_MG[11].position = new Vector3(-180, 0, 3800);



            //truck
            Truck.model = Truck1;
            Truck.position = new Vector3(-283, 32, 1200);
            Truck.Scale = 1f;

            //Tanker
            Tanker.model = Tanker1;
            Tanker.position = new Vector3(100, -25, 2500);
            Tanker.Scale = 1f;


            //SandBag
            for (int i = 0; i < sandBag.Length; i++)
            {

                sandBag[i].model = SandBag;
                sandBag[i].Scale = 1;
            }
            
            sandBag[0].position = new Vector3(170, 0, 1600);
            sandBag[1].position = new Vector3(170, 10, 1600);
            sandBag[2].position = new Vector3(-310, 0, 400);
            sandBag[3].position = new Vector3(-310, 0, 2300);
            sandBag[4].position = new Vector3(150, 0, 3200);

           //Door
            myDoor.model = Door;
            myDoor.Scale = 0.4f;
            myDoor.position = new Vector3(-300, -5, 10000);

            //Boxs
            if (marhale1 == true)
            {
                myBoxs[0].Type = ObjectType.HealthBox;
                myBoxs[0].position = new Vector3(-800, 0, 2400);
                myBoxs[0].model = HealthBox;

                myBoxs[1].Type = ObjectType.HealthBox;
                myBoxs[1].position = new Vector3(-250, 0, 1200);
                myBoxs[1].model = HealthBox;
            }
            else
            {
                myBoxs[0].Type = ObjectType.HealthBox;
                myBoxs[0].position = new Vector3(50, 0, 2200);
                myBoxs[0].model = HealthBox;

                myBoxs[1].Type = ObjectType.HealthBox;
                myBoxs[1].position = new Vector3(-150, 0, 900);
                myBoxs[1].model = HealthBox;
            }

            if (marhale1 == true)
            {
                myBoxs[2].Type = ObjectType.GunBox;
                myBoxs[2].position = new Vector3(-280, 0, 3000);
                myBoxs[2].model = FeshangBox;

                myBoxs[3].Type = ObjectType.GunBox;
                myBoxs[3].position = new Vector3(-236, 0, 400);
                myBoxs[3].model = FeshangBox;
            }
            else
            {
                myBoxs[2].Type = ObjectType.GunBox;
                myBoxs[2].position = new Vector3(-200, 0, 3000);
                myBoxs[2].model = FeshangBox;

                myBoxs[3].Type = ObjectType.GunBox;
                myBoxs[3].position = new Vector3(0, 0, 400);
                myBoxs[3].model = FeshangBox;
            }

            myBoxs[4].Type = ObjectType.GunBox;
            myBoxs[4].position = new Vector3(20, 0, 1600);
            myBoxs[4].model = FeshangBox;

            myBoxs[5].Type = ObjectType.GunBox;
            myBoxs[5].position = new Vector3(-100, 0, 1000);
            myBoxs[5].model = FeshangBox;

            myBoxs[6].Type = ObjectType.GunBox;
            myBoxs[6].position = new Vector3(-150, 0, 1700);
            myBoxs[6].model = FeshangBox;

            myBoxs[7].Type = ObjectType.GunBox;
            myBoxs[7].position = new Vector3(30, 0, 3250);
            myBoxs[7].model = FeshangBox;

         
          

            //WaterTower
            MyWaterTower[0].position = new Vector3(295,-25, 3500);
            MyWaterTower[0].model = WaterTower;
            MyWaterTower[0].Scale = 0.5f;

            MyWaterTower[1].position = new Vector3(300, -25, 1500);
            MyWaterTower[1].model = WaterTower;
            MyWaterTower[1].Scale = 0.5f;

            MyWaterTower[2].position = new Vector3(-650, -25, 5200);
            MyWaterTower[2].model = WaterTower;
            MyWaterTower[2].Scale = 0.5f;

            //Tree
            for (int i = 0; i < mytrees.Length; i++)
            {
                mytrees[i].Scale = 1;
                mytrees[i].model=tree;
            }
         
            mytrees[0].position = new Vector3(2, 70, 720);                     
            mytrees[1].position = new Vector3(-250, 70, 630);    
            mytrees[2].position = new Vector3(-50, 70, 510);       
            mytrees[3].position = new Vector3(-200, 70, 200);    
            mytrees[4].position = new Vector3(-150, 70, 420);
            mytrees[5].position = new Vector3(-220, 70, 850);
            mytrees[6].position = new Vector3(-40, 70, 250);
            mytrees[7].position = new Vector3(-50, 70, 930);
            mytrees[8].position = new Vector3(-290, 70, 1060);
            mytrees[9].position = new Vector3(30, 70, 1150); 
            mytrees[10].position = new Vector3(-70, 70, 1260);                  
            mytrees[11].position = new Vector3(-280, 70, 1340);       
            mytrees[12].position = new Vector3(-50, 70, 1550);
            mytrees[13].position = new Vector3(-210, 70, 1670);
            mytrees[14].position = new Vector3(-50, 70, 1900);
            mytrees[15].position = new Vector3(-50, 70, 2200);
            mytrees[16].position = new Vector3(-270, 70, 2300);
            mytrees[17].position = new Vector3(-100, 70, 2450);
            mytrees[18].position = new Vector3(20, 70, 2520);
            mytrees[19].position = new Vector3(-220, 70, 2610);
            mytrees[20].position = new Vector3(-40, 70, 2720);
            mytrees[21].position = new Vector3(-250, 70, 2830);
            mytrees[22].position = new Vector3(0, 70, 2950);
            mytrees[23].position = new Vector3(-150, 70, 3100);
            mytrees[24].position = new Vector3(-300, 70, 3200);
            mytrees[25].position = new Vector3(-10, 70, 3300);
            mytrees[26].position = new Vector3(-200, 70, 3500);
            mytrees[27].position = new Vector3(-50, 70, 3600);
        


            for (int i = 0; i < myBoxs.Length-1; i++)
            {
                myBoxs[i].Scale = 1f;
                myBoxs[i].Visible = true;
            }

            //Fighter Initialize
            fighter.skinnedModel = Content.Load<SkinnedModel>("Models\\PlayerMarine");
            fighter.animationController = new AnimationController(fighter.skinnedModel.SkeletonBones);
            fighter.animationController.StartClip(fighter.skinnedModel.AnimationClips["Run"]);
            myFighterState = FighterState.Run;
            fighter.model = fighter.skinnedModel.Model;
            fighter.position = new Vector3(-120, 0, 0); //4800
            fighter.Scale = 1;
            fighter.animationController.Speed = 2f;
            fighter.health = 10;
            Gun = 15;



            
            
            //enemy 
            Enemies[0].skinnedModel = Content.Load<SkinnedModel>("Models/EnemyBeast");
            Enemies[0].animationController = new AnimationController(Enemies[0].skinnedModel.SkeletonBones);
            Enemies[0].myEnemyState = EnemyState.Walk;
            Enemies[0].model = Enemies[0].skinnedModel.Model;
            Enemies[0].position = new Vector3(-200, 0, 1800);
            
            Enemies[1].skinnedModel = Content.Load<SkinnedModel>("Models/EnemyBeast");
            Enemies[1].animationController = new AnimationController(Enemies[1].skinnedModel.SkeletonBones);
            Enemies[1].myEnemyState = EnemyState.Walk;
            Enemies[1].model = Enemies[1].skinnedModel.Model;
            Enemies[1].position = new Vector3(-400, 0, 1000);

            Enemies[2].skinnedModel = Content.Load<SkinnedModel>("Models/EnemyBeast");
            Enemies[2].animationController = new AnimationController(Enemies[2].skinnedModel.SkeletonBones);
            Enemies[2].myEnemyState = EnemyState.Walk;
            Enemies[2].model = Enemies[2].skinnedModel.Model;
            Enemies[2].position = new Vector3(-200, 0, 500);

            Enemies[3].skinnedModel = Content.Load<SkinnedModel>("Models/EnemyBeast");
            Enemies[3].animationController = new AnimationController(Enemies[3].skinnedModel.SkeletonBones);
            Enemies[3].myEnemyState = EnemyState.Walk;
            Enemies[3].model = Enemies[3].skinnedModel.Model;
            Enemies[3].position = new Vector3(50, 0, 800);

            Enemies[4].skinnedModel = Content.Load<SkinnedModel>("Models/EnemyBeast");
            Enemies[4].animationController = new AnimationController(Enemies[4].skinnedModel.SkeletonBones);
            Enemies[4].myEnemyState = EnemyState.Walk;
            Enemies[4].model = Enemies[4].skinnedModel.Model;
            Enemies[4].position = new Vector3(-100, 0, 2200);

            Enemies[5].skinnedModel = Content.Load<SkinnedModel>("Models/EnemyBeast");
            Enemies[5].animationController = new AnimationController(Enemies[5].skinnedModel.SkeletonBones);
            Enemies[5].myEnemyState = EnemyState.Walk;
            Enemies[5].model = Enemies[5].skinnedModel.Model;
            Enemies[5].position = new Vector3(-50, 0, 3400);

            Enemies[6].skinnedModel = Content.Load<SkinnedModel>("Models/EnemyBeast");
            Enemies[6].animationController = new AnimationController(Enemies[6].skinnedModel.SkeletonBones);
            Enemies[6].myEnemyState = EnemyState.Walk;
            Enemies[6].model = Enemies[6].skinnedModel.Model;
            Enemies[6].position = new Vector3(-300, 0, 2600);

            Enemies[7].skinnedModel = Content.Load<SkinnedModel>("Models/EnemyBeast");
            Enemies[7].animationController = new AnimationController(Enemies[7].skinnedModel.SkeletonBones);
            Enemies[7].myEnemyState = EnemyState.Walk;
            Enemies[7].model = Enemies[7].skinnedModel.Model;
            Enemies[7].position = new Vector3(-240, 0, 4100);

            Enemies[8].skinnedModel = Content.Load<SkinnedModel>("Models/EnemyBeast");
            Enemies[8].animationController = new AnimationController(Enemies[8].skinnedModel.SkeletonBones);
            Enemies[8].myEnemyState = EnemyState.Walk;
            Enemies[8].model = Enemies[8].skinnedModel.Model;
            Enemies[8].position = new Vector3(0, 0, 3800);

            Enemies[9].skinnedModel = Content.Load<SkinnedModel>("Models/EnemyBeast");
            Enemies[9].animationController = new AnimationController(Enemies[9].skinnedModel.SkeletonBones);
            Enemies[9].myEnemyState = EnemyState.Walk;
            Enemies[9].model = Enemies[9].skinnedModel.Model;
            Enemies[9].position = new Vector3(0, 0, 2900);


            //Initialize modelAbsoluteBoneTransforms For Every SkinnedModel
          
            for (int i = 0; i < Enemies.Length; i++)
            {
                modelAbsoluteBoneTransforms[i] = new Matrix[Enemies[i].model.Bones.Count];
                Enemies[i].model.CopyAbsoluteBoneTransformsTo(
                    modelAbsoluteBoneTransforms[i]);
                Enemies[i].animationController.StartClip(Enemies[i].skinnedModel.AnimationClips["Walk"]);
                Enemies[i].health = 4;
                Enemies[i].Scale = 1f;
                Enemies[i].animationController.Speed = 2f;
                Enemies[i].Type = ObjectType.Enemy;
                Enemies[i].Visible = true;
            }

        
            //CrossHair Draw
            myCrossHair = Content.Load<Texture2D>("Textures/crosshair");
            
            //Audio
            mysong = Content.Load<Song>("Audio/Jungle");
            myendsong = Content.Load<Song>("Audio2/Wolfenstain");
            MediaPlayer.Play(mysong);
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume = 0.4f;
            
           

            mySoundEffect = Content.Load<SoundEffect>("Audio/Shoot");
            mySoundEffect1 = Content.Load<SoundEffect>("Audio/Die");
            mySoundEffect2 = Content.Load<SoundEffect>("Audio/Feshang");
            mySoundEffect3 = Content.Load<SoundEffect>("Audio/Horror");
            mySoundEffect4= Content.Load<SoundEffect>("Audio/Object");

            myBorder = Content.Load<Texture2D>("Textures/border");

            spriteBatch = new SpriteBatch(graphics.GraphicsDevice);
            spriteFont = Content.Load<SpriteFont>("hudFont");


         
                ground2 = Content.Load<Model>("Model3/ground");
            

            BoundingVolumeRendering.BoundingSphereRenderer.Initialize(GraphicsDevice, 10);
        
        }


        ///////////////////////////////////////////////////////////////////////////////////////////////////////
        protected override void Update(GameTime gameTime)
        {

            mymouse = Mouse.GetState();
            mymouseDown = mymouse;
            keyboard = Keyboard.GetState();
            mouseYposition = mymouse.Y;
            mouseXposition = mymouse.X;
            currentKey = keyboard;
            status = false;
            time1 = gameTime.TotalRealTime.Seconds;
            if (marhale1 == true && firstUse==true)
            {
                elapseTimeSecend = gameTime.TotalRealTime.Seconds;
                elapseTimeMinute = gameTime.TotalRealTime.Minutes;
            }

            
             //AI
             AI();

            
             if (fighter.position.Z > 4100 && marhale1==true)
                    endofgame = true;

             if ((fighter.position.Z > 3500) && (fighter.health>0) && (BombFind==true) && (marhale2==true ))
                 endofgame = true;
            //88
     

             if (status == false)
                  myColor = Color.White;

             
            //Stone PositionUpdate
             if (fighter.position.Z > 1700 && marhale2 == true || backControl == true)
             {
                 for (int i = 0; i < Stone.Length - 1; i++)
                 {
                     Stone[i].position.Z -= 4f;
                 }
                 backControl = true;
             }


            // Update Enemies Position
            UpdateEnemyPos();
            //Enemy Collision
            EnemiesCollisionCheck();

            //Stone Collision
            if (marhale2==true)
            StoneCollisionCheck();
            
                   
            //BOXES Collision
            collision = false;
            status = false;
            BoxCollisionCheck();


            if (marhale1 == true)
            {
                //Trees Collision
                TreesCollisionCheck();
            }

            
            //Box Rotate
            HealthBoxRotation += 0.07f;
            sr -= 0.2f;

            BombCollisionCheck();
            if ( BombFind==true )
            {
                Mybomb.Visible = false;
                BombFind = true;
                status = false;
                collision = false;
            }

            if (marhale2 == true)
            {
                TankerCollisionCheck();
                TruckCollisionCheck();
                

            }

          
            handelUpdate(gameTime);
            CameraUpdate(gameTime);

            perviousKey = currentKey;
                            
            base.Update(gameTime);
        }
        ///////////////////////////////////////////////////////////////////////////////////////////////////////
        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.Gray);
            GraphicsDevice.RenderState.DepthBufferEnable = true;
            GraphicsDevice.RenderState.DepthBufferFunction = CompareFunction.LessEqual;


            if (firstUse == false)
            {
                
                spriteBatch.Begin();
                spriteBatch.Draw(myStartSplash, new Vector2(0, 0), myColor);
                spriteBatch.End();
            
                MouseState mouse = Mouse.GetState();
                dynamicRec = new Rectangle((int)mouse.X, (int)mouse.Y, 50, 50);
                staticRec = new Rectangle(350, 525, 200, 2);

                if (dynamicRec.Intersects(staticRec) && (mouse.LeftButton == ButtonState.Pressed))
                {
                    this.LoadContent();
                    marhale1 = true;
                    firstUse = true;
                }
            }


            if (marhale1 == true)
            {
                //Draw Fighter        
                DrawModel(fighter.skinnedModel, world, cameraView, cameraProjection, fighter.animationController);//8888
                        

                //Draw Enemies
                for (int i = 0; i < Enemies.Length ; i++)
                {
                    {
                        DrawModel(Enemies[i].skinnedModel.Model, enemyworld[i], modelAbsoluteBoneTransforms[i], true, Enemies[i].animationController);
                    }
                }


               
                
                drawMesh(LeftWall, new Vector3(0, -50, 5000), 1, 0);
                drawMesh(RightWall, new Vector3(400, -50, 5000), 1, 0);
                drawMesh(EndWall, new Vector3(-200, 450, 10000), 0.5f, 0);
                drawMesh(myDoor.model, myDoor.position, myDoor.Scale, MathHelper.Pi);
                drawMesh(Tank, new Vector3(0, 50, 14500), 0.3f, 90);
                drawMesh(Tank, new Vector3(0, 50, 14300), 0.3f, 90);
                drawMesh(Tank, new Vector3(0, 50, 14000), 0.3f, 90);
                drawMesh(Tank, new Vector3(-400, 50, 15000), 0.3f, 90);
                drawMesh(Tank, new Vector3(-400, 50, 14500), 0.3f, 90);
                drawMesh(Tank, new Vector3(-400, 50, 14000), 0.3f, 90);
                drawMesh(Tank, new Vector3(-800, 50, 15000), 0.3f, 90);
                drawMesh(Tank, new Vector3(-800, 50, 14500), 0.3f, 90);
                drawMesh(Tank, new Vector3(-800, 50, 14000), 0.3f, 90);
              


                


                //Box
                for (int p = 0; p < myBoxs.Length - 1; p++)
                {
                    if (myBoxs[p].Visible == true)
                        drawMesh(myBoxs[p].model, myBoxs[p].position, myBoxs[p].Scale, HealthBoxRotation);
                }


                //draw Ground
                int k = 0;
                for (int i = 0; i < 15; i++)
                {
                    drawMesh(MyGround, new Vector3(-150, -10, k), 0.5f, 0);
                    k += 1015;
                }

                //Draw Trees

                for (int i = 0; i < mytrees.Length - 1; i++)
                {
                    drawMesh(mytrees[i].model, mytrees[i].position, mytrees[i].Scale, 0);

                }

                DrawModelNames();

                Draw2D();
               

                if (fighter.health == 0 || fighter.health<0)
                {

                    backControl = false;
                    BombFind = false;
                    Mybomb.Visible = true;
                    spriteBatch.Begin();
                    spriteBatch.Draw(GameOver, new Vector2(0, 0), Color.White);
                    spriteBatch.End();

                    MediaPlayer.Volume = 0;
                    dynamicRec = new Rectangle((int)mouseXposition, (int)mouseYposition, 50, 50);
                    staticRec = new Rectangle(180, 400, 210, 20);
                    staticRec2 = new Rectangle(250, 550, 50, 20);

                    if (dynamicRec.Intersects(staticRec) && (mymouse.LeftButton == ButtonState.Pressed))
                    {
                        
                        this.LoadContent();
                    }


                    if (dynamicRec.Intersects(staticRec2) && (mymouse.LeftButton == ButtonState.Pressed))
                        this.Exit();


                }

                if (endofgame == true && marhale2==false)
                {
                   
                    spriteBatch.Begin();
                    spriteBatch.Draw(myendsplash, new Vector2(0, 0), Color.White);
                    spriteBatch.End();
                    dynamicRec = new Rectangle((int)mouseXposition, (int)mouseYposition, 50, 50);
                    staticRec = new Rectangle(400, 490, 150, 2);
                    if (dynamicRec.Intersects(staticRec) && (mymouse.LeftButton == ButtonState.Pressed))
                    {
                        marhale1 = false;
                        marhale2 = true;
                        Gun = 15;
                        fighter.health = 10;
                        endofgame = false;
                        collision = false;
                        EnemyCounter = 10;
                        MediaPlayer.Play(myendsong);  
                        status = false;
                        fighter.position = Vector3.Zero;
                        fighter.animationController.StartClip(fighter.skinnedModel.AnimationClips["Idle"]);
                        myFighterState = FighterState.Idle;
                    
                        for (int p = 0; p < myBoxs.Length; p++)
                        {
                            myBoxs[p].Visible = true;
                        }

                        for (int i = 0; i < Enemies.Length; i++)
                        {
                            Enemies[i].animationController.StartClip( Enemies[i].skinnedModel.AnimationClips["Walk"]);
                        }

                        for (int i = 0; i < Enemies.Length ; i++)
                        {

                            Enemies[i].health = 4;
                        }

                    }
                       
                }
                
            }


            if (endofgame == true && marhale2==true)
            {
                spriteBatch.Begin();
                spriteBatch.Draw(myendsplash2, new Vector2(0, 0), Color.White);
                spriteBatch.End();
                MediaPlayer.Volume = 0;
                dynamicRec = new Rectangle((int)mouseXposition, (int)mouseYposition, 50, 50);
                staticRec = new Rectangle(450, 530, 75, 2);

                if (dynamicRec.Intersects(staticRec) && (mymouse.LeftButton == ButtonState.Pressed))
                {
                    this.Exit();
                }
                
            }


            
          if (marhale2 == true)
          {
                  
              //Draw Enemies
              for (int i = 0; i < Enemies.Length ; i++)
              {

                  DrawModel(Enemies[i].skinnedModel.Model, enemyworld[i], modelAbsoluteBoneTransforms[i], true, Enemies[i].animationController);
                
              }
             
              DrawModel(fighter.skinnedModel, world, cameraView, cameraProjection, fighter.animationController);
             
              //Ground Draw
              int k = 0;
              for (int i = 0; i < 20; i++)
              {
                  drawMesh(MyGround2, new Vector3(0, -10, k), 0.05f, 0);
                 
                  k += 5080;
              }

             
              k = 0;
              for (int i = 0; i < 20; i++)
              {
                  drawMesh(MyGround2, new Vector3(-5079, -10, k), 0.05f, 0);

                  k += 5080;
              }

              //Box
             
              
              for (int p = 0; p < myBoxs.Length-1; p++)
              {
                  if (myBoxs[p].Visible == true)
                      drawMesh(myBoxs[p].model, myBoxs[p].position, myBoxs[p].Scale, HealthBoxRotation);
              }


                     
              
              for (int i = 0; i < MyWaterTower.Length ; i++)
              {
                  drawMesh(MyWaterTower[i].model, MyWaterTower[i].position, MyWaterTower[i].Scale, 0);

              }


              //Bomb
              if (Mybomb.Visible == true)
              {
                  drawMesh(Mybomb.model, Mybomb.position, Mybomb.Scale, 0);
              }

              //Stone

              for (int p = 0; p < Stone.Length - 1; p++)
              {
                  //if (Stone[p].Visible == true)
                  drawMeshWithXRotate(Stone[p].model, Stone[p].position, 1, sr);
              }

              //SandBag
              for (int i = 0; i < sandBag.Length; i++)
              {
                  drawMesh(sandBag[i].model, sandBag[i].position, sandBag[i].Scale, 0);
              }

              //Tanker
              drawMesh(Tanker.model, Tanker.position, Tanker.Scale, -90);

              //Truck
              drawMesh(Truck.model, Truck.position, Truck.Scale, 50);

              //Tank_MG
              for (int i = 0; i < 3; i++)
              {
                  drawMesh(Tank_MG[i].model, Tank_MG[i].position, Tank_MG[i].Scale,MathHelper.Pi);
              }
              for (int i = 3; i < Tank_MG.Length; i++)
              {
                  drawMesh(Tank_MG[i].model, Tank_MG[i].position, Tank_MG[i].Scale, -45);
              }
             

              //Koh
              drawMesh(koh, new Vector3(-500, -10, 14000), 0.3f, 0);

              k = 0;
              for (int i = 0; i < 10; i++)
              {
                  drawMesh(koh,  new Vector3(1500, -10, k), 0.2f, 55);
                  k += 2400;
              }

              k = 0;
              for (int i = 0; i < 10; i++)
              {
                  drawMesh(koh, new Vector3(-2500, -10, k), 0.2f, 55);
                  k += 2400;
              }

            

              
              DrawModelNames();
              Draw2D();

              if (fighter.health == 0 || fighter.health < 0)
              {
                  backControl = false;
                  BombFind = false;
                  Mybomb.Visible = true;
                  spriteBatch.Begin();
                  spriteBatch.Draw(GameOver, new Vector2(0, 0), Color.White);
                  spriteBatch.End();
                  MediaPlayer.Volume = 0;
                  dynamicRec = new Rectangle((int)mouseXposition, (int)mouseYposition, 50, 50);
                  staticRec = new Rectangle(180, 400, 210, 20);
                  staticRec2 = new Rectangle(250, 550, 50, 20);

                  if (dynamicRec.Intersects(staticRec) && (mymouse.LeftButton == ButtonState.Pressed))
                  {
                      marhale1 = true;
                      //firstUse = false;
                      marhale2 = false;
                      this.LoadContent();
                  }



                  if (dynamicRec.Intersects(staticRec2) && (mymouse.LeftButton == ButtonState.Pressed))
                      this.Exit();


              }


              if (endofgame == true && marhale2 == true)
              {
                  spriteBatch.Begin();
                  spriteBatch.Draw(myendsplash2, new Vector2(0, 0), Color.White);
                  spriteBatch.End();
                  MediaPlayer.Volume = 0;
                  dynamicRec = new Rectangle((int)mouseXposition, (int)mouseYposition, 50, 50);
                  staticRec = new Rectangle(450, 530, 75, 2);

                  if (dynamicRec.Intersects(staticRec) && (mymouse.LeftButton == ButtonState.Pressed))
                  {
                      this.Exit();
                  }

              }



          }
          


          base.Draw(gameTime);
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////
        # region DrawModelWith Sphere
        private void DrawModel(Model model, Matrix worldTransform,
                               Matrix[] absoluteBoneTransforms, bool drawBoundingSphere, AnimationController myAnimationController)
        {           
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (SkinnedModelBasicEffect effect in mesh.Effects)
                {
                    
                    effect.LightEnabled = true;
                    effect.EnabledLights = EnabledLights.One;
                    effect.PointLights[0].Color = Vector3.One;
                    effect.Material.DiffuseColor = new Vector3(0.8f);
                    effect.Material.SpecularColor = new Vector3(0.3f);
                    effect.Material.SpecularPower = 8;
                    effect.Bones = myAnimationController.SkinnedBoneTransforms;


                    effect.World = absoluteBoneTransforms[mesh.ParentBone.Index] * worldTransform ;
                    effect.View = cameraView;
                    effect.Projection = cameraProjection;
                  
                    
                }
                mesh.Draw();

                if (drawBoundingSphere)
                {

                    Matrix world = absoluteBoneTransforms[mesh.ParentBone.Index] * worldTransform ;

                    BoundingSphere sphere = TransformBoundingSphere(mesh.BoundingSphere, world);

                    BoundingVolumeRendering.BoundingSphereRenderer.Draw(sphere, cameraView, cameraProjection);


                }
            }
        }

        #endregion
                       
        
        #region Draw 3D Model Of Fighter
        private void DrawModel(SkinnedModel model, Matrix objectWorldMatrix, Matrix view, Matrix projection,
         AnimationController myAnimationController)
        {
            for (int index = 0; index < model.Model.Meshes.Count; index++)
            {
                ModelMesh mesh = model.Model.Meshes[index];
            
                foreach (SkinnedModelBasicEffect effect in mesh.Effects)
                {

                    effect.World = mesh.ParentBone.Transform * objectWorldMatrix * Matrix.CreateScale(fighter.Scale);
                        

                     //OPTIONAL - Configure lights
                    effect.AmbientLightColor = new Vector3(0.1f);
                    effect.LightEnabled = true;
                    effect.EnabledLights = EnabledLights.One;
                    effect.PointLights[0].Color = Vector3.One;
                    effect.PointLights[0].Position = new Vector3(500);

                    //Effect FOG ra Faal MIkonad
                    //effect.FogColor = Color.White.ToVector3();
                    //effect.FogEnabled = true;
                    //effect.FogStart = 500;
                    //effect.FogEnd = 2500;

                    // OPTIONAL - Configure material
                    effect.Material.DiffuseColor = new Vector3(0.8f);
                    effect.Material.SpecularColor = new Vector3(0.3f);
                    effect.Material.SpecularPower = 8;


                    effect.Bones = myAnimationController.SkinnedBoneTransforms;


                   
                    effect.View = cameraView ;
                    effect.Projection = cameraProjection;
                }
                mesh.Draw();
                
            }
        }

        #endregion




        public void drawMeshWithXRotate(Model mymodel, Vector3 pos, float Scale, float AngleRotation)
        {

            Matrix[] trasform = new Matrix[mymodel.Bones.Count];
            mymodel.CopyAbsoluteBoneTransformsTo(trasform);

           Matrix  modelWorldTransforms = Matrix.CreateTranslation(pos);

            foreach (ModelMesh mesh in mymodel.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {

                    effect.EnableDefaultLighting();
                    effect.World = trasform[mesh.ParentBone.Index]
                        * Matrix.CreateRotationX(AngleRotation)
                        * Matrix.CreateTranslation(pos)
                    * Matrix.CreateScale(Scale);

                    effect.View = cameraView;
                    effect.Projection = cameraProjection;
              
                }
                mesh.Draw();
                
            }

        }



        #region Draw 3D MOdel(drawMesh)

        public void drawMesh(Model mymodel, Vector3 pos, float Scale,float AngleRotation)
        {

            Matrix[] trasform = new Matrix[mymodel.Bones.Count];
            mymodel.CopyAbsoluteBoneTransformsTo(trasform);

           Matrix  modelWorldTransforms = Matrix.CreateTranslation(pos);

            foreach (ModelMesh mesh in mymodel.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {

                    effect.EnableDefaultLighting();
                    effect.World = trasform[mesh.ParentBone.Index]
                        * Matrix.CreateRotationY(AngleRotation)
                        * Matrix.CreateTranslation(pos)
                    * Matrix.CreateScale(Scale);

                    effect.View = cameraView;
                    effect.Projection = cameraProjection;
              
                }
                mesh.Draw();
                
            }

        }

        #endregion

        #region Draw 2D Text
        private void Draw2D()
        {
            spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Deferred, SaveStateMode.SaveState);


            //Draw Border
            spriteBatch.Draw(myBorder, new Vector2(0, 625), myColor);
            spriteBatch.DrawString(spriteFont, "" + Gun,
              new Vector2(180, 665), Color.White);          
            spriteBatch.DrawString(spriteFont, "" + fighter.health,
               new Vector2(760, 665), Color.White);
            if (marhale1 == true)
            {
                spriteBatch.DrawString(spriteFont, "" + elapseTimeMinute + " : " + elapseTimeSecend,
                 new Vector2(430, 680), Color.White);

                spriteBatch.DrawString(spriteFont, "Enemy Count : " + EnemyCounter,
               new Vector2(20, 10), Color.White);

            }

            if (marhale2 == true)
            {
                spriteBatch.DrawString(spriteFont, "Enemy Count : "  + EnemyCounter ,
                 new Vector2(20, 10), Color.White);
            }

            if ((BombFind == false) && (fighter.position.Z>3400) && marhale2==true)
            {
                spriteBatch.DrawString(spriteFont, "You Must Find Bomb",
                   new Vector2(350, 660), Color.White);
            }
            spriteBatch.End();

        }
        #endregion

        #region Handel
        public void handelUpdate(GameTime gameTime)
        {


            if (keyboard.IsKeyDown(Keys.Escape))
                Exit();

           
           

            if (keyboard.IsKeyDown(Keys.Escape))
            {
                fighter.animationController.StartClip(fighter.skinnedModel.AnimationClips["Idle"]);
                myFighterState = FighterState.Idle;
            }
            //ViewPort Changing
            if (keyboard.IsKeyDown(Keys.NumPad1))
            {
                cameraOffset = new Vector3(0, 35, -65);
            }

            if (keyboard.IsKeyDown(Keys.NumPad2))
            {
                cameraOffset = new Vector3(0, 50, -95);
            }

            if (keyboard.IsKeyDown(Keys.NumPad3))
            {
                cameraOffset = new Vector3(0, 70, -150);  
            }


            if (angle == 0 && fighter.position.Z <7000)
            {
                if (keyboard.IsKeyDown(Keys.Space) && start==false  )
                {

                    fighter.position = new Vector3(fighter.position.X, 20, fighter.position.Z + 70);
                    start = true;
                }   
            }

            if (angle==MathHelper.Pi && fighter.position.Z>0)
            {
                if (keyboard.IsKeyDown(Keys.Space) && start == false)
                {

                    fighter.position = new Vector3(fighter.position.X, 20, fighter.position.Z -70);
                    start = true;
                }
            }

            if (start == true)
            {
                if (fighter.position.Y > 0)
                {
                    fighter.position.Y -= 2;
                    if (fighter.position.Y == 0)
                        start = false;
                }
            }


            //if (keyboard.IsKeyDown(Keys.Up))
            //{
            //    fighter.animationController.Speed = (fighter.animationController.Speed > 2.5f) ?
            //        2.5f : fighter.animationController.Speed + 0.005f;
            //}
           



            ///////////////////////////  Mouse  ///////////////////////////////////////////

            if (mouseXposition < 300)
            {
                angle = +1f;
            }
            if (mouseXposition > 500)
            {
                angle = -1f;
            }

            if (mouseXposition > 300 && mouseXposition < 500)
            {
                angle = 0f;
            }
            if (mouseYposition > 400)
                angle = MathHelper.Pi;

            if (mouseYposition > 400 && mouseXposition>500)
                angle = 10f;

            if (mouseYposition > 400 && mouseXposition < 300)
                angle = -10f;
            if ((mymouseDown.LeftButton == ButtonState.Pressed)&& (mymouseUp.LeftButton == ButtonState.Released))
            {
               
                if (Gun > 0)
                {
                    fighter.animationController.StartClip(fighter.skinnedModel.AnimationClips["Shoot"]);
                    myFighterState = FighterState.Shoot;
                    mySoundEffect.Play();
                    Gun--;
                }
                else
                {
                    fighter.animationController.StartClip(fighter.skinnedModel.AnimationClips["Idle"]);
                    myFighterState = FighterState.Idle;
                }
               
                
              //Check For Mouse Position On Enemies
                Ray cursorRay = cursor.CalculateCursorRay(cameraProjection, cameraView);
                   
                    for (int i = 0; i < Enemies.Length ; i++)
                    {
                        if (Gun > 0)
                        {

                            if (RayIntersectsModel(cursorRay, Enemies[i].skinnedModel.Model, modelWorldTransforms[i],
                                modelAbsoluteBoneTransforms[i]))
                            {
                                if (Enemies[i].health > 0)
                                {
                                    Enemies[i].health -= 1;

                                    if (Enemies[i].health == 0)
                                    {
                                        EnemyCounter--;
                                        calculateEnemycounter = 10 - EnemyCounter;

                                        if (calculateEnemycounter % 3 == 0)
                                        {
                                            if (fighter.health < 10)
                                                fighter.health += 1;

                                        }
                                    }
                                   
                                }
                                else
                                {
                                    mySoundEffect1.Play();
                                    Enemies[i].animationController.StartClip(Enemies[i].skinnedModel.AnimationClips["Die"]);
                                                                    
                                    Enemies[i].myEnemyState = EnemyState.Die;
                                }
                            }
                        }
                    }
                
                 

                if (mouseXposition < 300)
                {
                    angle = +1f;
                }
                if (mouseXposition > 500)
                {
                    angle = -1f;
                }

                if (mouseXposition > 300 && mouseXposition < 500)
                {
                    angle = 0f;
                }
                
            }
            mymouseUp = mymouseDown;

//////////////////////  KeyBoard In Marhale1  //////////////////////////////////////////////////////////
            if (marhale1==true)
            {
            //If Status=True Collision Is Occure
            if (status == false)
            {
                if (keyboard.IsKeyDown(Keys.W) && (fighter.position.Z < 4950))
                {
                    if (myFighterState != FighterState.Run)
                    {
                        fighter.animationController.StartClip(fighter.skinnedModel.AnimationClips["Run"]);
                    }
                    fighter.position += new Vector3(0, 0, 1);
                    angle = 0f;
                    myFighterState = FighterState.Run;

                }
            }
                //Dalil Estefade Az Sharte Zir In Ast Ke ,Agar Fighter Ba Yeki Az ModelHa Tasadof Dashte Bashad Digar Be Dakhel Model Nemiravad
            else if (currentKey != perviousKey )
            {
                if (keyboard.IsKeyDown(Keys.W) && (fighter.position.Z < 4950))
                {
                    if (myFighterState != FighterState.Run)
                    {
                        fighter.animationController.StartClip(fighter.skinnedModel.AnimationClips["Run"]);
                    }
                    fighter.position += new Vector3(0, 0, 1);
                    angle = 0f;
                    myFighterState = FighterState.Run;

                }

            }

            

            if (status == false)
            {
                if ((keyboard.IsKeyDown(Keys.A) && (fighter.position.X < 50)))
                {
                    if (myFighterState != FighterState.Run)
                    {
                        fighter.animationController.StartClip(fighter.skinnedModel.AnimationClips["Run"]);
                    }
                    fighter.position += new Vector3(1, 0, 0);
                    angle = 90f;
                    myFighterState = FighterState.Run;

                }
            }
            
            else if (currentKey != perviousKey)
            {
                if ((keyboard.IsKeyDown(Keys.A) && (fighter.position.X < 50)))
                {
                    if (myFighterState != FighterState.Run)
                    {
                        fighter.animationController.StartClip(fighter.skinnedModel.AnimationClips["Run"]);
                    }
                    fighter.position += new Vector3(1, 0, 0);
                    angle = 90f;
                    myFighterState = FighterState.Run;

                }

            }



            if (status == false)
            {
                if ((keyboard.IsKeyDown(Keys.D) && (fighter.position.X > -300)))
                {
                    if (myFighterState != FighterState.Run)
                    {
                        fighter.animationController.StartClip(fighter.skinnedModel.AnimationClips["Run"]);
                    }
                    fighter.position += new Vector3(-1, 0, 0);
                    angle = -90f;
                    myFighterState = FighterState.Run;

                }
            }

            else if (currentKey != perviousKey)
            {
                if ((keyboard.IsKeyDown(Keys.D) && (fighter.position.X > -300)))
                {
                    if (myFighterState != FighterState.Run)
                    {
                        fighter.animationController.StartClip(fighter.skinnedModel.AnimationClips["Run"]);
                    }
                    fighter.position += new Vector3(-1, 0, 0);
                    angle = -90f;
                    myFighterState = FighterState.Run;

                }

            }

         
        

           
            if (status == false)
            {
                if (keyboard.IsKeyDown(Keys.S))
                {
                    if (myFighterState != FighterState.Run)
                    {
                        fighter.animationController.StartClip(fighter.skinnedModel.AnimationClips["Run"]);
                    }
                    if(fighter.position.Z>0)
                    fighter.position += new Vector3(0, 0, -1);
                    angle = MathHelper.Pi;
                    myFighterState = FighterState.Run;

                }
            }
            else if (currentKey != perviousKey)
            {
                if (keyboard.IsKeyDown(Keys.S))
                {
                    if (myFighterState != FighterState.Run)
                    {
                        fighter.animationController.StartClip(fighter.skinnedModel.AnimationClips["Run"]);
                    }
                    if (fighter.position.Z > 0)
                    fighter.position += new Vector3(0, 0, -1);
                    angle = MathHelper.Pi;
                    myFighterState = FighterState.Run;

                }

            }



            if (status == false)
            {
                if ((keyboard.IsKeyDown(Keys.S) && keyboard.IsKeyDown(Keys.D)) && (fighter.position.X>-300))
                {
                    if (myFighterState != FighterState.Run)
                    {
                        fighter.animationController.StartClip(fighter.skinnedModel.AnimationClips["Run"]);
                    }
                    if (fighter.position.Z > 0)
                    fighter.position += new Vector3(-1, 0, -1);
                    angle = -15f;
                    myFighterState = FighterState.Run;
                }
            }
            else if (currentKey != perviousKey)
            {
                if ((keyboard.IsKeyDown(Keys.S) && keyboard.IsKeyDown(Keys.D)) && (fighter.position.X > -300))
                {
                    if (myFighterState != FighterState.Run)
                    {
                        fighter.animationController.StartClip(fighter.skinnedModel.AnimationClips["Run"]);
                    }
                    if (fighter.position.Z > 0)
                    fighter.position += new Vector3(-1, 0, -1);
                    angle = -15f;
                    myFighterState = FighterState.Run;
                }

            }



            if (status == false)
            {
                if ((keyboard.IsKeyDown(Keys.S) && keyboard.IsKeyDown(Keys.A)) && (fighter.position.X < 50))
                {
                    if (myFighterState != FighterState.Run)
                    {
                        fighter.animationController.StartClip(fighter.skinnedModel.AnimationClips["Run"]);
                    }
                    if (fighter.position.Z > 0)
                    fighter.position += new Vector3(1, 0, -1);
                    angle = 15f;
                    myFighterState = FighterState.Run;
                }
            }
            else if (currentKey != perviousKey)
            {
                if ((keyboard.IsKeyDown(Keys.S) && keyboard.IsKeyDown(Keys.A)) && (fighter.position.X < 50))
                {
                    if (myFighterState != FighterState.Run)
                    {
                        fighter.animationController.StartClip(fighter.skinnedModel.AnimationClips["Run"]);
                    }
                    if (fighter.position.Z > 0)
                    fighter.position += new Vector3(1, 0, -1);
                    angle = 15f;
                    myFighterState = FighterState.Run;
                }

            }



            if (status == false)
            {
                if ((keyboard.IsKeyDown(Keys.D) && keyboard.IsKeyDown(Keys.W)) && (fighter.position.X > -300) && (fighter.position.Z < 4950))
                {
                    if (myFighterState != FighterState.Run)
                    {
                        fighter.animationController.StartClip(fighter.skinnedModel.AnimationClips["Run"]);
                    }
                    fighter.position += new Vector3(-1, 0, 1);
                    angle = -1f;
                    myFighterState = FighterState.Run;
                }
            }
            else if (currentKey != perviousKey)
            {
                if ((keyboard.IsKeyDown(Keys.D) && keyboard.IsKeyDown(Keys.W)) && (fighter.position.X > -300) && (fighter.position.Z < 4950))
                {
                    if (myFighterState != FighterState.Run)
                    {
                        fighter.animationController.StartClip(fighter.skinnedModel.AnimationClips["Run"]);
                    }
                    fighter.position += new Vector3(-1, 0, 1);
                    angle = -1f;
                    myFighterState = FighterState.Run;
                }

            }



            if (status == false)
            {
                if ((keyboard.IsKeyDown(Keys.A) && keyboard.IsKeyDown(Keys.W)) && (fighter.position.X < 50) && (fighter.position.Z < 4950))
                {
                    if (myFighterState != FighterState.Run)
                    {
                        fighter.animationController.StartClip(fighter.skinnedModel.AnimationClips["Run"]);
                    }
                    fighter.position += new Vector3(1, 0, 1);
                    angle = 1f;
                    myFighterState = FighterState.Run;

                }
            }
            else if (currentKey != perviousKey)
            {

                if ((keyboard.IsKeyDown(Keys.A) && keyboard.IsKeyDown(Keys.W)) && (fighter.position.X < 50) && (fighter.position.Z < 4950))
                {
                    if (myFighterState != FighterState.Run)
                    {
                        fighter.animationController.StartClip(fighter.skinnedModel.AnimationClips["Run"]);
                    }
                    fighter.position += new Vector3(1, 0, 1);
                    angle = 1f;
                    myFighterState = FighterState.Run;

                }

            }
        }
            collision = false;
//////////////////////  KeyBoard In Marhale2   //////////////////////////////////////////////////////////
            if (marhale2 == true)
            {
                //If Status=True Collision Is Occure
                if (status == false)
                {
                    if (keyboard.IsKeyDown(Keys.W) && (fighter.position.Z < 3520))
                    {
                        if (myFighterState != FighterState.Run)
                        {
                            fighter.animationController.StartClip(fighter.skinnedModel.AnimationClips["Run"]);
                        }
                        fighter.position += new Vector3(0, 0, 1);
                        angle = 0f;
                        myFighterState = FighterState.Run;

                    }
                }
                //Dalil Estefade Az Sharte Zir In Ast Ke ,Agar Fighter Ba Yeki Az ModelHa Tasadof Dashte Bashad Digar Be Dakhel Model Nemiravad
                else if (currentKey != perviousKey)
                {
                    if (keyboard.IsKeyDown(Keys.W) && (fighter.position.Z < 3520))
                    {
                        if (myFighterState != FighterState.Run)
                        {
                            fighter.animationController.StartClip(fighter.skinnedModel.AnimationClips["Run"]);
                        }
                        fighter.position += new Vector3(0, 0, 1);
                        angle = 0f;
                        myFighterState = FighterState.Run;

                    }

                }



                if (status == false)
                {
                    if ((keyboard.IsKeyDown(Keys.A) && (fighter.position.X < 120)))
                    {
                        if (myFighterState != FighterState.Run)
                        {
                            fighter.animationController.StartClip(fighter.skinnedModel.AnimationClips["Run"]);
                        }
                        fighter.position += new Vector3(1, 0, 0);
                        angle = 90f;
                        myFighterState = FighterState.Run;

                    }
                }

                else if (currentKey != perviousKey)
                {
                    if ((keyboard.IsKeyDown(Keys.A) && (fighter.position.X < 120)))
                    {
                        if (myFighterState != FighterState.Run)
                        {
                            fighter.animationController.StartClip(fighter.skinnedModel.AnimationClips["Run"]);
                        }
                        fighter.position += new Vector3(1, 0, 0);
                        angle = 90f;
                        myFighterState = FighterState.Run;

                    }

                }



                if (status == false)
                {
                    if ((keyboard.IsKeyDown(Keys.D) && (fighter.position.X > -220)))
                    {
                        if (myFighterState != FighterState.Run)
                        {
                            fighter.animationController.StartClip(fighter.skinnedModel.AnimationClips["Run"]);
                        }
                        fighter.position += new Vector3(-1, 0, 0);
                        angle = -90f;
                        myFighterState = FighterState.Run;

                    }
                }

                else if (currentKey != perviousKey)
                {
                    if ((keyboard.IsKeyDown(Keys.D) && (fighter.position.X > -220)))
                    {
                        if (myFighterState != FighterState.Run)
                        {
                            fighter.animationController.StartClip(fighter.skinnedModel.AnimationClips["Run"]);
                        }
                        fighter.position += new Vector3(-1, 0, 0);
                        angle = -90f;
                        myFighterState = FighterState.Run;

                    }

                }





                if (status == false)
                {
                    if (keyboard.IsKeyDown(Keys.S))
                    {
                        if (myFighterState != FighterState.Run)
                        {
                            fighter.animationController.StartClip(fighter.skinnedModel.AnimationClips["Run"]);
                        }
                        if (fighter.position.Z > 0)
                            fighter.position += new Vector3(0, 0, -1);
                        angle = MathHelper.Pi;
                        myFighterState = FighterState.Run;

                    }
                }
                else if (currentKey != perviousKey)
                {
                    if (keyboard.IsKeyDown(Keys.S))
                    {
                        if (myFighterState != FighterState.Run)
                        {
                            fighter.animationController.StartClip(fighter.skinnedModel.AnimationClips["Run"]);
                        }
                        if (fighter.position.Z > 0)
                            fighter.position += new Vector3(0, 0, -1);
                        angle = MathHelper.Pi;
                        myFighterState = FighterState.Run;

                    }

                }



                if (status == false)
                {
                    if ((keyboard.IsKeyDown(Keys.S) && keyboard.IsKeyDown(Keys.D)) && (fighter.position.X > -220))
                    {
                        if (myFighterState != FighterState.Run)
                        {
                            fighter.animationController.StartClip(fighter.skinnedModel.AnimationClips["Run"]);
                        }
                        if (fighter.position.Z > 0)
                            fighter.position += new Vector3(-1, 0, -1);
                        angle = -15f;
                        myFighterState = FighterState.Run;
                    }
                }
                else if (currentKey != perviousKey)
                {
                    if ((keyboard.IsKeyDown(Keys.S) && keyboard.IsKeyDown(Keys.D)) && (fighter.position.X > -220))
                    {
                        if (myFighterState != FighterState.Run)
                        {
                            fighter.animationController.StartClip(fighter.skinnedModel.AnimationClips["Run"]);
                        }
                        if (fighter.position.Z > 0)
                            fighter.position += new Vector3(-1, 0, -1);
                        angle = -15f;
                        myFighterState = FighterState.Run;
                    }

                }



                if (status == false)
                {
                    if ((keyboard.IsKeyDown(Keys.S) && keyboard.IsKeyDown(Keys.A)) && (fighter.position.X < 120))
                    {
                        if (myFighterState != FighterState.Run)
                        {
                            fighter.animationController.StartClip(fighter.skinnedModel.AnimationClips["Run"]);
                        }
                        if (fighter.position.Z > 0)
                            fighter.position += new Vector3(1, 0, -1);
                        angle = 15f;
                        myFighterState = FighterState.Run;
                    }
                }
                else if (currentKey != perviousKey)
                {
                    if ((keyboard.IsKeyDown(Keys.S) && keyboard.IsKeyDown(Keys.A)) && (fighter.position.X < 120))
                    {
                        if (myFighterState != FighterState.Run)
                        {
                            fighter.animationController.StartClip(fighter.skinnedModel.AnimationClips["Run"]);
                        }
                        if (fighter.position.Z > 0)
                            fighter.position += new Vector3(1, 0, -1);
                        angle = 15f;
                        myFighterState = FighterState.Run;
                    }

                }



                if (status == false)
                {
                    if ((keyboard.IsKeyDown(Keys.D) && keyboard.IsKeyDown(Keys.W)) && (fighter.position.X > -220) && (fighter.position.Z < 3520))
                    {
                        if (myFighterState != FighterState.Run)
                        {
                            fighter.animationController.StartClip(fighter.skinnedModel.AnimationClips["Run"]);
                        }
                        fighter.position += new Vector3(-1, 0, 1);
                        angle = -1f;
                        myFighterState = FighterState.Run;
                    }
                }
                else if (currentKey != perviousKey)
                {
                    if ((keyboard.IsKeyDown(Keys.D) && keyboard.IsKeyDown(Keys.W)) && (fighter.position.X > -220) && (fighter.position.Z < 3520))
                    {
                        if (myFighterState != FighterState.Run)
                        {
                            fighter.animationController.StartClip(fighter.skinnedModel.AnimationClips["Run"]);
                        }
                        fighter.position += new Vector3(-1, 0, 1);
                        angle = -1f;
                        myFighterState = FighterState.Run;
                    }

                }



                if (status == false)
                {
                    if ((keyboard.IsKeyDown(Keys.A) && keyboard.IsKeyDown(Keys.W)) && (fighter.position.X < 120) && (fighter.position.Z < 3520))
                    {
                        if (myFighterState != FighterState.Run)
                        {
                            fighter.animationController.StartClip(fighter.skinnedModel.AnimationClips["Run"]);
                        }
                        fighter.position += new Vector3(1, 0, 1);
                        angle = 1f;
                        myFighterState = FighterState.Run;

                    }
                }
                else if (currentKey != perviousKey)
                {

                    if ((keyboard.IsKeyDown(Keys.A) && keyboard.IsKeyDown(Keys.W)) && (fighter.position.X < 120) && (fighter.position.Z < 3520))
                    {
                        if (myFighterState != FighterState.Run)
                        {
                            fighter.animationController.StartClip(fighter.skinnedModel.AnimationClips["Run"]);
                        }
                        fighter.position += new Vector3(1, 0, 1);
                        angle = 1f;
                        myFighterState = FighterState.Run;

                    }

                }
            }

            collision = false;
            

        }
        #endregion


        public void TreesCollisionCheck()
        {
            if (marhale1 == true)
            {
                for (int i = 0; i < mytrees.Length - 1; i++)
                {
                    if (fighter.health > 0)
                    {
                        mycheckCollision.CheckForCollisions(fighter, mytrees[i]);
                        status = collision;
                    }
                }
            }
        }


        

        public void BombCollisionCheck()
        {
            if (marhale2 == true)
            {
                if (fighter.health > 0)
                {
                    collision = false;
                    status = false;
                    mycheckCollision.CheckForCollisions(fighter, Mybomb);
                    status = collision;
                    if (status == true)
                        BombFind = true;

                }
            }
            
        }



        public void TankerCollisionCheck()
        {
            if (marhale2 == true)
            {
                if (fighter.health > 0)
                {
                    mycheckCollision.CheckForCollisions(fighter, Tanker);
                    status = collision;
                }
            }
        }   
            
        

        public void TruckCollisionCheck()
        {
            if (marhale2 == true)
            {
                if (fighter.health > 0)
                {
                    mycheckCollision.CheckForCollisions(fighter, Truck);
                    status = collision;
                }
            }
        }
                    


        public void StoneCollisionCheck()
        {
            if (marhale2 == true)
            {
                for (int i = 0; i < Stone.Length - 1; i++)
                {
                    collision = false;
                    mycheckCollision.CheckForCollisions(fighter, Stone[i]);

                    status = collision;
                    if (fighter.health > 0)
                    {
                        if (status == true)
                        {

                            myColor = Color.Red;
                            firstcol = time1;
                            if (lastcol != 0)
                            {
                                resultcol = firstcol - lastcol;

                                if (resultcol != 0)
                                    resultcol = 1;

                                float sZPoz = Stone[i].position.Z;
                                float fZpos = fighter.position.Z;
                                float subz = sZPoz - fZpos;
                                if (subz > -5 && subz < 5)
                                    resultcol = 10;

                                fighter.health -= resultcol;
                                myColor = Color.Red;
                            }
                        }
                    }
                    lastcol = firstcol;


                }
            }
        }

        
        public void BoxCollisionCheck()
        {
           
                for (int i = 0; i < myBoxs.Length - 1; i++)
                {
                    if (myBoxs[i].Type == ObjectType.HealthBox)
                    {
                        if (myBoxs[i].Visible == true && fighter.health<10)
                        {
                            mycheckCollision.CheckForCollisions(fighter, myBoxs[i]);
                            status = collision;

                            if (status == true)
                            {

                                mySoundEffect4.Play();
                                myBoxs[i].Visible = false;
                                if (fighter.health<=2)
                                    fighter.health += 2;
                                else
                                    fighter.health = 10;
                            }
                            collision = false;
                        }
                    }

                    if (myBoxs[i].Type == ObjectType.GunBox)
                    {
                        if (myBoxs[i].Visible == true)
                        {
                            mycheckCollision.CheckForCollisions(fighter, myBoxs[i]);
                            status = collision;
                            if (status == true)
                            {

                                mySoundEffect2.Play();
                                myBoxs[i].Visible = false;
                                Gun += 5;
                            }
                            collision = false;
                        }
                    }

                }
            
        }

               

                
            
        


        public void EnemiesCollisionCheck()
        {
            //if Fighter Collision With Enemies,Fighter Health Must Reduce.
            for (int k = 0; k < Enemies.Length ; k++)
            {
                if (Enemies[k].health > 0 && Enemies[k].Visible == true)
                {
                    mycheckCollision.CheckForCollisions(fighter, Enemies[k]);
                    
                    status = collision;
                    if (fighter.health > 0)
                    {
                        if (status == true)
                        {

                            myColor = Color.Red;
                            firstcol = time1;
                            if (lastcol != 0)
                            {
                                resultcol = firstcol - lastcol;

                                if (resultcol != 0)
                                    resultcol = 1;

                                Enemies[k].animationController.StartClip(Enemies[k].skinnedModel.AnimationClips["Stomp"]);
                                Enemies[k].myEnemyState = EnemyState.Stomp;
                                fighter.health -= resultcol;
                                myColor = Color.Red;
                            }
                        }
                    }
                    lastcol = firstcol;


                }
            }
        }






        public void UpdateEnemyPos()
        {

            for (int i = 0; i < Enemies.Length ; i++)
            {
                if (Enemies[i].health > 0)
                {
                    modelWorldTransforms[i] = Matrix.CreateTranslation(Enemies[i].position);
                }

                else
                {
                    if(marhale2==true)
                    Enemies[i].position.Y = 10f;
                    
                    if (marhale1==true)
                    Enemies[i].position.Y = 5f;

                }
            }
        }

        #region CameraUpdate
        public void CameraUpdate(GameTime gameTime)
        {
            Vector3 transformedOffset =
            Vector3.Transform(cameraOffset, Matrix.CreateRotationY(0) * Matrix.CreateScale(fighter.Scale)  );
            cameraPosition = fighter.position + transformedOffset;

            cameraView = Matrix.CreateLookAt(cameraPosition, fighter.position, Vector3.Up);
            cameraProjection = Matrix.CreatePerspectiveFieldOfView(1, 1, 1, 10000);


            world = Matrix.CreateRotationY(angle) * Matrix.CreateTranslation(fighter.position) ;
            fighter.animationController.Update(gameTime.ElapsedGameTime, Matrix.Identity);
            
            for (int i = 0; i < Enemies.Length ; i++)
            {
                    if(Enemies[i].position.Z<fighter.position.Z)
                    enemyworld[i] = Matrix.CreateRotationY(0) * Matrix.CreateTranslation(Enemies[i].position);

                    if (Enemies[i].position.Z > fighter.position.Z)
                        enemyworld[i] = Matrix.CreateRotationY(MathHelper.Pi) * Matrix.CreateTranslation(Enemies[i].position);

                    Enemies[i].animationController.Update(gameTime.ElapsedGameTime, Matrix.Identity);
                
                if (Enemies[i].health < 1)
                {
                    enemyworld[i] = Matrix.CreateRotationX(MathHelper.Pi) * Matrix.CreateTranslation(Enemies[i].position);
                    
                    
                    
                }
            }

        }
        #endregion

        
        #region DrawModelName

        private void DrawModelNames()
        {
            
            spriteBatch.Begin();
            Ray cursorRay = cursor.CalculateCursorRay(cameraProjection, cameraView);

            for (int i = 0; i < Enemies.Length; i++)
            {
                
                if (RayIntersectsModel(cursorRay, Enemies[i].skinnedModel.Model, modelWorldTransforms[i],
                    modelAbsoluteBoneTransforms[i]))
                {
                    Vector3 screenSpace = graphics.GraphicsDevice.Viewport.Project(
                        Vector3.Zero, cameraProjection, cameraView,
                        modelWorldTransforms[i]);
                 
                    Vector2 textPosition =
                        new Vector2(screenSpace.X, screenSpace.Y - 60);
                  
                    
                    spriteBatch.DrawString(spriteFont, ""+ Enemies[i].health,
                        textPosition, Color.White);
                }
            }


            spriteBatch.End();
        }

        #endregion

        
        #region RayIntersectsModel
        //Baraye Baresiye INke Mouse Roye Model Ghara Grefte Ast Ya Na.
        private static bool RayIntersectsModel(Ray ray, Model model,
           Matrix worldTransform, Matrix[] absoluteBoneTransforms)
        {
            foreach (ModelMesh mesh in model.Meshes)
            {
                Matrix world2 = absoluteBoneTransforms[mesh.ParentBone.Index] * worldTransform ;
                BoundingSphere sphere = TransformBoundingSphere(mesh.BoundingSphere, world2);
                
                if (sphere.Intersects(ray) != null)
                {
                    return true;
                }
            }

            return false;
        }
        #endregion


        public void AI()
        {
            if (firstUse == true)
            {
                float FighterZpos = fighter.position.Z;
                float FighterXpos = fighter.position.X;
                for (int i = 0; i < Enemies.Length; i++)
                {
                    if (Enemies[i].health > 0 && Enemies[i].Visible == true)
                    {
                        float fpos = fighter.position.Z;
                        float epos = Enemies[i].position.Z;
                        float sub = epos - fpos;
                        if (sub > -800 && sub < 800)
                        {
                            if (Enemies[i].position.X < FighterXpos)
                                Enemies[i].position += new Vector3(0.5f, 0, 0);

                            if (Enemies[i].position.X > FighterXpos)
                                Enemies[i].position += new Vector3(-0.5f, 0, 0);

                            if (Enemies[i].position.Z < FighterZpos)
                                Enemies[i].position += new Vector3(0, 0, 0.5f);

                            if (Enemies[i].position.Z > FighterZpos)
                                Enemies[i].position += new Vector3(0, 0, -0.5f);
                        }
                    }
                }
            }
        }


        #region TransformBoundingSphere
        // Modelhara Ba boundingShpere Miposhanad Ta Tadakhol Model Ba Mouse Tashkhis Dade Shavad
        private static BoundingSphere TransformBoundingSphere(BoundingSphere sphere, Matrix transform)
        {
            BoundingSphere transformedSphere;

            Vector3 scale3 = new Vector3(sphere.Radius, sphere.Radius, sphere.Radius);

            scale3 = Vector3.TransformNormal(scale3, transform);

            transformedSphere.Radius = Math.Max(scale3.X, Math.Max(scale3.Y, scale3.Z));

            transformedSphere.Center = Vector3.Transform(sphere.Center, transform);

            return transformedSphere;
        }
        #endregion
        


        #region Entry Point
        static void Main(string[] args)
        {
            using (XNAnimationSample game = new XNAnimationSample())
            {
                game.Run();
            }
        }
        #endregion
    }
}
