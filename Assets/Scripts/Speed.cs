using System.Collections;
using UnityEngine;

public class Speed : MonoBehaviour
{
    public float boostAmount = 500f;   
    public float duration = 5f;     

    private Player playerMovement;

    private void Start()
    {
        playerMovement = FindObjectOfType<Player>(); 
    }

    public void UseItem()
    {
        if (playerMovement != null)
        {
            StartCoroutine(SpeedBoost()); 
        }
    }

    private IEnumerator SpeedBoost()
    {
        playerMovement.controller.moveSpeed += boostAmount;

        yield return new WaitForSeconds(duration);

        playerMovement.controller.moveSpeed -= boostAmount;
    }
}
