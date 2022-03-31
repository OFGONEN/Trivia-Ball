/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerChanger : MonoBehaviour
{
	[ Layer ] public int collider_layer;
	public bool collider_trigger;

	public void OnTrigger( Collider collider )
    {
		collider.gameObject.layer = collider_layer;
		collider.isTrigger        = collider_trigger;
	}
}
