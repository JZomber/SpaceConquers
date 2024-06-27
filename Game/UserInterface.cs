using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public class TimeCounter : GameObject
    {
        public TimeCounter(float p_sizeX, float p_sizeY, string p_textura, int p_posicionX, int p_posicionY) : base(p_sizeX, p_sizeY, p_textura, p_posicionX, p_posicionY)
        {
            List<Texture> list = new List<Texture>();

            for (int i = 0; i < 20; i++)
            {
                list.Add(Engine.GetTexture($"TimeCounterSprites\\TimeCounter{20 - i}.jpg"));
            }

            idle = new Animation("idle", list, 1f, false);

            SetAnimation(idle);
        }

        public override void Draw()
        {
            base.Draw();
        }

        public override void Update()
        {
            base.Update();
        }
    }
}
