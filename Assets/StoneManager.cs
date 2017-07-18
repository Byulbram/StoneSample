//Stone SDK Sample 
//by Byulbram 2017.07.18

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using SnailLauncherSDK_CS; //if This line got error, check SnailLauncherSDK_Proxy_CS.dll's target. one for x86, other for x86_x64

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
        UpdateConsole("Start Stone SDK initialize");

        //Get Unity Application Path. 
        //SnailLauncherSDK.dll, SnailLauncherSDK_Proxy_C.dll should be at Project folder in Editor and exe folder at build.
        string ApplicationPath = Application.dataPath;
        ApplicationPath += "/../";
        
        UpdateConsole(ApplicationPath);
        E_InitResult_CS eRet = SnailLauncher_CS.LoadLibraries(ApplicationPath);

        
        if (eRet != E_InitResult_CS.OK)
        {
            UpdateConsole("Load STONE SDK Failed: " + eRet); // returns error code
        }
        else
        {
            bool bRet = SnailLauncher_CS.GetLauncher();
            if (bRet == true)
            {
                UpdateConsole("Get Launcher OK");
            }
            else
            {
                UpdateConsole("Get Launcher Fail");
            }
        }
    }


}
