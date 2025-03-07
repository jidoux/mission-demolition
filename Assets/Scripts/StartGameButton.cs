using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGameButton : MonoBehaviour {
    public void OnButtonPress() {
        StartGame();
    }

    private void StartGame() {
        SceneManager.LoadScene("GameScreen");
    }
}
