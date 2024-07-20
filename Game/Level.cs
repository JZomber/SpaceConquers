using Microsoft.VisualBasic.Compatibility.VB6;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;

namespace Game
{
    public abstract class Level
    {
        public abstract void Input();
        public abstract void Update();
        public abstract void Draw();

        public abstract void Reset();
    }

    public class Level_1 : Level
    {
        private int enemyCount = 8;
        private int currentEnemyCount;

        private float gameplayLimitTime = 20;
        private float currentGameplayTime = 0;

        private Player player;
        private Enemy enemy;
        private TimeCounter timeCounter;
        private List<GameObject> listGameObjects = new List<GameObject>();

        public Level_1() 
        {
           Initialize();
        }

        public override void Draw()
        {
            foreach (var gameObject in new List<GameObject>(listGameObjects))
            {
                gameObject.Draw();
            }
        }

        public override void Input()
        {
           player.Input();
        }

        public override void Reset()
        {
           Initialize();
        }

        public override void Update()
        {
            foreach (var gameObject in new List<GameObject>(listGameObjects))
            {
                gameObject.Update();
            }

            CheckBulletEnemyCollision();

            currentGameplayTime += Program.deltaTime;

            if (currentGameplayTime > gameplayLimitTime)
            {
                LevelManager.Instance.SetLevel("Defeat");
            }
        }

        private void Initialize()
        {
            listGameObjects.Clear();

            currentGameplayTime = 0;

            currentEnemyCount = enemyCount;

            timeCounter = new TimeCounter(1.25f, 1.25f, "ship.png", 80, 600);
            listGameObjects.Add(timeCounter);


            player = new Player(1, 5, 1, .50f, .50f, "ship.png", 100, 560);

            player.OnBulletFired += HandlerBulletFired;
            player.OnBulletDestroyed += HandlerBulletDestroyed;

            listGameObjects.Add(player);


            bool lap = false;

            for (int i = 1; i < enemyCount + 1; i++)
            {
                int vel = 10;

                if (lap)
                {
                    vel *= -1;
                    lap = false;
                }
                else
                {
                    lap = true;
                }

                enemy = new Enemy(1, vel, 1, .50f, .50f, "ship.png", 80 * i, 40 * i);
                enemy.onEnemyDeath += HandlerOnEnemyDeath;
                listGameObjects.Add(enemy);


                //Console.WriteLine($"ENEMY {i} VEL: {vel}");
            }

            player.gameplayLevel = this;
        }

        private void HandlerBulletFired(Bullet bullet)
        {
            listGameObjects.Add(bullet);
        }

        private void HandlerBulletDestroyed(Bullet bullet)
        {
            listGameObjects.Remove(bullet);
        }


        private void HandlerOnEnemyDeath()
        {
            currentEnemyCount--;
            Console.WriteLine($"ENEMIGO ELIMINADO | ENEMIGOS RESTANTES {currentEnemyCount}");

            if (currentEnemyCount <= 0)
            {
                foreach (var gameObject in listGameObjects)
                {
                    if (gameObject is Enemy enemy)
                    {
                        enemy.onEnemyDeath -= HandlerOnEnemyDeath;
                    }
                }

                player.OnBulletFired -= HandlerBulletFired;
                player.OnBulletDestroyed -= HandlerBulletDestroyed;

                LevelManager.Instance.SetLevel("Victory");
            }
        }

        private void CheckBulletEnemyCollision()
        {
            List<GameObject> objectsToRemove = new List<GameObject>();
            List<GameObject> copyListObjects = listGameObjects.ToList();

            foreach (var gameObject in copyListObjects)
            {
                if (gameObject is Bullet bullet && bullet.isAlive)
                {
                    foreach (var otherGameObject in copyListObjects)
                    {
                        if (otherGameObject is Enemy enemy)
                        {
                            if (CheckCollision(bullet.BottomCenterPosition, bullet.RealSize, enemy.BottomCenterPosition, enemy.RealSize))
                            {
                                bullet.OnCollision(enemy);
                                enemy.OnCollision(bullet);

                                if (!bullet.isAlive)
                                {
                                    objectsToRemove.Add(bullet);
                                }

                                objectsToRemove.Add(enemy);

                                //Console.Write("COLISIÓN");
                            }
                        }
                    }
                }
            }

            foreach (var obj in objectsToRemove.Distinct().ToList())
            {
                if (obj is Bullet)
                {
                    listGameObjects.Remove(obj);
                }

                if (obj is Enemy)
                {
                    listGameObjects.Remove(obj);
                }
            }
        }

        private bool CheckCollision(Vector2 posOne, Vector2 realSizeOne, Vector2 posTwo, Vector2 realSizeTwo)
        {
            float distanceX = Math.Abs(posOne.x - posTwo.x);
            float distanceY = Math.Abs(posOne.y - posTwo.y);

            float sumHalfWidths = realSizeOne.x / 2 + realSizeTwo.x / 2;
            float sumHalfHeights = realSizeOne.y / 2 + realSizeTwo.y / 2;

            return distanceX <= sumHalfWidths && distanceY <= sumHalfHeights;
        }
    }
    public class Level_2 : Level
    {
        private int enemyCount = 3;
        private int currentEnemyCount;

        private float gameplayLimitTime = 20;
        private float currentGameplayTime = 0;

        private Player player;
        private Enemy enemy;
        private TimeCounter timeCounter;
        private List<GameObject> listGameObjects = new List<GameObject>();

        public Level_2()
        {
            Initialize();
        }

        public override void Draw()
        {
            foreach (var gameObject in new List<GameObject>(listGameObjects))
            {
                gameObject.Draw();
            }
        }

        public override void Input()
        {
            player.Input();
        }

