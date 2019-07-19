using UnityEngine;
using System.Collections;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using ChilliConnect;
using System.IO;
using System.IO.Compression;
using System.Linq;
using UnityEditor;
using UnityEngine.UI;


public class StartOptions : MonoBehaviour {

	//name of the ZipPackage Key to look for
	const string UNITY_ASSETS_PACKAGE_KEY = "UNITYASSETS";

	//name of the AssetBundle to load
	const string ASSET_BUNDLE_NAME = "logoassetbundle";

	//specific to this code implementation - logo to look for in the assetbundle and to display on screen
	const string LOGO_TO_LOAD = "chilliConnectLogo.png";
	//const string LOGO_TO_LOAD = "unityLogo.png";

	//game token retrieved from dashboard
	const string GAME_TOKEN = "hdoWIGtMiUYYt9Rz2cdxHjsK0pXylDuI";

	public int sceneToStart = 1;										//Index number in build settings of scene to load if changeScenes is true
	public bool changeScenes;											//If true, load a new scene when Start is pressed, if false, fade out UI and continue in single scene
	public bool changeMusicOnStart;										//Choose whether to continue playing menu music or start a new music clip

	[HideInInspector] public bool inMainMenu = true;					//If true, pause button disabled in main menu (Cancel in input manager, default escape key)
	[HideInInspector] public Animator animColorFade; 					//Reference to animator which will fade to and from black when starting game.
	[HideInInspector] public Animator animMenuAlpha;					//Reference to animator that will fade out alpha of MenuPanel canvas group
	 public AnimationClip fadeColorAnimationClip;		//Animation clip fading to color (black default) when changing scenes
	[HideInInspector] public AnimationClip fadeAlphaAnimationClip;		//Animation clip fading out UI elements alpha

	private PlayMusic playMusic;										//Reference to PlayMusic script
	private float fastFadeIn = .01f;									//Very short fade time (10 milliseconds) to start playing music immediately without a click/glitch
	private ShowPanels showPanels;										//Reference to ShowPanels script on UI GameObject, to show and hide panels

	private ChilliConnectSdk chilliConnect;
	private string ChilliConnectId;
	private string ChilliConnectSecret;

	void Awake()
	{
		//Get a reference to ShowPanels attached to UI object
		showPanels = GetComponent<ShowPanels> ();

		//Get a reference to PlayMusic attached to UI object
		playMusic = GetComponent<PlayMusic> ();

		playMusic = GetComponent<PlayMusic> ();

		//setup ChilliConnect SDK with game token and verbose logging
		chilliConnect = new ChilliConnectSdk( GAME_TOKEN, true);
		ChilliConnectId = PlayerPrefs.GetString ("ChilliConnectId");
		ChilliConnectSecret = PlayerPrefs.GetString ("ChilliConnectSecret");
		UnityEngine.Debug.Log("Loaded from PlayerPrefs ChilliconnectId: "+ChilliConnectId+" ChilliConnectSecret: "+ChilliConnectSecret);

		//if player not created
		if (ChilliConnectId.Length == 0) {
			CreateAndLoginPlayer ();
		} else {
			LoginPlayer ();
		}
	}

	void CreateAndLoginPlayer()
	{
		System.Action<CreatePlayerRequest, CreatePlayerResponse> successCallback = (CreatePlayerRequest request, CreatePlayerResponse response) =>
		{
			ChilliConnectId = response.ChilliConnectId;
			ChilliConnectSecret = response.ChilliConnectSecret;

			//persist identifiers
			PlayerPrefs.SetString("ChilliConnectId", ChilliConnectId);
			PlayerPrefs.SetString("ChilliConnectSecret", ChilliConnectSecret);

			UnityEngine.Debug.Log("Player created with ChilliConnectId: " + ChilliConnectId + " ChilliSecret: " + ChilliConnectSecret);

			LoginPlayer();
		};

		System.Action<CreatePlayerRequest, CreatePlayerError> errorCallback = (CreatePlayerRequest request, CreatePlayerError error) =>
		{
			UnityEngine.Debug.Log("An error occurred while creating a new player: " + error.ErrorDescription);
		};

		var requestDesc = new CreatePlayerRequestDesc();
		chilliConnect.PlayerAccounts.CreatePlayer(requestDesc, successCallback, errorCallback);
	}

