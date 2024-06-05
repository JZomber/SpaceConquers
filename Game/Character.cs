using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.AccessControl;

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
        private bool isEnemy;

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
            int p_posicionX, int p_posicionY, bool Enemy) 
            : base(p_sizeX,p_sizeY,p_textura,p_posicionX, p_posicionY)
        {
            damage = p_damage;
            life = p_vida;
            velocidad = p_vel;
            isEnemy = Enemy;

            List<Texture> list = new List<Texture>();

            if (!Enemy)
            {
                for (int i = 0; i < 4; i++)
                {
                    list.Add(Engine.GetTexture($"{i}.png"));
                }
            }
            else
            {
                list.Add(Engine.GetTexture("enemy.png"));
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

            if (Engine.GetKey(Keys.A))
            {
                IncrementPosX(-velocidad);
            }

            if (Engine.GetKey(Keys.D))
            {
                IncrementPosX(velocidad);
            }
        }

        public override void Update()
        {
            if (isEnemy)
            {
                IncrementPosX(speed);
            }

            if (PosX > Program.WIDTH + currentAnimation.CurrentFrame.Width)
            {
                SetPosX(-30);
            }
            else if (PosX < -35)
            {
                SetPosX(850);
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