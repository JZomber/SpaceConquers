using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public struct Vector2
    {
        public float x { get; set; }
        public float y { get; set; }

        public Vector2(float x, float y)
        {
            this.x = x;
            this.y = y;
        }
    }

    public struct Transform
    {
        public Vector2 position;
        public Vector2 scale;
        public int rotation;

        public Transform(float posx = 0, float posy = 0, float scaleX = 1,
            float scaleY = 1, int rot = 0)
        {
            position = new Vector2(posx, posy);
            scale = new Vector2(scaleX,scaleY);
            rotation = rot;
        }

        public Transform(Vector2 pos, Vector2 p_scale, int rot)
        {
            position = pos;
            scale = p_scale;
            rotation = rot;
        }

    }
    public class Utils
    {

      
    }
}
