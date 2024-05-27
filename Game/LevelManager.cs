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
        Game,
        Win,
        Loose
    }

    public class LevelManager
    {
        private static LevelManager instance = new LevelManager();
        public static LevelManager Instance { get { return instance; } }

        private Dictionary<string,Level> levels = new Dictionary<string, Level>();

        private Level currentLevel = null;


        public LevelManager() 
        {
            levels.Clear();
            AddNewLevel("Menu", new Menu());
            AddNewLevel("Jugar", new Gameplay());



            SetLevel("Menu");
        }

        public Level CurrentLevel => currentLevel;

        public void SetLevel(string levelName)
        {
            if(levels.TryGetValue(levelName, out var l_currentLevel))
            {
                currentLevel = l_currentLevel;
                currentLevel.Reset();
            }
            else
            {
                Console.WriteLine($"El nivel: {levelName} no se encuentra registrado");
            }
        }

        public void SetLevel(LevelType p_level)
        {
            switch(p_level) 
            {
                case LevelType.Menu:
                    currentLevel = new Menu();
                    break;
                case LevelType.Game:
                    currentLevel = new Gameplay();
                    break;
            }
        }

        public void AddNewLevel(string name, Level level)
        {
            levels.Add(name, level);
        }

    }
}
