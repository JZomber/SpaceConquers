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

        public Gameplay gameplayLevel;

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


        public Character(int p_vida, int p_vel, int p_damage, float p_sizeX, float p_sizeY, string p_textura, int p_posicionX, int p_posicionY) :
                         base(p_sizeX,p_sizeY,p_textura,p_posicionX, p_posicionY)
        {
            damage = p_damage;
            life = p_vida;
            velocidad = p_vel;
        }

        public override void Input()
        {
            
        }

        public override void Update()
        {
            base.Update();
        }

        private void OnEnemyKilledHandler()
        {
            Console.WriteLine("ENEMY ELIMINADO");
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