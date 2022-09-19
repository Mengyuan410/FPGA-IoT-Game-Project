using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimap : MonoBehaviour
{
    public Transform players;
    // Start is called before the first frame update
    private void LateUpdate()
    {
        Vector3 newPosition = players.position;
        newPosition.z = transform.position.z;
        transform.position = newPosition;
    }
}
