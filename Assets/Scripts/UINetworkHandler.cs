using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class UINetworkHandler : MonoBehaviourPunCallbacks, IPunObservable
{

    public GameObject SliderGO;
    private GameObject playerGO;
    
    
    
    private Slider slider;
    private float sliderValue;
    private float neutralPlayerPos;
    
    public float level = 0.5f;
    
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            if (SliderGO != null)
            {
                sliderValue = slider.value;
                Debug.Log(sliderValue);
                stream.Serialize(ref sliderValue);
            }
            else
            {
                SliderGO = GameObject.FindWithTag("CoopSlider");
                if (SliderGO == null)
                {
                    throw new MissingComponentException();
                }

                Debug.Log("Settou GameObject da UI");
                slider = SliderGO.GetComponent<Slider>();
            }
        }
        else
        {
            stream.Serialize(ref sliderValue);
            if (playerGO != null)
            {
                var pos = playerGO.transform.position;
                pos = new Vector3(pos.x, neutralPlayerPos + (sliderValue -0.5f)* 3, pos.z);
                playerGO.transform.position = pos;
                Debug.Log(pos);
            }
            else
            {
                playerGO = GameObject.FindWithTag("Player");
                Debug.Log("Settou GameObject do Jogador");
                neutralPlayerPos = playerGO.transform.position.y;
                Debug.Log(playerGO.name);
            }
        }
    }
}
