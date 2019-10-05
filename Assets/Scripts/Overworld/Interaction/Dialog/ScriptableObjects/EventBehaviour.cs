using UnityEngine;

[CreateAssetMenu(fileName = "New Event", menuName = "Dialog/Event")]
public class EventBehaviour : ScriptableObject
{
    public void ChangeGender()
    {
        for (int i = 0; i < GameManager.instance.player.childCount; i++)
        {
            if (GameManager.instance.player.GetChild(i).gameObject.activeSelf == false)
            {
                GameManager.instance.player.GetChild(i).transform.position = GameManager.instance.player.GetChild(i + 1).transform.position;
                GameManager.instance.player.GetChild(i).GetComponent<PlayerMovement>().orientation = GameManager.instance.player.GetChild(i + 1).GetComponent<PlayerMovement>().orientation;
                GameManager.instance.player.GetChild(i + 1).gameObject.SetActive(false);
                GameManager.instance.player.GetChild(i).gameObject.SetActive(true);

                GameManager.instance.activePlayer = GameManager.instance.player.GetChild(i);
            }
            else
            {
                GameManager.instance.player.GetChild(i + 1).transform.position = GameManager.instance.player.GetChild(i).transform.position;
                GameManager.instance.player.GetChild(i + 1).GetComponent<PlayerMovement>().orientation = GameManager.instance.player.GetChild(i).GetComponent<PlayerMovement>().orientation;
                GameManager.instance.player.GetChild(i).gameObject.SetActive(false);
                GameManager.instance.player.GetChild(i + 1).gameObject.SetActive(true);
                GameManager.instance.activePlayer = GameManager.instance.player.GetChild(i + 1);
                return;
            }
        }
    }
}
