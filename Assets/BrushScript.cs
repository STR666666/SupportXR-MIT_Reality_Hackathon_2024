using System.Collections.Generic;
using UnityEngine;
using Oculus.Interaction;

public class Brush : MonoBehaviour
{
    // Prefab to instantiate when we draw a new brush stroke
    [SerializeField] private GameObject _brushStrokePrefab;

    // Which hand should this brush instance track?
    public GameObject LeftHand;
    public GameObject RightHand;
    public float Sensitivity;
    //public int Sensitivity;

    // Used to keep track of the current brush tip position and the actively drawing brush stroke
    private BrushStroke _activeBrushStrokeL;
    private BrushStroke _activeBrushStrokeR;

    private void Update()
    {
        // Start by figuring out which hand we're tracking
        var LHand = LeftHand.GetComponent<OVRHand>();
        var RHand = RightHand.GetComponent<OVRHand>();

        // Figure out if the trigger is pressed or not
        bool LtriggerPressed = (LHand.GetFingerPinchStrength(OVRHand.HandFinger.Ring) >= Sensitivity) && (LHand.GetFingerPinchStrength(OVRHand.HandFinger.Index) <= Sensitivity);
        bool RtriggerPressed = (RHand.GetFingerPinchStrength(OVRHand.HandFinger.Ring) >= Sensitivity) && (LHand.GetFingerPinchStrength(OVRHand.HandFinger.Index) <= Sensitivity);

        // If the trigger is pressed and we haven't created a new brush stroke to draw, create one!
        if (LtriggerPressed && _activeBrushStrokeL == null)
        {
            // Instantiate a copy of the Brush Stroke prefab.
            GameObject brushStrokeGameObject = Instantiate(_brushStrokePrefab);

            // Grab the BrushStroke component from it
            _activeBrushStrokeL = brushStrokeGameObject.GetComponent<BrushStroke>();

            // Tell the BrushStroke to begin drawing at the current brush position
            _activeBrushStrokeL.BeginBrushStrokeWithBrushTipPoint(LeftHand.transform.position, LeftHand.transform.rotation);
        }
        if (RtriggerPressed && _activeBrushStrokeR == null)
        {
            // Instantiate a copy of the Brush Stroke prefab.
            GameObject brushStrokeGameObject = Instantiate(_brushStrokePrefab);

            // Grab the BrushStroke component from it
            _activeBrushStrokeR = brushStrokeGameObject.GetComponent<BrushStroke>();

            // Tell the BrushStroke to begin drawing at the current brush position
            _activeBrushStrokeR.BeginBrushStrokeWithBrushTipPoint(RightHand.transform.position, RightHand.transform.rotation);
        }

        // If the trigger is pressed, and we have a brush stroke, move the brush stroke to the new brush tip position
        if (LtriggerPressed)
            _activeBrushStrokeL.MoveBrushTipToPoint(LeftHand.transform.position, LeftHand.transform.rotation);

        if (RtriggerPressed)
            _activeBrushStrokeR.MoveBrushTipToPoint(RightHand.transform.position, RightHand.transform.rotation);

        // If the trigger is no longer pressed, and we still have an active brush stroke, mark it as finished and clear it.
        if (!LtriggerPressed && _activeBrushStrokeL != null)
        {
            _activeBrushStrokeL.EndBrushStrokeWithBrushTipPoint(LeftHand.transform.position, LeftHand.transform.rotation);
            _activeBrushStrokeL = null;
        }

        if (!RtriggerPressed && _activeBrushStrokeR != null)
        {
            _activeBrushStrokeR.EndBrushStrokeWithBrushTipPoint(RightHand.transform.position, RightHand.transform.rotation);
            _activeBrushStrokeR = null;
        }
    }

    // ...
}