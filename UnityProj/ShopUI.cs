using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System;
using Unity.VisualScripting;

public class ShopUI : MonoBehaviour
{
    // UI Elements

    public TMP_Text upgradeNameText;      // Text for displaying the upgrade's name
    public TMP_Text upgradeDescriptionText;  // Text for displaying the upgrade's description
    public TMP_Text upgradeCostText;  // Text for displaying the upgrade's cost
    public Sprite[] upgradesSprites;
    public Image upgradeImage;        // Image for displaying the upgrade icon

    public bool[] staticUpgrades;

    public Button[] upgradeButtons;    // Buttons for upgrading each item in the shop

    private int selectedUpgradeIndex = -1;  // Index of the currently selected upgrade


    private void OnEnable()
    {
        SetStaticUpgrades();

        for (int i = 0; i < upgradeButtons.Length; i++)
        {
            int index = i; 
            upgradeButtons[i].onClick.AddListener(() => OnUpgradeButtonClick(index));
        }
        UpgradeButton.OnButtonHover += HandleButtonHover;
        CollorSprites(0);
        selectedUpgradeIndex = 0;///or set diffualt to nononononnoe
        UpdateUpgradeInfo();
        UpdateShopUI();
    }


    private void UpdateShopUI()
    {
        
        for (int i = 0; i < upgradeButtons.Length; i++)
        {
            Debug.Log(i + "  ---- " + IsUpgradeAffordable(i)  +" " +!IsMaxedUpgrade(i));
            upgradeButtons[i].interactable = IsUpgradeAffordable(i) && !IsMaxedUpgrade(i);
        }
  
    }

    private bool IsUpgradeAffordable(int index)
    {
       
        int cost = 0;
        if (index == 0) 
        {
            cost = GetUpgradeCost("Health Restore");
        }
        else if (index == 1) 
        {
            cost = GetUpgradeCost("Shield Restore");
        }
        else if (index == 2)
        {
            cost = GetUpgradeCost("Shield Upgrade I");
        }
        else if (index == 3)
        {
            if (staticUpgrades[index - 3])
                return false;
            cost = GetUpgradeCost("Shield Upgrade II");
        }
        else if (index == 4)
        {
            cost = GetUpgradeCost("Engine Upgrade I");
        }
        else if (index == 5)
        {
            if (staticUpgrades[index - 3])
                return false;
            cost = GetUpgradeCost("Engine Upgrade II");
        }
        else if (index == 6)
        {
            if (staticUpgrades[index - 3])
                return false;
            cost = GetUpgradeCost("Engine Upgrade III");
        }
        else if (index == 7)
        {
            if (staticUpgrades[8])
                return false;
            cost = GetUpgradeCost("Projectile Upgrade I");
        }
        else if (index == 8)
        {
            if (staticUpgrades[index - 3] || staticUpgrades[9])
                return false;
            cost = GetUpgradeCost("Projectile Upgrade II");
        }
        else if (index == 9)
        {
            if (staticUpgrades[index - 3] || staticUpgrades[10])
                return false;
            cost = GetUpgradeCost("Projectile Upgrade III");
        }
        else if (index == 10)
        {
            cost = GetUpgradeCost("Weapon Upgrade I");
        }
        else if (index == 11)
        {
            if (staticUpgrades[index - 3])
                return false;
            cost = GetUpgradeCost("Weapon Upgrade II");
        }
        else if (index == 12)
        {
            if (staticUpgrades[index-3])
                return false;
            cost = GetUpgradeCost("Weapon Upgrade III");
        }

        return GameManager.Instance.CurrentCurrency >= cost;
    }

    private bool IsMaxedUpgrade(int index)
    {
        
        
        if (index == 0) 
        {
            Debug.Log(" Noissim " + UpgradeManager.Instance.Maxhealth + " " + GameManager.Instance.playerHealth);
            return UpgradeManager.Instance.Maxhealth <= GameManager.Instance.playerHealth;  
        }
        else if (index == 1) 
        {
            Debug.Log(" Nissim " + UpgradeManager.Instance.MaxShield + " " + GameManager.Instance.playerShield);
            return UpgradeManager.Instance.MaxShield <= GameManager.Instance.playerShield ; 
        }
        else
        {
            return !staticUpgrades[index-2];
        }


    }

