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
        public Gameplay() 
        {
           Initialize();
        }

        public override void Draw()
        {
          character.Draw();
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
            character.Update();
        }

        private void Initialize()
        {
            character = new Character(1, 1, 1, .45f, .45f, "ship.png", 400, 30);
            character.speed = 0;
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
