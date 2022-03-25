/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;
using Sirenix.OdinInspector;


public class IncrementalButton_Int : IncrementalButton
{
    [ BoxGroup( "Setup" ) ] public IncrementalProgress_Int incrementalProgress;

	public override bool CanAfford()
    {
        return notifier_currency.sharedValue > incrementalProgress.Cost;
	}

	protected override void UpdateVisual()
    {
		text_value.text = incrementalProgress.ReturnValue();
		text_cost.text  = incrementalProgress.ReturnCost();

        if( CanAfford() )
			text_cost.color = text_color_positive;
        else
			text_cost.color = text_color_negative;
	}
}