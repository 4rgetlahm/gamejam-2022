using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class InteractionHandler : MonoBehaviour
{

    [SerializeField]
    private List<InteractableObject> interactables = new List<InteractableObject>();

    [SerializeField]
    private Camera localCamera;

    public float MinimumDistance;
    public float BossDistance;

    private PlayerData playerData;

    private InteractableObject currentlyInterractingObject = null;
    private CoinToss coinTosser = null;
    private DialogHandler dialogHandler;
    private AlertMaker alertMaker;

    [SerializeField]
    private Animator animator;

    void Start()
    {
        coinTosser = new CoinToss();
        dialogHandler = GameObject.FindGameObjectWithTag("DialogHandler").GetComponent<DialogHandler>();
        playerData = GameObject.Find("Player").GetComponent<PlayerData>();
        alertMaker = GameObject.FindGameObjectWithTag("AlertMaker").GetComponent<AlertMaker>();
        foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag("Tree"))
        {
            interactables.Add(new InteractableObject(gameObject, InteractionType.Wood));
        }
        foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag("Stone"))
        {
            interactables.Add(new InteractableObject(gameObject, InteractionType.Stone));
        }
        foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            interactables.Add(new InteractableObject(gameObject, InteractionType.Enemy));
        }
    }

    void Update()
    {
        if (!DialogHandler.IsAnyDialogOpen() && Input.GetMouseButtonDown(0))
        {
            Ray ray = localCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitPoint;
            Debug.LogError(5);

            if (Physics.Raycast(ray, out hitPoint))
            {
                if(hitPoint.transform == null || hitPoint.transform.gameObject == null){
                    return;
                }
                InteractableObject interactableObject = null;
                foreach(InteractableObject interactable in interactables){
                    if(interactable.gameObject == null){
                        continue;
                    }
                    Debug.LogError("I: " + interactable.gameObject);
                    if(interactable.gameObject.Equals(hitPoint.transform.gameObject)){
                        interactableObject = interactable;
                        break;
                    }
                }
                Debug.LogError(interactableObject);
                if (interactableObject == null)
                {
                    return;
                }
                if (interactableObject.interactionType == InteractionType.Boss && IsNear(interactableObject, BossDistance))
                {
                print(interactableObject.interactionType);
                    dialogHandler.OpenDialog("Do you want to fight the sVilius? Your success rate is: " + playerData.sucessRate + "%", OnAcceptFightBoss, ResetInteraction);
                }
                if (IsNear(interactableObject, MinimumDistance))
                {
                    currentlyInterractingObject = interactableObject;
                    switch (interactableObject.interactionType)
                    {
                        case InteractionType.Enemy:
                            dialogHandler.OpenDialog("Do you want to slay this creature? Your success rate is: " + playerData.sucessRate + "%", OnAcceptSlay, ResetInteraction);
                            break;
                        case InteractionType.Wood:
                            dialogHandler.OpenDialog("Do you want to chop the tree? Your success rate is: " + playerData.sucessRate + "%", OnAcceptChopWood, ResetInteraction);
                            break;
                        case InteractionType.Stone:
                            dialogHandler.OpenDialog("Do you want to mine the stones? Your success rate is: " + playerData.sucessRate + "%", OnAcceptMineStone, ResetInteraction);
                            break;
                        case InteractionType.WoodenChest:
                            dialogHandler.OpenDialog("Do you want to open this Wooden chest? Your success rate is: " + playerData.sucessRate + "%", OnAcceptWoodenChest, ResetInteraction);
                        break;
                        case InteractionType.SilverChest:
                            dialogHandler.OpenDialog("Do you want to open this Silver chest? Your success rate is: " + (playerData.sucessRate - 10f) + "%", OnAcceptSilverChest, ResetInteraction);
                        break;
                        case InteractionType.GoldChest:
                            dialogHandler.OpenDialog("Do you want to open this Golden chest? Your success rate is: " + (playerData.sucessRate - 20f) + "%", OnAcceptGoldenChest, ResetInteraction);
                        break;
                        case InteractionType.MagicChest:
                            dialogHandler.OpenDialog("Do you want to open this Magic chest? Your success rate is: " + (playerData.sucessRate - 30f) + "%", OnAcceptMagicChest, ResetInteraction);
                        break;
                        case InteractionType.DeathChest:
                            dialogHandler.OpenDialog("Do you want to open this Death chest? Your success rate is: " + (playerData.sucessRate - 50f) + "%", OnAcceptGoldenChest, ResetInteraction);
                        break;
                    }
                }
            }
        }
    }

    public IEnumerator PlayFightingAnimation(){
        Debug.Log("OK");
        animator.SetBool("IsFighting", true);
        yield return new WaitForSeconds(1f);
        animator.SetBool("IsFighting", false);
    }

    public void OnAcceptWoodenChest(){
        if (coinTosser.TossCoin((int)playerData.sucessRate) == Outcome.Lose)
        {
            playerData.Damage(10f);
            alertMaker.ShowAlert("You have failed to open the chest!", 2f);
            GameObject.Destroy(this.currentlyInterractingObject.gameObject);
            TouchedGameObjects.GameObjects.Remove(this.currentlyInterractingObject.gameObject);
            return;
        }
        int generatedBones = UnityEngine.Random.Range(1, 3);
        int generatedWood = UnityEngine.Random.Range(1, 6);
        int generatedStone = UnityEngine.Random.Range(1, 8);
        playerData.bones += generatedBones;
        playerData.wood += generatedWood;
        playerData.stone += generatedStone;
        GameObject.Destroy(this.currentlyInterractingObject.gameObject);
        TouchedGameObjects.GameObjects.Remove(this.currentlyInterractingObject.gameObject);
        alertMaker.ShowAlert("You have opened the chest and received: " + generatedBones + " bones, " + generatedWood + " wood, " + generatedStone + " stone", 2f);
        StartCoroutine("PlayFightingAnimation");
    }

    public void OnAcceptSilverChest(){
        if (coinTosser.TossCoin((int)playerData.sucessRate - 10) == Outcome.Lose)
        {
            playerData.Damage(20f);
            alertMaker.ShowAlert("You have failed to open the chest!", 2f);
            GameObject.Destroy(this.currentlyInterractingObject.gameObject);
            TouchedGameObjects.GameObjects.Remove(this.currentlyInterractingObject.gameObject);
            return;
        }
        int generatedBones = UnityEngine.Random.Range(4, 12);
        int generatedWood = UnityEngine.Random.Range(5, 15);
        int generatedStone = UnityEngine.Random.Range(4, 13);
        playerData.bones += generatedBones;
        playerData.wood += generatedWood;
        playerData.stone += generatedStone;
        GameObject.Destroy(this.currentlyInterractingObject.gameObject);
        TouchedGameObjects.GameObjects.Remove(this.currentlyInterractingObject.gameObject);
        alertMaker.ShowAlert("You have opened the chest and received: " + generatedBones + " bones, " + generatedWood + " wood, " + generatedStone + " stone", 2f);
        StartCoroutine("PlayFightingAnimation");
    }

    public void OnAcceptGoldenChest(){
        if (coinTosser.TossCoin((int)playerData.sucessRate - 20) == Outcome.Lose)
        {
            playerData.Damage(30f);
            alertMaker.ShowAlert("You have failed to open the chest!", 2f);
            GameObject.Destroy(this.currentlyInterractingObject.gameObject);
            TouchedGameObjects.GameObjects.Remove(this.currentlyInterractingObject.gameObject);
            return;
        }
        int generatedBones = UnityEngine.Random.Range(10, 30);
        int generatedWood = UnityEngine.Random.Range(10, 30);
        int generatedStone = UnityEngine.Random.Range(10, 30);
        playerData.bones += generatedBones;
        playerData.wood += generatedWood;
        playerData.stone += generatedStone;
        GameObject.Destroy(this.currentlyInterractingObject.gameObject);
        TouchedGameObjects.GameObjects.Remove(this.currentlyInterractingObject.gameObject);
        alertMaker.ShowAlert("You have opened the chest and received: " + generatedBones + " bones, " + generatedWood + " wood, " + generatedStone + " stone", 2f);
        StartCoroutine("PlayFightingAnimation");
    }

    public void OnAcceptMagicChest(){
        if (coinTosser.TossCoin((int)playerData.sucessRate - 30) == Outcome.Lose)
        {
            playerData.Damage(20f);
            alertMaker.ShowAlert("You have failed to open the chest!", 2f);
            GameObject.Destroy(this.currentlyInterractingObject.gameObject);
            TouchedGameObjects.GameObjects.Remove(this.currentlyInterractingObject.gameObject);
            return;
        }
        int generateChance = UnityEngine.Random.Range(1, 10);
        playerData.sucessRate += generateChance;
        GameObject.Destroy(this.currentlyInterractingObject.gameObject);
        TouchedGameObjects.GameObjects.Remove(this.currentlyInterractingObject.gameObject);
        alertMaker.ShowAlert("You have opened the chest and your luck increased by: " + generateChance + "%", 2f);
        StartCoroutine("PlayFightingAnimation");
    }

    public void OnAcceptDeathChest(){
        if (coinTosser.TossCoin((int)playerData.sucessRate - 50) == Outcome.Lose)
        {
            playerData.Damage(20f);
            alertMaker.ShowAlert("You have failed to open the chest!", 2f);
            GameObject.Destroy(this.currentlyInterractingObject.gameObject);
            TouchedGameObjects.GameObjects.Remove(this.currentlyInterractingObject.gameObject);
            return;
        }
        int generateChance = UnityEngine.Random.Range(10, 30);
        playerData.sucessRate += generateChance;
        GameObject.Destroy(this.currentlyInterractingObject.gameObject);
        TouchedGameObjects.GameObjects.Remove(this.currentlyInterractingObject.gameObject);
        alertMaker.ShowAlert("You have opened the chest and your luck increased by: " + generateChance + "%", 2f);
        StartCoroutine("PlayFightingAnimation");
    }

    public void OnAcceptSlay()
    {
        if (coinTosser.TossCoin((int)playerData.sucessRate) == Outcome.Lose)
        {
            playerData.Damage(10f);
            alertMaker.ShowAlert("You have failed to slay the creature!", 2f);
            return;
        }
        int generatedBones = UnityEngine.Random.Range(2, 10);
        playerData.bones += generatedBones;
        playerData.Heal(5f);
        GameObject.Destroy(this.currentlyInterractingObject.gameObject);
        TouchedGameObjects.GameObjects.Remove(this.currentlyInterractingObject.gameObject);
        alertMaker.ShowAlert("You have slayed the creature and received " + generatedBones + " bones", 2f);
        StartCoroutine("PlayFightingAnimation");
    }

    public void OnAcceptChopWood()
    {
        if (coinTosser.TossCoin((int)playerData.sucessRate) == Outcome.Lose)
        {
            playerData.Damage(2f);
            alertMaker.ShowAlert("You have failed to chop the tree!", 2f);
            return;
        }
        int generatedWood = UnityEngine.Random.Range(5, 20);
        playerData.wood += generatedWood;
        GameObject.Destroy(this.currentlyInterractingObject.gameObject);
        TouchedGameObjects.GameObjects.Remove(this.currentlyInterractingObject.gameObject);
        alertMaker.ShowAlert("You have chopped the tree and got " + generatedWood + " wood", 2f);
        StartCoroutine("PlayFightingAnimation");
    }

    public void OnAcceptMineStone()
    {
        if (coinTosser.TossCoin((int)playerData.sucessRate) == Outcome.Lose)
        {
            playerData.Damage(5f);
            alertMaker.ShowAlert("You have failed to mine the stone!", 2f);
            return;
        }
        int generatedStone = UnityEngine.Random.Range(3, 7);
        playerData.stone += generatedStone;
        GameObject.Destroy(this.currentlyInterractingObject.gameObject);
        TouchedGameObjects.GameObjects.Remove(this.currentlyInterractingObject.gameObject);
        alertMaker.ShowAlert("You have mined the stone and got " + generatedStone + " stone", 2f);
        StartCoroutine("PlayFightingAnimation");
    }

    public void OnAcceptFightBoss()
    {
        if (coinTosser.TossCoin((int)playerData.sucessRate - 100) == Outcome.Lose)
        {
            playerData.Damage(50f);
            alertMaker.ShowAlert("You have failed to kill sVilius!", 2f);
            return;
        }
        GameObject.Destroy(this.currentlyInterractingObject.gameObject);
        TouchedGameObjects.GameObjects.Remove(this.currentlyInterractingObject.gameObject);
        alertMaker.ShowAlert("Congratulations!!! You have killed sVilius!!!", 2f);
        StartCoroutine("PlayFightingAnimation");
    }

    public void ResetInteraction()
    {
        this.currentlyInterractingObject = null;
    }

    public void OnGoodClick()
    {
        //GameObject.Destroy(interactableObject.gameObject);
    }
    private bool IsNear(InteractableObject intObj, float minimumDistance)
    {
        return Vector3.Distance(Player.GetPosition(), intObj.gameObject.transform.position) <= MinimumDistance;
    }
}
