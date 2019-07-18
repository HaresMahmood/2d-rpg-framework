using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PokemonBase : MonoBehaviour
{
    public string name;
    public Sprite frontSprite;
    public Sprite backSprite;
    public Type type;
    public float baseHP;

    public enum Rarity
    {
        VeryCommon,
        Common,
        SemiRare,
        Rare,
        VeryRare
    }

    public enum Type
    {
        Fire,
        Grass,
        Water
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
}
