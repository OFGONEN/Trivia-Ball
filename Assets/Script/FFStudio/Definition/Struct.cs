/* Created by and for usage of FF Studios (2021). */

using System;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;
using DG.Tweening;

namespace FFStudio
{
	[ Serializable ]
	public struct TransformData
	{
		public Vector3 position;
		public Vector3 rotation; // Euler angles.
		public Vector3 scale; // Local scale.
	}

	[ Serializable ]
	public struct EventPair
	{
		public EventListenerDelegateResponse eventListener;
		public UnityEvent unityEvent;

		public void Pair()
		{
			eventListener.response = unityEvent.Invoke;
		}
	}

	[ Serializable ]
	public struct EventPairDelayed
	{
		public EventListenerDelegateResponse eventListener;
		public EventListenerDelegateResponse invoke_cancel;
		public UnityEvent unityEvent;
		public float invoke_delay;

		private Tween invoke_tween;

		public void OnEnable()
		{
			eventListener.OnEnable();
			invoke_cancel.OnEnable();
		}

		public void OnDisable()
		{
			eventListener.OnDisable();
			invoke_cancel.OnDisable();
		}

		public void Pair()
		{
			eventListener.response = InvokeDelayed;
			invoke_cancel.response = InvokeCanceled;
		}

		private void InvokeDelayed()
		{
			invoke_tween = DOVirtual.DelayedCall( invoke_delay, unityEvent.Invoke );
		}

		private void InvokeCanceled()
		{
			invoke_tween = invoke_tween.KillProper();
		}
	}

	[Serializable]
	public struct ParticleData
	{
		public ParticleSpawnEvent particle_event;
		public string alias;
		public bool parent;
		public Vector3 offset;
		public float size;
	}

	[Serializable]
	public struct PopUpTextData
	{
		public string text;
		public Color color;
		public bool parent;
		public Vector3 offset;
		public float size;
	}

	[Serializable]
	public struct RandomParticlePool
	{
		public string alias;
		public ParticleEffectPool[] particleEffectPools;

		public ParticleEffect GiveRandomEntity()
		{
			return particleEffectPools.ReturnRandom<ParticleEffectPool>().GetEntity();
		}
	}

	[ Serializable ]
	public struct AnimationParameterData
	{
		public AnimationParameterType parameterType;
		public string parameter_name;
		[ ShowIf( "parameterType", AnimationParameterType.Bool  )] public bool parameter_bool;
		[ ShowIf( "parameterType", AnimationParameterType.Int   )] public int parameter_int;
		[ ShowIf( "parameterType", AnimationParameterType.Float )] public float parameter_float;
	}

	[ Serializable ]
	public struct CameraTweenData
	{
		public Transform target_transform;

		public float tween_duration;
		public bool change_position;
		public bool change_rotation;

		[ ShowIf( "change_position" ) ] public Ease ease_position;
		[ ShowIf( "change_rotation" ) ] public Ease ease_rotation;

		public UnityEvent event_complete;
		public bool event_complete_alwaysInvoke;
	}
}