	void LoginPlayer()
	{
		System.Action<LogInUsingChilliConnectRequest, LogInUsingChilliConnectResponse> successCallback = (LogInUsingChilliConnectRequest request, LogInUsingChilliConnectResponse response) =>
		{
			UnityEngine.Debug.Log("Player logged in");
		};

		System.Action<LogInUsingChilliConnectRequest, LogInUsingChilliConnectError> errorCallback = (LogInUsingChilliConnectRequest request, LogInUsingChilliConnectError error) =>
		{
			UnityEngine.Debug.Log("An error occurred while logging in: " + error.ErrorDescription);
		};
		var requestDesc = new LogInUsingChilliConnectRequestDesc(ChilliConnectId, ChilliConnectSecret);
		chilliConnect.PlayerAccounts.LogInUsingChilliConnect(requestDesc, successCallback, errorCallback);
	}

	// Main entry point of the demo code
	public void StartButtonClicked()
	{
		var assetExists = CheckIfAssetExists();
		if(!assetExists){
			CheckAndDownloadDLCPackage();
		}

		//If changeMusicOnStart is true, fade out volume of music group of AudioMixer by calling FadeDown function of PlayMusic, using length of fadeColorAnimationClip as time. 
		//To change fade time, change length of animation "FadeToColor"
		if (changeMusicOnStart)
		{
			playMusic.FadeDown(fadeColorAnimationClip.length);
		}

		//If changeScenes is true, start fading and change scenes halfway through animation when screen is blocked by FadeImage
		if (changeScenes) 
		{
			//Use invoke to delay calling of LoadDelayed by half the length of fadeColorAnimationClip
			Invoke ("LoadDelayed", fadeColorAnimationClip.length * .5f);

			//Set the trigger of Animator animColorFade to start transition to the FadeToOpaque state.
			animColorFade.SetTrigger ("fade");
		}
		//If changeScenes is false, call StartGameInScene
		else 
		{
			//Call the StartGameInScene function to start game without loading a new scene.
			StartGameInScene();
		}
	}

	public bool CheckIfAssetExists()
	{
		if (System.IO.File.Exists(Path.Combine(Application.dataPath, "AssetBundles") + "/" + ASSET_BUNDLE_NAME))
		{
			LoadAndDisplayAssetLogo();
			return true;
		}

		return false;
	}

	//The main body of code that handles the download of the Zip file definitions
	void CheckAndDownloadDLCPackage()
	{
		System.Action<GetZipPackageDefinitionsRequest, GetZipPackageDefinitionsResponse> successCallback = (GetZipPackageDefinitionsRequest request, GetZipPackageDefinitionsResponse response) =>
		{
			UnityEngine.Debug.Log("Successfully retreieved list of ZipPackages");

			//upon successful call to chilliconnect iterate through all objects
			foreach(ZipPackageDefinition packageItem in response.Items){

				//select which package to extract based on const defined above.
				if(packageItem.Key == UNITY_ASSETS_PACKAGE_KEY){

					var packages = packageItem.Packages;
					//extract the Url of the needed package and download
					WWW www = new WWW(packageItem.Packages["Default"].Url);
         
					while (!www.isDone) {
						UnityEngine.Debug.Log("Still Downloading...");
					}
						
					//where to save the downloaded zip file 
					string fullPath = Application.persistentDataPath + "/asset_files.zip";
					File.WriteAllBytes (fullPath, www.bytes);
						
					UnityEngine.Debug.Log("Fully Downloaded");

					//decompress the downloaded Zip file
					DecompressToAssets(fullPath);

					LoadAndDisplayAssetLogo();
				}
			}		
		};

		System.Action<GetZipPackageDefinitionsRequest, GetZipPackageDefinitionsError> errorCallback = (GetZipPackageDefinitionsRequest request, GetZipPackageDefinitionsError error) =>
		{
			UnityEngine.Debug.Log("An error occurred while getting the DLC package from server: " + error.ErrorDescription);
		};

		//Make a call to ChilliConnect and get back all published ZipPackages
		var requestDesc = new GetZipPackageDefinitionsRequestDesc();
		chilliConnect.Catalog.GetZipPackageDefinitions(requestDesc, successCallback, errorCallback);
	}

