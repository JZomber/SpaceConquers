using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.AccessControl;

namespace Game
{
    public static class BulletFactory
    {
        public static Bullet CreateBullet(Vector2 pos)
        {
            Bullet bullet = new Bullet(.25f, .25f, "enemy.png", (int)pos.x, (int)pos.y);
            return bullet;
        }
    }

    public class Bullet : GameObject
    {
        private bool isALive = true;

        private float lifeCounter = 0;
        private float lifeTime = 5;

        public event Action<Bullet> onBulletDied;

        public Bullet(float p_sizeX, float p_sizeY, string p_textura, int p_posicionX, int p_posicionY) : base(p_sizeX, p_sizeY, p_textura, p_posicionX, p_posicionY)
        {
            isALive = true;

            List<Texture> list = new List<Texture>();

            for (int i = 0; i < 4; i++)
            {
                list.Add(Engine.GetTexture($"{i}.png"));
            }

            idle = new Animation("idle", list, .25f, true);

            currentAnimation = idle;
        }

        public override void Update()
        {
            base.Update();

            if (isALive)
            {
                lifeCounter += Program.deltaTime;

                if (lifeCounter > lifeTime)
                {
                    isALive = false;
                    onBulletDied?.Invoke(this);
                }

                if (PosY < 0 - currentAnimation.CurrentFrame.Height && isALive)
                {
                    isALive = false;
                    onBulletDied?.Invoke(this);
                }
                
                transform.position.y -= 500 * Program.deltaTime;
            }
        }

        public void Reset(Vector2 pos)
        {
            isALive = true;
            transform.position = pos;
            lifeCounter = 0;
            onBulletDied = null;
        }
    }

    public class Character : GameObject
    {
        public LifeChanged onLifeLoose;
        public LifeChanged onLifeGained;

        private ElementPool<Vector2, Bullet> bulletsPool = new ElementPool<Vector2, Bullet>(BulletFactory.CreateBullet);

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
        private bool isEnemy;

        private float shootCoolDown = .25f;
        private float currentShootCD = 0;

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
            if  (Engine.GetKey(Keys.SPACE) && currentShootCD > shootCoolDown)
            {
                var bullet = bulletsPool.GetElement(transform.position);
                bullet.Reset(transform.position);
                bullet.onBulletDied += ReleaseBullet;

                gameplayLevel.gameObjects.Add(bullet);
                currentShootCD = 0;
            }

            if (Engine.GetKey(Keys.A))
            {
                IncrementPosX(-velocidad);
            }

            if (Engine.GetKey(Keys.D))
            {
                IncrementPosX(velocidad);
            }

            if (Engine.GetKey(Keys.P))
            {
                Console.WriteLine("LLAMANDO PANTALLA DERROTA");
                LevelManager.Instance.SetLevel("Defeat");
            }

            if (Engine.GetKey(Keys.O))
            {
                Console.WriteLine("LLAMANDO PANTALLA VICTORIA");
                LevelManager.Instance.SetLevel("Victory");
            }
        }

        public override void Update()
        {
            currentShootCD += Program.deltaTime;

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

        private void ReleaseBullet(Bullet bulletToRelease)
        {
            bulletsPool.ReleaseObject(bulletToRelease);
            gameplayLevel.gameObjects.Remove(bulletToRelease);
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