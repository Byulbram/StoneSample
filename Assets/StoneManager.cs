//Stone SDK Sample 
//first created by Byulbram 2017.07.18
//Used Stone SDK 1.2 (2017.7.20)

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using SnailLauncherSDK_CS; //if This line got error, check SnailLauncherSDK_Proxy_CS.dll's target. one for x86, other for x86_x64

public class StoneManager : MonoBehaviour {
    public static StoneManager one;
    // Next two lines are needless for REAL project. Just used for display Debug Log on UI screen.
    public Text Console;    
    private StringBuilder ConsoleText = new StringBuilder();

    void Start ()
    {
        one = this;
        ConsoleText.Append(Console.text);
        DebugLog("Start Stone SDK initialize");

        //Get Unity Application Path. 
        //SnailLauncherSDK.dll, SnailLauncherSDK_Proxy_C.dll should be at Project folder in Editor and exe folder at build.
        string ApplicationPath = Application.dataPath;
        ApplicationPath += "/../";

        DebugLog(ApplicationPath);
        E_InitResult_CS eRet = SnailLauncher_CS.LoadLibraries(ApplicationPath);

        if (eRet != E_InitResult_CS.OK)
        {
            DebugLog("Load STONE SDK Failed: " + eRet); // returns error code
        }
    }
	

    //DebugLog function is only for display debug text on UI screen.
    //You can just use "Debug.Log" instead. (just insert "." between Debug and Log)
    public void DebugLog(string newText)
    {
        Debug.Log(newText);
        //3 Lines below this will be needless on your REAL PROJECT. Just used for display Debug log on UI Screen.
        ConsoleText.Append("\r\n");
        ConsoleText.Append(newText);
        Console.text = ConsoleText.ToString();
    } 

    public void GetLauncher()
    {
        if (SnailLauncher_CS.IsLibrariesLoaded())
        {
            //Get Stone Launcher
            bool bRet = SnailLauncher_CS.GetLauncher();
            if (bRet == true)
            {
                DebugLog("Get Launcher OK!!");
            }
            else
            {
                DebugLog("Get Launcher Failed");
            }
        }
        else
        {
            DebugLog("SDK Library not loaded");
        }
    }

    public void StartLauncher()
    {
        //Before you call StartLauncer,
        //You need to call RegisterStatusCheck first.
        //call GetLauncher, RegisterStatusCheck then call this.

        if (SnailLauncher_CS.IsLibrariesLoaded() == false)
        {
            DebugLog("SDK Library not loaded");
            return;
        }
        DebugLog("Start LaunchClientInterface");

        //StartLauncherW calls Stone Client Launcher.
        E_StartLauncher_Result_CS startResult = SnailLauncher_CS.StartLauncherW("The Silver Bullet: Prometheus", 30171, null);
        if (startResult == 0)
        {
            DebugLog("StartLauncher OK!!");
        }
        else
        {
            DebugLog("StartLauncher Failed : " + startResult);
        }
    }

    public void RegisterStatusCheck()
    {
        if (SnailLauncher_CS.IsLibrariesLoaded() == false)
        {
            DebugLog("SDK Library not loaded");
            return;
        }
        DebugLog("Start RegisterStatusCheck");
        E_SdkCodeError_CS retVal = SnailLauncher_CS.RegisterStatusCheckCallback(OnStatusCheckCallback);
        DebugLog("Result : " + retVal);
    }

    private static void OnStatusCheckCallback(EStatusCheck_CS status)
    {
        one.DebugLog("OnStatusCheckCallback Started");
        switch (status)
        {
            case EStatusCheck_CS.SUCCESS: one.DebugLog("OnStatusCheckCallback, status is ESuccess ."); break;

            case EStatusCheck_CS.ERROR: one.DebugLog("OnStatusCheckCallback, status is EError ."); break;

            case EStatusCheck_CS.EXIT: one.DebugLog("OnStatusCheckCallback, status is EExit ."); break;

            default:
                one.DebugLog("OnStatusCheckCallback , status is UnKnown !"); break;
        }
    }

