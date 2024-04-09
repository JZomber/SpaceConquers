using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;

//--------------------------------
// RAMA DEVELOP DEL RESPOSITORIO
//--------------------------------

namespace Game
{
    public class Program
    {
        static void Main(string[] args)
        {
            Engine.Initialize();

            while(true)
            {
                //todo: update
                Render();
            }
        }

        static void input()
        {

        }

        static void update() 
        {
        
        }

        static void Render()
        {
            Engine.Clear(); // Limpia la pantalla

            Engine.Draw("ship.png", 200, 200); // Path, X, Y

            Engine.Draw("alien.png", 600, 225, .10f, .10f); // Path, X, Y

            Engine.Show(); // Muestra los objetos en pantalla
        }
    }
}

/*
   Para agregar imágenes.

   En el explorador de soluciones, click-izq > agregar > Elemento existente > Imagen (PNG) descargada

   Luego en propiedades (la llave), "Copiar en el directorio de salida" > "Copiar Siempre" || (Así al momento de Debuggear, están todos los elementos)
*/ 