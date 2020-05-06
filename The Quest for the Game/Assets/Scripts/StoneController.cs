using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneController : CollectibleController
{
    public BreakableObjectController[] breakableObjects;
    public override void Collect()
    {
        // Gera alla hluti brjótanlega sem eiga að vera brjótanlegir
        foreach (BreakableObjectController breakableObject in breakableObjects)
        {
            breakableObject.SetBreakableStatus(true);
        }
        // Kalla í base Collect() aðferðina til að eyða hlutnum og spila hljóð
        base.Collect();
    }
}
