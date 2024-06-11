using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public class Enemy : Character
    {
        private int EnemyVel;

        public Enemy(int p_vida, int p_vel, int p_damage, float p_sizeX, float p_sizeY, string p_textura, int p_posicionX, int p_posicionY) : 
                     base(p_vida, p_vel, p_damage, p_sizeX, p_sizeY, p_textura, p_posicionX, p_posicionY)
        {
            List<Texture> enemyList = new List<Texture>();

            enemyList.Add(Engine.GetTexture("enemy.png"));

            idle = new Animation("idle", enemyList, .25f, false);
            currentAnimation = idle;

            EnemyVel = p_vel;
        }

        public override void Update()
        {
            IncrementPosX(EnemyVel);

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
    }
}
