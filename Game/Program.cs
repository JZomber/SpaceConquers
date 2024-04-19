using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;

//======================
// RAMA DEVELOP
//======================

namespace Game
{
   
    public class GameUpdateManager
    {
        private static GameUpdateManager instance = new GameUpdateManager();
        public static GameUpdateManager Instance { get { return instance; } }

        private List<Character> characterList = new List<Character>();

        public GameUpdateManager() { }

        public void AddUpdatableObj(Character p_character)
        {
            characterList.Add(p_character);
        }

        public void Input()
        { 
            foreach (var l_character in characterList)
            {
                l_character.Input();
            }
        }

        public void Update() 
        {
            foreach (var l_character in characterList)
            {
                l_character.Update();
            }

            CheckCollisions();
        }

        public void Render()
        {
            foreach (var l_character in characterList)
            {
                l_character.Draw();
            }
        }

        private void CheckCollisions()
        {
            for (int i = 0; i < characterList.Count; i++)
            {
                var l_naveUno = characterList[i];
                for (int j = 0; j < characterList.Count; j++)
                {
                    if (j == i)
                        continue;


                    var l_naveDos = characterList[j];
                    if (IsBoxColliding(l_naveUno.position, l_naveUno.RealSize,
                        l_naveDos.position, l_naveDos.RealSize))
                    {
                        Console.WriteLine("Alguien esta chocando");
                    }
                }
            }
        }

        private bool IsBoxColliding(int FirstPosX, int FirstPosY,
         int FirstRealWidth, int FirstRealHeight, int ScndPosX, int ScndPosY,
         int ScndRealWidth, int ScndRealHeight)
        {

            float distanceX = Math.Abs(FirstPosX - ScndPosX);
            float distanceY = Math.Abs(FirstPosY - ScndPosY);

            float sumHalfWidths = FirstRealWidth / 2 + ScndRealWidth / 2;
            float sumHalfHeights = FirstRealHeight / 2 + ScndRealHeight / 2;

            return distanceX <= sumHalfWidths && distanceY <= sumHalfHeights;
        }

        private bool IsBoxColliding(Vector2 posOne, Vector2 realSizeOne,
        Vector2 posTwo, Vector2 RealSizeTwo)
        {

            float distanceX = Math.Abs(posOne.x - posTwo.x);
            float distanceY = Math.Abs(posOne.y - posTwo.y);

            float sumHalfWidths = realSizeOne.x / 2 + RealSizeTwo.x / 2;
            float sumHalfHeights = realSizeOne.y / 2 + RealSizeTwo.y / 2;

            return distanceX <= sumHalfWidths && distanceY <= sumHalfHeights;
        }

    }

    public class Character
    {
        private int vida;
        private int velocidad;
        private int damage;

        private float sizeX;
        private float sizeY;

        private string textura;
        private int posicionX;
        private int posicionY;

        public int speed = 1;

        private Animation idle;
        private Animation currentAnimation;

        public int PosX => posicionX;
        public int PosY => posicionY;

        private int RealWidth => (int)(currentAnimation.CurrentFrame.Width * sizeX);
        private int RealHeight => (int)(currentAnimation.CurrentFrame.Height * sizeY);

        public Vector2 position => new Vector2(posicionX, posicionY);
        public Vector2 size => new Vector2(sizeX, sizeY);
        public Vector2 RealSize => new Vector2(RealWidth,RealHeight);

        public void IncrementPosX(int x)
        {
            posicionX += x * speed;
        }

        public void SetPosX(int x)
        {
            posicionX = x;
        }


        public Character(int p_vida, int p_vel, int p_damage, 
            float p_sizeX, float p_sizeY, string p_textura, 
            int p_posicionX, int p_posicionY)
        {
            vida = p_vida;
            velocidad = p_vel;
            damage = p_damage;
            sizeX = p_sizeX;
            sizeY = p_sizeY;
            textura = p_textura;
            posicionX = p_posicionX;
            posicionY = p_posicionY;


            List<Texture> list = new List<Texture>();
            for (int i = 0; i < 4; i++)
            {
                list.Add(Engine.GetTexture($"{i}.png"));
            }
            idle = new Animation("idle", list, .25f, true);

            currentAnimation = idle;

            GameUpdateManager.Instance.AddUpdatableObj(this);
        }

        public void Input()
        {

        }


        public void Update()
        {
            IncrementPosX(Program.GetRandom(1, 10));

            if (PosX > Program.WIDTH + currentAnimation.CurrentFrame.Width)
            {
                SetPosX(-50);
            }
            currentAnimation.Update();
        }

        public void Draw()
        {
            Engine.Draw(currentAnimation.CurrentFrame, posicionX, posicionY, sizeX,sizeY,90);
        }
    }

    public class Program
    {

        public static float deltaTime = 0;
        static DateTime lastFrameTime = DateTime.Now;

        private static float timeSpent = 0;


        public static int WIDTH = 800;
        public static int HEIGHT = 600;
        static Character nave;
        static Character navepp;


        static Random random = new Random();

        public static int GetRandom(int min, int max) => random.Next(min, max);

        static void Main(string[] args)
        {
            Engine.Initialize("pepito screen" , WIDTH, HEIGHT);

            Character l_static = new Character(1, 1, 1, .45f, .45f, "ship.png", 400, 30);
            l_static.speed = 0;
            for (int i = 0; i < 10; i++)
            {
               new Character(1, 1, 1,.45f, .45f, "ship.png", 0, i + i * 45);
            }

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
            GameUpdateManager.Instance.Input();
        }

        static void Update()
        {
            GameUpdateManager.Instance.Update();
        }

        static void Render()
        {
            Engine.Clear();

            GameUpdateManager.Instance.Render();

            Engine.Show();
        }

       
    }

/*
   Para agregar imágenes.

   En el explorador de soluciones, click-izq > agregar > Elemento existente > Imagen (PNG) descargada

   Luego en propiedades (la llave), "Copiar en el directorio de salida" > "Copiar Siempre" || (Así al momento de Debuggear, están todos los elementos)
*/
}