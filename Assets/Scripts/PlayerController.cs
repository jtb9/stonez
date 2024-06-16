using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    public AudioSource source;
    public AudioClip woodCut;
    public AudioClip rockMine;
    public AudioClip moveClick;
    public AudioClip actionClick;
    public UIDocument uiHook;
    public Inventory inventory;

    public NavMeshAgent agent;

    public GameObject clickPrefab;
    public Animator animator;
    private CameraController cameraController;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cameraController = GameObject.FindFirstObjectByType<CameraController>();

        source = GetComponent<AudioSource>();
        inventory = GetComponent<Inventory>();

        // target 60 fps
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;

        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        // load our persist global variables
        Global.Load();
        inventory.Load();

        var skillsButton = uiHook.rootVisualElement.Q<Button>("skills");
        skillsButton.clicked += SkillsMenuClick;

        var inventoryButton = uiHook.rootVisualElement.Q<Button>("inventory");
        inventoryButton.clicked += InventoryMenuClick;
    }

    void InventoryMenuClick()
    {
        InventoryViewController.Open();
    }

    void SkillsMenuClick()
    {
        SkillMenuController.Open();
    }

    void updateStatus(String newStatus)
    {
        Label l = uiHook.rootVisualElement.Query<Label>("status");
        l.text = newStatus;
    }

    void playBuildSound()
    {

    }

    void playMoveClick()
    {
        source.PlayOneShot(moveClick);
    }

    void playInteractClick()
    {
        source.PlayOneShot(actionClick);
    }

    void playRockMineSound()
    {
        source.PlayOneShot(rockMine);
    }

    void playWoodcutSound()
    {
        source.PlayOneShot(woodCut);
    }

    Vector3 getDestination(Vector3 target)
    {
        return new Vector3(
            target.x,
            transform.position.y,
            target.z
        );
    }

    private Vector3 targetLocation = new Vector3(0, 0, 0);
    private int targetAction = 0;
    private Interactible targetInteractible;

    void Update()
    {
        // determine our action
        if (Input.GetMouseButtonDown(0))
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                _HandleTouch(Input.mousePosition);
            }
        }

        // if (Input.touchCount > 0)
        // {
        //     Touch touch = Input.GetTouch(0);
        //     if (!EventSystem.current.IsPointerOverGameObject())
        //     {
        //         _HandleTouch(touch.position);
        //     }
        // }
        HandleTouchInput();

        if (Input.GetKeyUp(KeyCode.S))
        {
            ConfirmationHandler.Show("Baseball");
        }

        UpdateAction();
        UpdateGUI();

        if (animator)
        {
            animator.SetFloat("velocity", Vector3.Distance(Vector3.zero, agent.velocity));
        }

        HandleOutline();
    }

    private Vector2 startTouchPosition;
    private Vector2 lastTouchPosition;
    private bool isDragging = false;

    private float initialPinchDistance;
    private float initialPinchScale;

    private float tapThreshold = 0.2f;
    private float lastTapTime;

    private enum GestureState { None, Dragging, Pinching }
    private GestureState currentGesture = GestureState.None;
    private float pinchThreshold = 10f;

    private void HandleTouchInput()
    {
        if (Input.touchCount == 1 && currentGesture != GestureState.Pinching)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                startTouchPosition = touch.position;
                lastTouchPosition = touch.position;
                isDragging = true;
                currentGesture = GestureState.Dragging;
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                if (isDragging)
                {
                    Vector2 touchDelta = touch.position - lastTouchPosition;
                    HandleDrag(touchDelta);
                    lastTouchPosition = touch.position;
                }
            }
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                isDragging = false;
                if (Time.time - lastTapTime < tapThreshold)
                {
                    HandleTap(touch.position);
                }
                lastTapTime = Time.time;
                currentGesture = GestureState.None;
            }
        }
        else if (Input.touchCount == 2)
        {
            Touch touch1 = Input.GetTouch(0);
            Touch touch2 = Input.GetTouch(1);

            if (currentGesture != GestureState.Pinching)
            {
                if (touch1.phase == TouchPhase.Began || touch2.phase == TouchPhase.Began)
                {
                    // Do nothing until both touches have moved
                }
                else if ((touch1.phase == TouchPhase.Moved || touch1.phase == TouchPhase.Stationary) &&
                         (touch2.phase == TouchPhase.Moved || touch2.phase == TouchPhase.Stationary))
                {
                    float initialDistance = Vector2.Distance(touch1.position, touch2.position);
                    if (Mathf.Abs(initialDistance - initialPinchDistance) > pinchThreshold)
                    {
                        initialPinchDistance = initialDistance;
                        initialPinchScale = transform.localScale.x; // Assuming uniform scaling
                        isDragging = false; // Disable dragging when pinching
                        currentGesture = GestureState.Pinching;
                    }
                }
            }
            else if (touch1.phase == TouchPhase.Moved || touch2.phase == TouchPhase.Moved)
            {
                float currentPinchDistance = Vector2.Distance(touch1.position, touch2.position);
                if (Mathf.Abs(currentPinchDistance - initialPinchDistance) > pinchThreshold)
                {
                    float scaleFactor = currentPinchDistance / initialPinchDistance;
                    HandlePinch(scaleFactor);
                }
            }
            else if (touch1.phase == TouchPhase.Ended || touch2.phase == TouchPhase.Ended)
            {
                currentGesture = GestureState.None;
            }
        }
        else
        {
            isDragging = false; // Reset dragging if touch count is not 1 or 2
            currentGesture = GestureState.None;
        }
    }

    private bool IsPinching()
    {
        return Input.touchCount == 2;
    }

    private void HandleDrag(Vector2 delta)
    {
        // Vector3 newPosition = new Vector3(delta.x, delta.y, 0) * Time.deltaTime;
        // transform.Translate(newPosition, Space.World);

        cameraController.targetRotation += delta.x;

    }

    private void HandlePinch(float scaleFactor)
    {
        //transform.localScale = new Vector3(initialPinchScale * scaleFactor, initialPinchScale * scaleFactor, initialPinchScale * scaleFactor);
        cameraController.targetZoom = initialPinchScale * scaleFactor;
    }

    private void HandleTap(Vector2 position)
    {
        Debug.Log("Tap detected at position: " + position);
        // Add tap handling logic here

        _HandleTouch(position);
    }

    private Outline lastOutline;
    private bool inOutline = false;
    void HandleOutline()
    {
        GameObject objectUnderCursor = findObjectUnderCursor();

        if (inOutline)
        {
            if (lastOutline)
            {
                if (objectUnderCursor)
                {
                    if (lastOutline.gameObject.GetInstanceID() != objectUnderCursor.GetInstanceID())
                    {
                        lastOutline.enabled = false;
                        inOutline = false;
                    }
                }
                else
                {
                    lastOutline.enabled = false;
                    inOutline = false;
                }
            }
        }
        else
        {
            if (objectUnderCursor)
            {
                inOutline = true;

                Outline existingOutline = objectUnderCursor.GetComponent<Outline>();

                if (existingOutline)
                {
                    lastOutline = existingOutline;
                }
                else
                {
                    //lastOutline = objectUnderCursor.AddComponent<Outline>();
                }

                lastOutline.enabled = true;
                lastOutline.OutlineWidth = 2.0f;
            }
        }
    }

    private GameObject findObjectUnderCursor()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100.0f))
            {
                if (hit.transform.gameObject.tag == "Interactible")
                {
                    return hit.transform.gameObject;
                }
            }
        }

        return null;
    }

    void _HandleTouch(Vector3 raw)
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(raw);
        if (Physics.Raycast(ray, out hit, 100.0f))
        {
            if (hit.transform.gameObject.tag == "Ground")
            {
                targetLocation = getDestination(hit.point);
                HandleTouch(targetLocation);
                targetAction = 1;
                playMoveClick();
            }
            else
            {
                if (hit.transform.gameObject.tag == "Interactible")
                {
                    // handle the interaction
                    targetInteractible = hit.transform.gameObject.GetComponent<Interactible>();


                    targetLocation = getDestination(targetInteractible.dock.transform.position);
                    HandleTouch(targetLocation);
                    targetAction = 2;

                    playInteractClick();
                }
            }
        }
    }

    void HandleTouch(Vector3 position)
    {
        GameObject g = GameObject.Instantiate(clickPrefab);
        g.transform.position = new Vector3(position.x, 4.5f, position.z);
    }

    void UpdateGUI()
    {
        Label l = uiHook.rootVisualElement.Query<Label>("inventory");
        l.text = inventory.toString();

        Label l3 = uiHook.rootVisualElement.Query<Label>("stats");
        l3.text = Global.statsAsString();

        Label l2 = uiHook.rootVisualElement.Query<Label>("log");
        l2.text = Global._log;
    }

    void ClearFlags()
    {
        firstBattleConfirmationFlag = false;
        firstShopOpenFlag = false;
    }

    bool firstBattleConfirmationFlag = false;
    bool firstShopOpenFlag = false;
    void UpdateAction()
    {
        switch (targetAction)
        {
            case 0:
                ClearFlags();
                updateStatus("idle");

                if (Input.GetKeyDown(KeyCode.B))
                {
                    BuildNow();
                }
                break;
            case 1:
                ClearFlags();
                updateStatus("moving");
                agent.SetDestination(targetLocation);

                // check for going back to idle
                if (Vector3.Distance(targetLocation, getDestination(transform.position)) <= 0.1f)
                {
                    targetAction = 0;
                }
                break;
            case 2:
                agent.SetDestination(targetLocation);
                // interactible
                if (targetInteractible.type == "tree")
                {
                    updateStatus("woodcutting");
                    if (Vector3.Distance(targetLocation, getDestination(transform.position)) <= 0.3f)
                    {
                        UpdateWoodcutting();
                    }
                }
                if (targetInteractible.type == "rock")
                {
                    updateStatus("mining");
                    if (Vector3.Distance(targetLocation, getDestination(transform.position)) <= 0.3f)
                    {
                        UpdateMining();
                    }
                }
                if (targetInteractible.type == "battle")
                {
                    updateStatus("battle");
                    if (Vector3.Distance(targetLocation, getDestination(transform.position)) <= 0.3f)
                    {
                        UpdateBattle();
                    }
                }
                if (targetInteractible.type == "sell_wood")
                {
                    updateStatus("selling");
                    if (Vector3.Distance(targetLocation, getDestination(transform.position)) <= 0.3f)
                    {
                        UpdateSellWood();
                    }
                }
                break;
        }
    }


    void BuildNow()
    {
    }

    DateTime lastWoodSale = DateTime.Now;
    void UpdateSellWood()
    {
        if (firstShopOpenFlag == false)
        {
            ShopMenuController.Open();
            firstShopOpenFlag = true;
        }
    }

    void UpdateBattle()
    {
        if (firstBattleConfirmationFlag == false)
        {
            firstBattleConfirmationFlag = true;
            BattleMenuController.Open();
        }
        // if (ConfirmationHandler.confirmed)
        // {
        //     ConfirmationHandler.Clear();
        //     // load the battle scene
        //     SceneManager.LoadScene("Battle");
        // }
    }
    DateTime lastMine = DateTime.Now;
    void UpdateMining()
    {
        DateTime now = DateTime.Now;
        RockInteractible rock = targetInteractible.GetComponent<RockInteractible>();

        if ((now - lastMine).TotalSeconds >= 3.0)
        {
            lastMine = now;

            if (rock.canGetResource)
            {
                rock.TakeResource();

                InventoryItem loot = rock.itemType;

                inventory.addItemByQuantity(loot);

                // chance to increase mining level -- lower since the value is higher
                if (UnityEngine.Random.Range(0, 12) >= 11)
                {
                    Global.Log("Leveled up mining!");
                    Global._miningLevel += 1;
                    Global.Save();
                }

                Global.Log("Got " + loot.itemQuantity.ToString() + " of " + loot.itemName);

                playRockMineSound();
            }
        }
    }

    DateTime lastCut = DateTime.Now;
    void UpdateWoodcutting()
    {
        DateTime now = DateTime.Now;
        TreeInteractible tree = targetInteractible.GetComponent<TreeInteractible>();

        if ((now - lastCut).TotalSeconds >= 3.0)
        {
            lastCut = now;

            if (tree.canGetResource)
            {
                tree.TakeResource();

                InventoryItem loot = tree.itemType;

                inventory.addItemByQuantity(loot);

                // chance to increase woodcutting level
                if (UnityEngine.Random.Range(0, 10) >= 9)
                {
                    Global.Log("Leveled up woodcutting!");
                    Global._woodcutLevel += 1;
                    Global.Save();
                }

                Global.Log("Got " + loot.itemQuantity.ToString() + " of " + loot.itemName);

                playWoodcutSound();
            }
        }
    }
}


