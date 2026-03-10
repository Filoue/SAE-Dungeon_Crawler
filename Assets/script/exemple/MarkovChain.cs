using UnityEngine;

public class MarkovChain : MonoBehaviour
{

    private Markov _sunny =  new Markov("Sunny");
    private Markov _rainy =  new Markov("Rainy");
    
    private Markov _chain;

    public int _maxStates = 1000000;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _sunny.AddLink(new MarkovLink(0.9f, _sunny));
        _sunny.AddLink(new MarkovLink(0.1f, _rainy));
        
        _rainy.AddLink(new MarkovLink(0.5f, _rainy));
        _rainy.AddLink(new MarkovLink(0.5f, _sunny));

        _chain = _sunny;
        
        for (int i = 0; i < _maxStates; i++)
        {
            Debug.Log(_chain.Name);
            _chain = _chain.NextState();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
