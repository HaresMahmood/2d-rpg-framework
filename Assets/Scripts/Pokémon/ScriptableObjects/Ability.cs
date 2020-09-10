using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "New Ability", menuName = "Ability")]
public class Ability : ScriptableObject
{
    #region Fields

    [SerializeField] private new string name;
    [SerializeField] private int id;
    [SerializeField] private string description;
    [SerializeField] private bool requiresTarget;
    [SerializeField] private GameObject particleEffect;
    [SerializeField] private UnityEvent logic;

    #endregion

    #region Properties

    public string Name
    {
        get { return name; }
        set { name = value; }
    }

    public int ID
    {
        get { return id; }
        set { id = value; }
    }

    public string Description
    {
        get { return description; }
        set { description = value; }
    }

    public bool RequiresTarget
    {
        get { return requiresTarget; }
        set { requiresTarget = value; }
    }

    public GameObject PartcileEffect
    {
        get { return particleEffect; }
        set { particleEffect = value; }
    }

    public UnityEvent Logic
    {
        get { return logic; }
        set { logic = value; }
    }

    #endregion
}
