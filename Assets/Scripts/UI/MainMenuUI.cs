using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    private GameManager GameManager => ManagerLocator.Get<GameManager>();
    private PlayerManager PlayerManager => ManagerLocator.Get<PlayerManager>();
    
    [SerializeField] private InputField initialsInputField;
    [SerializeField] private Button startGameButton;
    
    public void SetPlayerInitials(string initials)
    {
        startGameButton.interactable = initials.Length > 0;
        PlayerManager.SetPlayerInitials(initials);
    }

    public void StartGame()
    {
        GameManager.StartGame();
    }
}
