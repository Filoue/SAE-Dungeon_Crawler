using System.Collections.Generic;
using UnityEngine;


public struct MarkovLink
{
    public float Probability;
    public Markov State;
    
    public MarkovLink(float probability, Markov state)
    {
        Probability = probability;
        State = state;
    }
}
public class Markov
{
    private List<MarkovLink> _links;
    private string _name;

    public string Name => _name;

    public Markov(string name)
    {
        _name = name;
        _links = new List<MarkovLink>();
    }



    public void AddLink(MarkovLink link)
    {
        if (!_links.Exists(l => l.State == link.State))
        {
            _links.Add(link);
        }
    }

    public Markov NextState()
    {
        if (_links.Count > 0)
        {
            float rng = Random.value;
            int idx = Mathf.FloorToInt(rng * _links.Count);
            return _links[idx].State;
        }
        return null;
    }
}
