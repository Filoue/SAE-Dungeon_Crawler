using UnityEngine;

public class RoomChain : MonoBehaviour
{

    [SerializeField] private SO_RoomStereotype _startRoom;
    
    private SO_RoomStereotype _currentRoom;
    
    private void Start()
    {
        
    }
    
    private void OntriggerEnter(Collider other)
    {
        
    }
    
    private void NextRoom()
    {
        _currentRoom = _currentRoom.NextRoom();
    }

}
