using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public enum AttackType
{
    Reconnaissance,
    ResourceDevelopment,
    InitialAccess,
    Execution,
    Persistence,
    PrivilegeEscalation,
    DefenseEvasion,
    CredentialAccess,
    Discovery,
    LateralMovement,
    Collection,
    CommandControl,
    Exfiltration,
    Impact
}

public interface IChainable 
{
    //public AttackType Type { get; }

    public DateTime Timestamp { get; }

    public string Location { get; }

    public int Team { get; }

    public string EventName { get; }

}
