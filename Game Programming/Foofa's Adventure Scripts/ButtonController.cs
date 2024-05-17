using UnityEngine.SceneManagement;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    public string sceneName; // the name of the scene you want to switch to
    
    public void SwitchScene()
    {
        SceneManager.LoadScene(sceneName); // load the scene with the specified name
    }
}
