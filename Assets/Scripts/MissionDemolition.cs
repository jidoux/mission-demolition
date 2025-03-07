using System;
using System.Diagnostics;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public enum GameMode {
    idle,
    playing,
    levelEnd,
}

// the game's game state manager
public class MissionDemolition : MonoBehaviour {
    static private MissionDemolition SingletonInstance;
    [Header("Set in Inspector")]
    public TMP_Text levelText;
    public TMP_Text shotsText;
    public TMP_Text buttonViewText;
    public Vector3 placeToPutCastle; // 50 -9.5 0
    public GameObject[] allCastles;
    [Header("Set Dynamically")]
    public int currLevel;
    public int numLevels;
    public int shotsTaken;
    public GameObject currCastle;
    public GameMode mode = GameMode.idle;
    public string showing = "Show Slingshot"; // the current FollowCamera mode I guess
    [SerializeField] private LineController lineController;
    [SerializeField] private AudioSource bandSnappingAudio;
    [SerializeField] private AudioClip audioClip;


    void Start() {
        SingletonInstance = this;
        currLevel = 0;
        numLevels = allCastles.Length;
        StartLevel();

    }

    void Update() {
        UpdateGUI();
        // checking if the level should end
        if ((mode == GameMode.playing) && Goal.goalMet) {
            mode = GameMode.levelEnd;
            // this zooms out
            SwitchView("Show Both");
            // starting next level after 2 seconds
            Invoke("NextLevel", 2f);
        }
    }

    void StartLevel() {
        DestroyOldLevelGameObjects();
        PlaceCastle();
        shotsTaken = 0;
        ResetCamera();

        // resetting the goal
        Goal.goalMet = false;

        UpdateGUI();

        mode = GameMode.playing;
    }

    void DestroyOldLevelGameObjects() {
        // In this case we check if its null since
        // it has been a week, i do not know what this since was. I guess its obvious just if there is a castle but idk for sure. 
        if (currCastle != null) {
            Destroy(currCastle);
        }
        // destroying old projectiles if they exist
        // TODO how to do this optimally?
        GameObject[] projectilesToDestroy = GameObject.FindGameObjectsWithTag("Projectile");
        // just learned that i gotta iterate through this backwords since I'm destroying elements of the list itself
        // so the indices update as the Destroy occurs which can cause problems. Dats crazy.
        for (int i = projectilesToDestroy.Length - 1; i >= 0; i--) {
            Destroy(projectilesToDestroy[i]);
        }
    }

    void PlaceCastle() {
        // instantiate the new castle
        currCastle = Instantiate<GameObject>(allCastles[currLevel]);
        currCastle.transform.position = placeToPutCastle;
    }

    void ResetCamera() {
        SwitchView("Show Both");
        ProjectileLine.SingletonInstance.Clear();
    }

    void UpdateGUI() {
        levelText.text = "Level: " + (currLevel + 1) + " of " + numLevels;
        shotsText.text = "Shots Taken: " + shotsTaken;
    }

    void NextLevel() {
        currLevel++;
        if (currLevel == numLevels) {
            SceneManager.LoadScene("EndScreen");
        }
        else { // it must be in the else here or else it will try to "start the level" on the end screen -_-
            StartLevel();
        }
    }

    public void ResetLevel() {
        DestroyOldLevelGameObjects();
        PlaceCastle();
        ResetCamera();
    }

    // does this imply eView is an optimal parameter or what?
    // TODO this function sucks. I'd say just make 3 buttons or something.
    public void SwitchView(string eView = "") {
        if (eView == "") {
            eView = buttonViewText.text;
        }
        showing = eView;
        switch (showing) {
            case "Show Slingshot":
                FollowCamera.pointOfInterest = null;
                buttonViewText.text = "Show Castle";
                break;
            case "Show Castle":
                FollowCamera.pointOfInterest = SingletonInstance.currCastle;
                buttonViewText.text = "Show Both";
                break;
            case "Show Both":
                FollowCamera.pointOfInterest = GameObject.Find("ViewBoth");
                buttonViewText.text = "Show Slingshot";
                break;
        }
    }

    // allowing anyone, anywhere, to increment shots taken
    public void ShotFired() {
        print("Shot fired");
        UnityEngine.Debug.Log("Shot fired");
        lineController.RemoveProjectileFromLine();
        SingletonInstance.shotsTaken++;
        bandSnappingAudio.PlayOneShot(audioClip);
    }
}
