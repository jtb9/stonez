using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class SplashSceneController : MonoBehaviour
{
    public UIDocument uIDocument;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Button b = uIDocument.rootVisualElement.Query<Button>("play");
        b.clicked += OnPlay;

        // target 60 fps
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;

        // don't let the screen grey out
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    bool loading = false;
    void OnPlay() {
        if (loading == false) {
            loading = true;
            Button b = uIDocument.rootVisualElement.Query<Button>("play");
            b.style.visibility = Visibility.Hidden;
            
            Global.Load();
            Global.temp_hasStarted = true;
            SceneManager.LoadScene("Game");
        }
    }
}
