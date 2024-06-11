using System;
using System.Collections.Generic;

namespace Game
{
    public class GameUpdateManager
    {
        private static GameUpdateManager instance = new GameUpdateManager();
        public static GameUpdateManager Instance { get { return instance; } }

        private List<GameObject> gameobjectList = new List<GameObject>();

        public GameUpdateManager() { }

        public void AddUpdatableObj(GameObject p_character)
        {
            gameobjectList.Add(p_character);
        }

        public void Input()
        { 
            foreach (var l_go in gameobjectList)
            {
                l_go.Input();
            }
        }

        public void Update() 
        {
            foreach (var l_go in gameobjectList)
            {
                l_go.Update();
            }

            CheckCollisions();
        }

        public void Render()
        {
            foreach (var l_go in gameobjectList)
            {
                l_go.Draw();
            }
        }

        private void CheckCollisions()
        {
            for (int i = 0; i < gameobjectList.Count; i++)
            {
                var objOne = gameobjectList[i];
                for (int j = 0; j < gameobjectList.Count; j++)
                {
                    if (j == i)
                        continue;


                    var objTwo = gameobjectList[j];
                    if (IsBoxColliding(objOne.Transform.position, objOne.RealSize, objTwo.Transform.position, objTwo.RealSize))
                    {
                        Console.WriteLine("Alguien esta chocando");
                        objOne.OnCollision(objTwo);
                        objTwo.OnCollision(objOne);
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
}