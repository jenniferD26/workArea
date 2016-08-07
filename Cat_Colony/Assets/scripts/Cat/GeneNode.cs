using UnityEngine;
using System.Collections;

// gene node class
public class GeneNode
{
    Genetics geneOne;
    Genetics geneTwo;

    // default constructor
    public GeneNode()
    {
        geneOne = 0;
        geneTwo = 0;
    }

    public Genetics GeneOne
    {
        get { return geneOne; }
        set { geneOne = value; }
    }

    public Genetics GeneTwo
    {
        get { return geneTwo; }
        set { geneTwo = value; }
    }

    // returns the genenode as a string
    public string GeneToString()
    {
        return geneOne.ToString() + geneTwo.ToString();
    }

    // sorts the gene nodes so that the most dominant gene is always first, to make it easier to represent

    public void Sort()
    {
        Genetics temp;
        if(geneTwo < geneOne)
        {
            temp = geneOne;
            geneOne = geneTwo;
            geneTwo = temp;
        }
    }
}
