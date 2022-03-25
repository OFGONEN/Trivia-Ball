/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;
using Sirenix.OdinInspector;

public abstract class IncrementalProgress< T > : ScriptableObject
{
    [ BoxGroup( "Setup" ), SerializeField ] protected SharedIntNotifier notifier_currency;
    [ BoxGroup( "Setup" ), SerializeField ] protected string incremental_name;
    [ BoxGroup( "Setup" ), SerializeField ] protected T incremental_default; //Info same as game_setting value 
    [ BoxGroup( "Setup" ), SerializeField ] protected IncrementalData< T >[] incremeantal_data;

    public string ReturnCost()
    {
		var index = ReturnIndex();
		return incremeantal_data[ index ].incremental_cost.ToString();
    }

	public string ReturnValue()
    {
		var index = ReturnIndex();
		return incremeantal_data[ index ].incremental_value_text;
	}

	public void UnlockIncremental()
    {
		SpendCost();
		AddIncremental();
	}

    protected void SpendCost()
    {
		var index = ReturnIndex();

		notifier_currency.SharedValue -= incremeantal_data[ index ].incremental_cost;

		PlayerPrefs.SetInt( "currency", notifier_currency.sharedValue );
	}

	protected abstract void AddIncremental();

	protected int ReturnIndex()
    {
		return PlayerPrefs.GetInt( incremental_name + "_index", 0 );
    }
}