/**
 * UserMessagesFactory: Factory class for making the user's messages
 *
 * Copyright(C) 2022
 *
 * Prefix: RTDM_

 * @Author: Dr. Ram�n Moll� Vay�
 * @Date:	11/2022
 * @Version: 2.0
 *
 * Update: 07.01.2025
 * Date:	
 * Version: 
 * Changes:
 *
 */

#if !OS_OPERATINGSYSTEM
#define OS_OPERATINGSYSTEM
#define OS_MSWINDOWS
#define OS_64BITS
#endif

//----constantes y tipos-----
#if OS_MSWINDOWS
using RTT_Time = System.Int64;
using HRT_Time = System.Int64;
#elif OS_LINUX
#elif OS_OSX
#elif OS_ANDROID
#endif

using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public enum RTDESKMsgTypes
{
  Input,  //Input Manager message
  RTDESK_MAX_MsgTypes
};

public enum KeyState
{
  DOWN,   //The key is pushed down 
  UP		//The key is released
}
public class RTDESKInputMsg : MsgContent
{
  public KeyCode c;   //The code read by the input manager
  public KeyState s;	 //The state of the key. May be pushed down o released
}

/// ACHTUNG: do not touch anything above
/// Restricted for ineternal use only


//Examples of different types of messages to interchange among different GameObjects
public class TransformMsg : MsgContent
{
  public Vector2 V2;

  public TransformMsg() { Type = (int)UserMsgTypes.Position; }
}

public class SpriteAnimateMsg : MsgContent
{
  public SpriteRenderer spriteRenderer;

  public SpriteAnimateMsg() { Type = (int)UserMsgTypes.Animation; }
}

public enum UserMsgTypes
{
  Position = RTDESKMsgTypes.RTDESK_MAX_MsgTypes,  //The first enumerated user message type is the last used by the RTDESK system
  Speed, Animation, Action, TotalAmountUserMsgTypes
};

public enum UserActions
{
  Start,
  LiveState,
  GetSteady,  //Stop the movement of the object
  Move,   //Start moving the object
  End
};

//The component that creates an internal RTDESK Engine
public class UserMessagesFactory
{
  public const int RTDM_NO_TYPE_MSG = -1;

  public int MsgAmount = (int)UserMsgTypes.TotalAmountUserMsgTypes;

  public MsgContent CreateMsg(int type)
  {
    MsgContent msg;

    switch (type)
    {
      case (int)RTDESKMsgTypes.Input:
        msg = new RTDESKInputMsg();
        break;
      case (int)UserMsgTypes.Position:
        msg = new TransformMsg();
        break;
      case (int)UserMsgTypes.Speed:
        msg = new TransformMsg();
        break;
      case (int)UserMsgTypes.Animation:
        msg = new SpriteAnimateMsg();
        break;
      default:
        msg = new MsgContent();
        break;
    }
    msg.Type = type;
    return msg;
  }
}