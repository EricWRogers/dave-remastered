using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public TextMeshPro Text;
    public TextMeshPro pointReq;
    public bool gameModeTutorial = true;
    public bool gameModeDefault = false;
    public bool gameModeSandbox = false;
    public bool gameModeMilitary = false;
    public int pointLimit = 0;
    public int buildingCount = 0;
    public Animator anim;
    public Animator dooranim;
    private GameObject door;

    public bool startAI = false;
    public bool canLoad = true;

    public enum LevelState {DEFAULT, SANDBOX, MILITARY, TUTORIAL}
    //Default mode is just the timer counting down, which is the lose condition. You win by meeting the point threshold ie destroy all the buildings.
    //Sandbox mode is simply playing for as long as you want with enemies and buildings respawning every so often. Lose condition is your health reaching 0.
    //Military mode is the timer counting down and health reaching 0 via enemy ai for the lose condition, and all buildings being destroyed as the win condition.
    //Tutorial will transport us to the tutorial scene.
    public LevelState state;
    
    private GameObject player;
    private GameObject playerbody;
    private GameObject cageAnim;
    private GameObject cage;
    private OVRPlayerController OPC;
    private PointManager PM;
    private Health HH;
    private TimerDown TD;
    private TimerUp TU;
    public string sceneName;

    // Start is called before the first frame update
    void Start()
    {
        
        if(state == LevelState.MILITARY)
        {
            pointLimit += pointLimit + (buildingCount + 200); //Change this up as needed. idk what a balanced point cap is. I can beat the level in less than 5 min.
        }

        if(state == LevelState.DEFAULT)
        {
            pointLimit = pointLimit + buildingCount -3; //Change this up as needed. idk what a balanceed point cap is. I can beat the level in less than 5 min.
        }

            //assigning all of that important information the Gamemanager needs.
            player = GameObject.Find("Player");
            cage = GameObject.Find("Player/CageAnimation/Cage");
        if (state != LevelState.TUTORIAL) //So long we are not in the tutorial scene.
        {
            playerbody = GameObject.Find("Player/PlayerBody");
            cageAnim = GameObject.Find("Player/CageAnimation");
            door = GameObject.Find("Cage/Door");
            dooranim = door.GetComponent<Animator>();
            OPC = player.GetComponent<OVRPlayerController>(); //Gathering all of the needed variables.
            PM = GetComponent<PointManager>();
            HH = playerbody.GetComponent<Health>();
            TD = GetComponent<TimerDown>();
            TU = GetComponent<TimerUp>();
            
        }

        Scene currentScene = SceneManager.GetActiveScene(); //Get current scene 
        sceneName = currentScene.name; //Convert to string
        if(sceneName == "LogansTutorial") //Setting game state appropriatly.
        {
            gameModeDefault = false;
            gameModeMilitary = false;
            gameModeSandbox = false;
            gameModeTutorial = true;
            Text.text = "Play: Tutorial";
            state = LevelState.TUTORIAL;
            cage.SetActive(false);
        }

        if(sceneName == "RL_LevelDesignTest 1") //Setting game state appropriatly.
        {
            pointReq.text = "Target: " + pointLimit;
            gameModeDefault = true;
            gameModeMilitary = false;
            gameModeSandbox = false;
            gameModeTutorial = false;
            Text.text = "Play: Default";
            state = LevelState.DEFAULT;
            cage.SetActive(false);
            TU.enabled = false;
            TD.enabled = true;
        }

        if(sceneName == "SandboxLevel") //Setting game state appropriatly.
        {
            
            gameModeDefault = false;
            gameModeMilitary = false;
            gameModeSandbox = true;
            gameModeTutorial = false;
            Text.text = "Play: SandBox";
            state = LevelState.SANDBOX;
            cage.SetActive(false);
            TD.enabled = false;
            TU.enabled = true;
        }

        if(sceneName == "MilitaryLevel") //Setting game state appropriatly.
        {
            pointReq.text = "Target: " + pointLimit;
            gameModeDefault = false;
            gameModeMilitary = true;
            gameModeSandbox = false;
            gameModeTutorial = false;
            Text.text = "Play: Military";
            state = LevelState.MILITARY;
            cage.SetActive(false);
            TU.enabled = false;
            TD.enabled = true;
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        if(state != LevelState.TUTORIAL)
        {
            
            if(anim == null) //This is redundant but i am pretty sure it is here for a reason -logan
            {
                anim = cageAnim.GetComponent<Animator>(); //we constantly try and find an animator.
            }

            if(state != LevelState.SANDBOX && PM.score >= 85 || HH.currentHealth <= 0 || TD.timerCount <=0) //If we hit the point limit or if we die for mil and def
            {
                ResetPlayer();
                /*//redundantly rest all of our values. Just in case.
                PM.score = 0;
                HH.currentHealth = 100;
                TD.timerCount = 200;
                TU.timer = 0;
                */
            }

            if(state == LevelState.SANDBOX && HH.currentHealth <= 0 ) // if we die for sandbox.
            {
                ResetPlayer();

            }
        }

        if(player.transform.position.y <= -1) //Checks to make sure the player is not falling into the void.
        {
            ResetPlayer();
        }

       
    }

     async public void ResetPlayer()
    {
        if(canLoad)
        {
            canLoad = false;
            cage.SetActive(true); //Showing the cage
            OPC.enabled = !OPC.enabled; //Disables player movement
            anim.SetBool("Start", true); //Dropping the cage


            await Task.Delay(4000); //Delays the following lines of code.
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); //reloads the current scene
            canLoad = true;
        }
     }

    //Actually starts the game
   

    public void Tutorial()
    {
        if(state != LevelState.TUTORIAL) 
        {
            state = LevelState.TUTORIAL;
            SceneManager.LoadScene("LogansTutorial"); //Change according to the appropriate scene name
        }
    }

    //These are currently relying on button presses.
    //They are going to change scenes for us and ensure we are in the correct game state.
    public void ToMode()
    {
        if(gameModeDefault == true)//Launches the functions based off what level the player choice.
        {
            if(state != LevelState.DEFAULT)
            {
            state = LevelState.DEFAULT;
            SceneManager.LoadScene("RL_LevelDesignTest 1"); //Change according to the appropriate scene name
            }
            if(state == LevelState.DEFAULT)
            {
                dooranim.SetBool("Open", true);
                startAI = true;
            }
        }
        if(gameModeSandbox == true)
        {
            
            if(state != LevelState.SANDBOX)
            {
            state = LevelState.SANDBOX;
           SceneManager.LoadScene("SandboxLevel"); //Change according to the appropriate scene name
            }
            if(state == LevelState.SANDBOX)
            {
                dooranim.SetBool("Open", true);
                startAI = true;
            }
        }
        if(gameModeMilitary == true)
        {
            if(state != LevelState.MILITARY) 
            {
            state = LevelState.MILITARY;
            SceneManager.LoadScene("MilitaryLevel"); //Change according to the appropriate scene name
            }
            if(state == LevelState.MILITARY)
            {
                dooranim.SetBool("Open", true);
                startAI = true;
            }
        }

        if(gameModeTutorial == true)
        {
            if(state != LevelState.TUTORIAL) 
            {
            state = LevelState.TUTORIAL;
            SceneManager.LoadScene("LogansTutorial"); //Change according to the appropriate scene name
            }
            if(state == LevelState.TUTORIAL)
            {
                //dooranim.SetBool("Open", true);
                startAI = true;
            }
        }
    }

    public void modeSelectD()
    {
        gameModeDefault = true;
        gameModeMilitary = false;
        gameModeSandbox = false;
        gameModeTutorial = false;
        Text.text = "Play: Default";
    }
    public void modeSelectM()
    {
        gameModeMilitary = true;
        gameModeDefault = false;
        gameModeSandbox = false;
        gameModeTutorial = false;
        Text.text = "Play: Military";
    }
    public void modeSelectS()
    {
        gameModeSandbox = true;
        gameModeMilitary = false;
        gameModeDefault = false;
        gameModeTutorial = false;
        Text.text = "Play: Sandbox";
    }
    public void modeSelectT()
    {
        gameModeTutorial = true;
        gameModeMilitary = false;
        gameModeSandbox = false;
        gameModeDefault = false;
        Text.text = "Play: Tutorial";
    }
}
