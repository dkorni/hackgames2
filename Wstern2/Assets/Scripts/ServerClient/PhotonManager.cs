using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class PhotonManager : MonoBehaviour
{
    public string Version;

    public Transform[] Spawns;

    public GameObject PlayerPrefab;

    public float WaitTimer = 3;

    private Text _spawnTimerText;

    public void Connect()
    {
        PhotonNetwork.ConnectUsingSettings(Version);
        DontDestroyOnLoad(this);
    }

    private void OnJoinedLobby()
    {
        Debug.Log($"Joined to {PhotonNetwork.lobby.Name} lobby");

        var roomOptions = new RoomOptions();

        roomOptions.maxPlayers = 20;


        PhotonNetwork.JoinOrCreateRoom("Room", roomOptions, TypedLobby.Default);
        PhotonNetwork.LoadLevel(1);
    }

    private void OnJoinedRoom()
    {

        _spawnTimerText = GameObject.Find("spawnTimerText").GetComponent<Text>();

        var spawnGameobjects = GameObject.FindGameObjectsWithTag("Spawn");

        Spawns = spawnGameobjects.Select(s => s.GetComponent<Transform>()).ToArray();

       Debug.Log($"Joined to {PhotonNetwork.lobby.Name} room");
        StartCoroutine(Spawn());
    }

    private IEnumerator Spawn()
    {
        while (WaitTimer > 0)
        {
            yield return new WaitForSeconds(1);
            WaitTimer--;
            _spawnTimerText.text = $"You will be spawned after {WaitTimer.ToString()}";           
        }

        _spawnTimerText.gameObject.SetActive(false);

        var GO = PhotonNetwork.Instantiate(PlayerPrefab.name, Spawns[Random.RandomRange(0, Spawns.Length - 1)].position, Quaternion.identity,0);

    }


   
}
