using UnityEngine;

public class HexAvailabilityController : MonoBehaviour
{
    [SerializeField] private Color defaultColor = Color.green;
    [SerializeField] private Color availableColor = Color.cyan;

    private Renderer rend;
    private bool isAvailable = false;

    private void Awake()
    {
        rend = GetComponent<Renderer>();
        if (rend != null)
            rend.material.color = defaultColor;
    }

    public void SetAvailable(bool value)
    {
        isAvailable = value;

        if (rend == null) return;

        Debug.Log($"SetAvailable: {gameObject.name} => {value}");

        if (isAvailable)
            rend.material.color = availableColor;
        else
            rend.material.color = defaultColor;
    }

    public bool IsAvailable()
    {
        return isAvailable;
    }
}
