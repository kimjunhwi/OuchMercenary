using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.IO;
using System.Text;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using ReadOnlys;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;
using UnityEngine.SocialPlatforms.GameCenter;


//모든 데이터 및 로드, 세이브를 관리하는 클래스 
//어디서든 사용해야 하기 때문에 제네릭싱글톤을 통해 구현
public class GameManager : GenericMonoSingleton<GameManager>
{

	public GameObject Root_ui;

	LoginManager loginManager;

	public bool bIsLoad = false;

	private Player m_Player;

	public IEnumerator DataLoad()
    {
		loginManager = GameObject.Find("LoginManager").GetComponent<LoginManager>();

		#if UNITY_EDITOR

		loginManager.bIsSuccessed = true;

		#elif UNITY_IOS

		GameCenterPlatform.ShowDefaultAchievementCompletionBanner (true);

		Social.localUser.Authenticate ((bool success) => 
		{
		if(success)
		{
		loginManager.bIsSuccessed = true;
		}
		else
		{

		}
		});

		#endif

		m_Player = new Player ();

		m_Player.Init ();

        yield break;
    }


	#region LoadTableInfo
	/*
    void Load_TableInfo_Weapon()
    {
        if (cWeaponInfo.Length != 0) return;

        string txtFilePath = "Weapon";
        TextAsset ta = LoadTextAsset(txtFilePath);
        List<string> line = LineSplit(ta.text);

        CGameWeaponInfo[] kInfo = new CGameWeaponInfo[line.Count - 1];

        for (int i = 0; i < line.Count; i++)
        {
            //Console.WriteLine("line : " + line[i]);
            if (line[i] == null) continue;
            if (i == 0) continue; 	// Title skip

            string[] Cells = line[i].Split("\t"[0]);	// cell split, tab
            if (Cells[0] == "") continue;

            kInfo[i - 1] = new CGameWeaponInfo();
            kInfo[i - 1].nIndex = int.Parse(Cells[0]);
			kInfo[i - 1].strPath = Cells[1];
			kInfo[i - 1].strName = Cells[2];
			kInfo [i - 1].dMaxComplate = double.Parse (Cells [3]);
			kInfo [i - 1].dMinusRepair= double.Parse(Cells[4]);
			kInfo [i - 1].fMinusChargingWater = float.Parse (Cells [5]);
			kInfo [i - 1].dMinusCriticalDamage = double.Parse (Cells [6]);
			kInfo [i - 1].fMinusUseWater= float.Parse(Cells[7]);
			kInfo [i - 1].fMinusCriticalChance = float.Parse (Cells [8]);
			kInfo [i - 1].fMinusAccuracy= float.Parse(Cells[9]);
			kInfo[i - 1].dGold = double.Parse(Cells[10]);
			kInfo[i - 1].dHonor = double.Parse(Cells[11]);
			kInfo[i - 1].fLimitedTime = float.Parse(Cells[12]);
			kInfo[i - 1].nGrade = int.Parse(Cells[13]);
            kInfo[i - 1].WeaponSprite = ObjectCashing.Instance.LoadSpriteFromCache(kInfo[i - 1].strPath);
        }
        cWeaponInfo = kInfo;
    }
	*/
	#endregion

	#region SplitText

    TextAsset LoadTextAsset(string _txtFile)
    {
        TextAsset ta;
        ta = Resources.Load("Data/" + _txtFile) as TextAsset;
        return ta;
    }

    public List<string> LineSplit(string text)
    {
        //Console.WriteLine("LineSplit " + text.Length);

        char[] text_buff = text.ToCharArray();

        List<string> lines = new List<string>();

        int linenum = 0;
        bool makecell = false;

        StringBuilder sb = new StringBuilder("");

        for (int i = 0; i < text.Length; i++)
        {
            char c = text_buff[i];
            //int value = Convert.ToInt32(c); Console.WriteLine(String.Format("{0:x4}", value) + " " + c.ToString());

            if (c == '"')
            {
                char nc = text_buff[i + 1];
                if (nc == '"') { i++; } //next char
                else
                {
                    if (makecell == false) { makecell = true; c = nc; i++; } //next char
                    else { makecell = false; c = nc; i++; } //next char
                }
            }

            //0x0a : LF ( Line Feed : 다음줄로 캐럿을 이동 '\n')
            //0x0d : CR ( Carrage Return : 캐럿을 제일 처음으로 복귀 )			    
            if (c == '\n' && makecell == false)
            {
                char pc = text_buff[i - 1];
                if (pc != '\n')	//file end
                {
                    lines.Add(sb.ToString()); sb.Remove(0, sb.Length);
                    linenum++;
                }
            }
            else if (c == '\r' && makecell == false)
            {
            }
            else
            {
                sb.Append(c.ToString());
            }
        }

        return lines;
    }
	#endregion


    #region window popup

    // 윈도우 팝업 ---------------------------------------------------------------------------------------
	//CGame.Instance.Window_notice("213123 213123 ", rt => { if (rt == "0") print("notice");  });
	public void Window_notice(string _msg, System.Action<string> _callback)
	{
		//GameObject Root_ui = GameObject.Find("root_window)"); //ui attach
		GameObject go = GameObject.Instantiate(Resources.Load("prefabs/Window_notice"), Vector3.zero, Quaternion.identity) as GameObject;
		go.transform.parent = Root_ui.transform;
		go.transform.localPosition = Vector3.zero;
		go.transform.localRotation = Quaternion.identity;
		go.transform.localScale = Vector3.one;

		CWindowNotice w = go.GetComponent<CWindowNotice>();
		w.Show(_msg, _callback);
	}

	public void Window_yesno(string strTitle, string strValue,Sprite _sprite,  System.Action<string> _callback)
	{
		//GameObject Root_ui = GameObject.Find("root_window)"); //ui attach
		GameObject go = GameObject.Instantiate(Resources.Load("Prefabs/Window_yesno"), Vector3.zero, Quaternion.identity) as GameObject;
		go.transform.parent = Root_ui.transform;
		go.transform.localPosition = Vector3.zero;
		go.transform.localRotation = Quaternion.identity;
		go.transform.localScale = Vector3.one;

		CWindowYesNo w = go.GetComponent<CWindowYesNo>();
		w.Show(strTitle,strValue,_sprite, _callback);
	}

	public void Window_Check(string strValue,System.Action<string> _callback)
	{
		//GameObject Root_ui = GameObject.Find("root_window)"); //ui attach
		GameObject go = GameObject.Instantiate(Resources.Load("Prefabs/Window_Check"), Vector3.zero, Quaternion.identity) as GameObject;
		go.transform.parent = Root_ui.transform;
		go.transform.localPosition = Vector3.zero;
		go.transform.localRotation = Quaternion.identity;
		go.transform.localScale = Vector3.one;

		CWindowCheck w = go.GetComponent<CWindowCheck>();
		w.Show(strValue, _callback);
	}

	public void Window_Goblin_yesno(string strTitle, string strValue,Sprite _spriteGoods, System.Action<string> _callback)
	{
		//GameObject Root_ui = GameObject.Find("root_window)"); //ui attach
		GameObject go = GameObject.Instantiate(Resources.Load("Prefabs/Window_Goblin_Buff_Yes_No"), Vector3.zero, Quaternion.identity) as GameObject;
		go.transform.parent = Root_ui.transform;
		go.transform.localPosition = Vector3.zero;
		go.transform.localRotation = Quaternion.identity;
		go.transform.localScale = Vector3.one;
	}
    #endregion
}