    private int GetUpgradeCost(string upgradeName)
    {
        
        if (upgradeName == "Health Restore")
        {
            return (500);  
        }
        else if (upgradeName == "Shield Restore")
        {
            return (1000);
        }
        else if (upgradeName == "Shield Upgrade I")
        {
            return (3000);
        }
        else if (upgradeName == "Shield Upgrade II")
        {
            return (5000);
        }
        else if (upgradeName == "Engine Upgrade I")
        {
            return (4000);
        }
        else if (upgradeName == "Engine Upgrade II")
        {
            return (6000);
        }
        else if (upgradeName == "Engine Upgrade III")
        {
            return (7500);
        }
        else if (upgradeName == "Projectile Upgrade I")
        {
            return (5000);
        }
        else if (upgradeName == "Projectile Upgrade II")
        {
            return (8000);
        }
        else if (upgradeName == "Projectile Upgrade III")
        {
            return (13500);
        }
        else if (upgradeName == "Weapon Upgrade I")
        {
            return (5000);
        }
        else if (upgradeName == "Weapon Upgrade II")
        {
            return (8000);
        }
        else if (upgradeName == "Weapon Upgrade III")
        {
            return (11500);
        }

        return 0;
    }

    private void OnUpgradeButtonClick(int index)
    {
        
        if (index == 0 && IsUpgradeAffordable(index) && !IsMaxedUpgrade(index))
        {
           
            GameManager.Instance.playerHealth++;
            UIManager.Instance.ChangeHealth(true);
            GameManager.Instance.CurrentCurrency -= GetUpgradeCost("Health Restore");
            Debug.Log("Health restored");

        }
        else if (index == 1 && IsUpgradeAffordable(index) && !IsMaxedUpgrade(index))
        {
            GameManager.Instance.playerShield++;
            UIManager.Instance.ChangeShield(true);
            GameManager.Instance.CurrentCurrency -= GetUpgradeCost("Shield Restore");
            Debug.Log("Shield restored");

        }
        else if (index == 2 && IsUpgradeAffordable(index) && !IsMaxedUpgrade(index))
        {
            UpgradeManager.Instance.MaxShield = 3;
            UIManager.Instance.ChangeMaxShield();
            staticUpgrades[index - 2] = false;
            GameManager.Instance.CurrentCurrency -= GetUpgradeCost("Shield Upgrade I");
            Debug.Log("Shield Cap Increased: 1 -> 3");

        }
        else if (index == 3 && IsUpgradeAffordable(index) && !IsMaxedUpgrade(index))
        {
            UpgradeManager.Instance.MaxShield = 5;
            UIManager.Instance.ChangeMaxShield();
            staticUpgrades[index - 2] = false;
            GameManager.Instance.CurrentCurrency -= GetUpgradeCost("Shield Upgrade II");
            Debug.Log("Shield Cap Increased: 3 -> ");

        }
        else if (index == 4 && IsUpgradeAffordable(index) && !IsMaxedUpgrade(index))
        {
            GameManager.Instance.setPlayerSprite(index);
           // UpgradeManager.Instance.movementSpd += 2;
            staticUpgrades[index - 2] = false;
            GameManager.Instance.CurrentCurrency -= GetUpgradeCost("Engine Upgrade I");
            Debug.Log("Engine Upgraded! Movement speed Increased: " + (UpgradeManager.Instance.movementSpd - 2f) + " -> " + UpgradeManager.Instance.movementSpd);

        }
        else if (index == 5 && IsUpgradeAffordable(index) && !IsMaxedUpgrade(index))
        {
            GameManager.Instance.setPlayerSprite(index);
           // UpgradeManager.Instance.movementSpd += 3;
            staticUpgrades[index - 2] = false;
            GameManager.Instance.CurrentCurrency -= GetUpgradeCost("Engine Upgrade II");
            Debug.Log("Engine Upgraded! Movement speed Increased: " + (UpgradeManager.Instance.movementSpd - 3f) + " -> " + UpgradeManager.Instance.movementSpd);

        }
        else if (index == 6 && IsUpgradeAffordable(index) && !IsMaxedUpgrade(index))
        {
            GameManager.Instance.setPlayerSprite(index);
           // UpgradeManager.Instance.movementSpd += 5;
            staticUpgrades[index - 2] = false;
            GameManager.Instance.CurrentCurrency -= GetUpgradeCost("Engine Upgrade III");
            Debug.Log("Engine Upgraded! Movement speed Increased: " + (UpgradeManager.Instance.movementSpd - 5f) + " -> " + UpgradeManager.Instance.movementSpd);

        }
        else if (index == 7 && IsUpgradeAffordable(index) && !IsMaxedUpgrade(index))
        {
            GameManager.Instance.setPlayerSprite(index - 2);
           // UpgradeManager.Instance.projectileSpd += 2;
            staticUpgrades[index - 2] = false;
            GameManager.Instance.CurrentCurrency -= GetUpgradeCost("Projectile Upgrade I");
            Debug.Log("Projectile Upgraded! Projectile speed Increased: " + (UpgradeManager.Instance.projectileSpd - 2f) + " -> " + UpgradeManager.Instance.projectileSpd);

        }
        else if (index == 8 && IsUpgradeAffordable(index) && !IsMaxedUpgrade(index))
        {
            GameManager.Instance.setPlayerSprite(index);
           // UpgradeManager.Instance.projectileSpd += 3;
            staticUpgrades[index - 2] = false;
            GameManager.Instance.CurrentCurrency -= GetUpgradeCost("Projectile Upgrade II");
            Debug.Log("Projectile Upgraded! Projectile speed Increased: " + (UpgradeManager.Instance.projectileSpd - 3f) + " -> " + UpgradeManager.Instance.projectileSpd);

        }
        else if (index == 9 && IsUpgradeAffordable(index) && !IsMaxedUpgrade(index))
        {
            GameManager.Instance.setPlayerSprite(index);
           // UpgradeManager.Instance.projectileSpd += 5;
            staticUpgrades[index - 2] = false;
            GameManager.Instance.CurrentCurrency -= GetUpgradeCost("Projectile Upgrade III");
            Debug.Log("Projectile Upgraded! Projectile speed Increased: " + (UpgradeManager.Instance.projectileSpd - 5f) + " -> " + UpgradeManager.Instance.projectileSpd);

        }
        else if (index == 10 && IsUpgradeAffordable(index) && !IsMaxedUpgrade(index))
        {
            GameManager.Instance.setPlayerSprite(index);
            //UpgradeManager.Instance.damage += 2;
            staticUpgrades[index - 2] = false;
            GameManager.Instance.CurrentCurrency -= GetUpgradeCost("Weapon Upgrade I");
            Debug.Log("Weapon Upgraded! Damage Increased: " + (UpgradeManager.Instance.damage - 2f) + " -> " + UpgradeManager.Instance.damage);

        }
        else if (index == 11 && IsUpgradeAffordable(index) && !IsMaxedUpgrade(index))
        {
            GameManager.Instance.setPlayerSprite(index);
            //UpgradeManager.Instance.damage += 3;
            staticUpgrades[index - 2] = false;
            GameManager.Instance.CurrentCurrency -= GetUpgradeCost("Weapon Upgrade II");
            Debug.Log("Weapon Upgraded! Damage Increased: " + (UpgradeManager.Instance.damage - 3f) + " -> " + UpgradeManager.Instance.damage);

        }
        else if (index == 12 && IsUpgradeAffordable(index) && !IsMaxedUpgrade(index))
        {
            GameManager.Instance.setPlayerSprite(index);
            //UpgradeManager.Instance.damage += 5;
            staticUpgrades[index - 2] = false;
            GameManager.Instance.CurrentCurrency -= GetUpgradeCost("Weapon Upgrade III");
            Debug.Log("Weapon Upgraded! Damage Increased: " + (UpgradeManager.Instance.damage - 5f) + " -> " + UpgradeManager.Instance.damage);

        }
        UIManager.Instance.ChangeCash();
        UpdateShopUI();
    }

