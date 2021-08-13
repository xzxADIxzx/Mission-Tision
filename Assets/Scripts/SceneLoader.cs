using System;
using System.Collections;
using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [Header("Images")]
    [SerializeField] Image fill;
    [SerializeField] GameObject[] image;

    public IEnumerator Loading(string scene, Action callback)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(scene);
        operation.allowSceneActivation = false;
        while (operation.progress != 0.9f)
        {
            fill.fillAmount = operation.progress / 0.9f;
            yield return null;
        }
        foreach (GameObject obj in image) Alpha.Off(obj, 1, 255, 0, false, true, false);
        Alpha.Off(fill.gameObject, 1, 255, 0, false, true, false);
        fill.fillAmount = operation.progress / 0.9f;
        operation.allowSceneActivation = true;
        yield return null;
        callback?.Invoke();
    }

    public void Load(string scene, Action callback = null)
    {
        foreach (GameObject obj in image) Alpha.On(obj, 1, 0, 255, false, true, true);
        Alpha.On(fill.gameObject, 1, 0, 255, false, true, true, delegate { StartCoroutine(Loading(scene, callback)); });
        fill.fillAmount = 0;
    }
}
