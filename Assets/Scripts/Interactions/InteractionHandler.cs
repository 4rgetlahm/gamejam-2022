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
    private AlertMaker alertMaker;

    void Start()
    {
        coinTosser = new CoinToss();
        dialogHandler = GameObject.FindGameObjectWithTag("DialogHandler").GetComponent<DialogHandler>();
        playerData = GameObject.Find("Player").GetComponent<PlayerData>();
        alertMaker = GameObject.FindGameObjectWithTag("AlertMaker").GetComponent<AlertMaker>();
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
            alertMaker.ShowAlert("You have failed to slay the creature!", 2f);
            return;
        }
        int generatedBones = UnityEngine.Random.Range(2, 10);
        playerData.bones += generatedBones;
        playerData.Heal(5f);
        GameObject.Destroy(this.currentlyInterractingObject.gameObject);
        alertMaker.ShowAlert("You have slayed the creature and received " + generatedBones + " bones", 2f);
    }

    public void OnAcceptChopWood()
    {
        if(coinTosser.TossCoin((int) playerData.sucessRate) == Outcome.Lose){
            playerData.Damage(2f);
            alertMaker.ShowAlert("You have failed to chop the tree!", 2f);
            return;
        }
        int generatedWood = UnityEngine.Random.Range(5, 20);
        playerData.wood += generatedWood;
        GameObject.Destroy(this.currentlyInterractingObject.gameObject);
        alertMaker.ShowAlert("You have chopped the tree and got " + generatedWood + " wood", 2f);
    }

    public void OnAcceptMineStone(){
        if(coinTosser.TossCoin((int) playerData.sucessRate) == Outcome.Lose){
            playerData.Damage(5f);
            alertMaker.ShowAlert("You have failed to mine the stone!", 2f);
            return;
        }
        int generatedStone = UnityEngine.Random.Range(3, 7);
        playerData.stone += generatedStone;
        GameObject.Destroy(this.currentlyInterractingObject.gameObject);
        alertMaker.ShowAlert("You have mined the stone and got " + generatedStone + " stone", 2f);
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
