using SpaceInvaders.src;
using SpaceInvaders.src.Systems;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Schema;

namespace SpaceInvaders
{
    /// <summary>
    /// the coordinator is the main piece of our ECS implementation
    /// it is used to link systems, entites and component
    /// for exemple the coordinator will be in charge to initialize all of our starting entities
    /// one of the main coordinator functionality is to render the scene 
    /// </summary>
    class Coordinator
    {
        private static Dictionary<int, Bitmap> ennemiesSprite;

        private static void InitializeDictionary()
        {
            ennemiesSprite = new Dictionary<int, Bitmap>
            {
                { 1, Properties.Resources.ship3 },
                { 2, Properties.Resources.ship2 },
                { 3, Properties.Resources.ship2 },
                { 4, Properties.Resources.ship1 },
                { 5, Properties.Resources.ship1 },
            };
        }
        
        private SystemManager systemManager;
        private EntityManager entityManager;

        public EntityManager EntityManager
        {
            get => this.entityManager;
            set => this.entityManager = value;         
        }

        public Coordinator()
        {
            this.systemManager = SystemManager.CreateSystemManager();
            this.entityManager = EntityManager.CreateEntityManager();
            InitializeDictionary();
        }

        public void Init()
        {
            this.InitEntities();
            this.InitSystems();
        }

        /// <summary>
        /// initialize all entites that are present when the game start
        /// </summary>
        private void InitEntities()
        {
            this.InitPlayer();
            this.InitBunkers();
            this.InitEnemies();
        }

        /// <summary>
        /// Init the player entity
        /// </summary>
        private void InitPlayer()
        {
            Entity player = new Entity(); // the player need to be the first entity created, to have the id 0
            Bitmap playerSprite = Properties.Resources.player1;
            Vec2 playerPosition = new Vec2(Game.windowSize.Width / 2 - playerSprite.Width / 2, 550);
            player.RegisterComponents
                (
                    new PositionComponent(playerPosition),
                    new SpriteComponent(playerSprite),
                    new DirectionComponent(new Vec2(0, 0)),
                    new InputComponent(),
                    new VelocityComponent(Config.PLAYER_VELOCITY.Clone()),
                    new ShootComponent(),
                    new HitboxComponent(playerPosition, playerSprite.Width, playerSprite.Height),
                    new HealthComponent(Config.PLAYER_LIVES),
                    new CollidableComponent(),
                    new FacingComponent(-1),
                    new PixelToSwitchComponent()
                );
            this.entityManager.AddEntity(player);
        }

        /// <summary>
        /// Init a player entity
        /// </summary>
        /// <param name="x"> the enemy x position </param>
        /// <param name="y"> the enemy y position </param>
        /// <param name="ship"> the enemy ship sprite </param>
        private void InitEnemy(double x, double y, Bitmap ship)
        {
            Entity enemy = new Entity();
            Vec2 enemyPosition = new Vec2(x, y);
            enemy.RegisterComponents
                (
                    new PositionComponent(enemyPosition),
                    new SpriteComponent(ship),
                    new HitboxComponent(enemyPosition, ship.Width, ship.Height),
                    new CollidableComponent(),
                    new BlockConstituentComponent(),
                    new VelocityComponent(Config.ENEMY_VELOCITY.Clone()),
                    new DirectionComponent(new Vec2(1, 0)),
                    new HealthComponent(Config.ENEMY_LIVES),
                    new ShootComponent(),
                    new FacingComponent(1),
                    new PixelToSwitchComponent(),
                    new DeathSpriteComponent(),
                    new DeathSoundComponent()
                );
            ;
            this.entityManager.AddEntity(enemy);
        }

        /// <summary>
        /// init all enemies who are present in the game 
        /// </summary>
        private void InitEnemies()
        {
            int numberOfRow = 6;
            int numberOfEnnemiesPerRow = 7;
            int initialSpacingValue = 100;
            int spacing = initialSpacingValue;
            int colScale = 25;
            int y = 0;
            for (int row = 1; row < numberOfRow; row++)
            {
                for (int col = 1; col <= numberOfEnnemiesPerRow; col++)
                {
                    Bitmap enemySprite =  (Bitmap) Coordinator.ennemiesSprite[row].Clone();
                    InitEnemy(col * colScale + spacing - enemySprite.Width / 2, y, enemySprite);
                    spacing += 30;
                }
                spacing = initialSpacingValue;
                y += 35;
            }
        }

        /// <summary>
        /// Init a bunker 
        /// </summary>
        /// <param name="x"> the bunker x position </param>
        /// <param name="y"> the bunker y position </param>
        private void InitBunker(double x, double y)
        {
            Entity bunker = new Entity();
            Vec2 bunkerPosition = new Vec2(x, y);
            Bitmap bunkerSprite = Properties.Resources.bunker;
            bunker.RegisterComponents
                (
                    new PositionComponent(new Vec2(x, y)),
                    new HitboxComponent(bunkerPosition, bunkerSprite.Width, bunkerSprite.Height),
                    new SpriteComponent(bunkerSprite),
                    new CollidableComponent(),
                    new PixelToSwitchComponent()
                );
            this.entityManager.AddEntity(bunker);
        }

        /// <summary>
        /// initialize all the bunker present who are present in the game  
        /// </summary>
        private void InitBunkers()
        {
            int numberOfBunkers = 3;
            for (int i = 1; i <= numberOfBunkers; i++)
            {
                int bunkerWidth = Properties.Resources.bunker.Width;
                InitBunker(i * Game.windowSize.Width / (numberOfBunkers + 1) - bunkerWidth / 2, Game.windowSize.Height - 150);
            }
        }

