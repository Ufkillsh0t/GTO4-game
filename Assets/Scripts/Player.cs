using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
    //Fields
    public Resource[] resources = new Resource[] { new Resource("Mana", 0), new Resource("Gold", 0), new Resource("Lumber", 0) };

    //Properties
    public string Name { get; set; }

    public Player(string name)
    {
        this.Name = name;
    }

    public void AddResources(string resourceName, int amount)
    {
        for(int i = 0; i < resources.Length; i++)
        {
            if(resources[i].Name == resourceName)
            {
                resources[i].AddAmount(amount);
                return;
            }
        }
        Debug.Log("The following resource wasn't found: " + resourceName);
    }

    public void RemoveResources(string resourceName, int amount)
    {
        for(int i = 0; i < resources.Length; i++)
        {
            if(resources[i].Name == resourceName)
            {
                resources[i].RemoveAmount(amount);
                return;
            }
        }
        Debug.Log("The following resource wasn't found: " + resourceName);
    }
}
