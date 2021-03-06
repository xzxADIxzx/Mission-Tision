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

    public static async void On(GameObject image, int delay)
    {
        float A = 0;
        for (int i = 0; i < 255; i++)
        {
            A++;
            if (image.GetComponent<Image>())
                image.GetComponent<Image>().color = ColorA(image.GetComponent<Image>().color, A);
            if (image.GetComponent<SpriteRenderer>())
                image.GetComponent<SpriteRenderer>().color = ColorA(image.GetComponent<SpriteRenderer>().color, A);
            if (image.GetComponent<Text>())
                image.GetComponent<Text>().color = ColorA(image.GetComponent<Text>().color, A);
            if (image.GetComponent<TextMesh>())
                image.GetComponent<TextMesh>().color = ColorA(image.GetComponent<TextMesh>().color, A);
            await Task.Delay(delay);
        }
    }

    public static async void Off(GameObject image, int delay)
    {
        float A = 255;
        for (int i = 255; i > 0; i--)
        {
            A--;
            if (image.GetComponent<Image>())
                image.GetComponent<Image>().color = ColorA(image.GetComponent<Image>().color, A);
            if (image.GetComponent<SpriteRenderer>())
                image.GetComponent<SpriteRenderer>().color = ColorA(image.GetComponent<SpriteRenderer>().color, A);
            if (image.GetComponent<Text>())
                image.GetComponent<Text>().color = ColorA(image.GetComponent<Text>().color, A);
            if (image.GetComponent<TextMesh>())
                image.GetComponent<TextMesh>().color = ColorA(image.GetComponent<TextMesh>().color, A);
            await Task.Delay(delay);
        }
    }

    public static async void On(GameObject image, int delay, bool destroyOnEnd)
    {
        On(image, delay);
        for (int i = 255; i > 0; i--)
            await Task.Delay(delay);
        if(destroyOnEnd)
            MonoBehaviour.Destroy(image);
    }

    public static async void Off(GameObject image, int delay, bool destroyOnEnd)
    {
        Off(image, delay);
        for (int i = 255; i > 0; i--)
            await Task.Delay(delay);
        if(destroyOnEnd)
            MonoBehaviour.Destroy(image);
    }

    public static async void On(GameObject image, int delay, bool setActiveOnStart, bool setActiveOnEnd)
    {
        image.SetActive(setActiveOnStart);
        On(image, delay);
        for (int i = 255; i > 0; i--)
            await Task.Delay(delay);
        image.SetActive(setActiveOnEnd);
    }

    public static async void Off(GameObject image, int delay, bool setActiveOnStart, bool setActiveOnEnd)
    {
        image.SetActive(setActiveOnStart);
        Off(image, delay);
        for (int i = 255; i > 0; i--)
            await Task.Delay(delay);
        image.SetActive(setActiveOnEnd);
    }
}