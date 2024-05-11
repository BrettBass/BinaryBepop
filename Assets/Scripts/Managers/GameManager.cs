using UnityEngine;

public class GameManager : MonoBehaviour
{
    bool QuitGameKey => Input.GetKeyUp(KeyCode.Escape);
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(QuitGameKey)
        {
            GameState();
        }
    }

    void GameState()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
       Application.Quit();
#endif
    }
}
