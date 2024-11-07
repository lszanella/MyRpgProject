using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Player_Health : MonoBehaviour
{
    public int currentHealth;
    public int maxHealth;
    public TMP_Text healthText;
    public Animator healthTextAnim;

    private void Start(){
        healthText.text = "HP: "+currentHealth+" / "+maxHealth;
    }

    public void ChangeHealth(int amount){
        currentHealth += amount;
        healthTextAnim.Play("TextUpdate");
        healthText.text = "HP: "+currentHealth+" / "+maxHealth;
        if (currentHealth<=0){
            gameObject.SetActive(false);
        }
    }
}
