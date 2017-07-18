//Stone SDK Sample 
//by Byulbram 2017.07.18

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using SnailLauncherSDK_CS; //if This line got error, check SnailLauncherSDK_Proxy_CS.dll's target. one for x86, other for x86_x64

public class StoneManager : MonoBehaviour {
    public static StoneManager SM;
    public Text Console;    
    private StringBuilder ConsoleText = new StringBuilder();

    // Use this for initialization
    void Start ()
    {
        SM = this;
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
                UpdateConsole("Get Launcher OK!!");
            }
            else
            {
                UpdateConsole("Get Launcher Failed");
            }
        }
    }

    public void LaunchClientInterface()
    {
        UpdateConsole("Start LaunchClientInterface");
        E_StartLauncher_Result_CS startResult = SnailLauncher_CS.StartLauncherW("The Silver Bullet: Prometheus", 30171, null);
        if (startResult == 0)
        {
            UpdateConsole("StartLauncher OK!!");
        }
        else
        {
            UpdateConsole("StartLauncher Failed : " + startResult);
        }
    }


    public void GetServiceTicketSync()
    {
        UpdateConsole("Start GetServiceTicketSync");
        int iErrCode = 0;
        SnailLauncherSDK_CS.StServiceTicketInfo_CS stTemp = SnailLauncher_CS.GetServiceTicket_Sync(ref iErrCode);
        UpdateConsole("Result : " + stTemp);
    }

    public void GetServiceTicketASync()
    {
        UpdateConsole("Start GetServiceTicketASync");
        E_SdkCodeError_CS retVal = SnailLauncher_CS.GetServiceTicket_ASync(onServiceTicketCallback);
        UpdateConsole("Result : " + retVal);
    }

    private static void onServiceTicketCallback(StServiceTicketInfo_CS ticket, E_SdkCodeError_CS error)
    {
        string strServiceTicket = System.Text.Encoding.Default.GetString(ticket.szServiceTicket);
        SM.UpdateConsole(strServiceTicket.ToString());
    }

    public void RegisterStatusCheck()
    {
        UpdateConsole("Start RegisterStatusCheck");
        E_SdkCodeError_CS retVal = SnailLauncher_CS.RegisterStatusCheckCallback(OnStatusCheckCallback);
        UpdateConsole("Result : " + retVal);
    }

    private static void OnStatusCheckCallback(EStatusCheck_CS status)
    {
        switch (status)
        {
            case EStatusCheck_CS.SUCCESS: SM.UpdateConsole("OnStatusCheckCallback, status is ESuccess ."); break;

            case EStatusCheck_CS.ERROR: SM.UpdateConsole("OnStatusCheckCallback, status is EError ."); break;

            case EStatusCheck_CS.EXIT: SM.UpdateConsole("OnStatusCheckCallback, status is EExit ."); break;

            default:
                SM.UpdateConsole("OnStatusCheckCallback , status is UnKnown !"); break;
        }
    }

    public void RegisterAntiAddiction()
    {
        UpdateConsole("Start RegisterAntiAddiction");
        E_SdkCodeError_CS retVal = SnailLauncher_CS.RegisterAntiAddictionCallback(OnAntiAddictionCallback); SnailLauncher_CS.ReportPlayTheGameTimeReachCycle(10);
        UpdateConsole("Result : " + retVal);
    }


    private static void OnAntiAddictionCallback(S_AntiAddiction_CS antiAddiction)
    {
        switch (antiAddiction.type)
        {
            case E_AntiAddiction_CS.OK:
                //SnailLauncher_CS.Shutdown();
                //TerminateApp();
                SM.UpdateConsole("OnAntiAddictionCallback, Non anti-addiction account(EAntiAddictionOK).");
                break;
            case E_AntiAddiction_CS.A_FORCE_GAME_END:
                SM.UpdateConsole("OnAntiAddictionCallback,  anti-addiction  game  play  time  reach(EAntiAddictionForceGameEnd).");
                break;

            case E_AntiAddiction_CS.A_180_Minutes:
                SM.UpdateConsole("OnAntiAddictionCallback,  anti-addiction  game  play  time	180  minutes(E AntiAddiction180Minutes).");
                break;

            case E_AntiAddiction_CS.A_120_Minutes:
                SM.UpdateConsole("OnAntiAddictionCallback,  anti-addiction  game  play  time	120  minutes(E AntiAddiction120Minutes).");
                break;

            case E_AntiAddiction_CS.A_60_Minutes:
                SM.UpdateConsole("OnAntiAddictionCallback,  anti-addiction  game  play  time	60  minutes(EA ntiAddiction60Minutes).");
                break;

            case E_AntiAddiction_CS.A_PLAY_GAME:
                SM.UpdateConsole("OnAntiAddictionCallback,anti-addiction is EAntiAddictionPlayGame ."); break;

            default:
                SM.UpdateConsole("OnAntiAddictionCallback, antiAddiction.type is UnKnown !"); break;
        }
    }

    void OnApplicationQuit()
    {
        SnailLauncher_CS.FreeLaunchInstance();
        bool bRet = SnailLauncher_CS.Shutdown();
        Debug.Log("Shutdown result = " + bRet);
    }






}
