using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

public class GameException : System.Exception
{
    public GameException() { }
    public GameException(string message) : base(message) { }
    public GameException(string message, int code) : base(message + ". with error code :" + code.ToString()) { }
    public GameException(string message, System.Exception inner) : base(message, inner) { }
    protected GameException(SerializationInfo info, StreamingContext context) : base(info, context) { }
}