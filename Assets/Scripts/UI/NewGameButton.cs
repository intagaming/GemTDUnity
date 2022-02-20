using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewGameButton : MonoBehaviour
{
  public void GoToScene(string sceneName)
  {
    LevelManager.Instance.LoadScene(sceneName);
  }
}
