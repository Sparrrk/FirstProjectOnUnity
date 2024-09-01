using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerCoinScript : MonoBehaviour
{
    [SerializeField] PlayerScript player;
    [SerializeField] TextMeshProUGUI textMP;



    // Update is called once per frame
    void Update()
    {
        if (textMP != null)
        {
            textMP.text = "Money: " + player.coinAmount.ToString();
        }

    }
}
