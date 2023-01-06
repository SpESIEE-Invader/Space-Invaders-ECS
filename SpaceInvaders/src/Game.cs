using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using SpaceInvaders.src;
using System.Media;
using System.Drawing.Text;

namespace SpaceInvaders
{
    /// <summary>
    /// This class represents the entire game, it implements the singleton pattern
    /// </summary>
    class Game
    {
        public static Size windowSize;
        public static double deltaT;
        #region static
        public static SoundPlayer playerShootingSoundPlayer = new SoundPlayer(Properties.Resources.shoot);
        public static SoundPlayer enemyDyingSoundPlayer = new SoundPlayer(Properties.Resources.invaderkilled);
        public static SoundPlayer playerDyingSoundPlayer = new SoundPlayer(Properties.Resources.explosion);


        public static Game game
        {
            get;
            private set;
        }

        public static Game CreateGame(Size windowSize)
        {
            if (game == null)
                game = new Game(windowSize);
            return game;
        }

        public static HashSet<Keys> keyPressed = new HashSet<Keys>();
        public static SolidBrush blackBrush = new SolidBrush(Color.Black);

        public static GameState State
        {
            get => game.state;
            set => game.state = value;
        }

        public static Font GetCustomFont(int size) 
        {
            return new Font(game.privateFontCollection.Families[0], size, FontStyle.Regular, GraphicsUnit.Pixel);
        }

        /// <summary>
        /// a shared black brush
        /// </summary>
        public static SolidBrush brush = new SolidBrush(Color.Black);

        #endregion

        private GameState state;
        private bool scoreAlreadyWritten;
        private bool[] keysPrevious;
        private Coordinator coordinator;
        private PrivateFontCollection privateFontCollection;

        private Game(Size windowSize)
        {
            Game.windowSize = windowSize;
            this.coordinator = new Coordinator();
            this.coordinator.Init();
            this.state = GameState.Running;
            this.scoreAlreadyWritten = false;
            this.keysPrevious =  new bool[2];
            this.privateFontCollection = new PrivateFontCollection();
            this.LoadCustomFont();
        }

        private void LoadCustomFont()
        {
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string wantedPath = System.IO.Path.Combine(baseDirectory, @"..\..\Resources\space_invaders.ttf");
            string fullPath = System.IO.Path.GetFullPath(wantedPath);
            this.privateFontCollection.AddFontFile(fullPath);
        }

        /// <summary>
        /// update the game accordingly to the game state
        /// </summary>
        public void Update(double deltaT)
        {
            Game.deltaT = deltaT;
            this.UpdateKeys();
            switch (this.state)
            {
                case GameState.Running:
                    this.scoreAlreadyWritten = false;
                    this.coordinator.Update();
                    if (this.coordinator.EntityManager.GetEntityById(0) == null)
                        this.state = GameState.Lost;
                    if (this.AllEnemiesAreDead())
                        this.state = GameState.Win;
                    break;
                case GameState.Pause:
                    break;
                case GameState.Win:
                case GameState.Lost:
                    if (!this.scoreAlreadyWritten)
                    {
                        this.UpdateLeaderboard();
                        this.scoreAlreadyWritten = true;
                    }
                    if (!keyPressed.Contains(Keys.Space) && this.keysPrevious[1])
                        RelauchGame();
                    break;
            }
            this.keysPrevious[1] = keyPressed.Contains(Keys.Space);
        }

        /// <summary>
        /// check for action caused by the player keystrokes 
        /// </summary>
        private void UpdateKeys()
        {
            this.CheckForStateChange();
        }

        /// <summary>
        /// check if the player have pressed the pause key or not 
        /// </summary>
        private void CheckForStateChange()
        {
            if (keyPressed.Contains(Keys.P) && !this.keysPrevious[0])
            {
                switch (this.state)
                {
                    case GameState.Running:
                        this.state = GameState.Pause;
                        break;
                    case GameState.Pause:
                        this.state = GameState.Running;
                        break;
                }
            }
            this.keysPrevious[0] = keyPressed.Contains(Keys.P);
        }

        /// <summary>
        /// reset the game 
        /// </summary>
        private void RelauchGame()
        {
            this.coordinator.ResetGame();
            this.state = GameState.Running;
        }

        /// <summary>
        /// when the game is over, update the leaderboard with the player score 
        /// </summary>
        private void UpdateLeaderboard()
        {
            Leaderboard.Instance.TryAddScoreLeaderboard();
            Leaderboard.Instance.CurrentGameScore = 0;
        }

        /// <summary>
        /// check if all enemies are dead.
        /// all enemies are dead if their only 4 entity left (the player and the three bunker)
        /// </summary>
        /// <returns> true if all the enmies are dead else false </returns>
        private bool AllEnemiesAreDead()
        {
            return (this.coordinator.EntityManager.Entities.Count == 4 && this.coordinator.EntityManager.GetEntityById(0) != null);
        }

        /// <summary>
        /// render the scene to the screen. 
        /// </summary>
        /// <param name="graphics">the form graphics reference</param>
        public void RenderScene(Graphics graphics)
        {
            switch (this.state)
            {
                case GameState.Running:
                case GameState.Pause:
                    this.coordinator.RenderSceneGameRunning(graphics);
                    break;
                case GameState.Win:
                    this.coordinator.RenderSceneEndGame(graphics, "You have won");
                    break;
                case GameState.Lost:
                    this.coordinator.RenderSceneEndGame(graphics, "You have lost");
                    break;
            }
        }
    }
}
