using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;
using ReadOnlys;

public class LoadAssetBundle : MonoBehaviour 
{

    public LoginManager loginManager;
  

    public void StartDownLoadAssetData()
    {
        //Debug.Log("Start LoadAsset");
        StartCoroutine(DownLoadAssetDataAndSave(E_CHECK_ASSETDATA.PREFABIMAGES));
        StartCoroutine(ShowCurrentLoadAssetState());
    }
    
    IEnumerator ShowCurrentLoadAssetState()
    {
        int nPotCount = 0;
        string text = "에셋 데이터 불러오는중";
        while (GameManager.Instance.bIsFinishLoadAssetData == false)
        {
          
            yield return new WaitForSeconds(0.2f);

            if (nPotCount == 0)
            {
                loginManager.loginState_Text.text = text + ".";
                nPotCount++;
            }
            else if (nPotCount == 1)
            {
                loginManager.loginState_Text.text = text + "..";
                nPotCount++;
            }
            else
            {
                loginManager.loginState_Text.text = text + "...";
                nPotCount = 0;
            }
        }
    }

	IEnumerator DownLoadAssetDataAndSave(E_CHECK_ASSETDATA _assetData)
	{
		string assetBundleDirectory_Android = Application.persistentDataPath;
        string uri = "";

        //PatchAsset 비교
       
        switch (_assetData)
        { 
            case E_CHECK_ASSETDATA.PREFABIMAGES:

                //0이면 새로운 데이터 안받아도됨
                if (GameManager.Instance.lDBPatchAsset[(int)_assetData].nChanged == 0)
                {
                    //다음 데이터 체크
                    StartCoroutine(DownLoadAssetDataAndSave(E_CHECK_ASSETDATA.PREFABS));
                    break;
                }
                else
                {
                    uri = "https://s3.ap-northeast-2.amazonaws.com/whoopsmercenarydlc/AssetBundle/mainscene/images";
                    // 에셋 번들을 저장할 경로의 폴더가 존재하지 않는다면 생성시킨다.

                    if (!Directory.Exists(assetBundleDirectory_Android + "/AssetBundle/MainScene"))
                        Directory.CreateDirectory(assetBundleDirectory_Android + "/AssetBundle/MainScene");

                    //받아온 데이터 체크
                    UnityWebRequest request_mainsceneImage = UnityWebRequest.Get(uri);
                    yield return request_mainsceneImage.Send();

                    // 파일 입출력을 통해 받아온 에셋을 저장하는 과정
                    //해당하는 경로에 없으면 새로 만든다.
                    if (!File.Exists(assetBundleDirectory_Android + "/AssetBundle/MainScene/images"))
                    {
                        FileStream fs_mainsceneImage = new FileStream(assetBundleDirectory_Android + "/AssetBundle/MainScene/images", System.IO.FileMode.Create);
                        fs_mainsceneImage.Write(request_mainsceneImage.downloadHandler.data, 0, (int)request_mainsceneImage.downloadedBytes);
                        fs_mainsceneImage.Close();
                    }
                    //만약 경로에 있다면 해당 파일을 지우고 다시 만든다.
                    else
                    {
                        File.Delete(assetBundleDirectory_Android + "/AssetBundle/MainScene/images");

                        FileStream fs_mainsceneImage = new FileStream(assetBundleDirectory_Android + "/AssetBundle/MainScene/images", System.IO.FileMode.Create);
                        fs_mainsceneImage.Write(request_mainsceneImage.downloadHandler.data, 0, (int)request_mainsceneImage.downloadedBytes);
                        fs_mainsceneImage.Close();
                    }

                    StartCoroutine(DownLoadAssetDataAndSave(E_CHECK_ASSETDATA.PREFABS));
                }
                
                break;
            case E_CHECK_ASSETDATA.PREFABS:
                uri = "https://s3.ap-northeast-2.amazonaws.com/whoopsmercenarydlc/AssetBundle/mainscene/prefabs";

                //만약 경로가 없으면 해당 경로에 폴더를 만든다.
                if (!Directory.Exists(assetBundleDirectory_Android + "/AssetBundle/MainScene"))
                    Directory.CreateDirectory(assetBundleDirectory_Android + "/AssetBundle/MainScene");


                //0이면 새로운 데이터 안받아도됨
                if (GameManager.Instance.lDBPatchAsset[(int)_assetData].nChanged == 0)
                {
                    //다음 데이터 체크
                    StartCoroutine(DownLoadAssetDataAndSave(E_CHECK_ASSETDATA.POST_SLOTS));
                    break;
                }
               
                UnityWebRequest request_mainscenePrefabs = UnityWebRequest.Get(uri);
                yield return request_mainscenePrefabs.Send();

                // 파일 입출력을 통해 받아온 에셋을 저장하는 과정
                //request.
                if (!File.Exists(assetBundleDirectory_Android + "/AssetBundle/MainScene/prefabs"))
                {
                    FileStream fs_mainscenePrefabs = new FileStream(assetBundleDirectory_Android + "/AssetBundle/MainScene/prefabs", System.IO.FileMode.Create);
                    fs_mainscenePrefabs.Write(request_mainscenePrefabs.downloadHandler.data, 0, (int)request_mainscenePrefabs.downloadedBytes);
                    fs_mainscenePrefabs.Close();
                }
                else
                {
                    File.Delete(assetBundleDirectory_Android + "/AssetBundle/MainScene/prefabs");

                    FileStream fs_mainsceneImage = new FileStream(assetBundleDirectory_Android + "/AssetBundle/MainScene/prefabs", System.IO.FileMode.Create);
                    fs_mainsceneImage.Write(request_mainscenePrefabs.downloadHandler.data, 0, (int)request_mainscenePrefabs.downloadedBytes);
                    fs_mainsceneImage.Close();
                }

                StartCoroutine(DownLoadAssetDataAndSave(E_CHECK_ASSETDATA.POST_SLOTS));
                break;
            case E_CHECK_ASSETDATA.POST_SLOTS:
                uri = "https://s3.ap-northeast-2.amazonaws.com/whoopsmercenarydlc/AssetBundle/mainscene/slot";
                if (!Directory.Exists(assetBundleDirectory_Android + "/AssetBundle/MainScene"))
                    Directory.CreateDirectory(assetBundleDirectory_Android + "/AssetBundle/MainScene");

                //0이면 새로운 데이터 안받아도됨
                if (GameManager.Instance.lDBPatchAsset[(int)_assetData].nChanged == 0)
                {
                    //다음 데이터 체크
                    StartCoroutine(DownLoadAssetDataAndSave(E_CHECK_ASSETDATA.EMPLOY_CHARACTERS));
                    break;
                }

                UnityWebRequest request_mainsceneSlot = UnityWebRequest.Get(uri);
                yield return request_mainsceneSlot.Send();

                // 파일 입출력을 통해 받아온 에셋을 저장하는 과정
                //request.
                if (!File.Exists(assetBundleDirectory_Android + "/AssetBundle/MainScene/slot"))
                {
                    FileStream fs_mainsceneSlot = new FileStream(assetBundleDirectory_Android + "/AssetBundle/MainScene/slot", System.IO.FileMode.Create);
                    fs_mainsceneSlot.Write(request_mainsceneSlot.downloadHandler.data, 0, (int)request_mainsceneSlot.downloadedBytes);
                    fs_mainsceneSlot.Close();
                }

                else
                {
                    File.Delete(assetBundleDirectory_Android + "/AssetBundle/MainScene/slot");

                    FileStream fs_mainsceneImage = new FileStream(assetBundleDirectory_Android + "/AssetBundle/MainScene/slot", System.IO.FileMode.Create);
                    fs_mainsceneImage.Write(request_mainsceneSlot.downloadHandler.data, 0, (int)request_mainsceneSlot.downloadedBytes);
                    fs_mainsceneImage.Close();
                }
                //다음 데이터 체크
                StartCoroutine(DownLoadAssetDataAndSave(E_CHECK_ASSETDATA.EMPLOY_CHARACTERS));
                break;
            case E_CHECK_ASSETDATA.EMPLOY_CHARACTERS:
                uri = "https://s3.ap-northeast-2.amazonaws.com/whoopsmercenarydlc/AssetBundle/mainscene/employ/characters";

                if (!Directory.Exists(assetBundleDirectory_Android + "/AssetBundle/MainScene/Employ"))
                    Directory.CreateDirectory(assetBundleDirectory_Android + "/AssetBundle/MainScene/Employ");

                //0이면 새로운 데이터 안받아도됨
                if (GameManager.Instance.lDBPatchAsset[(int)_assetData].nChanged == 0)
                {
                    //다음 데이터 체크
                    StartCoroutine(DownLoadAssetDataAndSave(E_CHECK_ASSETDATA.EMPLOY_IMAGES));
                    break;
                }

                UnityWebRequest request_mainsceneEmploy_Character = UnityWebRequest.Get(uri);
                yield return request_mainsceneEmploy_Character.Send();

                // 파일 입출력을 통해 받아온 에셋을 저장하는 과정
                //request.
                if (!File.Exists(assetBundleDirectory_Android + "/AssetBundle/MainScene/Employ/characters"))
                {
                    FileStream fs_mainsceneEmploy_Character = new FileStream(assetBundleDirectory_Android + "/AssetBundle/MainScene/Employ/characters", System.IO.FileMode.Create);
                    fs_mainsceneEmploy_Character.Write(request_mainsceneEmploy_Character.downloadHandler.data, 0, (int)request_mainsceneEmploy_Character.downloadedBytes);
                    fs_mainsceneEmploy_Character.Close();
                }
                else
                {
                    File.Delete(assetBundleDirectory_Android + "/AssetBundle/MainScene/characters");

                    FileStream fs_mainsceneEmploy_Character = new FileStream(assetBundleDirectory_Android + "/AssetBundle/MainScene/Employ/characters", System.IO.FileMode.Create);
                    fs_mainsceneEmploy_Character.Write(request_mainsceneEmploy_Character.downloadHandler.data, 0, (int)request_mainsceneEmploy_Character.downloadedBytes);
                    fs_mainsceneEmploy_Character.Close();

                }
                //다음 데이터 체크
                StartCoroutine(DownLoadAssetDataAndSave(E_CHECK_ASSETDATA.EMPLOY_IMAGES));
                break;

            case E_CHECK_ASSETDATA.EMPLOY_IMAGES:
                uri = "https://s3.ap-northeast-2.amazonaws.com/whoopsmercenarydlc/AssetBundle/mainscene/employ/images";
                if (!Directory.Exists(assetBundleDirectory_Android + "/AssetBundle/MainScene/Employ"))
                    Directory.CreateDirectory(assetBundleDirectory_Android + "/AssetBundle/MainScene/Employ");

                //0이면 새로운 데이터 안받아도됨
                if (GameManager.Instance.lDBPatchAsset[(int)_assetData].nChanged == 0)
                {
                    //다음 데이터 체크
                    StartCoroutine(DownLoadAssetDataAndSave(E_CHECK_ASSETDATA.INVENTORY_ITEM_WEAPONIMAGES));
                    break;
                }

                UnityWebRequest request_mainsceneEmploy_Images = UnityWebRequest.Get(uri);
                yield return request_mainsceneEmploy_Images.Send();

                // 파일 입출력을 통해 받아온 에셋을 저장하는 과정
                //request.
                if (!File.Exists(assetBundleDirectory_Android + "/AssetBundle/MainScene/Employ/images"))
                {
                    FileStream fs_mainsceneEmploy_Images = new FileStream(assetBundleDirectory_Android + "/AssetBundle/MainScene/Employ/images", System.IO.FileMode.Create);
                    fs_mainsceneEmploy_Images.Write(request_mainsceneEmploy_Images.downloadHandler.data, 0, (int)request_mainsceneEmploy_Images.downloadedBytes);
                    fs_mainsceneEmploy_Images.Close();
                }
                else
                {
                    File.Delete(assetBundleDirectory_Android + "/AssetBundle/MainScene/Employ/images");

                    FileStream fs_mainsceneEmploy_Images = new FileStream(assetBundleDirectory_Android + "/AssetBundle/MainScene/Employ/images", System.IO.FileMode.Create);
                    fs_mainsceneEmploy_Images.Write(request_mainsceneEmploy_Images.downloadHandler.data, 0, (int)request_mainsceneEmploy_Images.downloadedBytes);
                    fs_mainsceneEmploy_Images.Close();

                }
                //다음 데이터 체크
                StartCoroutine(DownLoadAssetDataAndSave(E_CHECK_ASSETDATA.INVENTORY_ITEM_WEAPONIMAGES));
                break;
            case E_CHECK_ASSETDATA.INVENTORY_ITEM_WEAPONIMAGES:
                uri = "https://s3.ap-northeast-2.amazonaws.com/whoopsmercenarydlc/AssetBundle/mainscene/inventory/item/images/weapon";
                if (!Directory.Exists(assetBundleDirectory_Android + "/AssetBundle/MainScene/Inventory/item/images"))
                    Directory.CreateDirectory(assetBundleDirectory_Android + "/AssetBundle/MainScene/Inventory/item/images");

                //0이면 새로운 데이터 안받아도됨
                if (GameManager.Instance.lDBPatchAsset[(int)_assetData].nChanged == 0)
                {
                    //다음 데이터 체크
                    StartCoroutine(DownLoadAssetDataAndSave(E_CHECK_ASSETDATA.INVENTORY_ITEM_ARMORIMAGES));
                    break;
                }

                UnityWebRequest request_mainsceneInventory_ItemWeapon = UnityWebRequest.Get(uri);
                yield return request_mainsceneInventory_ItemWeapon.Send();

                // 파일 입출력을 통해 받아온 에셋을 저장하는 과정
                //request.
                if (!File.Exists(assetBundleDirectory_Android + "/AssetBundle/MainScene/Inventory/item/images/weapon"))
                {
                    FileStream fs_mainsceneInventory_ItemWeapon = new FileStream(assetBundleDirectory_Android + "/AssetBundle/MainScene/Inventory/item/images/weapon", System.IO.FileMode.Create);
                    fs_mainsceneInventory_ItemWeapon.Write(request_mainsceneInventory_ItemWeapon.downloadHandler.data, 0, (int)request_mainsceneInventory_ItemWeapon.downloadedBytes);
                    fs_mainsceneInventory_ItemWeapon.Close();
                }

                else
                {
                    File.Delete(assetBundleDirectory_Android + "/AssetBundle/MainScene/Inventory/item/images/weapon");

                    FileStream fs_mainsceneInventory_ItemWeapon = new FileStream(assetBundleDirectory_Android + "/AssetBundle/MainScene/Inventory/item/images/weapon", System.IO.FileMode.Create);
                    fs_mainsceneInventory_ItemWeapon.Write(request_mainsceneInventory_ItemWeapon.downloadHandler.data, 0, (int)request_mainsceneInventory_ItemWeapon.downloadedBytes);
                    fs_mainsceneInventory_ItemWeapon.Close();
                }
                //다음 데이터 체크
                StartCoroutine(DownLoadAssetDataAndSave(E_CHECK_ASSETDATA.INVENTORY_ITEM_ARMORIMAGES));
                break;
            case E_CHECK_ASSETDATA.INVENTORY_ITEM_ARMORIMAGES:
                uri = "https://s3.ap-northeast-2.amazonaws.com/whoopsmercenarydlc/AssetBundle/mainscene/inventory/item/images/armor";
                if (!Directory.Exists(assetBundleDirectory_Android + "/AssetBundle/MainScene/Inventory/item/images"))
                    Directory.CreateDirectory(assetBundleDirectory_Android + "/AssetBundle/MainScene/Inventory/item/images");

                //0이면 새로운 데이터 안받아도됨
                if (GameManager.Instance.lDBPatchAsset[(int)_assetData].nChanged == 0)
                {
                    //다음 데이터 체크
                    StartCoroutine(DownLoadAssetDataAndSave(E_CHECK_ASSETDATA.INVENTORY_ITEM_GLOVEIMAGES));
                    break;
                }

                UnityWebRequest request_mainsceneInventory_ItemArmor = UnityWebRequest.Get(uri);
                yield return request_mainsceneInventory_ItemArmor.Send();

                // 파일 입출력을 통해 받아온 에셋을 저장하는 과정
                //request.
                if (!File.Exists(assetBundleDirectory_Android + "/AssetBundle/MainScene/Inventory/item/images/armor"))
                {
                    FileStream fs_mainsceneInventory_ItemArmor = new FileStream(assetBundleDirectory_Android + "/AssetBundle/MainScene/Inventory/item/images/armor", System.IO.FileMode.Create);
                    fs_mainsceneInventory_ItemArmor.Write(request_mainsceneInventory_ItemArmor.downloadHandler.data, 0, (int)request_mainsceneInventory_ItemArmor.downloadedBytes);
                    fs_mainsceneInventory_ItemArmor.Close();
                }

                else
                {
                    File.Delete(assetBundleDirectory_Android + "/AssetBundle/MainScene/Inventory/item/images/armor");

                    FileStream fs_mainsceneInventory_ItemArmor = new FileStream(assetBundleDirectory_Android + "/AssetBundle/MainScene/Inventory/item/images/armor", System.IO.FileMode.Create);
                    fs_mainsceneInventory_ItemArmor.Write(request_mainsceneInventory_ItemArmor.downloadHandler.data, 0, (int)request_mainsceneInventory_ItemArmor.downloadedBytes);
                    fs_mainsceneInventory_ItemArmor.Close();
                }
                //다음 데이터 체크
                StartCoroutine(DownLoadAssetDataAndSave(E_CHECK_ASSETDATA.INVENTORY_ITEM_GLOVEIMAGES));

                break;
            case E_CHECK_ASSETDATA.INVENTORY_ITEM_GLOVEIMAGES:
                uri = "https://s3.ap-northeast-2.amazonaws.com/whoopsmercenarydlc/AssetBundle/mainscene/inventory/item/images/glove";
                if (!Directory.Exists(assetBundleDirectory_Android + "/AssetBundle/MainScene/Inventory/item/images"))
                    Directory.CreateDirectory(assetBundleDirectory_Android + "/AssetBundle/MainScene/Inventory/item/images");

                //0이면 새로운 데이터 안받아도됨
                if (GameManager.Instance.lDBPatchAsset[(int)_assetData].nChanged == 0)
                {
                    //다음 데이터 체크
                    StartCoroutine(DownLoadAssetDataAndSave(E_CHECK_ASSETDATA.INVENTORY_ITEM_ACCESSORYIMAGES));
                    break;
                }

                UnityWebRequest request_mainsceneInventory_ItemGlove = UnityWebRequest.Get(uri);
                yield return request_mainsceneInventory_ItemGlove.Send();

                // 파일 입출력을 통해 받아온 에셋을 저장하는 과정
                //request.
                if (!File.Exists(assetBundleDirectory_Android + "/AssetBundle/MainScene/Inventory/item/images/glove"))
                {
                    FileStream fs_mainsceneInventory_ItemGlove = new FileStream(assetBundleDirectory_Android + "/AssetBundle/MainScene/Inventory/item/images/glove", System.IO.FileMode.Create);
                    fs_mainsceneInventory_ItemGlove.Write(request_mainsceneInventory_ItemGlove.downloadHandler.data, 0, (int)request_mainsceneInventory_ItemGlove.downloadedBytes);
                    fs_mainsceneInventory_ItemGlove.Close();
                }
                else
                {
                    File.Delete(assetBundleDirectory_Android + "/AssetBundle/MainScene/Inventory/item/images/glove");

                    FileStream fs_mainsceneInventory_ItemGlove = new FileStream(assetBundleDirectory_Android + "/AssetBundle/MainScene/Inventory/item/images/glove", System.IO.FileMode.Create);
                    fs_mainsceneInventory_ItemGlove.Write(request_mainsceneInventory_ItemGlove.downloadHandler.data, 0, (int)request_mainsceneInventory_ItemGlove.downloadedBytes);
                    fs_mainsceneInventory_ItemGlove.Close();
                }
                //다음 데이터 체크
                StartCoroutine(DownLoadAssetDataAndSave(E_CHECK_ASSETDATA.INVENTORY_ITEM_ACCESSORYIMAGES));
                break;
            case E_CHECK_ASSETDATA.INVENTORY_ITEM_ACCESSORYIMAGES:
                uri = "https://s3.ap-northeast-2.amazonaws.com/whoopsmercenarydlc/AssetBundle/mainscene/inventory/item/images/accessory";
                if (!Directory.Exists(assetBundleDirectory_Android + "/AssetBundle/MainScene/Inventory/item/images"))
                    Directory.CreateDirectory(assetBundleDirectory_Android + "/AssetBundle/MainScene/Inventory/item/images");

                //0이면 새로운 데이터 안받아도됨
                if (GameManager.Instance.lDBPatchAsset[(int)_assetData].nChanged == 0)
                {
                    loginManager.loginState_Text.text = "";
                    GameManager.Instance.bIsFinishLoadAssetData = true;
                    break;
                }

                UnityWebRequest request_mainsceneInventory_ItemAccessory = UnityWebRequest.Get(uri);
                yield return request_mainsceneInventory_ItemAccessory.Send();

                // 파일 입출력을 통해 받아온 에셋을 저장하는 과정
                //request.

                if (!File.Exists(assetBundleDirectory_Android + "/AssetBundle/MainScene/Inventory/item/images/accessory"))
                {
                    FileStream fs_mainsceneInventory_ItemAccessory = new FileStream(assetBundleDirectory_Android + "/AssetBundle/MainScene/Inventory/item/images/accessory", System.IO.FileMode.Create);
                    fs_mainsceneInventory_ItemAccessory.Write(request_mainsceneInventory_ItemAccessory.downloadHandler.data, 0, (int)request_mainsceneInventory_ItemAccessory.downloadedBytes);
                    fs_mainsceneInventory_ItemAccessory.Close();
                }
                else
                {
                    File.Delete(assetBundleDirectory_Android + "/AssetBundle/MainScene/Inventory/item/images/accessory");

                    FileStream fs_mainsceneInventory_ItemAccessory = new FileStream(assetBundleDirectory_Android + "/AssetBundle/MainScene/Inventory/item/images/accessory", System.IO.FileMode.Create);
                    fs_mainsceneInventory_ItemAccessory.Write(request_mainsceneInventory_ItemAccessory.downloadHandler.data, 0, (int)request_mainsceneInventory_ItemAccessory.downloadedBytes);
                    fs_mainsceneInventory_ItemAccessory.Close();

                   
                }
                loginManager.loginState_Text.text = "";
                GameManager.Instance.bIsFinishLoadAssetData = true;
                break;
            default:
                break;
        }
        Debug.Log ("다운완료!");
	}
}
