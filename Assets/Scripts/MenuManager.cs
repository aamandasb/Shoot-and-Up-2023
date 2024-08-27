using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] GameObject registerWindow;
    [SerializeField] Text playerNameUI;
    [SerializeField] InputField playerField;
    [SerializeField] Button confirmPlayerName;


    [SerializeField] TextMeshProUGUI nicknameUI, roomNameUI, playerList;

    [SerializeField] Button joinButton, createButton, leaveButton, startButton;

    [SerializeField] GameObject menu, lobby;

    public static MenuManager instance;

    private void Awake()
    {
        #region Singleton
        // Verifica se a instância é nula
        if (instance == null)
        {
            instance = this; // Define a instância para este objeto
        }
        else if (instance != this)
        {
            Destroy(gameObject); // Destroi o objeto se já houver uma instância existente
        }
        #endregion

        joinButton.onClick.AddListener(JoinRoom);
        createButton.onClick.AddListener(CreateRoom);
        leaveButton.onClick.AddListener(LeaveRoom);
        startButton.onClick.AddListener(delegate { StartGame("SampleScene"); });

        // Define os botões joinButton e createButton como não interativos inicialmente.
        joinButton.interactable = false;
        createButton.interactable = false;

        // Chama o método SwitchWindow para exibir o menu inicial.
        SwitchWindow(false);
    }

    private void Start()
    {
        
        registerWindow.SetActive(!PlayerPrefs.HasKey("PlayerName"));
        playerNameUI.text = PlayerStats.playerName;
        Debug.Log(PlayerStats.playerName);

        //AdsManager.instance.SetBanner(true);
    }

    public void LoadScene(string sceneName)
    {
        GameManager.instance.LoadScene(sceneName);
        //AdsManager.instance.SetBanner(false);
    }

    public void SavePlayerName()
    {
        PlayerStats.playerName = playerField.text;
        PlayerPrefs.SetString("PlayerName", PlayerStats.playerName);
        playerNameUI.text = PlayerStats.playerName;
    }

    public void CheckPlayerName()
    {
        confirmPlayerName.interactable = playerField.text != "";
    }

    public void ResetSave()
    {
        PlayerPrefs.DeleteAll();
        registerWindow.SetActive(true);
    }

    public void RequestAdCoin()
    {
        GameManager.instance.RequestAdCoin();
    }

    public void Connected()
    {
        joinButton.interactable = true;
        createButton.interactable = true;
    }

    // Método público que atualiza a lista de jogadores na interface.
    public void UpdatePlayerList(string list)
    {
        playerList.text = list;
    }

    // Método privado que é chamado quando joinButton é clicado.
    private void JoinRoom()
    {
        // Chama o método JoinRoom no NetworkManager para se juntar a uma sala.
        NetworkManager.instance.JoinRoom(roomNameUI.text, nicknameUI.text);
        // Alterna para a janela do lobby.
        SwitchWindow(true);
    }

    // Método privado que é chamado quando createButton é clicado.
    private void CreateRoom()
    {
        // Chama o método CreateRoom no NetworkManager para criar uma sala.
        NetworkManager.instance.CreateRoom(roomNameUI.text, nicknameUI.text);
        // Alterna para a janela do lobby.
        SwitchWindow(true);
    }

    // Método privado que é chamado quando leaveButton é clicado.
    private void LeaveRoom()
    {
        // Chama o método LeaveRoom no NetworkManager para sair da sala.
        NetworkManager.instance.LeaveRoom();
        // Alterna para a janela do menu.
        SwitchWindow(false);
    }

    // Método público que inicia o jogo, carregando a cena especificada.
    public void StartGame(string sceneName)
    {
        NetworkManager.instance.LoadScene(sceneName);
    }

    // Método público que define se o botão startButton está interativo com base no valor de isMaster.
    public void SetStartButton(bool isMaster)
    {
        startButton.interactable = isMaster;
    }

    // Método público que alterna entre o menu e a janela do lobby.
    public void SwitchWindow(bool onLobby)
    {
        // Define a visibilidade dos GameObjects menu e lobby.
        menu.SetActive(!onLobby);
        lobby.SetActive(onLobby);
    }
}
