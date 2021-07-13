using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Chain
{
    private List<IChainable> links;
    private bool activeChain;
    private string location;

    public bool ActiveChain
    {
        get { return activeChain; }
    }

    public int Length
    {
        get { return links.Count; }
    }

    public string Location
    {
        get { return location; }
    }

    public IChainable this[int i]
    {
        get { return links[i]; }
    }

    public Chain(IChainable link)
    {
        links = new List<IChainable>();
        links.Add(link);
        location = link.Location;
    }

    public void AddLink(IChainable link)
    {
        links.Add(link);
    }

    public void RemoveLink(int index)
    {
        links.RemoveAt(index);
        if (CheckEmpty())
        {
            activeChain = false;
        }
    }

    public void RemoveLink(IChainable link)
    {
        links.Remove(link);
        if (CheckEmpty())
        {
            activeChain = false;
        }
    }

    public bool CheckEmpty()
    {
        return Length <= 0;
    }

    public void CheckActive()
    {
        if (DateTime.Now.Minute - links[Length - 1].Timestamp.Minute > 20)
        {
            activeChain = false;
        }
    }
}
