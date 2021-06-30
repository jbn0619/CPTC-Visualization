using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chain
{
    private List<IChainable> links;
    private AttackType currentType;



    public int Length
    {
        get { return links.Count; }
    }

    public IChainable this[int i]
    {
        get { return links[i]; }
    }

    public void AddLink(IChainable link)
    {
        links.Add(link);

    }

    public void RemoveLink(int index)
    {
        links.RemoveAt(index);
    }

    public void RemoveLink(IChainable link)
    {
        links.Remove(link);
    }
}
