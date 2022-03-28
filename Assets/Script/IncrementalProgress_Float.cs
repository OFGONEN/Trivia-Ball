/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;

[ CreateAssetMenu( fileName = "incremental_", menuName = "FF/Data/Incremental Float" ) ]
public class IncrementalProgress_Float : IncrementalProgress< float >
{
	protected override void AddIncremental()
    {
		var index = ReturnIndex();
		var value = PlayerPrefs.GetFloat( incremental_name, incremental_default );

		PlayerPrefs.SetFloat( incremental_name, value + incremeantal_data[ index ].incremental_value );
	}

	private void OnValidate()
	{
		incremental_default = GameSettings.Instance.ball_default_power;
	}
}