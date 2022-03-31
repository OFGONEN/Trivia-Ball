/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;
using Sirenix.OdinInspector;

public class RigidbodyInterface : MonoBehaviour
{
#region Fields
    public Rigidbody rb;
    public RigidbodyData[] rb_data;
#endregion

#region Properties
#endregion

#region Unity API
#endregion

#region API
    public void ApplyData( int index )
    {
		var data = rb_data[ index ];

		rb.AddForce( Random.onUnitSphere.SetY( 0 ).normalized * data.force, data.force_mode );
		rb.AddTorque( Random.onUnitSphere * data.torque, data.force_mode );
	}
#endregion

#region Implementation
#endregion

#region Editor Only
#if UNITY_EDITOR
#endif
#endregion
}

[ System.Serializable ]
public struct RigidbodyData
{
	public float force;
	public float torque;
	public ForceMode force_mode;
}