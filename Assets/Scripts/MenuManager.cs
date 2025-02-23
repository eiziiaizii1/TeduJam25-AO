using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void PlayGame()
    {
        SoundManager.Instance.PlayEffectSound(SoundManager.Instance.ButtonSE);
        SceneManager.LoadScene(1);
    }

    public void ExitGame()
    {
        SoundManager.Instance.PlayEffectSound(SoundManager.Instance.ButtonSE);
        Debug.Log("Quitting ...");
        Application.Quit();
    }
}
