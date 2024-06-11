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
        }

        private void Initialize()
        {
            character = new Character(1, 5, 1, .45f, .45f, "ship.png", 400, 530, false);

            for (int i = 0; i < 5; i++)
            {
                enemy = new Character(1, 0, 1, .75f, .75f, "ship.png", 10 * i, 10 * i, true);
                enemy.speed = 2;
            }

            character.gameplayLevel = this;

            gameObjects.Clear();
            gameObjects.Add(character);
            gameObjects.Add(enemy);
        }
    }

    public class Menu : Level
    {
        private string backgroundImage = "fondo.png";

        private string messageImage = "mensaje.png";

        public Menu() 
        {
            
        }

        public override void Draw()
        {
            Engine.Draw(backgroundImage);
            Engine.Draw(messageImage);
        }

        public override void Input()
        {
            if (Engine.GetKey(Keys.SPACE))
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
}
