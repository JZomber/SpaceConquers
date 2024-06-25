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

    public class Gameplay : Level
    {
        private int enemyCount = 8;
        private int currentEnemyCount;

        private float gameplayLimitTime = 30;
        private float currentGameplayTime = 0;

        private Player player;
        private Enemy enemy;
        public List<GameObject> listGameObjects = new List<GameObject>();

        public Gameplay() 
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
            currentGameplayTime = 0;

            currentEnemyCount = enemyCount;

            listGameObjects.Clear();

            player = new Player(1, 5, 1, .45f, .45f, "ship.png", 100, 530);
            listGameObjects.Add(player);


            bool lap = false;

            for (int i = 0; i < enemyCount; i++)
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

                enemy = new Enemy(1, vel, 1, .50f, .50f, "ship.png", 100 * i, 10 * i * 5);
                enemy.onEnemyDeath += HandlerOnEnemyDeath;
                listGameObjects.Add(enemy);

                Console.WriteLine($"ENEMY {i} VEL: {vel}");
            }

            player.gameplayLevel = this;
        }


        private void HandlerOnEnemyDeath()
        {
            currentEnemyCount--;
            Console.WriteLine($"ENEMIGO ELIMINADO | ENEMIGOS RESTANTES {currentEnemyCount}");

            if (currentEnemyCount <= 0)
            {
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

                                Console.Write("COLISIÓN");
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

        public bool CheckCollision(Vector2 posOne, Vector2 realSizeOne, Vector2 posTwo, Vector2 realSizeTwo)
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
                LevelManager.Instance.SetLevel("Jugar");
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
