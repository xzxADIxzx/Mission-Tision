using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using System;
using UnityEngine;
using UnityEngine.UI;

public class SettingsControl : MonoBehaviour
{
    [Header("Objects")]
    [SerializeField] GameObject[] buttons;
    [SerializeField] GameObject[] SCUI;
    public static bool isWorking;
    static string SavePath;
    Control C = new Control();
    KeyCode key, lastKey;

    void ButtonsOnOff(bool active)
    {
        Color enabled = new Color(1, 1, 1, 1);
        Color disabled = new Color(1, 1, 1, 0.5f);
        foreach(GameObject obj in buttons)
        {
            obj.GetComponent<Button>().enabled = active;
            if(active)
                obj.GetComponent<Text>().color = enabled;
            else
                obj.GetComponent<Text>().color = disabled;

        }
    }

    void OnGUI()
    {
        if (Input.GetKey(KeyCode.LeftShift))
            key = KeyCode.LeftShift;
        else
        {
            if (Input.anyKey)
            {
                Event e = Event.current;
                if (e.isKey && e.keyCode != KeyCode.None)
                    key = e.keyCode;
            }
        }
    }

    IEnumerator CC(string name)
    {
        while (true)
        {
            if (Input.anyKey)
            {
                if (key != KeyCode.Escape && key != lastKey)
                {
                	int i = 0;
                    Color red = new Color(1, 0, 0);
                    switch (name)
                    {
                        case "up":
                            C.up = key;
                            i = 0;
                            break;
                        case "down":
                            C.down = key;
                            i = 1;
                            break;
                        case "right":
                            C.right = key;
                            i = 2;
                            break;
                        case "left":
                            C.left = key;
                            i = 3;
                            break;
                        case "run":
                            C.run = key;
                            i = 4;
                            break;
                        case "squat":
                            C.squat = key;
                            i = 5;
                            break;
                        case "liedown":
                            C.liedown = key;
                            i = 6;
                            break;
                        case "shoot":
                            C.shoot = key;
                            i = 7;
                            break;
                        case "aim":
                            C.aim = key;
                            i = 8;
                            break;
                        case "reload":
                            C.reload = key;
                            i = 9;
                            break;
                        case "selFirst":
                            C.selFirst = key;
                            i = 10;
                            break;
                        case "selSecond":
                            C.selSecond = key;
                            i = 11;
                            break;
                        case "selThirt":
                            C.selThirt = key;
                            i = 12;
                            break;
                        case "buy":
                            C.buy = key;
                            i = 13;
                            break;
                        case "customize":
                            C.customize = key;
                            i = 14;
                            break;
                        case "drop":
                            C.drop = key;
                            i = 15;
                            break;
                        case "action":
                            C.action = key;
                            i = 16;
                            break;
                    }
                    SCUI[i].GetComponent<Text>().text = $"[{key}]";
                    SCUI[i].GetComponent<Text>().color = red;
                    lastKey = key;
                    break;
                }
                else
                    break;
            }
            yield return null;
        }
        ButtonsOnOff(true);
        isWorking = false;
    }

    public void ChangeControl(string name)
    {
        StartCoroutine("CC", name);
        ButtonsOnOff(false);
        isWorking = true;
    }

    public void Apply()
    {
        File.WriteAllText(SavePath, JsonUtility.ToJson(C));
        Settings.up = C.up;
        Settings.down = C.down;
        Settings.right = C.right;
        Settings.left = C.left;
        Settings.run = C.run;
        Settings.squat = C.squat;
        Settings.liedown = C.liedown;
        Settings.shoot = C.shoot;
        Settings.aim = C.aim;
        Settings.reload = C.reload;
        Settings.selFirst = C.selFirst;
        Settings.selSecond = C.selSecond;
        Settings.selThirt = C.selThirt;
        Settings.buy = C.buy;
        Settings.customize = C.customize;
        Settings.drop = C.drop;
        Settings.action = C.action;
    }

    async void Start()
    {
        await Task.Delay(1);
        SavePath = Path.Combine(Application.dataPath + "/Saves", "Control.json");

        if (!File.Exists(SavePath))
            File.WriteAllText(SavePath, JsonUtility.ToJson(C));
        C = JsonUtility.FromJson<Control>(File.ReadAllText(SavePath));
        Apply();
    }

    [Serializable]
    public class Control
    {
        public KeyCode up = KeyCode.W;
        public KeyCode down = KeyCode.S;
        public KeyCode right = KeyCode.D;
        public KeyCode left = KeyCode.A;
        public KeyCode run = KeyCode.LeftShift;
        public KeyCode squat = KeyCode.LeftControl;
        public KeyCode liedown = KeyCode.Z;
        public KeyCode shoot = KeyCode.Mouse0;
        public KeyCode aim = KeyCode.Mouse1;
        public KeyCode reload = KeyCode.R;
        public KeyCode selFirst = KeyCode.Alpha1;
        public KeyCode selSecond = KeyCode.Alpha2;
        public KeyCode selThirt = KeyCode.Alpha3;
        public KeyCode buy = KeyCode.Tab;
        public KeyCode customize = KeyCode.T;
        public KeyCode drop = KeyCode.C;
        public KeyCode action = KeyCode.E;

    }
}