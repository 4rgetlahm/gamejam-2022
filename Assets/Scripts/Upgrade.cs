using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Upgrade : MonoBehaviour
{
    private PlayerData playerData;
    private AlertMaker alertMaker;
    private DialogHandler dialogHandler;
    private CoinToss coinTosser;

    [SerializeField]
    private float timeBetweenAlerts;
    private float timeSinceLastAlert = 0;

    [SerializeField]
    private int bonesForUpgrade = 10;
    [SerializeField]
    private int woodForUpgrade = 10;
    [SerializeField]
    private int stoneForUpgrade = 10;
    [SerializeField]
    private float upgradeByRate = 25f;

    // Start is called before the first frame update
    void Start()
    {
        playerData = GameObject.Find("Player").GetComponent<PlayerData>();
        alertMaker = GameObject.FindGameObjectWithTag("AlertMaker").GetComponent<AlertMaker>();
        dialogHandler = GameObject.FindGameObjectWithTag("DialogHandler").GetComponent<DialogHandler>();
        coinTosser = new CoinToss();
    }

    void Update(){
        if(IsEnoughResources()){
            timeSinceLastAlert += Time.deltaTime;
            if(timeSinceLastAlert >= timeBetweenAlerts){
                timeSinceLastAlert = -2f;
                alertMaker.ShowAlert("You have upgrade available, click C to upgrade", 2f);
            }
        }
        if(!Input.GetKeyDown(KeyCode.C)){
            return;
        }
        if(DialogHandler.IsAnyDialogOpen()){
            return;
        }

        dialogHandler.OpenDialog(
            "Do you want to upgrade your coin?\nIt will cost you " + bonesForUpgrade + " bones, " + woodForUpgrade + " wood, " + stoneForUpgrade + " stone\nThe success rate of this action is: " + playerData.sucessRate + "%",
            UpgradeCoin,
            delegate {}
        );
        
    }

    bool IsEnoughResources(){
        return playerData.bones >= bonesForUpgrade && playerData.stone >= stoneForUpgrade && playerData.wood >= woodForUpgrade;
    }

    public void UpgradeCoin(){
        playerData.bones -= bonesForUpgrade;
        playerData.wood -= woodForUpgrade;
        playerData.stone -= stoneForUpgrade;
        if(coinTosser.TossCoin((int) playerData.sucessRate) == Outcome.Lose){
            alertMaker.ShowAlert("You have failed to upgrade your coin", 2f);
            return;
        }
        playerData.sucessRate += upgradeByRate;
        alertMaker.ShowAlert("You have suceeded in upgrading your coin! Your success rate is now: " + playerData.sucessRate + "%!", 2f);
    }

}
