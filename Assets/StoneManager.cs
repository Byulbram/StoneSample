using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using SnailLauncherSDK_CS;

public class StoneManager : MonoBehaviour {
    public Text Console;    
    private StringBuilder ConsoleText = new StringBuilder();

    // Use this for initialization
    void Start ()
    {
        ConsoleText.Append(Console.text);
        UpdateConsole("Program Started");
	}
	
	// Update is called once per frame
	void Update ()
    {
	    	
	}

    public void UpdateConsole(string newText)
    {
        ConsoleText.Append("\r\n");
        ConsoleText.Append(newText);
        Console.text = ConsoleText.ToString();
    } 

    public void Initialize()
    {
        UpdateConsole("Start initialize");
        string ApplicationPath = Application.dataPath;
#if UNITY_EDITOR
#else
        ApplicationPath += "/../";
#endif
        UpdateConsole(ApplicationPath);
        E_InitResult_CS eRet = SnailLauncher_CS.LoadLibraries(ApplicationPath);


        
        if (eRet != E_InitResult_CS.OK)
        {
            UpdateConsole("Load STONE SDK Fail");
        }
        else
        {
            bool bRet = SnailLauncher_CS.GetLauncher();
            if (bRet == true)
            {
                UpdateConsole("Get Launver OK");
            }
            else
            {
                UpdateConsole("Get Launver Fail");
            }
        }
    }


}
