using System.Collections;
using System.Collections.Generic;
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
    public AttackType Type { get; }

}
