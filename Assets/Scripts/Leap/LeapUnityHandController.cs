/******************************************************************************\
* Copyright (C) Leap Motion, Inc. 2011-2013.                                   *
* Leap Motion proprietary and  confidential.  Not for distribution.            *
* Use subject to the terms of the Leap Motion SDK Agreement available at       *
* https://developer.leapmotion.com/sdk_agreement, or another agreement between *
* Leap Motion and you, your company or other organization.                     *
\******************************************************************************/

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Leap;

/// <summary>
/// This class manipulates the hand representation in the unity scene based on the
/// input from the leap device. Fingers and Palm objects are moved around between
/// higher level 'hand' objects that mainly serve to organize.  Be aware that when
/// fingers are lost, unity does not dispatch OnTriggerExit events.
/// </summary>
public class LeapUnityHandController : MonoBehaviour 
{	
	public GameObject[]				m_palms		= null;
	public GameObject[]				m_fingers	= null;
	public GameObject[]				m_hands 	= null;
	public bool						m_DisplayHands = true;
	
	//These arrays allow us to use our game object arrays much like pools.
	//When a new hand/finger is found, we mark a game object by active
	//by storing it's id, and when it goes out of scope we make the
	//corresponding gameobject invisible & set the id to -1.
	private int[]					m_fingerIDs = null;
	private int[]					m_handIDs	= null;
	
	void SetCollidable( GameObject obj, bool collidable )
	{
		foreach( Collider component in obj.GetComponents<Collider>() )
			component.enabled = collidable;
	
		foreach( Collider child in obj.GetComponentsInChildren<Collider>() )
			child.enabled = collidable;
	}
	
	void SetVisible( GameObject obj, bool visible )
	{
		foreach( Renderer component in obj.GetComponents<Renderer>() ) {
			component.enabled = visible && m_DisplayHands;
		}
		
		foreach( Renderer child in obj.GetComponentsInChildren<Renderer>() ) {
			child.enabled = visible && m_DisplayHands;
		}
		
		foreach( Light light in obj.GetComponents<Light>() ) {
			light.enabled = visible && m_DisplayHands;
		}	
		
		foreach( Light light in obj.GetComponentsInChildren<Light>() ) {
			light.enabled = visible && m_DisplayHands;
		}
	}
	
	void Start()
	{
		m_fingerIDs = new int[10];
		for( int i = 0; i < m_fingerIDs.Length; i++ )
		{
			m_fingerIDs[i] = -1;	
		}
		
		m_handIDs = new int[2];
		for( int i = 0; i < m_handIDs.Length; i++ )
		{
			m_handIDs[i] = -1;	
		}
		
		LeapInput.GetLeapInput().HandFound += new LeapInput.HandFoundHandler(OnHandFound);
		LeapInput.GetLeapInput().HandLost += new LeapInput.ObjectLostHandler(OnHandLost);
		LeapInput.GetLeapInput().HandUpdated += new LeapInput.HandUpdatedHandler(OnHandUpdated);
		LeapInput.GetLeapInput().PointableFound += new LeapInput.PointableFoundHandler(OnPointableFound);
		LeapInput.GetLeapInput().PointableLost += new LeapInput.ObjectLostHandler(OnPointableLost);
		LeapInput.GetLeapInput().PointableUpdated += new LeapInput.PointableUpdatedHandler(OnPointableUpdated);
		
		//do a pass to hide the objects.
		foreach( GameObject palm in m_palms )
		{
			updatePalm(Leap.Hand.Invalid, palm);
		}
		foreach( GameObject finger in m_fingers)
		{
			updatePointable(Leap.Pointable.Invalid, finger);
		}
	}
	
	//When an object is found, we find our first inactive game object, activate it, and assign it to the found id
	//When lost, we deactivate the object & set it's id to -1
	//When updated, load the new data
	void OnPointableUpdated( Pointable p )
	{
		int index = Array.FindIndex(m_fingerIDs, id => id == p.Id);
		if( index != -1 )
		{
			updatePointable( p, m_fingers[index] );	
		}
	}
	void OnPointableFound( Pointable p )
	{
		int index = Array.FindIndex(m_fingerIDs, id => id == -1);
		if( index != -1 )
		{
			m_fingerIDs[index] = p.Id;
			// set position manually to bypass the raycast.
			m_fingers[index].transform.localPosition = p.TipPosition.ToUnityTranslated();
			updatePointable( p, m_fingers[index] );
		}
	}
	void OnPointableLost( int lostID )
	{
		int index = Array.FindIndex(m_fingerIDs, id => id == lostID);
		if( index != -1 )
		{
			updatePointable( Pointable.Invalid, m_fingers[index] );
			m_fingerIDs[index] = -1;
		}
	}

