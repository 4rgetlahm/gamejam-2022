using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    private float health = 100f;
    public float sucessRate = 50f;
    public int wood = 0;
    public int stone = 0;
    public int bones = 0;

    [SerializeField]
    private TMP_Text bonesText;
    [SerializeField]
    private TMP_Text woodText;
    [SerializeField]
    private TMP_Text stoneText;
    

    public void Damage(float damage){
        if(this.health - damage <= 0f){
            OnDeath();
        }
        this.health = this.health - damage <= 0 ? 0.0f : this.health - damage;
    }

    public void Heal(float addHealth){
        this.health = this.health + addHealth >= 100.0f ? 100.0f : this.health + addHealth;
    }

    private void OnDeath(){
        Debug.Log("Death");
    }

    void Update(){
        this.bonesText.text = bones.ToString();
        this.woodText.text = wood.ToString();
        this.stoneText.text = stone.ToString();
    }

    

}
