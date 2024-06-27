using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    internal interface ICollidable
    {
        void OnCollision(GameObject other);
    }

    internal interface IMoveable
    {
        void Move(float speed);
    }

    internal interface IShootable
    {
        bool isAlive { get; }

        void Shoot(Vector2 startPosicion);
        void Die();
    }
}
