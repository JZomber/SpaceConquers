using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
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
            UnsubscribeAllEvents();

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

            player = new Player(3, 5, .50f, .50f, "ship.png", 100, 560);
            player.OnBulletFired += HandlerBulletFired;
            player.OnBulletDestroyed += HandlerBulletDestroyed;
            player.OnPlayerDeath += HandlerOnPlayerDeath;

            listGameObjects.Add(player);


            bool lap = false;

            for (int i = 1; i < enemyCount + 1; i++)
            {
                if (lap)
                {
                    lap = false;
                    enemy = new Enemy(80 * i, 40 * i, EnemyType.Ranger);
                }
                else
                {
                    lap = true;
                    enemy = new Enemy(80 * i, 40 * i, EnemyType.Normal);
                }

                enemy.onEnemyDeath += HandlerOnEnemyDeath;
                enemy.onBulletFired += HandlerBulletFired;
                enemy.onBulletDestroyed += HandlerBulletDestroyed;
                listGameObjects.Add(enemy);
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


        private void HandlerOnEnemyDeath(Enemy obj)
        {
            currentEnemyCount--;
            Console.WriteLine($"ENEMIGO ELIMINADO | ENEMIGOS RESTANTES {currentEnemyCount}");

            obj.onEnemyDeath -= HandlerOnEnemyDeath;
            obj.onBulletFired -= HandlerBulletFired;
            obj.onBulletDestroyed -= HandlerBulletDestroyed;
            listGameObjects.Remove(obj);

            if (currentEnemyCount <= 0)
            {
                UnsubscribePlayerEvents();

                listGameObjects.Clear();

                LevelManager.Instance.SetLevel("Victory");
            }
        }

        private void HandlerOnPlayerDeath()
        {
            foreach (Enemy enemy in listGameObjects.OfType<Enemy>())
            {
                enemy.onEnemyDeath -= HandlerOnEnemyDeath;
                enemy.onBulletFired -= HandlerBulletFired;
                enemy.onBulletDestroyed -= HandlerBulletDestroyed;
            }

            UnsubscribePlayerEvents();

            listGameObjects.Clear();

            LevelManager.Instance.SetLevel("Defeat");
        }

        private void UnsubscribePlayerEvents()
        {
            player.OnBulletFired -= HandlerBulletFired;
            player.OnBulletDestroyed -= HandlerBulletDestroyed;
            player.OnPlayerDeath -= HandlerOnPlayerDeath;
        }

        private void UnsubscribeAllEvents()
        {
            foreach (var gameObject in listGameObjects)
            {
                if (gameObject is Player player)
                {
                    UnsubscribePlayerEvents();
                }
                else if (gameObject is Enemy enemy)
                {
                    enemy.onEnemyDeath -= HandlerOnEnemyDeath;
                    enemy.onBulletFired -= HandlerBulletFired;
                    enemy.onBulletDestroyed -= HandlerBulletDestroyed;
                }
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
                            if (CheckCollision(bullet.BottomCenterPosition, bullet.RealSize, enemy.BottomCenterPosition, enemy.RealSize) && bullet.GetIsPlayerBullet)
                            {
                                bullet.OnCollision(enemy);
                                enemy.OnCollision(bullet);

                                if (!bullet.isAlive)
                                {
                                    objectsToRemove.Add(bullet);
                                }
                            }
                        }
                        else if (otherGameObject is Player player)
                        {
                            if (CheckCollision(bullet.BottomCenterPosition, bullet.RealSize, player.BottomCenterPosition, player.RealSize) && !bullet.GetIsPlayerBullet)
                            {
                                bullet.OnCollision(player);
                                player.OnCollision(bullet);

                                if (!bullet.isAlive)
                                {
                                    objectsToRemove.Add(bullet);
                                }
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
}
