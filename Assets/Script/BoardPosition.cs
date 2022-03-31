/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using FFStudio;
using DG.Tweening;
using Sirenix.OdinInspector;

public class BoardPosition : MonoBehaviour
{
#region Fields
    [ BoxGroup( "Setup" ) ] public SharedReferenceNotifier notifier_questionHeader;
    [ BoxGroup( "Setup" ) ] public SharedReferenceNotifier notifier_camera;
    [ BoxGroup( "Setup" ) ] public GameEvent event_level_start;
#endregion

#region Properties
#endregion

#region Unity API
#endregion

#region API
    public void RepositionBoard() 
    {
		var camera          = ( notifier_camera.sharedValue as Transform ).GetComponent<Camera>();
		var cameraTransform = camera.transform;
		var rectTransform   = notifier_questionHeader.sharedValue as RectTransform;

		var positionBottom   = rectTransform.position.AddY( -rectTransform.sizeDelta.y );
		    positionBottom.z = camera.nearClipPlane;

		var nearPosition = camera.ScreenToWorldPoint( positionBottom );
		var delta        = nearPosition - cameraTransform.position;
		var angle_rad    = Mathf.Acos( Vector3.Dot( delta.normalized, Vector3.down ) );

		var hypho   = nearPosition.y / Mathf.Cos( angle_rad ) + delta.magnitude;
		var zOffset = Mathf.Sin( angle_rad ) * hypho;

		var targetPoint = cameraTransform.position + Vector3.forward * zOffset;
		    targetPoint = targetPoint.SetY( 0 ).AddZ( -GameSettings.Instance.board_length / 2f );

		transform.DOMove(
			targetPoint,
			GameSettings.Instance.board_reposition_duration )
			.SetEase( GameSettings.Instance.board_reposition_ease ).OnComplete( event_level_start.Raise );
	}
#endregion

#region Implementation
#endregion
}
