using System.Collections;
using System.Collections.Generic;
using System;
using System.Collections;


using UnityEngine;
using UnityEngine.SceneManagement;


using Photon.Pun;
using Photon.Realtime;


public class GameManager : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    

    public override void OnLeftRoom(){
        SceneManager.LoadScene(0);
    }

    // Update is called once per frame
    public void LeaveRoom(){
        PhotonNetwork.LeaveRoom();
        
    }

    void LoadLevel()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            Debug.LogError("Deve ser o cliente master para carregar a fase!");
        }
            Debug.LogFormat("PhotonNetwork : Carregando Nível");
        PhotonNetwork.LoadLevel(1);
    }
}
