using System;
using System.Collections.Generic;

namespace Game
{
    public class Character : GameObject
    {
        public LifeChanged onLifeLoose;
        public LifeChanged onLifeGained;
        //private event LifeChanged _onLifeChanged;

        /*public event LifeChanged onLifeChanged
        {
            add { onLifeLoose = value; }
            remove { onLifeGained = value; }
        }*/

        private event Action<int> _onLifeChanged;

        public event Action<int> onLifeChanged
        {
            add { _onLifeChanged += value; }
            remove { _onLifeChanged -= value; }
        }

        private int life;
        private int velocidad;
        private int damage;

        public int speed = 1;


        public void IncrementPosX(int x)
        {
            transform.position.x += x * speed;
        }

        public void SetPosX(int x)
        {
            transform.position.x = x;
        }


        public Character(int p_vida, int p_vel, int p_damage, 
            float p_sizeX, float p_sizeY, string p_textura, 
            int p_posicionX, int p_posicionY) 
            : base(p_sizeX,p_sizeY,p_textura,p_posicionX, p_posicionY)
        {
            damage = p_damage;
            life = p_vida;
            velocidad = p_vel;

            List<Texture> list = new List<Texture>();
            for (int i = 0; i < 4; i++)
            {
                list.Add(Engine.GetTexture($"{i}.png"));
            }
            idle = new Animation("idle", list, .25f, true);

            currentAnimation = idle;
        }

        public override void Input()
        {
            if (Engine.GetKey(Keys.Q))
            {
                LifeLoose();
            }

            if (Engine.GetKey(Keys.E))
            {
                LifeGained();
            }
        }

        public override void Update()
        {
            IncrementPosX(Program.GetRandom(1, 10));

            if (PosX > Program.WIDTH + currentAnimation.CurrentFrame.Width)
            {
                SetPosX(-50);
            }

            base.Update();
        }

        private void LifeGained()
        {

            life += 1;
            //onLifeGained(life);
            //onLifeGained?.Invoke(life);
            _onLifeChanged(life);
        }

        private void LifeLoose()
        {

            life -= 1;
            //onLifeGained(life);
            //onLifeLoose?.Invoke(life);
            _onLifeChanged(life);
        }
    }
}