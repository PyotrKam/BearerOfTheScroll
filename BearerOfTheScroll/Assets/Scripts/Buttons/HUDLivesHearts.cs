using UnityEngine;
using UnityEngine.UI;

public class HUDLivesHearts : MonoBehaviour
{
    [SerializeField] private FallManager fallManager;   
    [SerializeField] private GameObject[] hearts;       

    private bool subscribed;

    private void Awake()
    {       
        EnsureFallManagerRef();
    }

    private void OnEnable()
    {
        EnsureFallManagerRef();
        TrySubscribe();
        ForceRefresh();
    }

    private void OnDisable()
    {
        TryUnsubscribe();
    }

    private void EnsureFallManagerRef()
    {
        
        if (fallManager == null || !fallManager.gameObject.scene.IsValid())
        {
            fallManager = FindObjectOfType<FallManager>(); 
        }
    }

    private void TrySubscribe()
    {
        if (fallManager != null && !subscribed)
        {
            fallManager.OnLivesChanged += UpdateHearts;
            subscribed = true;
        }
    }

    private void TryUnsubscribe()
    {
        if (fallManager != null && subscribed)
        {
            fallManager.OnLivesChanged -= UpdateHearts;
            subscribed = false;
        }
    }

    private void Start()
    {
        ForceRefresh();
    }

    private void ForceRefresh()
    {
        if (fallManager != null)
            UpdateHearts(fallManager.Lives);
        else
            ShowAllHearts(); 
    }

    private void ShowAllHearts()
    {
        for (int i = 0; i < hearts.Length; i++)
            if (hearts[i]) hearts[i].SetActive(true);
    }

    private void UpdateHearts(int currentLives)
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (!hearts[i]) continue;
            var img = hearts[i].GetComponent<Image>();
            if (img != null)
            {
                var c = img.color;
                c.a = (i < currentLives) ? 1f : 0.3f; 
                img.color = c;
            }
        }
    }
}
