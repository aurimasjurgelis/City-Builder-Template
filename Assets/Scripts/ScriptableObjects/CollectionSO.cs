using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Collection", menuName = "City Builder/CollectionSO")]
public class CollectionSO : ScriptableObject
{
	public RoadStructureSO roadStructure;
	public List<SingleStructureBaseSO> singleStructureList;
	public List<ZoneStructureSO> zoneList;
}
