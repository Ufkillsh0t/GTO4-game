using UnityEngine;
using System.Collections;

public class Resource {

	public string Name { get; set; }
    public int Amount { get; set; }

    public Resource(string name, int amount)
    {
        this.Name = name;
        this.Amount = amount;
    }

    public void AddAmount(int amount)
    {
        Amount += amount;
    }

    public void RemoveAmount(int amount)
    {
        Amount -= amount;
    }
}
