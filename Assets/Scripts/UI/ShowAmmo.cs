using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class ShowAmmo : MonoBehaviour
{
    [SerializeField] GameObject repeate;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            int[] mags = {30, 21, 10};
            Show(mags, 2, 30);
        }
    }

    public async void Show(int[] mags, int curr, int max)
    {
        Repeate rep = Instantiate(repeate, transform).GetComponent<Repeate>();
        rep.count = mags.Length;
        Transform repTrn = rep.transform;
        await Task.Delay(1);
        // Ждём до следушего кадра, что бы rep создал дочерние объекты
        float mod = 1f / max;
        for(int i = 0; i < repTrn.childCount; i++)
        {
            Transform child = repTrn.GetChild(i);
            Image fill = child.GetChild(0).GetComponent<Image>();
            fill.fillAmount = mags[i] * mod;
            Alpha.On(child.gameObject, 1);
            Alpha.On(fill.gameObject, 1);
            if(i == repTrn.childCount - 1)
            {
                Vector3 pos = child.position;
                pos = new Vector3(pos.x - 0.089f, pos.y, pos.z);
                child.position = pos;
            }
        }
    }
}
