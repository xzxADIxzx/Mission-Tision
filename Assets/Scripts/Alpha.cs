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
        float time = Time.time * 1000;
        float A = 0;
        while(A < 255)
        {
            A += Mathf.Round((Time.time * 1000 - time) / delay);
            A = Mathf.Clamp(A, 0, 255);
            time = Time.time * 1000;
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
        float time = Time.time * 1000;
        float A = 255;
        while(A > 0)
        {
            A -= Mathf.Round((Time.time * 1000 - time) / delay);
            A = Mathf.Clamp(A, 0, 255);
            time = Time.time * 1000;
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
        float time = Time.time * 1000;
        float A = 0;
        while(A < 255)
        {
            A += Mathf.Round((Time.time * 1000 - time) / delay);
            A = Mathf.Clamp(A, 0, 255);
            time = Time.time * 1000;
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
        if(destroyOnEnd)
            MonoBehaviour.Destroy(image);
    }

    public static async void Off(GameObject image, int delay, bool destroyOnEnd)
    {
        float time = Time.time * 1000;
        float A = 255;
        while(A > 0)
        {
            A -= Mathf.Round((Time.time * 1000 - time) / delay);
            A = Mathf.Clamp(A, 0, 255);
            time = Time.time * 1000;
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
        if(destroyOnEnd)
            MonoBehaviour.Destroy(image);
    }

    public static async void On(GameObject image, int delay, bool setActiveOnStart, bool setActiveOnEnd)
    {
        image.SetActive(setActiveOnStart);
        float time = Time.time * 1000;
        float A = 0;
        while(A < 255)
        {
            A += Mathf.Round((Time.time * 1000 - time) / delay);
            A = Mathf.Clamp(A, 0, 255);
            time = Time.time * 1000;
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
        image.SetActive(setActiveOnEnd);
    }

    public static async void Off(GameObject image, int delay, bool setActiveOnStart, bool setActiveOnEnd)
    {
        image.SetActive(setActiveOnStart);
        float time = Time.time * 1000;
        float A = 255;
        while(A > 0)
        {
            A -= Mathf.Round((Time.time * 1000 - time) / delay);
            A = Mathf.Clamp(A, 0, 255);
            time = Time.time * 1000;
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
        image.SetActive(setActiveOnEnd);
    }
}