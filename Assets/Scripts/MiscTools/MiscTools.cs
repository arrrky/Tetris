using UnityEngine.SceneManagement;
using UnityEngine;

namespace MiscTools
{
    public enum FieldState
    {
        Empty = 0,
        Falling = 1,
        Fallen = 2
    }    

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

        public static void LoadMainMenu()
        {
            SceneManager.LoadScene(0);
        }

        public static Color32[] rainbowColors =
    {
        Color.red,
        new Color32(255, 128, 0, 255), // оранжевый
        Color.yellow,
        Color.green,
        new Color32(32, 72, 203, 255), // голубой
        Color.blue,
        new Color32(42, 7 ,49 , 255) // фиолетовый
    };
    }
}