        public override void Reset()
        {
            Initialize();
        }

        public override void Update()
        {
            foreach (var gameObject in new List<GameObject>(listGameObjects))
            {
                gameObject.Update();
            }

            CheckBulletEnemyCollision();

            currentGameplayTime += Program.deltaTime;

            if (currentGameplayTime > gameplayLimitTime)
            {
                LevelManager.Instance.SetLevel("Defeat");
            }
        }

        private void Initialize()
        {
            listGameObjects.Clear();

            currentGameplayTime = 0;

            currentEnemyCount = enemyCount;

            timeCounter = new TimeCounter(1.25f, 1.25f, "ship.png", 80, 600);
            listGameObjects.Add(timeCounter);


            player = new Player(1, 5, 1, .50f, .50f, "ship.png", 100, 560);

            player.OnBulletFired += HandlerBulletFired;
            player.OnBulletDestroyed += HandlerBulletDestroyed;

            listGameObjects.Add(player);


            bool lap = false;

            for (int i = 1; i < enemyCount + 1; i++)
            {
                int vel = 10;

                if (lap)
                {
                    vel *= -1;
                    lap = false;
                }
                else
                {
                    lap = true;
                }

                enemy = new Enemy(1, vel, 1, .50f, .50f, "ship.png", 80 * i, 40 * i);
                enemy.onEnemyDeath += HandlerOnEnemyDeath;
                listGameObjects.Add(enemy);


                //Console.WriteLine($"ENEMY {i} VEL: {vel}");
            }

            player.gameplayLevel = this;
        }

        private void HandlerBulletFired(Bullet bullet)
        {
            listGameObjects.Add(bullet);
        }

        private void HandlerBulletDestroyed(Bullet bullet)
        {
            listGameObjects.Remove(bullet);
        }

        private void HandlerOnEnemyDeath()
        {
            currentEnemyCount--;
            Console.WriteLine($"ENEMIGO ELIMINADO | ENEMIGOS RESTANTES {currentEnemyCount}");

            if (currentEnemyCount <= 0)
            {
                foreach (var gameObject in listGameObjects)
                {
                    if (gameObject is Enemy enemy)
                    {
                        enemy.onEnemyDeath -= HandlerOnEnemyDeath;
                    }
                }

                player.OnBulletFired -= HandlerBulletFired;
                player.OnBulletDestroyed -= HandlerBulletDestroyed;

                LevelManager.Instance.SetLevel("Victory");
            }
        }

        private void CheckBulletEnemyCollision()
        {
            List<GameObject> objectsToRemove = new List<GameObject>();
            List<GameObject> copyListObjects = listGameObjects.ToList();

            foreach (var gameObject in copyListObjects)
            {
                if (gameObject is Bullet bullet && bullet.isAlive)
                {
                    foreach (var otherGameObject in copyListObjects)
                    {
                        if (otherGameObject is Enemy enemy)
                        {
                            if (CheckCollision(bullet.BottomCenterPosition, bullet.RealSize, enemy.BottomCenterPosition, enemy.RealSize))
                            {
                                bullet.OnCollision(enemy);
                                enemy.OnCollision(bullet);

                                if (!bullet.isAlive)
                                {
                                    objectsToRemove.Add(bullet);
                                }

                                objectsToRemove.Add(enemy);

                                //Console.Write("COLISIÓN");
                            }
                        }
                    }
                }
            }

            foreach (var obj in objectsToRemove.Distinct().ToList())
            {
                if (obj is Bullet)
                {
                    listGameObjects.Remove(obj);
                }

                if (obj is Enemy)
                {
                    listGameObjects.Remove(obj);
                }
            }
        }

        private bool CheckCollision(Vector2 posOne, Vector2 realSizeOne, Vector2 posTwo, Vector2 realSizeTwo)
        {
            float distanceX = Math.Abs(posOne.x - posTwo.x);
            float distanceY = Math.Abs(posOne.y - posTwo.y);

            float sumHalfWidths = realSizeOne.x / 2 + realSizeTwo.x / 2;
            float sumHalfHeights = realSizeOne.y / 2 + realSizeTwo.y / 2;

            return distanceX <= sumHalfWidths && distanceY <= sumHalfHeights;
        }
    }

    public class Menu : Level
    {
        private string backgroundImage = "menu.jpg";

        public Menu() 
        {
            
        }

        public override void Draw()
        {
            Engine.Draw(backgroundImage);
        }

        public override void Input()
        {
            if (Engine.GetKey(Keys.RETURN))
            {
                LevelManager.Instance.SetLevel("Level_1");
            }
        }


        public override void Update()
        {
           
        }

        public override void Reset()
        {

        }
    }

    public class Victory : Level
    {
        private string backgroundImage = "victory.jpg";

        public Victory()
        {
            
        }
    

        public override void Draw()
        {
            Engine.Draw(backgroundImage);
        }

        public override void Input()
        {
            if (Engine.GetKey(Keys.K))
            {
                if (LevelManager.Instance.LastLevelName == "Level_1")
                {
                    LevelManager.Instance.SetLevel("Level_2");
                }
                else 
                {
                    LevelManager.Instance.SetLevel("Menu");
                }
            }
        }


        public override void Update()
        {

        }

        public override void Reset()
        {

        }
    }

    public class Defeat : Level
    {
        private string backgroundImage = "defeat.jpg";

        public Defeat()
        {
            
        }

        public override void Draw()
        {
            Engine.Draw(backgroundImage);
        }

        public override void Input()
        {
            if (Engine.GetKey(Keys.K))
            {
                LevelManager.Instance.SetLevel("Menu");
            }
        }


        public override void Update()
        {

        }

        public override void Reset()
        {

        }
    }
}
