using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interaction : MonoBehaviour
{
    [SerializeField] private GameObject ItemInfoUI;
    [SerializeField] private TextMeshProUGUI ItemNameText;
    [SerializeField] private TextMeshProUGUI ItemDescriptionText;

    private float lastCheckTime;
    public float checkRate = 0.05f;
    public float maxCheckDistance;
    public LayerMask interactableLayerMask;

    private PlayerController controller;
    private Camera cam;
    private IInteractable curInteractable;
    private GameObject curInteractingObject;

    private void Start()
    {
        ItemInfoUI.SetActive(false);
        controller = CharacterManager.Instance.player.controller;
        cam = Camera.main;
    }

    private void Update()
    {
        if (Time.time - lastCheckTime > checkRate)
        {
            lastCheckTime = Time.time;
            Search();
        }
    }

    private void setPromptText()
    {
        ItemInfoUI.SetActive(true);
        ItemNameText.text = curInteractable.GetInteractNamePrompt();
        ItemDescriptionText.text = curInteractable.GetInteractDescriptionPrompt();
    }

    public void OnInteraction(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            curInteractable.OnInteract();
            curInteractable = null;
            curInteractingObject = null;
            ItemInfoUI.SetActive(false);
        }
    }


    private void Search()
    {
        Ray ray;
        RaycastHit hit;
        if (!controller.isTPView)
            ray = cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        else
            ray = new Ray(transform.position, transform.forward * maxCheckDistance);
        if (Physics.Raycast(ray, out hit, maxCheckDistance, interactableLayerMask))
        {
            if (hit.collider.gameObject != curInteractingObject)
            {
                curInteractingObject = hit.collider.gameObject;
                curInteractable = curInteractingObject.GetComponent<IInteractable>();
                setPromptText();
            }
        }
        else
        {
            curInteractingObject = null;
            curInteractable = null;
            ItemInfoUI.SetActive(false);
        }
    }
}