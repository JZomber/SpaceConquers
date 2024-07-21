using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{

    public enum LevelType
    {
        Menu,
        Level1,
        Level2,
        Win,
        Loose
    }

    public class LevelManager
    {
        private static LevelManager instance = new LevelManager();
        public static LevelManager Instance { get { return instance; } }

        private Dictionary<string,Level> levels = new Dictionary<string, Level>();

        private Level currentLevel = null;

        private string lastLevelName = null;


        public LevelManager() 
        {
            levels.Clear();
            AddNewLevel("Menu", new Menu());
            AddNewLevel("Level_1", new Level_1());
            AddNewLevel("Level_2", new Level_2());
            AddNewLevel("Victory", new Victory());
            AddNewLevel("Defeat", new Defeat());



            SetLevel("Menu");
        }

        public Level CurrentLevel => currentLevel;
        public string LastLevelName => lastLevelName;

        public void SetLevel(string levelName)
        {
            if(levels.TryGetValue(levelName, out var l_currentLevel))
            {
                if (currentLevel != null)
                {
                    lastLevelName = currentLevel.GetType().Name;
                }
                else
                {
                    lastLevelName = null;
                }

                currentLevel = l_currentLevel;
                currentLevel.Reset();
            }
            else
            {
                Console.WriteLine($"El nivel: {levelName} no se encuentra registrado");
            }
        }

        public void SetLevel(LevelType p_level, LevelType levelType)
        {
            switch(p_level) 
            {
                case LevelType.Menu:
                    currentLevel = new Menu();
                    break;
                case LevelType.Level1:
                    currentLevel = new Level_1();
                    break;
                case LevelType.Level2:
                    currentLevel = new Level_2();
                    break;
                case LevelType.Win:
                    currentLevel = new Victory();
                    break;
                case LevelType.Loose:
                    currentLevel = new Defeat();
                    break;
            }
        }

        public void AddNewLevel(string name, Level level)
        {
            levels.Add(name, level);
        }
    }
}
