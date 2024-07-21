using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public class Player : Character
    {
        private ElementPool<Vector2, Bullet> bulletsPool = new ElementPool<Vector2, Bullet>(pos => BulletFactory.CreateBullet(pos, true));

        private float shootCoolDown = .25f;
        private float currentShootCD = 0;
        private int playerVel;
        private bool spaceReleased = true;
        public int playerLife { get ; private set; }

        public event Action<Bullet> OnBulletFired;
        public event Action<Bullet> OnBulletDestroyed;

        public event Action<PowerUpType> OnPowerUpCollected;
        public event Action OnPlayerLifeGained;
        public event Action OnPlayerLifeLoosed;
        public event Action OnPlayerDeath;

        public Player(int p_vida, int p_vel, float p_sizeX, float p_sizeY, string p_textura, int p_posicionX, int p_posicionY) : 
                      base(p_vida, p_vel, p_sizeX, p_sizeY, p_textura, p_posicionX, p_posicionY)
        {
            List<Texture> playerList = new List<Texture>();

            playerList.Add(Engine.GetTexture($"playerShip.png"));

            idle = new Animation("idle", playerList, .25f, true);
            SetAnimation(idle);

            playerVel = p_vel;
            playerLife = GetMaxLife;
        }

        public override void Input()
        {
            if (Engine.GetKey(Keys.SPACE) && spaceReleased && currentShootCD > shootCoolDown)
            {
                var bullet = bulletsPool.GetElement(cTransform.position);
                bullet.Shoot(cTransform.position);
                bullet.onBulletDied += ReleaseBulletHandler;

                OnBulletFired?.Invoke(bullet);
                currentShootCD = 0;

                spaceReleased = false;
            }

            if (!Engine.GetKey(Keys.SPACE))
            {
                spaceReleased = true;
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
            OnBulletDestroyed?.Invoke(bulletToRelease);
        }

        public override void OnCollision(GameObject other)
        {
            if (other is Bullet)
            {
                if (playerLife - 1 <= 0)
                {
                    OnPlayerDeath?.Invoke();
                }
                else
                {
                    playerLife--;
                    OnPlayerLifeLoosed?.Invoke();
                }
            }
            else if (other is PowerUp power)
            {
                if (power.currentType == PowerUpType.Health)
                {
                    playerLife++;
                    OnPlayerLifeGained?.Invoke();
                }
                else if (power.currentType == PowerUpType.Time)
                {
                    OnPowerUpCollected?.Invoke(power.currentType);
                }
            }
        }
    }
}