        /// <summary>
        /// Initialize all systems ref and adding them into the system manager 
        /// </summary>
        private void InitSystems()
        {
            this.systemManager.AddSystems
                (
                     new TransformSystem
                        (
                            this.entityManager.Entities,
                            "PositionComponent", "DirectionComponent", "VelocityComponent", "HitboxComponent"
                        ),
                     new InputSystem
                        (
                            this.entityManager.Entities,
                            "InputComponent", "DirectionComponent", "ShootComponent"
                        ),
                    new ShootSystem
                        (
                            this.entityManager.Entities,
                            this.entityManager,
                            "ShootComponent", "PositionComponent", "FacingComponent"
                        ),
                     new DeathSystem
                        (
                            this.entityManager.Entities,
                            this.entityManager,
                            "HealthComponent" 
                        ),
                    new HitSystem
                        (
                            this.entityManager.Entities,
                            "HealthComponent"
                        ),
                    new CollidableSystem
                        (
                            this.entityManager.Entities,
                            "PositionComponent", "HitboxComponent", "SpriteComponent"
                        ),
                    new BlockSystem
                        (
                            this.entityManager.Entities,
                            "PositionComponent", "BlockConstituentComponent", "DirectionComponent", "HitboxComponent"
                        ),
                    new HitboxSyncSystem
                        (
                            this.entityManager.Entities,
                            "HitboxComponent", "PositionComponent"
                        ),
                    new AiSystem
                        (
                            this.entityManager.Entities,
                            "ShootComponent" , "BlockConstituentComponent", "PositionComponent"
                        ),
                    new ChangeColorPixelSystem
                        (
                            this.entityManager.Entities,
                            "PixelToSwitchComponent", "SpriteComponent"
                        )
                 );
        }

        /// <summary>
        /// reset the game.
        /// this function is to be used when the player win, or lose and want to play again  
        /// </summary>
        public void ResetGame()
        {
            this.entityManager.currentEntityId = 0;
            this.entityManager.Entities.Clear();
            this.InitEntities();
        }

        /// <summary>
        /// update the system manager 
        /// </summary>
        public void Update()
        {
            this.systemManager.Update(this.entityManager.Entities);
        }

        /// <summary>
        /// render the scene when the game is running,
        /// that is when the player is actually playing and have not : lose, win or pause 
        /// </summary>
        /// <param name="graphics"> game graphics instance </param>
        public void RenderSceneGameRunning(Graphics graphics)
        {
            foreach (Entity entity in this.entityManager.Entities)
            {
                if (entity.HasComponents( "SpriteComponent", "PositionComponent"))
                {
                    PositionComponent positionComponent = (PositionComponent) entity.GetComponent("PositionComponent");
                    SpriteComponent spriteComponent = (SpriteComponent) entity.GetComponent("SpriteComponent");
                    int width = spriteComponent.Sprite.Width;
                    int height = spriteComponent.Sprite.Height;
                    graphics.DrawImage
                        (
                            spriteComponent.Sprite,
                            (float)(positionComponent.Position.X),
                            (float)(positionComponent.Position.Y),
                            width,
                            height
                        );
                }
                this.RenderPlayerLives(graphics);
                this.RenderCurrentPlayerScore(graphics);
            }
        }


        /// <summary>
        /// render the player lives in the left corner of the screen 
        /// </summary>
        /// <param name="graphics"> game graphics instance </param>
        public void RenderPlayerLives(Graphics graphics)
        {
            Entity player = this.entityManager.GetEntityById(0);
            HealthComponent playerHealthComponent = (HealthComponent) player.GetComponent("HealthComponent");
            Bitmap heartSprite = Properties.Resources.heart;
            for (int live = 0; live < playerHealthComponent.Health; live++)
            {
                graphics.DrawImage(heartSprite, 10 + (live * heartSprite.Width + 10), 570);
            }
        }

        /// <summary>
        /// Render the 3 best score in the leaderboard
        /// </summary>
        /// <param name="graphics"> the game graphic instance </param>
        public void RenderLeaderboard(Graphics graphics, StringFormat stringFormat)
        {
            string[] scores = Leaderboard.Instance.GetNSortedScoresFromTxt(3);
            graphics.DrawString("Top scores :", Game.GetCustomFont(26), Game.blackBrush, Game.windowSize.Width / 2, Game.windowSize.Height / 2, stringFormat);
            for (int i = 0; i < scores.Length; i++)
            {
                if (scores[i].Equals(""))
                    continue;
                graphics.DrawString
                    (
                        scores[i],
                        Game.GetCustomFont(20),
                        Game.blackBrush,
                        Game.windowSize.Width / 2,
                        Game.windowSize.Height / 2 + 40 + 30 * i,
                        stringFormat
                    );
            }
        }

        public void RenderCurrentPlayerScore(Graphics graphics)
        {
            graphics.DrawString
                (
                    "score : " + Leaderboard.Instance.CurrentGameScore,
                    Game.GetCustomFont(12),
                    Game.blackBrush,
                    Game.windowSize.Width - 120,
                    572
                );
        }

        /// <summary>
        /// Render the scene when the game end,
        /// that is when the player win or lose 
        /// this will also render the leaderboard
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="message"></param>
        public void RenderSceneEndGame(Graphics graphics, string message)
        {
            StringFormat stringFormat = new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };
            graphics.DrawString(message, Game.GetCustomFont(45), Game.blackBrush, Game.windowSize.Width / 2, Game.windowSize.Height / 3, stringFormat);
            this.RenderLeaderboard(graphics, stringFormat);
        }
    }
}
