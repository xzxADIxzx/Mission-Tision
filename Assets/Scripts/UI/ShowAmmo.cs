using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class ShowAmmo : MonoBehaviour
{
    [SerializeField] private GameObject repeate;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            int[] mags = {30, 21, 10};
            Show(mags, Mathf.RoundToInt(Random.Range(0, 3)), 30);
        }
    }

    public async void Show(int[] mags, int cur, int max)
    {
        Repeate rep = Instantiate(repeate, transform).GetComponent<Repeate>();
        rep.count = mags.Length;
        Transform repTrn = rep.transform;
        await Task.Delay(1);
        // Ждём до следушего кадра, что бы rep создал дочерние объекты
        int isFound = 0;
        int curMag = mags[cur];
        int[] modMags = new int[mags.Length - 1];
        for (int i = 0; i <= modMags.Length; i++) if (i != cur) modMags[i - isFound] = mags[i]; else isFound = 1;
        mags = modMags;
        // Удаляем используемый в данный момент магазин
        float mod = 1f / max;
        for (int i = 0; i < repTrn.childCount; i++)
        {
            Transform child = repTrn.GetChild(i);
            Image fill = child.GetChild(0).GetComponent<Image>();
            Alpha.On(child.gameObject, 1);
            Alpha.On(fill.gameObject, 1);
            if(i == repTrn.childCount - 1)
            {
                Vector3 pos = child.position;
                pos = new Vector3(pos.x - 0.089f, pos.y, pos.z);
                child.position = pos;
                fill.fillAmount = curMag * mod;
            }
            else fill.fillAmount = mags[i] * mod;
        }
        await Task.Delay(1500);
        for (int i = 0; i < repTrn.childCount; i++)
        {
            Transform child = repTrn.GetChild(i);
            Transform fill = child.GetChild(0);
            Alpha.Off(child.gameObject, 2, 255, 0, true);
            Alpha.Off(fill.gameObject, 2, 255, 0, true);
        }
    }
}
