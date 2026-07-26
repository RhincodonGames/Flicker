using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public GameObject mainMenuPanel;
    public GameObject rulesPanel;

    public GameObject ButtonContainer;

    public GameObject FlickerTitle;

    public bool rulesPanelActive = false;

    public void StartGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void QuitGame()
    {
        Debug.Log("Quit!");
        Application.Quit();
    }

    public void OpenRules()
    {
        rulesPanel.SetActive(true);
        rulesPanelActive = true;

        ButtonContainer.SetActive(false);
        FlickerTitle.SetActive(false);


    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            rulesPanel.SetActive(false);
            rulesPanelActive = false;

            ButtonContainer.SetActive(true);
            FlickerTitle.SetActive(true);
        }
    }
}