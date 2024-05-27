using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Security.Policy;
using static System.Net.Mime.MediaTypeNames;

namespace Game
{
    public delegate void LifeChanged(int p_newLife);

    public class Program
    {

        public static float deltaTime = 0;
        static DateTime lastFrameTime = DateTime.Now;

        private static float timeSpent = 0;


        public static int WIDTH = 800;
        public static int HEIGHT = 600;


      

        static Random random = new Random();

        public static int GetRandom(int min, int max) => random.Next(min, max);

        static void Main(string[] args)
        {
            Engine.Initialize("pepito screen" , WIDTH, HEIGHT);

            while (true)
            {
                
                Input();
                Update();
                Render();

                CalcDeltaTime();
            }
        }

      

       

        private static void CalcDeltaTime()
        {
            TimeSpan deltaSpan = DateTime.Now - lastFrameTime;
            deltaTime = (float)deltaSpan.TotalSeconds;
            lastFrameTime = DateTime.Now;
        }

        static void Input()
        {
            LevelManager.Instance.CurrentLevel.Input();
        }

        static void Update()
        {
            LevelManager.Instance.CurrentLevel.Update();
        }

        static void Render()
        {
            Engine.Clear();

            LevelManager.Instance.CurrentLevel.Draw();

            Engine.Show();
        }

       
    }
}