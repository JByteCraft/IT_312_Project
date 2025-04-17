using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField] private GameObject[] traps;
    private Vector3[] initialPosition;

    private void Awake()
    {
        initialPosition = new Vector3[traps.Length];

        for (int i = 0; i < traps.Length; i++)
        {
            if (traps[i] != null)
            {
                initialPosition[i] = traps[i].transform.position;
            }
        }
    }

    public void ActivateRoom(bool _status)
    {
        for (int i = 0; i < traps.Length; i++)
        {
            if (traps[i] != null)
            {
                traps[i].SetActive(_status);
                traps[i].transform.position = initialPosition[i];
            }
        }
    }
}
