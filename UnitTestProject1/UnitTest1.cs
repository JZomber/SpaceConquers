using Game;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestCollision()
        {
            TestLevel testLevel = new TestLevel();
            Proyectile proyectile = new Proyectile(new Vector2(0, 0), new Vector2(10, 10));
            Target target = new Target(new Vector2(0, 0), new Vector2(30, 30));

            bool isCollision = false;
            int currentProyectileCount = testLevel.proyectilesCount;

            if (testLevel.CheckCollision(proyectile.p_Position, proyectile.p_RealSize, target.p_Position, target.p_RealSize))
            {
                isCollision = true;
            }

            Assert.IsTrue(isCollision);
        }
    }

    [TestClass]
    public class UnitTest2
    {
        [TestMethod]
        public void CreateCharacter()
        {
            TestLevel testLevel = new TestLevel();

            testLevel.CreateNewSubject();

            Assert.IsNotNull(testLevel.copySubjectList);
        }
    }
}
