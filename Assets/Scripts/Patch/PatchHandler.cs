using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ReadOnlys;


using Amazon;
using Amazon.CognitoSync;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.Runtime;
using Amazon.CognitoIdentity;
using Amazon.CognitoIdentity.Model;
using Amazon.CognitoSync.SyncManager;

public class PatchHandler : MonoBehaviour
{
    public LoadAssetBundle loadAssetBundle;
    public LoginManager loginManager;

    //현재 클라이언트의 버전
    float fClientVersion = 0f;

    private void Awake()
    {
        PlayerPrefs.SetFloat("Version", 1.3f);
        //Debug.Log("Current Version : " + PlayerPrefs.GetFloat("Version"));
        loginManager.LoginManager_Init();
        StartGetPlayersData();

    }

    public void StartGetPlayersData()
    {
        StartCoroutine(GetPlayerData());
    }


    public IEnumerator GetPlayerData()
    {
        yield return null;

        DBPatchVersion_ForGet version = null;

        //Load Table Info
        loginManager.Context.LoadAsync<DBPatchVersion_ForGet>(0, (result) => {
            if (result.Exception == null)
            {
                version = result.Result as DBPatchVersion_ForGet;
                // Update few properties.

                Debug.Log("Version : " + version.Version + "\n");
                //GetCharacter= character;

                //버전에 대한 정보
                //없으면 해당 버전에 대한 정보를 만든다.
                if (PlayerPrefs.HasKey("Version") == false)
                    PlayerPrefs.SetFloat("Version", 1.0f);
                //서버에서 버전 정보와 대조
                //클라에 있는 버전이랑 서버에 있는 버전이랑 비교 체크
                
                fClientVersion = PlayerPrefs.GetFloat("Version");

                //서로 둘다 다르면 다시 다운로드를 한다.
                if (fClientVersion != version.Version)
                {
                    Debug.Log("버전이 다름으로 해당하는 에셋과 데이터를 다운로드");
                    //에셋 데이터 로드앤 체크
                    //loadAssetBundle.StartDownLoadAssetData();
                    //해당버전은 데이터가 다완료된 후에 해준다.
                    GameManager.Instance.fVersionInfo = version.Version;
                    GameManager.Instance.bIsPatch = true;
                    StartGetPatchCount();
                }
                else
                {
                    Debug.Log("에셋과 데이터를 체크 ");
                    GameManager.Instance.bIsPatch = false;
                    GameManager.Instance.bIsFinishLoadAssetData = true;
                }
            }
        });
    }

    public void StartGetPatchCount()
    {
        StartCoroutine(GetPatchCount());
    }

    IEnumerator GetPatchCount()
    {
        yield return null;

        DBPatchVersion_ForGet version = null;

        //Load Table Info
        loginManager.Context.LoadAsync<DBPatchVersion_ForGet>(1, (result) =>
        {
            if (result.Exception == null)
            {
                version = result.Result as DBPatchVersion_ForGet;
                // Update few properties.
                GameManager.Instance.nPatchAssetCount = version.Version;

                version = null;
            }
        });


        //Load Table Info
        loginManager.Context.LoadAsync<DBPatchVersion_ForGet>(2, (result) =>
        {
            if (result.Exception == null)
            {
                version = result.Result as DBPatchVersion_ForGet;
                // Update few properties.
                GameManager.Instance.nPatchDataCount = version.Version;

                version = null;
            }
        });

        while(true)
        {
            loginManager.loginState_Text.text = "패치 데이터를 가져오는중....";
            //Debug.Log("패치 데이터를 가져오는중....");
            if(GameManager.Instance.nPatchAssetCount != 0 && GameManager.Instance.nPatchDataCount != 0)
            {
                loginManager.StartGetPatchInfo();
                break;
            }
            yield return null;
        }

    }
}


[DynamoDBTable("DBPatchVersion")]
public class DBPatchVersion_ForGet
{
    [DynamoDBHashKey]
    public int Index { get; set; }                              // Hash key.

    [DynamoDBProperty("Version")]
    public float Version { get; set; }

}
