using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Healthbar manager
/// </summary>
public class LifeCrystalUI : MonoBehaviour {


    /// <summary>
    /// Current life total
    /// </summary>
    int lifeCount
    {
        get { return lifeCount; }
        set { }
    }

    int goldCount
    {
        get { return goldCount; }
        set { }
    }
	
	void Start ()
    {
		
	}
	
	
	void Update ()
    {
		
	}



    /// <summary>
    /// Take given amount of damage
    /// </summary>
    /// <param name="dmg">Amount of damage to take</param>
    public void TakeDamege(int dmg)
    {
        
    }
    
    /// <summary>
    /// Gain given amount of gold
    /// </summary>
    /// <param name="gold">Amount of gold to gain</param>
    public void GainGold(int gold)
    {

    }

    /// <summary>
    /// Gain a LifeCrystal
    /// </summary>
    /// <param name="frag">LifeCrystal to gain</param>
    /// <returns>returns false if no cell taken, true otherwise</returns>
    public bool GainLifeCrystal(LifeCrystal frag)
    {
        return false;
    }

    /// <summary>
    /// Use given amount of gold
    /// </summary>
    /// <param name="gold">Amount of gold to use</param>
    /// <returns>Returns false when money not enough, true otherwise</returns>
    public bool UseGold(int gold)
    {
        return false;
    }

}
