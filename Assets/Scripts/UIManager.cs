using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class UIManager : MonoBehaviourPunCallbacks
{
    public override void OnEnable()
    {
        PhotonNetwork.Instantiate("UICommunications", new Vector3(0, 0, 0), Quaternion.identity, 0);
    }

    
}