    void OnApplicationQuit()
    {
        //Free Launcher instance and shutdown when finished.
        SnailLauncher_CS.FreeLaunchInstance();
        bool bRet = SnailLauncher_CS.Shutdown();
        Debug.Log("Shutdown result = " + bRet);
    }


    // ON-LINE GAME ONLY:
    // RegisterAntiAddictionCallback, GetServiceTicketSync and GetServiceTicketASync is used for Online-game with Server.
    // If you make Single-Player Game, this two functions are not needed.
    public void RegisterAntiAddiction()
    {
        if (SnailLauncher_CS.IsLibrariesLoaded() == false)
        {
            DebugLog("SDK Library not loaded");
            return;
        }
        DebugLog("Start RegisterAntiAddiction");
        E_SdkCodeError_CS retVal = SnailLauncher_CS.RegisterAntiAddictionCallback(OnAntiAddictionCallback);
        SnailLauncher_CS.ReportPlayTheGameTimeReachCycle(10); //Anti-Addiction Check time per minute.
        DebugLog("Result : " + retVal);
    }

    private static void OnAntiAddictionCallback(S_AntiAddiction_CS antiAddiction)
    {
        one.DebugLog("OnAntiAddictionCallback Started");
        switch (antiAddiction.type)
        {
            case E_AntiAddiction_CS.OK:
                one.DebugLog("OnAntiAddictionCallback, Non anti-addiction account(EAntiAddictionOK).");
                break;
            case E_AntiAddiction_CS.A_FORCE_GAME_END:
                one.DebugLog("OnAntiAddictionCallback,  anti-addiction  game  play  time  reach(EAntiAddictionForceGameEnd).");
                break;

            case E_AntiAddiction_CS.A_180_Minutes:
                one.DebugLog("OnAntiAddictionCallback,  anti-addiction  game  play  time	180  minutes(E AntiAddiction180Minutes).");
                break;

            case E_AntiAddiction_CS.A_120_Minutes:
                one.DebugLog("OnAntiAddictionCallback,  anti-addiction  game  play  time	120  minutes(E AntiAddiction120Minutes).");
                break;

            case E_AntiAddiction_CS.A_60_Minutes:
                one.DebugLog("OnAntiAddictionCallback,  anti-addiction  game  play  time	60  minutes(EA ntiAddiction60Minutes).");
                break;

            case E_AntiAddiction_CS.A_PLAY_GAME:
                one.DebugLog("OnAntiAddictionCallback,anti-addiction is EAntiAddictionPlayGame ."); break;

            default:
                one.DebugLog("OnAntiAddictionCallback, antiAddiction.type is UnKnown !"); break;
        }
    }

    public void GetServiceTicketSync()
    {
        if (SnailLauncher_CS.IsLibrariesLoaded() == false)
        {
            DebugLog("SDK Library not loaded");
            return;
        }
        DebugLog("Start GetServiceTicketSync");
        int iErrCode = 0;
        SnailLauncherSDK_CS.StServiceTicketInfo_CS stTemp = SnailLauncher_CS.GetServiceTicket_Sync(ref iErrCode);
        DebugLog("Result : " + stTemp);
    }

    public void GetServiceTicketASync()
    {
        if (SnailLauncher_CS.IsLibrariesLoaded() == false)
        {
            DebugLog("SDK Library not loaded");
            return;
        }
        DebugLog("Start GetServiceTicketASync");
        E_SdkCodeError_CS retVal = SnailLauncher_CS.GetServiceTicket_ASync(onServiceTicketCallback);
        DebugLog("Result : " + retVal);
    }

    private static void onServiceTicketCallback(StServiceTicketInfo_CS ticket, E_SdkCodeError_CS error)
    {
        one.DebugLog("onServiceTicketCallback Started");
        string strServiceTicket = System.Text.Encoding.Default.GetString(ticket.szServiceTicket);
        one.DebugLog(strServiceTicket.ToString());
    }

}