	public static void DecompressToAssets(string zipFilePath)
    {
		// Normalizes the path.
        var extractPath = Path.GetFullPath(Path.Combine(Application.dataPath, "AssetBundles"));

        using (ZipArchive archive = ZipFile.OpenRead(zipFilePath))
        {
            foreach (ZipArchiveEntry entry in archive.Entries)
            {
				// Gets the full path to ensure that relative segments are removed.
				string destinationPath = Path.GetFullPath(Path.Combine(extractPath, entry.FullName));
				entry.ExtractToFile(destinationPath);
            }
        }
    }

	public static void LoadAndDisplayAssetLogo()
    {
       //Take the Asset Bundle Name from the consts defined at the top of the script and load into an AssetBundle object
		var myLoadedAssetBundle = AssetBundle.LoadFromFile(Path.Combine(Application.dataPath, "AssetBundles") + "/" + ASSET_BUNDLE_NAME);

		if (myLoadedAssetBundle == null) {
			Debug.Log("Failed to load AssetBundle!");
			return;
		}

		Debug.Log("Asset Bundle Successfully Loaded.");	

		//Load the wanted asset from the bundle object file and apply to a GameObject titled "Logo" on screen.
		var asset = myLoadedAssetBundle.LoadAssetAsync<Texture2D>(LOGO_TO_LOAD);
		Texture2D loadedAsset = asset.asset as Texture2D;

		GameObject rawImage = GameObject.Find("Logo");
		rawImage.GetComponent<RawImage>().texture = loadedAsset;
    }

	//Once the level has loaded, check if we want to call PlayLevelMusic
	void OnLevelWasLoaded()
	{
		//if changeMusicOnStart is true, call the PlayLevelMusic function of playMusic
		if (changeMusicOnStart)
		{
			playMusic.PlayLevelMusic ();
		}	
	}

	public void LoadDelayed()
	{
		//Pause button now works if escape is pressed since we are no longer in Main menu.
		inMainMenu = false;

		//Hide the main menu UI element
		showPanels.HideMenu ();

		//Load the selected scene, by scene index number in build settings
		SceneManager.LoadScene (sceneToStart);
	}

	public void HideDelayed()
	{
		//Hide the main menu UI element after fading out menu for start game in scene
		showPanels.HideMenu();
	}

	public void StartGameInScene()
	{
		//Pause button now works if escape is pressed since we are no longer in Main menu.
		inMainMenu = false;

		//If changeMusicOnStart is true, fade out volume of music group of AudioMixer by calling FadeDown function of PlayMusic, using length of fadeColorAnimationClip as time. 
		//To change fade time, change length of animation "FadeToColor"
		if (changeMusicOnStart) 
		{
			//Wait until game has started, then play new music
			Invoke ("PlayNewMusic", fadeAlphaAnimationClip.length);
		}
		//Set trigger for animator to start animation fading out Menu UI
		animMenuAlpha.SetTrigger ("fade");
		Invoke("HideDelayed", fadeAlphaAnimationClip.length);
		Debug.Log ("Game started in same scene! Put your game starting stuff here.");
	}


	public void PlayNewMusic()
	{
		//Fade up music nearly instantly without a click 
		playMusic.FadeUp (fastFadeIn);
		//Play music clip assigned to mainMusic in PlayMusic script
		playMusic.PlaySelectedMusic (1);
	}
}
