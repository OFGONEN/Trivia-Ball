/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;

[ CreateAssetMenu( fileName = "incremental_", menuName = "FF/Data/Incremental Int" ) ]
public class IncrementalProgress_Int : IncrementalProgress< int >
{
	protected override void AddIncremental()
    {
		var index = ReturnIndex();
		var value = PlayerPrefs.GetInt( incremental_name, incremental_default );

		PlayerPrefs.SetInt( incremental_name, value + incremeantal_data[ index ].incremental_value );
	}
}