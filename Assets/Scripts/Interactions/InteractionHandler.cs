using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class InteractionHandler : MonoBehaviour
{

    [SerializeField]
    private List<InteractableObject> interactables;

    [SerializeField]
    private Camera localCamera;

    public float MinimumDistance;

    private PlayerData playerData;

    private InteractableObject currentlyInterractingObject = null;
    private CoinToss coinTosser = null;
    private DialogHandler dialogHandler;

    void Start()
    {
        coinTosser = new CoinToss();
        dialogHandler = GameObject.FindGameObjectWithTag("DialogHandler").GetComponent<DialogHandler>();
        playerData = GameObject.Find("Player").GetComponent<PlayerData>();
        foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag("Tree"))
        {
            interactables.Add(new InteractableObject(gameObject, InteractionType.WOOD));
        }
        foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag("Stone")){
            interactables.Add(new InteractableObject(gameObject, InteractionType.STONE));
        }
        foreach(GameObject gameObject in GameObject.FindGameObjectsWithTag("Enemy")){
            interactables.Add(new InteractableObject(gameObject, InteractionType.ENEMY));
        }
    }

    void Update()
    {
        if (!DialogHandler.IsAnyDialogOpen() && Input.GetMouseButtonDown(0))
        {
            Ray ray = localCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitPoint;

            if (Physics.Raycast(ray, out hitPoint))
            {
                InteractableObject interactableObject = interactables.Where(i => i.gameObject.Equals(hitPoint.transform.gameObject)).FirstOrDefault();
                if (interactableObject == null)
                {
                    return;
                }

                if (IsNear(interactableObject))
                {
                    currentlyInterractingObject = interactableObject;
                    switch (interactableObject.interactionType){
                        case InteractionType.ENEMY:
                            dialogHandler.OpenDialog("Do you want to slay this creature? Your success rate is: " + playerData.sucessRate + "%", OnAcceptSlay, ResetInteraction);
                            break;
                        case InteractionType.WOOD:
                            dialogHandler.OpenDialog("Do you want to chop the tree? Your success rate is: " + playerData.sucessRate + "%", OnAcceptChopWood, ResetInteraction);
                            break;
                        case InteractionType.STONE:
                            dialogHandler.OpenDialog("Do you want to mine the stones? Your success rate is: " + playerData.sucessRate + "%", OnAcceptMineStone, ResetInteraction);
                            break;
                        
                    }
                }
            }
        }
    }

    public void OnAcceptSlay()
    {
        if(coinTosser.TossCoin((int) playerData.sucessRate) == Outcome.Lose){
            playerData.Damage(10f);
            return;
        }
        playerData.bones += UnityEngine.Random.Range(2, 10);
        playerData.Heal(5f);
        GameObject.Destroy(this.currentlyInterractingObject.gameObject);
    }

    public void OnAcceptChopWood()
    {
        if(coinTosser.TossCoin((int) playerData.sucessRate) == Outcome.Lose){
            playerData.Damage(2f);
            return;
        }
        playerData.wood += UnityEngine.Random.Range(5, 20);
        GameObject.Destroy(this.currentlyInterractingObject.gameObject);
    }

    public void OnAcceptMineStone(){
        if(coinTosser.TossCoin((int) playerData.sucessRate) == Outcome.Lose){
            playerData.Damage(5f);
            return;
        }
        playerData.stone += UnityEngine.Random.Range(3, 7);
        GameObject.Destroy(this.currentlyInterractingObject.gameObject);
    }

    public void ResetInteraction(){
        this.currentlyInterractingObject = null;
    }


    public void OnGoodClick()
    {
        Debug.Log(":)))))))");
        //GameObject.Destroy(interactableObject.gameObject);
    }
    private bool IsNear(InteractableObject intObj)
    {
        return Vector3.Distance(Player.GetPosition(), intObj.gameObject.transform.position) <= MinimumDistance;
    }
}
