using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        private int bulletSpeed = 1000;

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

                transform.position.y -= bulletSpeed * Program.deltaTime;
            }

            base.Update();
        }

        public void Reset(Vector2 pos)
        {
            isALive = true;
            transform.position = pos;
            lifeCounter = 0;
            onBulletDied = null;
        }
    }
}