	void OnHandFound( Hand h )
	{
		int index = Array.FindIndex(m_handIDs, id => id == -1);
		if( index != -1 )
		{
			m_handIDs[index] = h.Id;
			updatePalm(h, m_palms[index]);
		}
	}
	void OnHandUpdated( Hand h )
	{
		int index = Array.FindIndex(m_handIDs, id => id == h.Id);
		if( index != -1 )
		{
			updatePalm(	h, m_palms[index] );
		}
	}
	void OnHandLost(int lostID)
	{
		int index = Array.FindIndex(m_handIDs, id => id == lostID);
		if( index != -1 )
		{
			updatePalm(Hand.Invalid, m_palms[index]);
			m_handIDs[index] = -1;
		}
	}
	
	void updatePointable( Leap.Pointable pointable, GameObject fingerObject )
	{
		updateParent( fingerObject, pointable.Hand.Id );
		
		SetVisible(fingerObject, pointable.IsValid);
		SetCollidable(fingerObject, pointable.IsValid);
		
		if ( pointable.IsValid )
		{
			Vector3 localFingerPos = pointable.TipPosition.ToUnityTranslated();
			
			float fingerRadius = fingerObject.GetComponent<SphereCollider>().radius - 0.3f;
			RaycastHit target;
			
			// need to convert it to a vec4 for it to take translation into account when converting to world space.
			Vector4 localFingerPt = new Vector4(localFingerPos.x, localFingerPos.y, localFingerPos.z, 1.0f);
			Vector3 worldTargetPos = fingerObject.transform.parent.localToWorldMatrix * localFingerPt;

			Vector3 worldMovementDir = worldTargetPos - fingerObject.transform.position;
			worldMovementDir.Normalize();
		
			Vector3 worldRayTarget = worldTargetPos + worldMovementDir * fingerRadius;
			
			/*if (Physics.Linecast(fingerObject.transform.position, worldRayTarget, out target)) {
				Vector4 targetPoint = new Vector4(target.point.x, target.point.y, target.point.z, 1.0f);
				Vector3 localTargetPoint = fingerObject.transform.parent.worldToLocalMatrix * targetPoint;
				Vector3 localMovementDir = localFingerPos - fingerObject.transform.localPosition;
				localMovementDir.Normalize();
				fingerObject.transform.localPosition = localTargetPoint - localMovementDir * fingerRadius; */
			//} else {
				fingerObject.transform.localPosition = localFingerPos;
			//}
			
			Vector3 vFingerDir = pointable.Direction.ToUnity();
			fingerObject.transform.localRotation = Quaternion.FromToRotation( Vector3.forward, vFingerDir );
			fingerObject.GetComponent<LeapFinger>().m_hand = pointable.Hand;
			fingerObject.rigidbody.velocity = pointable.TipVelocity.ToUnityScaled();
		}
	}

	void updatePalm( Leap.Hand leapHand, GameObject palmObject )
	{
		updateParent( palmObject, leapHand.Id);
		
		SetVisible(palmObject, leapHand.IsValid);
		SetCollidable(palmObject, leapHand.IsValid);
		
		if( leapHand.IsValid )
		{
			palmObject.transform.localPosition = leapHand.PalmPosition.ToUnityTranslated();
		}
	}	
	
	void updateParent( GameObject child, int handId )
	{
		//check the hand & update the parent
		int handIndex = Array.FindIndex(m_handIDs, id => id == handId);
		if( handIndex == -1 || handId == -1 )
			handIndex = 2;
		
		GameObject parent = m_hands[handIndex];
		if( child.transform.parent != parent.transform )
		{
			child.transform.parent = parent.transform;
		}
	}
}
