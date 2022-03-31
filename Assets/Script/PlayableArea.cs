/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;
using Shapes;
using Sirenix.OdinInspector;

public class PlayableArea : MonoBehaviour
{
#region Fields
    [ BoxGroup( "Shared" ) ] public SharedReferenceNotifier notifier_bar_transform;

    // Private \\
    private Transform bar_transform;
	private Rectangle area_rectangle;

	// Delegates
	private UnityMessage updateMethod;

    private float area_size;
    private float bar_distance;
#endregion

#region Properties
#endregion

#region Unity API
    private void Awake()
    {
		updateMethod = ExtensionMethods.EmptyMethod;
        area_rectangle = GetComponent< Rectangle >();
	}

    private void Start()
    {
		bar_transform = notifier_bar_transform.sharedValue as Transform;

		area_size    = area_rectangle.Height;
		bar_distance = bar_transform.position.z - transform.position.z;
	}

    private void Update()
    {
		updateMethod();
	}
#endregion

#region API
    public void StartTrackingBar()
    {
		updateMethod = TrackBar;
	}

    public void StopTrackingBar()
    {
		updateMethod = ExtensionMethods.EmptyMethod;
	}
#endregion

#region Implementation
    private void TrackBar()
    {
		var currentDistance = bar_transform.position.z - transform.position.z;

		area_rectangle.Height = currentDistance * area_size / bar_distance;
	}
#endregion

#region Editor Only
#if UNITY_EDITOR
#endif
#endregion
}
