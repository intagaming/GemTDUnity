using System.Collections;
using System.Collections.Generic;
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

  public async void LoadScene(string sceneName)
  {
    var scene = SceneManager.LoadSceneAsync(sceneName);
    scene.allowSceneActivation = false;

    _loadingCanvas.gameObject.SetActive(true);
    do
    {
      await Task.Delay(500);
      _progressBarForeground.fillAmount = scene.progress;
    } while (scene.progress < 0.9f);

    scene.allowSceneActivation = true;
    await Task.Delay(500);
    _loadingCanvas.gameObject.SetActive(false);
  }
}
