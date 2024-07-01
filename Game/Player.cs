using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public class Player : Character
    {
        private ElementPool<Vector2, Bullet> bulletsPool = new ElementPool<Vector2, Bullet>(BulletFactory.CreateBullet);

        private float shootCoolDown = .25f;
        private float currentShootCD = 0;

        private int playerVel;

        public event Action<Bullet> OnBulletFired;
        public event Action<Bullet> OnBulletDestroyed;

        public Player(int p_vida, int p_vel, int p_damage, float p_sizeX, float p_sizeY, string p_textura, int p_posicionX, int p_posicionY) : 
                      base(p_vida, p_vel, p_damage, p_sizeX, p_sizeY, p_textura, p_posicionX, p_posicionY)
        {
            List<Texture> playerList = new List<Texture>();

            for (int i = 0; i < 4; i++)
            {
                playerList.Add(Engine.GetTexture($"{i}.png"));
            }

            idle = new Animation("idle", playerList, .25f, true);
            SetAnimation(idle);

            playerVel = p_vel;
        }

        public override void Input()
        {
            if (Engine.GetKey(Keys.SPACE) && currentShootCD > shootCoolDown)
            {
                var bullet = bulletsPool.GetElement(cTransform.position);
                bullet.Shoot(cTransform.position);
                bullet.onBulletDied += ReleaseBulletHandler;

                //gameplayLevel.listGameObjects.Add(bullet);
                OnBulletFired?.Invoke(bullet);
                currentShootCD = 0;
            }

            if (Engine.GetKey(Keys.A))
            {
                Move(-playerVel);
            }

            if (Engine.GetKey(Keys.D))
            {
                Move(playerVel);
            }

            base.Input();
        }

        public override void Update()
        {
            currentShootCD += Program.deltaTime;

            if (PosX > Program.WIDTH + renderer.GetWidth())
            {
                SetPosX(-30);
            }
            else if (PosX < -35)
            {
                SetPosX(850);
            }

            base.Update();
        }

        private void ReleaseBulletHandler(Bullet bulletToRelease)
        {
            bulletsPool.ReleaseObject(bulletToRelease);
            //gameplayLevel.listGameObjects.Remove(bulletToRelease);
            OnBulletDestroyed?.Invoke(bulletToRelease);
        }
    }
}
