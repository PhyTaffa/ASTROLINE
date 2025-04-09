using UnityEngine;

[CreateAssetMenu(fileName = "NewScanData", menuName = "Scan/Scan Data")]
public class ScanData : ScriptableObject
{
    public string objectName;
    public Sprite objectIcon;
    public string description;
    public string location;
    public string habitat;
}

