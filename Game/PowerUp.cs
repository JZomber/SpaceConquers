using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public enum PowerUpType
    {
        Health,
        Time
    }

    internal class PowerUp : GameObject, IMoveable
    {
        private float moveSpeed = 150;
        public bool isAlive { get; private set; } = true;
        public PowerUpType currentType { get; private set; }

        public event Action<PowerUpType> onPowerUpPickedUp;
        public event Action<PowerUp> onPowerUpDestroyed;

        public PowerUp(int p_posicionX, int p_posicionY, PowerUpType type = PowerUpType.Health) :
                       base(GetSizeX(type), GetSizeY(type), GetTexture(type), p_posicionX, p_posicionY)
        {
            List<Texture> list = new List<Texture>();

            list.Add(Engine.GetTexture(GetTexture(type)));

            idle = new Animation("idle", list, .25f, false);
            SetAnimation(idle);

            currentType = type;
        }

        public override void Update()
        {
            Move(moveSpeed);

            base.Update();
        }

        public void Move(float speed)
        {
            cTransform.position.y += speed * Program.deltaTime;
        }

        public override void OnCollision(GameObject other)
        {
            if (other is Player)
            {
                isAlive = false;
                onPowerUpPickedUp?.Invoke(currentType);
            }
        }

        private static float GetSizeX(PowerUpType type)
        {
            switch (type)
            {
                case PowerUpType.Health:
                case PowerUpType.Time:
                default:
                    return 0.50f;
            }
        }

        private static float GetSizeY(PowerUpType type)
        {
            switch (type)
            {
                case PowerUpType.Health:
                case PowerUpType.Time:
                default:
                    return 0.50f;
            }
        }

        private static string GetTexture(PowerUpType type)
        {
            switch (type)
            {
                case PowerUpType.Health:
                    return "healthPowerup.png";
                case PowerUpType.Time:
                    return "timePowerup.png";
                default:
                    return null;

            }
        }
    }
}
