using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public static class Alpha
{
	public static Color ColorA(Color color, float Alpha255){
		return new Color(color.r, color.g, color.b, Alpha255 / 255);
	}

    public static async void On(GameObject image, int delay = 1, int sA = 0, int eA = 255, bool destroy = false, bool sActive = true, bool eActive = true)
    {
        image.SetActive(sActive);
        float time = Time.time * 1000;
        while(sA < eA)
        {
            sA += Mathf.RoundToInt((Time.time * 1000 - time) / delay);
            sA = Mathf.Clamp(sA, 0, eA);
            time = Time.time * 1000;
            if (image.GetComponent<Image>())
                image.GetComponent<Image>().color = ColorA(image.GetComponent<Image>().color, sA);
            if (image.GetComponent<SpriteRenderer>())
                image.GetComponent<SpriteRenderer>().color = ColorA(image.GetComponent<SpriteRenderer>().color, sA);
            if (image.GetComponent<Text>())
                image.GetComponent<Text>().color = ColorA(image.GetComponent<Text>().color, sA);
            if (image.GetComponent<TextMesh>())
                image.GetComponent<TextMesh>().color = ColorA(image.GetComponent<TextMesh>().color, sA);
            await Task.Delay(delay);
        }
        image.SetActive(eActive);
        if (destroy)
            MonoBehaviour.Destroy(image);
    }

    public static async void Off(GameObject image, int delay = 1, int sA = 255, int eA = 0, bool destroy = false, bool sActive = true, bool eActive = true)
    {
        image.SetActive(sActive);
        float time = Time.time * 1000;
        while (sA > eA)
        {
            sA -= Mathf.RoundToInt((Time.time * 1000 - time) / delay);
            sA = Mathf.Clamp(sA, eA, 255);
            time = Time.time * 1000;
            if (image.GetComponent<Image>())
                image.GetComponent<Image>().color = ColorA(image.GetComponent<Image>().color, sA);
            if (image.GetComponent<SpriteRenderer>())
                image.GetComponent<SpriteRenderer>().color = ColorA(image.GetComponent<SpriteRenderer>().color, sA);
            if (image.GetComponent<Text>())
                image.GetComponent<Text>().color = ColorA(image.GetComponent<Text>().color, sA);
            if (image.GetComponent<TextMesh>())
                image.GetComponent<TextMesh>().color = ColorA(image.GetComponent<TextMesh>().color, sA);
            await Task.Delay(delay);
        }
        image.SetActive(eActive);
        if (destroy)
            MonoBehaviour.Destroy(image);
    }
}