using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public enum EnemyType
    {
        Normal,
        Ranger,
        Tank
    }

    public class Enemy : Character
    {
        private int enemyVel;
        private int enemyLife;
        private EnemyType currentType;

        private float shootCoolDown = 2f;
        private float currenShootCD = 0;

        private ElementPool<Vector2, Bullet> bulletsPool = new ElementPool<Vector2, Bullet>(pos => BulletFactory.CreateBullet(pos, false));

        public event Action<Enemy> onEnemyDeath;
        public event Action<Bullet> onBulletFired;
        public event Action<Bullet> onBulletDestroyed;

        public Enemy(int p_posicionX, int p_posicionY, EnemyType type = EnemyType.Normal) :
                     base(GetLife(type), GetVel(type), GetSizeX(type), GetSizeY(type), GetTexture(type), p_posicionX, p_posicionY)
        {
            List<Texture> enemyList = new List<Texture>();

            enemyList.Add(Engine.GetTexture(GetTexture(type)));

            idle = new Animation("idle", enemyList, .25f, false);
            SetAnimation(idle);

            enemyVel = GetVel(type);
            enemyLife = GetLife(type);
            currentType = type;
        }

        public override void Update()
        {
            Move(enemyVel);

            if (PosX > Program.WIDTH + renderer.GetWidth())
            {
                enemyVel *= -1;
            }
            else if (PosX < -35)
            {
                enemyVel *= -1;
            }

            if (currentType == EnemyType.Ranger)
            {
                currenShootCD += Program.deltaTime;
                if (currenShootCD > shootCoolDown)
                {
                    Shoot();
                    currenShootCD = 0;
                }
            }

            base.Update();
        }

        private void Shoot()
        {
            var bullet = bulletsPool.GetElement(cTransform.position);
            bullet.Shoot(cTransform.position);
            bullet.onBulletDied += ReleaseBulletHandler;

            onBulletFired?.Invoke(bullet);
        }

        private void ReleaseBulletHandler(Bullet bulletToRelease)
        {
            bulletsPool.ReleaseObject(bulletToRelease);
            onBulletDestroyed?.Invoke(bulletToRelease);
        }

        public override void OnCollision(GameObject other)
        {
            if (other is Bullet)
            {
                if (enemyLife - 1 <= 0)
                {
                    onEnemyDeath?.Invoke(this);
                }
                else
                {
                    enemyLife--;
                }
            }
        }

        private static int GetLife(EnemyType type)
        {
            switch (type)
            {
                case EnemyType.Normal:
                    return 1;
                case EnemyType.Ranger:
                    return 2;
                case EnemyType.Tank:
                    return 3;
                default:
                    return 1;
            }
        }

        private static int GetVel(EnemyType type)
        {
            switch (type)
            {
                case EnemyType.Normal:
                    return 12; // 12
                case EnemyType.Ranger:
                    return 10; // 10
                case EnemyType.Tank:
                    return 8; // 8
                default:
                    return 10;

            }
        }

        private static float GetSizeX(EnemyType type)
        {
            switch (type)
            {
                case EnemyType.Normal:
                case EnemyType.Ranger:
                case EnemyType.Tank:
                default:
                    return 0.50f;
            }
        }

        private static float GetSizeY(EnemyType type)
        {
            switch (type)
            {
                case EnemyType.Normal:
                case EnemyType.Ranger:
                case EnemyType.Tank:
                default:
                    return 0.50f;
            }
        }

        private static string GetTexture(EnemyType type)
        {
            switch (type)
            {
                case EnemyType.Normal:
                    return "enemy.png";
                case EnemyType.Ranger:
                    return "ranger.png";
                case EnemyType.Tank:
                    return "tank.png";
                default:
                    return null;

            }
        }
    }
}
