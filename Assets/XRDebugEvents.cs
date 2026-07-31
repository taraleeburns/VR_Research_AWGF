using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XRDebugEvents : MonoBehaviour
{
    public void Activated(ActivateEventArgs args)
    {
        Debug.Log(
        $"Wish ACTIVATED by {args.interactorObject.transform.name} at frame {Time.frameCount}"
    );
    }

    public void Deactivated(DeactivateEventArgs args)
    {
        Debug.Log(
        $"Wish DEACTIVATED by {args.interactorObject.transform.name} at frame {Time.frameCount}"
    );
    }
}