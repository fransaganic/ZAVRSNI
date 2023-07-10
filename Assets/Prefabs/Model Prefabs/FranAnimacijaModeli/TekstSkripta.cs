using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TekstSkripta : MonoBehaviour
{
    public TextMeshProUGUI text;
    // Start is called before the first frame update
    void Start()
{
    
}

    void OnEnable()
    {
        StartCoroutine(waiter());
    }

IEnumerator waiter()
{
    

    text.text = "Čovjek i medvjedić imaju različite animacije.";

    yield return new WaitForSeconds(9);

    
    text.text = "Dolazi do prebacivanja animacije!";

    
}
}
