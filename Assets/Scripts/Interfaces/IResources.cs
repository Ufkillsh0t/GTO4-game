using UnityEngine;
using System.Collections;

public interface IResources {
    void AddResources(ResourceType resource, int amount);
    void RemoveResources(ResourceType resource, int amount);
}
