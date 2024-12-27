using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static UnityEngine.InputSystem.InputAction;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;

    [SerializeField] private EventSystem eventSystem;
    [SerializeField] private PauseMenu pauseMenu;
    [SerializeField] private InventoriesManager inventoriesManager;
    [SerializeField] private DialogueManager dialogueManager;

    private Options OptionsMenu;

    [SerializeField] private PlayerInput playerInput;

    private string priorActionMap = "Player";

    [HideInInspector] public bool inMenu = false;
    [HideInInspector] public bool isPaused = false;

    public MenuState menuState = MenuState.playing;

    private bool usingMnK = true;

    /*[SerializeField] private GameObject ui_Canvas;
    private GraphicRaycaster ui_raycaster;

    private PointerEventData click_data;
    private List<RaycastResult> click_results;*/

    private void Awake()
    {
        if (Instance != this)
        {
            Destroy(Instance);
            Instance = this;
        }

        if (inventoriesManager == null)
        {
            inventoriesManager = InventoriesManager.instance;
        }

        // inventoriesManager.CreateDemoInventory();

    }

    public void Start()
    {
        if (Options.Instance != null)
        {
            OptionsMenu = Options.Instance;

        }
        else { Debug.LogWarning("Options is not currently initialized.", this.gameObject); }

        StartCoroutine(DisablePauseMenuAtStart());
    }

    private IEnumerator DisablePauseMenuAtStart()
    {
        yield return new WaitForSeconds(.01f);

        //Debug.Log(playerInput.currentActionMap.name + " is current input map.");
        playerInput.SwitchCurrentActionMap("PauseMenu");
        //Debug.Log(playerInput.currentActionMap.name + " is current input map.");
        playerInput.SwitchCurrentActionMap("Player");
        //Debug.Log(playerInput.currentActionMap.name + " is current input map.");

    }

    public void usingMouseAndKeyboard()
    {
        if (!usingMnK)
        {
            //Debug.Log("Using Mouse and Keyboard");
            usingMnK = true;
            if (inMenu || isPaused)
            {
                showMouse();
            }
        }
    }

    public void usingGamepad()
    {
        if (usingMnK)
        {
            //Debug.Log("Using Gamepad");
            usingMnK = false;
            if (inMenu || isPaused)
            {
                hideMouse();
            }
        }
    }

    private void showMouse()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    private void hideMouse()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void openMenu()
    {
        if (!inMenu)
        {
            inMenu = true;
            if (usingMnK)
            {
                showMouse();
            }
        }
    }

    public void closeMenu()
    {
        if (inMenu)
        {
            inMenu = false;
            if (usingMnK)
            {
                hideMouse();
            }
        }
    }

    public void openPauseMenu()
    {
        //Debug.Log("Opening pause menu by " + playerInput.currentActionMap.name + ".");
        if (!isPaused)
        {
            menuState = MenuState.pauseMenus;
            isPaused = true;
            EventSystem.current.SetSelectedGameObject(pauseMenu.resumeButton.gameObject);
            priorActionMap = playerInput.currentActionMap.name;
            playerInput.SwitchCurrentActionMap("PauseMenu");
            Time.timeScale = 0;
            pauseMenu.gameObject.SetActive(true);
            if (usingMnK)
            {
                showMouse();
            }
        }
    }

    public void closePauseMenu()
    {
        //Debug.Log("Closing pause menu by " + playerInput.currentActionMap.name + ".");
        if (isPaused)
        {
            menuState = MenuState.playing;
            pauseMenu.gameObject.SetActive(false);
            Time.timeScale = 1;
            playerInput.SwitchCurrentActionMap(priorActionMap);
            isPaused = false;
            if (usingMnK && !inMenu)
            {
                hideMouse();
            }
        }
    }

    public void togglePauseMenu()
    {
        if (isPaused)
        {
            closePauseMenu();
        }
        else
        {
            openPauseMenu();
        }
    }

    public void openPauseMenu(CallbackContext callbackContext)
    {
        //Debug.Log((playerInput.currentActionMap == null ? "null" : playerInput.currentActionMap.name) + ": Open pause menu context: " + callbackContext.phase);
        if (callbackContext.performed)
        {
            openPauseMenu();
        }
    }

    public void closePauseMenu(CallbackContext callbackContext)
    {
        //Debug.Log((playerInput.currentActionMap == null ? "null" : playerInput.currentActionMap.name) + ": Close pause menu context: " + callbackContext.phase);
        if (callbackContext.performed)
        {
            closePauseMenu();
        }
    }

    public void togglePauseMenu(CallbackContext callbackContext)
    {
        if (callbackContext.performed)
        {
            togglePauseMenu();
        }
    }

    public void ToggleInventory(CallbackContext callback)
    {
        if (!callback.performed) { return; }
        if (menuState == MenuState.inventoryMenu)
        {
            closeInventory();
        }
        else
        {
            openInventory();
        }
    }

    public void openInventory()
    {
        if (menuState == MenuState.playing)
        {
            menuState = MenuState.inventoryMenu;
            inventoriesManager.EnableMenu();
            isPaused = true;
            EventSystem.current.SetSelectedGameObject(pauseMenu.resumeButton.gameObject);
            priorActionMap = playerInput.currentActionMap.name;
            playerInput.SwitchCurrentActionMap("UI");
            Time.timeScale = 0;
            if (usingMnK)
            {
                showMouse();
            }
        }
        else if (menuState == MenuState.pauseMenus)
        {
            menuState = MenuState.inventoryMenu;
            playerInput.SwitchCurrentActionMap("UI");
            inventoriesManager.EnableMenu();
            pauseMenu.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Cant open inventory right now.");
        }
    }

    public void closeInventory()
    {
        if (menuState == MenuState.inventoryMenu)
        {
            isPaused = false;
            menuState = MenuState.playing;
            inventoriesManager.DisableMenu();
            Time.timeScale = 1;
            playerInput.SwitchCurrentActionMap(priorActionMap);
            isPaused = false;
            if (usingMnK && !inMenu)
            {
                hideMouse();
            }
        }
        else
        {
            Debug.Log("Tried to close the inventory when it was already closed");
        }
    }

    public void startDialogue(Dialogue dialogue)
    {
        if (menuState == MenuState.playing)
        {
            menuState = MenuState.dialogue;
            dialogueManager.StartDialogue(dialogue);
            isPaused = true;
            EventSystem.current.SetSelectedGameObject(pauseMenu.resumeButton.gameObject);
            priorActionMap = playerInput.currentActionMap.name;
            //playerInput.SwitchCurrentActionMap("");
            //Time.timeScale = 0;
            if (usingMnK)
            {
                showMouse();
            }
        }
    }

    public void endDialogue()
    {
        if (menuState == MenuState.dialogue)
        {
            isPaused = false;
            menuState = MenuState.playing;
            //inventoriesManager.DisableMenu();
            Time.timeScale = 1;
            playerInput.SwitchCurrentActionMap("Player");
            isPaused = false;
            if (usingMnK && !inMenu)
            {
                hideMouse();
            }

        }

    }
}

public enum MenuState 
{ 
    playing,
    pauseMenus,
    inventoryMenu,
    dialogue
}