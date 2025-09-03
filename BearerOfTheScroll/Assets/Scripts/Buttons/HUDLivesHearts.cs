using UnityEngine;

public class HUDLivesHearts : MonoBehaviour
{
    [SerializeField] private FallManager fallManager;

    [Header("Hearts")]
    [SerializeField] private GameObject[] hearts; // Heart_1, Heart_2, Heart_3

    private void OnEnable()
    {
        if (fallManager != null)
            fallManager.OnLivesChanged += UpdateHearts;
    }

    private void OnDisable()
    {
        if (fallManager != null)
            fallManager.OnLivesChanged -= UpdateHearts;
    }

    private void Start()
    {
        if (fallManager != null)
            UpdateHearts(fallManager.Lives);
    }

    private void UpdateHearts(int currentLives)
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (hearts[i] != null)
                hearts[i].SetActive(i < currentLives);
        }
    }
}
