using System;
using System.Collections.Generic;
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
        private int EnemyVel;

        public event Action onEnemyDeath;

        public Enemy(int p_posicionX, int p_posicionY, EnemyType type = EnemyType.Normal) : 
                     base(GetLife(type), GetVel(type), GetSizeX(type), GetSizeY(type), GetTexture(type), p_posicionX, p_posicionY)
        {
            List<Texture> enemyList = new List<Texture>();

            enemyList.Add(Engine.GetTexture(GetTexture(type)));

            idle = new Animation("idle", enemyList, .25f, false);
            SetAnimation(idle);

            EnemyVel = GetVel(type);
        }

        public override void Update()
        {
            Move(EnemyVel);

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

        public override void OnCollision(GameObject other)
        {
            if (other is Bullet)
            {
                onEnemyDeath?.Invoke();
            }
        }

        private static int GetLife(EnemyType type)
        {
            switch (type)
            {
                case EnemyType.Normal:
                case EnemyType.Ranger:
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
                    return 12;
                case EnemyType.Ranger:
                    return 8;
                case EnemyType.Tank:
                    return 6;
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
