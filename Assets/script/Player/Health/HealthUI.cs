using System;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    [Header("Références")] 
    public HealthSystem HealthSystem;
    public GameObject heartPrefab;
    public Transform heartsContainer;

    [Header("Sprites")] 
    public Sprite heartFull;
    public Sprite heartHalf;
    public Sprite heartEmpty;

    private Image[] heartImages;


    private void Start()
    {
        BuildHearts();
        HealthSystem.onHealthChanged.AddListener(UpdateHearts);
    }

    private void UpdateHearts()
    {
        int halfHearts = HealthSystem.GetCurrentHalfHearts();

        for (int i = 0; i < heartImages.Length; i++)
        {
            int heartValue = halfHearts - (i * 2);

            if (heartValue >= 2)
                heartImages[i].sprite = heartFull;
            else if (heartValue == 1)
                heartImages[i].sprite = heartHalf;
            else
                heartImages[i].sprite = heartEmpty;
        }
    }

    private void BuildHearts()
    {
        foreach (Transform child in heartsContainer)
        {
            Destroy(child.gameObject);
        }

        heartImages = new Image[HealthSystem.GetMaxHearts()];

        for (int i = 0; i < HealthSystem.GetMaxHearts(); i++)
        {
            GameObject heart = Instantiate(heartPrefab, heartsContainer);
            heartImages[i] = heart.GetComponent<Image>();
        }
        
        UpdateHearts();
    }
}
