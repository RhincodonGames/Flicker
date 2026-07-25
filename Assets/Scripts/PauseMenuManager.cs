using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{
    public GameObject PauseOverlay;

    public static bool IsPaused { get; private set; } = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (IsPaused)
                Resume();
            else
                Pause();
        }
    }

    public void Pause()
    {
        PauseOverlay.SetActive(true);
        Time.timeScale = 0f;
        IsPaused = true;

        // Free the cursor so it can click UI buttons
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Resume()
    {
        PauseOverlay.SetActive(false);
        Time.timeScale = 1f;
        IsPaused = false;

        // Re-lock the cursor for camera look
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        IsPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        SceneManager.LoadScene("GameScene");
    }

    public void QuitSession()
    {
        Time.timeScale = 1f;
        IsPaused = false;
        Cursor.lockState = CursorLockMode.None; // menu scene probably wants a free cursor, not locked
        Cursor.visible = true;
        SceneManager.LoadScene("MenuScene");
    }
}