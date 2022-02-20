using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnToMenuButton : MonoBehaviour
{
  public void GoToScene(string sceneName)
  {
    LevelManager.Instance.LoadSceneSync(sceneName);
  }
}
