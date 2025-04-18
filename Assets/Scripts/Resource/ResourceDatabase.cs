using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ResourceDatabase", menuName = "Database/ResourceDatabase")]
public class ResourceDatabase : ScriptableObject
{
    protected static ResourceDatabase _instance;

    public List<ResourceTemplate> resources;

    public static ResourceDatabase Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = Resources.Load<ResourceDatabase>("ResourceDatabase");
            }
            return _instance;
        }
    }

    public ResourceTemplate GetTemplate(string id)
    {
        return resources.Find(resource => resource.uid == id);
    }


}
