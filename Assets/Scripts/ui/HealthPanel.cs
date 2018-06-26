using UnityEngine;

public class HealthPanel: MonoBehaviour {

    
    [SerializeField]
    private GameObject emptyHeartContainer;

    [SerializeField]
    private GameObject fullHeartContainer;

    void Start() {
        // Get the player - we'll subscribe to its hit event so that
        // we can update health bar accordingly.
        Health playerHealth = FetchUtils.FetchGameObjectByTag(Tags.PLAYER).GetComponent<Health>();

        playerHealth.OnHit += OnHit;
        UpdateHeartContainers(playerHealth.CurrentHealth(), playerHealth.MaxHealth());
    }

    private void UpdateHeartContainers(int currentHealth, int maxHealth) {
        int fullContainers = currentHealth;
        int emptyContainers = currentHealth < maxHealth ? maxHealth - currentHealth : 0;

        // Remove all children
        foreach (Transform child in transform) {
            Destroy(child.gameObject);
        }

        // Add them back!
        for (int i = 0; i < fullContainers; i++) {
            Instantiate(fullHeartContainer, new Vector2(0,0), Quaternion.identity, transform);
        }
        for (int i = 0; i < emptyContainers; i++) {
            Instantiate(emptyHeartContainer, new Vector2(0,0), Quaternion.identity, transform);
        }

        // Calculate new width for the panel. More hearts need to increase the size of the panel.
        // Hearts are 24pixel wide (1 unit basically, so 1.1xnumber of hearts to leave some space in between)
        RectTransform rt = (RectTransform) transform;
        float newWidth = 1.2f * 24 * (fullContainers + emptyContainers);
        rt.sizeDelta = new Vector2(newWidth, rt.rect.height);
    }

    private void OnHit(Health health) {
        UpdateHeartContainers(health.CurrentHealth(), health.MaxHealth());
    }
}