    public void HandleButtonHover(int index)
    {
        // Get the index of the hovered upgrade and display details
        selectedUpgradeIndex = index;
        UpdateUpgradeInfo();
    }

    private void UpdateUpgradeInfo()
    {
        string temp = "";
        if (selectedUpgradeIndex >= 0)
        {
            
            upgradeImage.sprite = upgradesSprites[selectedUpgradeIndex];
            CollorSprites(selectedUpgradeIndex);

            if (selectedUpgradeIndex == 0) 
            {
                upgradeNameText.text = "Health Restore";
                upgradeDescriptionText.text = "Restore the player hp by 1. (Capped at max hp: " + UpgradeManager.Instance.Maxhealth + ").";
                upgradeCostText.text = GetUpgradeCost(upgradeNameText.text).ToString();
                
            }
            else if (selectedUpgradeIndex == 1) 
            {
                upgradeNameText.text = "Shield Restore";
                upgradeDescriptionText.text = "Restore the player hp by 1. (Capped at max shield: " + UpgradeManager.Instance.MaxShield + ").";
                upgradeCostText.text = GetUpgradeCost(upgradeNameText.text).ToString();
            }
            else if (selectedUpgradeIndex == 2)
            {
                temp = "Shield Upgrade I";
                upgradeNameText.text = "Turtle Shell Defense";
                upgradeDescriptionText.text = "Engineered by studying the natural armor of turtles, this upgrade incorporates advanced space-grade materials, mimicking the turtle's resilient shell.\nThis upgrade will increase shield cap to 3";
                upgradeCostText.text = GetUpgradeCost(temp).ToString();
            }
            else if (selectedUpgradeIndex == 3)
            {
                temp = "Shield Upgrade II";
                upgradeNameText.text = "Pangolin Shell Defense ";
                upgradeDescriptionText.text = "Crafted after the impenetrable scales of a pangolin, this upgrade combines cutting-edge alloys with advanced energy-absorbing technology.\nThis upgrade will increase shield cap to 5";
                upgradeCostText.text = GetUpgradeCost(temp).ToString();
            }
            else if (selectedUpgradeIndex == 4)
            {
                temp = "Engine Upgrade I";
                upgradeNameText.text = "Pulse Engine";
                upgradeDescriptionText.text = "Reliable and efficient, the Pulse Engine offers steady propulsion, ideal for maneuvering through space with precision.";
                upgradeCostText.text = GetUpgradeCost(temp).ToString();
            }
            else if (selectedUpgradeIndex == 5)
            {
                temp = "Engine Upgrade II";
                upgradeNameText.text = "Burst Engine";
                upgradeDescriptionText.text = "With a burst of power, the Burst Engine delivers rapid acceleration, propelling you faster than most ships can handle in short bursts.";
                upgradeCostText.text = GetUpgradeCost(temp).ToString();
            }
            else if (selectedUpgradeIndex == 6)
            {
                temp = "Engine Upgrade III";
                upgradeNameText.text = "LightSpeed Engine";
                upgradeDescriptionText.text = "Unlock the impossible with the Lightspeed Engine, bending space-time itself to achieve faster-than-light travel, a true marvel of engineering.";
                upgradeCostText.text = GetUpgradeCost(temp).ToString();
            }
            else if (selectedUpgradeIndex == 7)
            {
                temp = "Projectile Upgrade I";
                upgradeNameText.text = "Golden Void Bullets";
                upgradeDescriptionText.text = "Crafted from rare cosmic materials, these bullets tear through space itself, leaving a void in their wake that devours all they touch.";
                upgradeCostText.text = GetUpgradeCost(temp).ToString();
            }
            else if (selectedUpgradeIndex == 8)
            {
                temp = "Projectile Upgrade II";
                upgradeNameText.text = "Plasma Pulse";
                upgradeDescriptionText.text = "Raw concentrated energy that pierces through anything in its path.";
                upgradeCostText.text = GetUpgradeCost(temp).ToString();
            }
            else if (selectedUpgradeIndex == 9)
            {
                temp = "Projectile Upgrade III";
                upgradeNameText.text = "Laser";
                upgradeDescriptionText.text = "Yes we got Lasers here. Trust me bro";
                upgradeCostText.text = GetUpgradeCost(temp).ToString();
            }
            else if (selectedUpgradeIndex == 10)
            {
                temp = "Weapon Upgrade I";
                upgradeNameText.text = "Double Trouble";
                upgradeDescriptionText.text = "Pretty self-explanatory: two boom booms are better then one boom boom.\nright?";
                upgradeCostText.text = GetUpgradeCost(temp).ToString();
            }
            else if (selectedUpgradeIndex == 11)
            {
                temp = "Weapon Upgrade II";
                upgradeNameText.text = "ShockY";
                upgradeDescriptionText.text = "This plasma weapon delivers a solid mix of power and precision, making it a reliable choice in the midst of battle and also it sounds cool ngl.(Purchasing will enable corrisponding projectile upgrade)";
                upgradeCostText.text = GetUpgradeCost(temp).ToString();
            }
            else if (selectedUpgradeIndex == 12)
            {
                temp = "Weapon Upgrade III";
                upgradeNameText.text = "TheF***Aliens2000";
                upgradeDescriptionText.text = "This beautiful piece of art is, as some are saying, the best weapon in the galaxy. While others spread the rumor that the developer simply forgot to remove a zero... (Purchasing will enable corrisponding projectile upgrade) ";
                upgradeCostText.text = GetUpgradeCost(temp).ToString();
            }
        }
    }

    private void SetStaticUpgrades()
    {
       staticUpgrades  = new bool[11];

        for (int i = 0; i < staticUpgrades.Length; i++)
        {
            
            staticUpgrades[i] = true;

        }
    }

    private void OnDisable()
    {
        // Unsubscribe to prevent memory leaks
        UpgradeButton.OnButtonHover -= HandleButtonHover;
    }

    public void CollorSprites(int index)
    {
        if(index == 2)
        {
            upgradeImage.color = Color.green;
        }
        else if(index == 3)
        {
            upgradeImage.color = Color.blue;
        }
        else
        {
            upgradeImage.color = Color.white;
        }
    }
}
