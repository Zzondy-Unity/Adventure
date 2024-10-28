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
            if (curInteractable == null) return;
            curInteractable.OnInteract();
            curInteractable = null;
            curInteractingObject = null;
            ItemInfoUI.SetActive(false);
        }
    }


    private void Search()
    {
        Ray[] ray;
        RaycastHit hit;
        if (controller.isTPView)
        {
            ray = new Ray[5]
            {
                new Ray(transform.position + Vector3.up * 0.01f, Vector3.forward),
                new Ray(transform.position + Vector3.up * 0.1f, Vector3.forward),
                new Ray(transform.position + Vector3.up * 0.9f, Vector3.forward),
                new Ray(transform.position + Vector3.up * 1.2f, Vector3.forward),
                new Ray(transform.position + Vector3.up * 1.8f, Vector3.forward)
            };
        }
        else
        {
            ray = new Ray[1] { cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0)) };
        }

        for (int i = 0; i < ray.Length; i++)
        {
            if (Physics.Raycast(ray[i], out hit, maxCheckDistance, interactableLayerMask))
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
}