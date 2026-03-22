using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    Image healthBar;
    public float Maxhealth = 100f;
    public float HP;

    void Start()
    {
        healthBar = GetComponent<Image>();
        HP = Maxhealth;
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.fillAmount = HP / Maxhealth;
    }
}
