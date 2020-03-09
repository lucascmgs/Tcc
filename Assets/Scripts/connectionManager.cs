using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class connectionManager : MonoBehaviourPunCallbacks
{
    #region UIHandles
    public GameObject statusText;
    public GameObject NickNameInput;

    public GameObject RoomNameInput;

    public GameObject CreateButton;
    public GameObject JoinButton;
    
    public GameObject ConfirmButton;
    public GameObject CancelButton;

    #endregion

    private RoomOptions roomOptions = new RoomOptions();

    private enum Mode {Creating, Joining, Joined};

    private Mode startMode;

    private Color grayColor = new Color(0.5f, 0.5f, 0.5f);
    private Color whiteColor = new Color(1, 1, 1);
    
    #region Status message helper
    private void SetStatusMessage(string message)
    {
        statusText.GetComponent<Text>().text = message;
    }

    private string statusMessage
    {
        get { return statusText.GetComponent<Text>().text; }
        set
        {
            SetStatusMessage(value);
        }
    }
    
    #endregion

    void Start()
    {
        roomOptions.IsVisible = false;
        roomOptions.MaxPlayers = 2;
        PhotonNetwork.ConnectUsingSettings();
        
    }

    #region PUN connection callbacks
    public override void OnCreatedRoom()
    {
        statusMessage = "Created room\n";
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        statusMessage = message;

    }

    public override void OnJoinedRoom()
    {
        startMode = Mode.Joined;
        statusMessage += "Entered room\n";
        LogCurrentPlayers();

        StartCoroutine(WaitPlayer());
    }
    
    

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        statusMessage = message;
    }


    public override void OnConnectedToMaster(){
        Debug.Log("Connected to region: " + PhotonNetwork.CloudRegion + "\n");
        ConfirmButton.GetComponent<Image>().color = whiteColor;
    }

    #endregion

    private void LogCurrentPlayers()
    {
        statusMessage += PhotonNetwork.CurrentRoom.Players.Count + " Players present:\n";
        foreach (var player in PhotonNetwork.CurrentRoom.Players)
        {
            statusMessage += (player.Value.NickName + "\n");
        }
    }
    
    
    private void CreateRoomMode(){
        startMode = Mode.Creating;
        JoinButton.GetComponent<Image>().color = whiteColor;
        CreateButton.GetComponent<Image>().color = grayColor;
        ConfirmButton.SetActive(true);
        CancelButton.SetActive(true);
        RoomNameInput.SetActive(true);
    }

    private void JoinRoomMode(){
        startMode = Mode.Joining;
        JoinButton.GetComponent<Image>().color = grayColor;
        CreateButton.GetComponent<Image>().color = whiteColor;
        ConfirmButton.SetActive(true);
        CancelButton.SetActive(true);
        RoomNameInput.SetActive(true);
    }

    public void Confirm(){
        if(!PhotonNetwork.IsConnected){
            Debug.Log("Wait until connection!");
            return;
        }
        var roomName = RoomNameInput.GetComponent<InputField>().text;
        switch (startMode)
        {
            case Mode.Creating:
                CreateRoom(roomName);
                break;
            case Mode.Joining:
                JoinRoom(roomName);
                break;
            case Mode.Joined:
                break;
        }

    }

    private void CreateRoom(string roomName){
        PhotonNetwork.CreateRoom(roomName, roomOptions);
    }

    private void JoinRoom(string roomName){
        PhotonNetwork.JoinRoom(roomName);
    }

    public void Cancel(){
        CreateButton.SetActive(true);
        JoinButton.SetActive(true);
        
        CreateButton.GetComponent<Image>().color = whiteColor;
        JoinButton.GetComponent<Image>().color = whiteColor;
        RoomNameInput.SetActive(false);
        ConfirmButton.SetActive(false);

    }

    public void SetNickName(){
        var nickName = NickNameInput.GetComponent<InputField>().text;
        
        PhotonNetwork.NickName = nickName;
    }

    private IEnumerator WaitPlayer()
    {
        statusMessage += "Waiting for the other player\n";
        while (PhotonNetwork.CurrentRoom.PlayerCount < 2)
        {
            yield return new WaitForSeconds(0.1f);
        }
        if (PhotonNetwork.IsMasterClient)
        {
            SceneManager.LoadScene(1);
        }
        else
        {
            SceneManager.LoadScene(2);
        }
    }
    
}
