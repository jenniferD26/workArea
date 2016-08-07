using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Mansion : MonoBehaviour {

    void ExitMansion()
    {
        SceneManager.LoadScene("LevelOne");
    }
}
