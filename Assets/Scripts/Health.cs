using UnityEngine;
using UnityEngine.UIElements;

public class Health : MonoBehaviour
{
    public int health = 100;
    public int maxHealth = 100;
    private Alliance a;
        private UIDocument overlay;
    private VisualElement healthBarContainer;
    private VisualElement subHealthBar;
    private VisualElement healthBar;
    private Label healthLabel;
    private float displayedHealth;
    public float subBarDecreaseSpeed = 20f;

    
    void Start() {
        a = GetComponent<Alliance>();
        overlay = GameObject.Find("Overlay").GetComponent<UIDocument>();

        setupHealthBar();
    }

    
    void setupHealthBar()
    {
        var root = overlay.rootVisualElement;

        healthBarContainer = new VisualElement();
        healthBarContainer.style.position = Position.Absolute;
        healthBarContainer.style.width = 100;
        healthBarContainer.style.height = 20;
        healthBarContainer.style.backgroundColor = new Color(0, 0, 0, 0.5f);

        subHealthBar = new VisualElement();
        subHealthBar.style.width = Length.Percent(100);
        subHealthBar.style.height = Length.Percent(100);
        subHealthBar.style.backgroundColor = new Color(1, 1, 1, 0.5f); // Semi-transparent white for the sub-bar

        healthBar = new VisualElement();
        healthBar.style.width = Length.Percent(100);
        healthBar.style.height = Length.Percent(100);
        healthBar.style.backgroundColor = Color.green;

        healthLabel = new Label();
        healthLabel.style.position = Position.Absolute;
        healthLabel.style.width = 100;
        healthLabel.style.height = 20;
        healthLabel.style.color = Color.white;
        healthLabel.style.unityTextAlign = TextAnchor.MiddleCenter;

        healthBarContainer.Add(subHealthBar);
        healthBarContainer.Add(healthBar);
        healthBarContainer.Add(healthLabel);
        root.Add(healthBarContainer);

        // Set initial health
        health = maxHealth;
        displayedHealth = health;
        UpdateHealthBar();
    }

    void UpdateHealthBar()
    {
        // Calculate health percentage
        float healthPercentage = health / maxHealth;
        healthBar.style.width = new Length(healthPercentage * 100f, LengthUnit.Percent);
        healthLabel.text = $"{health} / {maxHealth}";

    }

    void UpdateHealthBarPosition()
    {
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(transform.position + Vector3.up * 2f); // Offset the health bar above the target
        healthBarContainer.style.left = screenPosition.x - healthBarContainer.resolvedStyle.width / 2;
        healthBarContainer.style.top = Screen.height - screenPosition.y;
    }

     void UpdateSubHealthBar()
    {
        float displayedHealthPercentage = displayedHealth / maxHealth;
        subHealthBar.style.width = new Length(displayedHealthPercentage * 100f, LengthUnit.Percent);
    }

    void Update()
    {
        if (health <= 0) {
            HandleDeath();
        }
        else {
            UpdateHealthBar();
            UpdateHealthBarPosition();


            if (displayedHealth > health)
            {
                displayedHealth -= subBarDecreaseSpeed * Time.deltaTime;
                if (displayedHealth < health)
                {
                    displayedHealth = health;
                }
                UpdateSubHealthBar();
            }
        }
    }

    void HandleDeath() {
        if (healthBarContainer != null)
        {
            healthBarContainer.RemoveFromHierarchy();
        }

        if (a.alliance == 0) {
            BaseEnemy e = GetComponent<BaseEnemy>();
            e.HandleDeath();
        }
        else {
            PlayerNPC p = GetComponent<PlayerNPC>();
            p.HandleDeath();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // if (hasPainted == false) {
        //     Alliance a = other.gameObject.GetComponent<Alliance>();

        //     if (a) {
        //         if (a.alliance != spawningAlliance) {
        //             hasPainted = true;
        //             HandleDoingDamage(a);
        //         }
        //     }
        // }
    }
}
