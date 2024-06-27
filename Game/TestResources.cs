using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public class TestLevel : Level
    {
        private List<Proyectile> proyectiles = new List<Proyectile>(); 

        public int proyectilesCount => proyectiles.Count;


        private List<TestSubject> subjects = new List<TestSubject>();

        public List<TestSubject> copySubjectList => subjects;

        TestSubject subject;

        public override void Draw()
        {
        }

        public override void Input()
        {
        }

        public override void Reset()
        {
        }

        public override void Update()
        {
        }

        public bool CheckCollision(Vector2 posOne, Vector2 realSizeOne,
        Vector2 posTwo, Vector2 RealSizeTwo)
        {

            float distanceX = Math.Abs(posOne.x - posTwo.x);
            float distanceY = Math.Abs(posOne.y - posTwo.y);

            float sumHalfWidths = realSizeOne.x / 2 + RealSizeTwo.x / 2;
            float sumHalfHeights = realSizeOne.y / 2 + RealSizeTwo.y / 2;

            return distanceX <= sumHalfWidths && distanceY <= sumHalfHeights;
        }

        public void CreateNewSubject()
        {
            subject = new TestSubject();
            subjects.Add(subject);
        }
    }

    public class Proyectile
    {
        Vector2 position;

        Vector2 realSize;

        public Vector2 p_Position => position;

        public Vector2 p_RealSize => realSize;

        public Proyectile(Vector2 p_position, Vector2 p_realSize)
        {
            position = p_position;
            realSize = p_realSize;
        }
    }

    public class Target
    {
        Vector2 position;

        Vector2 realSize;

        public Vector2 p_Position => position;

        public Vector2 p_RealSize => realSize;

        public Target(Vector2 p_position, Vector2 p_realSize)
        {
            position = p_position;
            realSize = p_realSize;
        }
    }

    public class TestSubject
    {

    }
}
