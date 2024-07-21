using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public static class BulletFactory
    {
        public static Bullet CreateBullet(Vector2 pos, bool isPlayerBullet)
        {
            Bullet bullet = new Bullet(.25f, .25f, "Bullet.png", (int)pos.x, (int)pos.y, isPlayerBullet);
            return bullet;
        }
    }

    public class Bullet : GameObject, IShootable, IMoveable, ICollidable
    {
        public bool isAlive { get; private set; } = true;

        private float maxLifeTime = 5;
        private float currentLifeTime = 0;

        private int bulletSpeed = 1000;
        private bool isPlayerBullet;

        public bool GetIsPlayerBullet => isPlayerBullet;

        public event Action<Bullet> onBulletDied;

        public Bullet(float p_sizeX, float p_sizeY, string p_textura, int p_posicionX, int p_posicionY, bool p_isPlayerBullet) : base(p_sizeX, p_sizeY, p_textura, p_posicionX, p_posicionY)
        {
            isAlive = true;
            isPlayerBullet = p_isPlayerBullet;

            List<Texture> list = new List<Texture>();

            for (int i = 0; i < 4; i++)
            {
                list.Add(Engine.GetTexture($"Bullet.png"));
            }

            idle = new Animation("idle", list, .25f, false);

            SetAnimation(idle);
        }

        public override void Update()
        {
            if (isAlive)
            {
                currentLifeTime += Program.deltaTime;

                if (currentLifeTime > maxLifeTime)
                {
                    isAlive = false;
                    onBulletDied?.Invoke(this);
                }

                if (PosY < 0 - renderer.GetHeight() && isAlive)
                {
                    isAlive = false;
                    onBulletDied?.Invoke(this);
                }

                if (PosY > Program.HEIGHT + renderer.GetHeight() && !isPlayerBullet && isAlive)
                {
                    isAlive = false;
                    onBulletDied?.Invoke(this);
                }

                Move(bulletSpeed);
            }

            base.Update();
        }

        public void Shoot(Vector2 startPosicion)
        {
            isAlive = true;
            cTransform.position = startPosicion;
            currentLifeTime = 0;
            onBulletDied = null;
        }

        public void Move(float speed)
        {
            cTransform.position.y += (isPlayerBullet ? -1 : 0.5f) * speed * Program.deltaTime;
        }



        void IShootable.Die()
        {
            isAlive = false;
            onBulletDied?.Invoke(this);
        }

        public override void OnCollision(GameObject other)
        {
            if (other is Enemy && isPlayerBullet)
            {
                isAlive = false;
                onBulletDied?.Invoke(this);
            }
            else if (other is Player && !isPlayerBullet)
            {
                isAlive = false;
                onBulletDied?.Invoke(this);
            }
        }
    }
}
