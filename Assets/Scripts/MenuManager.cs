using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.IO;
using Newtonsoft.Json;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;

    [SerializeField] Menu[] menus;

    public TMP_Text scoreText;

    public Slider volumeSlider;

    public void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        if(Instance != this)
        {
            Destroy(this);
        }
    }
    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;

        if(!File.Exists(Path.Combine(Application.persistentDataPath, PlayerPrefs.GetString("PlayerName"), "settings.davedata")))
        {
            Settings settings = new Settings();
            SaveManager.SaveSettings(settings);
        }
        else
        {
            LoadSettings();
            SaveSettings();
        }
    }
    private void Update()
    {
        SaveSettings();
    }
    public void OpenMenu(string name)
    {
        for(int i = 0; i < menus.Length; i++)
        {
            if(menus[i].menuName == name)
            {
                OpenMenu(menus[i]);
            }
            else if(menus[i].open)
            {
                CloseMenu(menus[i]);
            }
        }
    }
    public void OpenMenu(Menu menu)
    {
        for(int i = 0; i < menus.Length; i++)
        {
            if(menus[i].open)
            {
                CloseMenu(menus[i]);
            }
        }
        menu.Open();
    }
    public void CloseMenu(Menu menu)
    {
        menu.Close();
    }
    public void LoadScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }
    public void DeleteData()
    {
        string path = Path.Combine(Application.persistentDataPath, PlayerPrefs.GetString("PlayerName"));
        string[] files = Directory.GetFiles(path);

        for (int i = 0; i < files.Length; i++)
        {
            File.Delete(files[i]);
        }

        Directory.Delete(path);
        LoadScene("NameEnter");
    }
    public void SaveSettings()
    {
        Settings settings = new Settings();

        settings.volume = volumeSlider.value;

        SaveManager.SaveSettings(settings);
    }
    public void LoadSettings()
    {
        Settings settings = SaveManager.LoadSettings();

        volumeSlider.value = settings.volume;
    }
}