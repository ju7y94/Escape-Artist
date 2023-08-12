//Script written by Leith Abdul-Hussain [001104598] (C) GameMasters Inc. 2023

using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class KeybindManager : MonoBehaviour
{
    [System.Serializable]
    public class Action
    {
        public string name;
        public KeyCode key;
        public Image image;
        public Button changeButton;
        public KeyCode defaultKey;
    }

    public List<Action> actions;
    public List<KeyCode> reservedKeys;
    public Text messageText;
    public KeyImage[] keyImages;
    public Button resetButton;

    private Dictionary<KeyCode, Action> keybindings = new Dictionary<KeyCode, Action>();

    private void Start()
    {
        foreach (Action action in actions)
        {
            action.key = action.defaultKey;
            keybindings[action.key] = action;
            action.changeButton.onClick.AddListener(delegate { OnChangeButtonClicked(action); });
        }

        UpdateKeyImages();

        resetButton.onClick.AddListener(Reset);
    }

    private void Update()
    {
        foreach (KeyCode key in System.Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKeyDown(key))
            {
                Debug.Log("Key pressed: " + key.ToString());
            }
        }
    }
    
    public void ChangeKeybinding(Action action, KeyCode newKey)
    {
        Debug.Log("Changing keybinding for " + action.name + " from " + action.key + " to " + newKey);

        if (keybindings.ContainsKey(newKey) && keybindings[newKey] != action)
        {
            ShowMessage("Key " + newKey.ToString() + " is already bound to another action.");
            return;
        }

        if (reservedKeys.Contains(newKey))
        {
            ShowMessage("Key " + newKey.ToString() + " is already reserved.");
            return;
        }

        keybindings.Remove(action.key);
        keybindings[newKey] = action;
        action.key = newKey;

        ShowMessage("Keybinding for " + action.name + " has been changed to " + newKey.ToString() + ".");

        UpdateKeyImages();
    }

    public void UpdateKeyImages()
    {
        foreach (KeyImage keyImage in keyImages)
        {
            keyImage.image.color = Color.clear;

            KeyCode key = keyImage.key;
            if (keybindings.ContainsKey(key))
            {
                Sprite sprite = keybindings[key].image.sprite;
                keyImage.image.sprite = sprite;
                keyImage.image.color = Color.white;
            }
        }
    }

    public void ShowMessage(string text)
    {
        messageText.text = text;
    }

    public void OnChangeButtonClicked(Action action)
    {
        Debug.Log("ChangeButton clicked");
        KeyCode currentKey = action.key;
        Image currentImage = action.image;

        // Clear the Image and assigned KeyCode
        action.image.sprite = null;
        action.key = KeyCode.None;

        ShowMessage("Press a key to bind " + action.name + ".");

        StartCoroutine(WaitForKeyPress(action, currentKey, currentImage));
    }

    public IEnumerator<WaitForEndOfFrame> WaitForKeyPress(Action action, KeyCode currentKey, Image currentImage)
    {
        while (true)
        {
            if (Input.anyKeyDown)
            {
                KeyCode newKey = GetNewKey();
                if (newKey != KeyCode.None)
                {
                    ChangeKeybinding(action, newKey);
                    action.image = currentImage;

                    ShowMessage("");

                    break;
                }
            }

            yield return new WaitForEndOfFrame();
        }

        action.key = currentKey;
        action.image = currentImage;
    }

    public KeyCode GetNewKey()
    {
        Debug.Log("Waiting for new key input...");
        foreach (KeyCode key in System.Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKeyDown(key))
            {
                if (reservedKeys.Contains(key))
                {
                    ShowMessage("Key " + key.ToString() + " is reserved.");
                }
                else
                {
                    Debug.Log("New key detected: " + key.ToString());
                    return key;
                }
            }
        }
        Debug.Log("KeyCode.None is returned");
        return KeyCode.None;
    }

    public void Reset()
    {
        foreach (Action action in actions)
        {
            keybindings[action.key] = action;
            action.key = action.defaultKey;
        }

        UpdateKeyImages();
    }
}

[System.Serializable]
public class KeyImage
{
    public KeyCode key;
    public Image image;
}