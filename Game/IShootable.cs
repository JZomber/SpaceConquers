using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    internal interface IShootable
    {
        bool isAlive { get; }

        void Shoot(Vector2 startPosicion);
        void Die();
    }
}
