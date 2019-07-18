//TODO make BoxColliders consistent size.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomEncounters : MonoBehaviour
{
    public bool canEncounter = true;
    public bool inBattle = false;

    public string playerTag = "Player";

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
        {
            Debug.Log("Entered grass");

            if (canEncounter && !inBattle)
            {
                if (EncounterPokemon())
                {
                    canEncounter = false;
                    inBattle = true;
                }
                else
                {
                    canEncounter = true;
                    inBattle = false;
                }
            }
        }
    }
    
    private bool EncounterPokemon()
    {
        float veryCommon = 10 / 187.5f;
        float common = 8.5f / 187.5f;
        float semiRare = 6.75f / 187.5f;
        float rare = 3.33f / 187.5f;
        float veryRare = 1.25f / 187.5f;

        float p = Random.Range(0.0f, 100.0f);

        if (p < veryRare * 100)
        {
            Debug.Log("Very rare Pokemon");
            return true;
        }
        else if (p < rare * 100)
        {
            Debug.Log("Rare Pokemon");
            return true;
        }
        else if (p < semiRare * 100)
        {
            Debug.Log("Semi rare Pokemon");
            return true;
        }
        else if (p < common * 100)
        {
            Debug.Log("Common Pokemon");
            return true;

        }
        else if (p < veryCommon * 100)
        {
            Debug.Log("Very common Pokemon");
            return true;
        }
        else
        {
            Debug.Log("No Pokemon");
            return false;
        }
        return false;
    }
}
