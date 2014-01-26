/******************************************************************************\
* Copyright (C) Leap Motion, Inc. 2011-2013.                                   *
* Leap Motion proprietary and  confidential.  Not for distribution.            *
* Use subject to the terms of the Leap Motion SDK Agreement available at       *
* https://developer.leapmotion.com/sdk_agreement, or another agreement between *
* Leap Motion and you, your company or other organization.                     *
\******************************************************************************/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Leap;

/// <summary>
/// This static class serves as a static wrapper to provide some helpful C# functionality.
/// The main use is simply to provide the most recently grabbed frame as a singleton.
/// Events on aquiring, moving or loosing hands are also provided.  If you want to do any
/// global processing of data or input event dispatching, add the functionality here.
/// It also stores leap input settings such as how you want to interpret data.
/// To use it, you must call Update from your game's main loop.  It is not fully thread safe
/// so take care when using it in a multithreaded environment.
/// </summary>
public class LeapInput 
{	
	public bool EnableTranslation = true;
	public bool EnableRotation = true;
	public bool EnableScaling = false;
	/// <summary>
	/// Delegates for the events to be dispatched.  
	/// </summary>
	public delegate void PointableFoundHandler( Pointable p );
	public delegate void PointableUpdatedHandler( Pointable p );
	public delegate void HandFoundHandler( Hand h );
	public delegate void HandUpdatedHandler( Hand h );
	public delegate void ObjectLostHandler( int id );
	
	/// <summary>
	/// Event delegates are trigged every frame in the following order:
	/// Hand Found, Pointable Found, Hand Updated, Pointable Updated,
	/// Hand Lost, Hand Found.
	/// </summary>
	public event PointableFoundHandler PointableFound;
	public event PointableUpdatedHandler PointableUpdated;
	public event ObjectLostHandler PointableLost;
	
	public event HandFoundHandler HandFound;
	public event HandUpdatedHandler HandUpdated;
	public event ObjectLostHandler HandLost;
	
	public Leap.Frame Frame
	{
		get { return m_Frame; }
	}
	
	public void Update() 
	{	
		if( m_controller != null )
		{
			
			Frame lastFrame = m_Frame == null ? Frame.Invalid : m_Frame;
			m_Frame	= m_controller.Frame();
			
			DispatchLostEvents(Frame, lastFrame);
			DispatchFoundEvents(Frame, lastFrame);
			DispatchUpdatedEvents(Frame, lastFrame);
		}
	}
	
	//*********************************************************************
	// Private data & functions
	//*********************************************************************
	private enum HandID : int
	{
		Primary		= 0,
		Secondary	= 1
	};
	
	//Private variables
	public Leap.Controller 		m_controller	= new Leap.Controller();
	Leap.Frame			m_Frame			= null;
	
	private void DispatchLostEvents(Frame newFrame, Frame oldFrame)
	{
		foreach( Hand h in oldFrame.Hands )
		{
			if( !h.IsValid )
				continue;
			if( !newFrame.Hand(h.Id).IsValid && HandLost != null )
				HandLost(h.Id);
		}
		foreach( Pointable p in oldFrame.Pointables )
		{
			if( !p.IsValid )
				continue;
			if( !newFrame.Pointable(p.Id).IsValid && PointableLost != null )
				PointableLost(p.Id);
		}
	}
	private void DispatchFoundEvents(Frame newFrame, Frame oldFrame)
	{
		foreach( Hand h in newFrame.Hands )
		{
			if( !h.IsValid )
				continue;
			if( !oldFrame.Hand(h.Id).IsValid && HandFound != null)
				HandFound(h);
		}
		foreach( Pointable p in newFrame.Pointables )
		{
			if( !p.IsValid )
				continue;
			if( !oldFrame.Pointable(p.Id).IsValid && PointableFound != null )
				PointableFound(p);
		}
	}
	private void DispatchUpdatedEvents(Frame newFrame, Frame oldFrame)
	{
		foreach( Hand h in newFrame.Hands )
		{
			if( !h.IsValid )
				continue;
			if( oldFrame.Hand(h.Id).IsValid && HandUpdated != null)
				HandUpdated(h);
		}
		foreach( Pointable p in newFrame.Pointables )
		{
			if( !p.IsValid )
				continue;
			if( oldFrame.Pointable(p.Id).IsValid && PointableUpdated != null)
				PointableUpdated(p);
		}
	}

	public static LeapInput leapInputSingleton = null;
	public static LeapInput GetLeapInput() {
		// return a singleton
		if (leapInputSingleton == null) {
			leapInputSingleton = new LeapInput();
		}
		return leapInputSingleton;
	}

	public static void Destroy()
	{
		leapInputSingleton = new LeapInput();
	}
}
