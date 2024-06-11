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

        public event Action OnEnemyDeath;

        private int playerVel;

        public Player(int p_vida, int p_vel, int p_damage, float p_sizeX, float p_sizeY, string p_textura, int p_posicionX, int p_posicionY) : 
                      base(p_vida, p_vel, p_damage, p_sizeX, p_sizeY, p_textura, p_posicionX, p_posicionY)
        {
            List<Texture> playerList = new List<Texture>();

            for (int i = 0; i < 4; i++)
            {
                playerList.Add(Engine.GetTexture($"{i}.png"));
            }

            idle = new Animation("idle", playerList, .25f, true);
            currentAnimation = idle;

            playerVel = p_vel;
        }

        public override void Input()
        {
            if (Engine.GetKey(Keys.SPACE) && currentShootCD > shootCoolDown)
            {
                var bullet = bulletsPool.GetElement(transform.position);
                bullet.Reset(transform.position);
                bullet.onBulletDied += ReleaseBulletHandler;

                gameplayLevel.gameObjects.Add(bullet);
                currentShootCD = 0;
            }

            if (Engine.GetKey(Keys.A))
            {
                IncrementPosX(-playerVel);
            }

            if (Engine.GetKey(Keys.D))
            {
                IncrementPosX(playerVel);
            }

            if (Engine.GetKey(Keys.K)) //Debug purpose
            {
                OnEnemyDeath?.Invoke();
            }

            base.Input();
        }

        public override void Update()
        {
            currentShootCD += Program.deltaTime;

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

        private void ReleaseBulletHandler(Bullet bulletToRelease)
        {
            bulletsPool.ReleaseObject(bulletToRelease);
            gameplayLevel.gameObjects.Remove(bulletToRelease);
        }
    }
}
