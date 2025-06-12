using UnityEngine;

public class PlayerManager : MonoBehaviour, IManager
{
    public string PlayerInitials { get; private set; }

    public void SetPlayerInitials(string initials)
    {
        PlayerInitials = initials;
    }
}
