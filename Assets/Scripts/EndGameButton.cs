using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameButton : MonoBehaviour {
    public void OnButtonPress() {
        StartGame();
    }

    private void StartGame() {
        SceneManager.LoadScene("GameScreen");
    }
}
