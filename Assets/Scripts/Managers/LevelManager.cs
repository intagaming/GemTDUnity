using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
  [SerializeField]
  private Canvas _loadingCanvas;
  [SerializeField]
  private Image _progressBarForeground;
  [SerializeField]
  private Animator _transitionAnimator;

  private bool _transitioning = false;

  private static LevelManager _instance;
  public static LevelManager Instance { get => _instance; }

  void Awake()
  {
    if (_instance == null)
    {
      _instance = this;
      DontDestroyOnLoad(gameObject);
    }
    else
    {
      Destroy(gameObject);
    }
  }

  public void LoadSceneSync(string sceneName){
    LoadSceneSync(sceneName, true);
  }

  public void LoadSceneSync(string sceneName, bool load)
  {
    if (_transitioning) return;
    _transitioning = true;
    LoadScene(sceneName,load);
  }

  // We are currently using artificial wait time for visual purposes.
  private void LoadScene(string sceneName){
    LoadScene(sceneName, true);
  }
  private async void LoadScene(string sceneName, bool load)
  {
    _progressBarForeground.fillAmount = 0;

    var scene = SceneManager.LoadSceneAsync(sceneName);
    scene.allowSceneActivation = false;
    Time.timeScale = 1;

    _transitionAnimator.SetTrigger("Start");
    await Task.Delay(1000);

    if (load)
    {
      _loadingCanvas.gameObject.SetActive(true);
      _transitionAnimator.SetTrigger("Stop");
      do
      {
        await Task.Delay(500);
        _progressBarForeground.fillAmount = scene.progress;
      } while (scene.progress < 0.9f);

      scene.allowSceneActivation = true;
      await Task.Delay(500);
      _progressBarForeground.fillAmount = 1f;
      _transitionAnimator.SetTrigger("Start");
      await Task.Delay(1000);
      _loadingCanvas.gameObject.SetActive(false);
      _transitionAnimator.SetTrigger("Stop");
    }
    else
    {
      scene.allowSceneActivation = true;
      _transitionAnimator.SetTrigger("Stop");
    }

    _transitioning = false;
  }
}
