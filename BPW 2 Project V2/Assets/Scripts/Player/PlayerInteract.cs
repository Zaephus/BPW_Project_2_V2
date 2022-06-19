using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerInteract : MonoBehaviour {

    private PlayerManager player;

    private IInteractable interactable;

    private bool canInteract = false;

    public TMP_Text interactText;

    public void OnStart(PlayerManager p) {
        player = p;
    }

    public void OnUpdate() {

        if(canInteract) {
            interactText.enabled = true;

            if(Input.GetButtonDown("Interact")) {
                interactable.Interact(player);
                canInteract = false;
            }
        }
        else {
            interactText.enabled = false;
        }
        
    }

    public void OnTriggerEnter2D(Collider2D other) {
        if(other.GetComponent<IInteractable>() != null) {
            canInteract = true;
            interactable = other.GetComponent<IInteractable>();
            if(other.GetComponent<IFightable>() != null) {
                Manager.instance.StartCoroutine(Manager.instance.StartBattle(other.GetComponent<EnemyController>().unit));
            }
        }
    }

    public void OnTriggerExit2D(Collider2D other) {
        if(other.GetComponent<IInteractable>() != null) {
            canInteract = false;
        }
    }

}