using Microsoft.VisualBasic.Compatibility.VB6;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        private int enemyCount = 5;
        private int currentEnemyCount;

        private float gameplayLimitTime = 30;
        private float currentGameplayTime = 0;

        private Character character;
        private Character enemy;
        public List<GameObject> gameObjects = new List<GameObject>();

        public Gameplay() 
        {
           Initialize();
        }

        public override void Draw()
        {
            foreach (var gameObject in new List<GameObject>(gameObjects)) 
            {
                gameObject.Draw();
            }

          //character.Draw();
          //enemy.Draw();
        }

        public override void Input()
        {
           character.Input();
        }

        public override void Reset()
        {
           Initialize();
        }

        public override void Update()
        {
            foreach (var gameObject in new List<GameObject>(gameObjects))
            {
                gameObject.Update();
            }

            //character.Update();
            //enemy.Update();

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

            gameObjects.Clear();

            character = new Character(1, 5, 1, .45f, .45f, "ship.png", 400, 530, false);
            gameObjects.Add(character);

            for (int i = 0; i < enemyCount; i++)
            {
                enemy = new Character(1, 0, 1, .75f, .75f, "ship.png", 10 * i * 5, 10 * i * 5, true);
                enemy.speed = 2;

                gameObjects.Add(enemy);
            }

            character.onEnemyDeath += OnEnemyDeathHandler;

            character.gameplayLevel = this;

            //gameObjects.Clear();
            //gameObjects.Add(character);
            //gameObjects.Add(enemy);
        }

        private void OnEnemyDeathHandler()
        {
            currentEnemyCount--;
            Console.WriteLine($"ENEMIGO ELIMINADO | ENEMIGOS RESTANTES {currentEnemyCount}");

            if (currentEnemyCount <= 0)
            {
                LevelManager.Instance.SetLevel("Victory");
            }
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
            if (Engine.GetKey(Keys.SPACE))
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
            if (Engine.GetKey(Keys.SPACE))
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
