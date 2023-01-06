using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using System.Diagnostics;
using System.Windows.Forms.VisualStyles;

namespace SpaceInvaders
{
    /// <summary>
    /// this class handle the userboard,
    /// which is a .txt file (to save data even if we relaunch the game (entire window not just the game instance))
    /// </summary>
    class Leaderboard
    {
        private int currentGameScore;
        private string path;
        private static Leaderboard instance = null;

        public static Leaderboard Instance
        {
            get 
            {
                if (instance == null)
                    instance = new Leaderboard();
                return instance;
            }
        }

        private Leaderboard()
        {
            this.currentGameScore = 0;
            this.path = InitializePath();
            this.CreateFileIfMissing();
        }

        /// <summary>
        /// if the leaderboard file doesn't already exist , create it 
        /// </summary>
        private void CreateFileIfMissing()
        {
            if (!System.IO.File.Exists(this.path))
                System.IO.File.Create(this.path);
        }

        /// <summary>
        /// get the user base directory for the binary, search for the leaderboard.txt in the resources folder 
        /// </summary>
        /// <returns></returns>
        private string InitializePath()
        {
            string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string wantedPath = System.IO.Path.Combine(currentDirectory, @"..\..\Resources\leaderboard.txt");
            string fullWantedPath = System.IO.Path.GetFullPath(wantedPath);
            return fullWantedPath; 
        }
        public int CurrentGameScore
        {
            get => this.currentGameScore;
            set => this.currentGameScore = value;
        }

        public void UpdateScore(int scoreToAdd)
        {
            this.currentGameScore += scoreToAdd;
        }
        
        /// <summary>
        /// try to add the <c>this.currentGameScore</c> inside the txt file 
        /// if the score is to low, it will be not added 
        /// </summary>
        public void TryAddScoreLeaderboard()
        {
            string[] lines = this.GetNewStringArrayScoreAdded();
            System.IO.File.WriteAllLines(this.path, lines);
        }

        /// <summary>
        /// get a sorted array of string of the data inside the txt file 
        /// the sorted order is reversed to render conviniently on the screen 
        /// </summary>
        /// <param name="toKeep"> number of scores to keep in the list </param>
        /// <returns> a array of string representing the reverse sorted data inside the leaderboard </returns>
        public string[] GetNSortedScoresFromTxt(int toKeep)
        {
            List<int> scoresList= this.GetScoresIntFromTxt().ToList();
            scoresList.Sort();
            scoresList.Reverse();
            return scoresList.ConvertAll<string>(x => x.ToString()).Take(toKeep).ToArray();
        }
        
        /// <summary>
        /// get a array of int of the data inside the leaderboard 
        /// </summary>
        /// <returns> a array of int representing the data inside the leaderboard </returns>
        private int[] GetScoresIntFromTxt()
        {
            return System.IO.File.ReadAllLines(this.path).ToList().Select(int.Parse).ToList().ToArray();
        }

        /// <summary>
        /// this will get the data inside the leaderboard,
        /// transforming it in a int array, and add the current game Score to it 
        /// </summary>
        /// <returns>a array of int representing the current leaderboard + the current game Score added </returns>
        private int[] GetNewIntArrayScoreAdded()
        {
            int[] scoresBuffer = this.GetScoresIntFromTxt();
            int lenBuffer = scoresBuffer.Length;
            int[] newScoresBuffer = new int[lenBuffer + 1];
            Array.Copy(scoresBuffer, newScoresBuffer, lenBuffer);
            newScoresBuffer[newScoresBuffer.Length - 1] = this.currentGameScore;
            return newScoresBuffer;
        }

        /// <summary>
        /// this will get the data inside the leaderboard,
        /// transforming it in a string array, and add the current game Score to it 
        /// </summary>
        /// <returns>a array of string representing the current leaderboard + the current game Score added </returns>
        private string[] GetNewStringArrayScoreAdded()
        {
            return this.GetNewIntArrayScoreAdded().ToList().ConvertAll<string>(x => x.ToString()).ToArray();
        }
    }
}
