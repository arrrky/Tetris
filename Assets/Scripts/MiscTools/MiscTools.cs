using UnityEngine.SceneManagement;
using UnityEngine;
using System.Collections.Generic;

public enum Scenes
{
    MainMenu = 0,
    Game = 1,
}

namespace MiscTools
{
    public class Tools
    {
        public static FieldState[,] ConvertToFieldState(int[,] element)
        {
            FieldState[,] temp = new FieldState[element.GetLength(0), element.GetLength(1)];

            for (int y = 0; y < element.GetLength(0); y++)
                for (int x = 0; x < element.GetLength(1); x++)
                    temp[y, x] = (FieldState)element[y, x];
            return temp;
        }        

        public static void LoadScene(Scenes scene)
        {
            SceneManager.LoadScene((int)scene);
        }

        public static void CurrentSceneReload()
        {
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }

        public static Vector3 GetScreenBounds()
        {
            Camera mainCamera = Camera.main;
            Vector3 screenVector = new Vector3(Screen.width, Screen.height, mainCamera.transform.position.z);
            return mainCamera.ScreenToWorldPoint(screenVector);
        }

        /// <summary>
        /// Получение смещения для инстанирования объекта в зависимости от размеров спрайта
        /// </summary>
        /// <param name="borderBlockPrefab"></param>
        public static Vector2 GetSpriteShift(GameObject borderBlockPrefab)
        {
            SpriteRenderer spriteRenderer = borderBlockPrefab.GetComponent<SpriteRenderer>();
            return new Vector2(spriteRenderer.bounds.extents.x, spriteRenderer.bounds.extents.y);
        }    

        public static Dictionary<string, Color32> rainbowColors = new Dictionary<string, Color32>
        {
            {"red",      new Color32(255, 50, 19, 255) },
            {"orange",   new Color32(255, 128, 0, 255) },
            {"yellow",   new Color32(255, 213, 0, 255)  },
            {"green",    new Color32 (114, 203, 59, 255) },
            {"lightblue",new Color32(10, 150, 203, 255) },
            {"blue",     new Color32(3, 65, 174, 255) },
            {"violet",   new Color32(136, 43, 222 , 255) }
        };

        public static Dictionary<string, Color32> additionalColors = new Dictionary<string, Color32>
        {
            {"grey",     new Color32(160, 160, 160, 255) },
            {"brown",    new Color32(153, 76, 0, 255) },
            {"pink",     new Color32(255, 0, 127, 255) },
        };
    }
}