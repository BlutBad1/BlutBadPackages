using CreatureNS;
using ScriptableObjectNS.Creature;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using System.Collections.Generic;

public abstract class Creature : MonoBehaviour, ICreature, ISerializationCallbackReceiver
{
    public static List<string> creatureNames = new List<string>();

    public static List<string> CreatureNames
    {
        get
        {
            if (creatureNames == null)
                creatureNames = new List<string>();
            if (creatureNames.Count == 0)
                creatureNames = CreatureTypes.Instance.Names;
            return creatureNames;
        }
        set { creatureNames = value; }
    }

    [SerializeField, FormerlySerializedAs("CreatureType"), ListToPopup(typeof(Creature), "creatureNames")]
    private string creatureType;

    public void OnAfterDeserialize() { }
    public void OnBeforeSerialize() =>
         CreatureNames = CreatureTypes.Instance.Names;
    public GameObject GetCreatureGameObject() =>
         gameObject;
    public string GetCreatureName() =>
         creatureType;
    public abstract void BlockMovement();
    public abstract void UnblockMovement();
    public abstract void SetPositionAndRotation(Vector3 position, Quaternion rotation);
    public abstract void SetSpeedCoef(float speedCoef);
}