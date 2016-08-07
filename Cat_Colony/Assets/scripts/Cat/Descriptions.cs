using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// contains all of the descriptions for the different types of genes
// right now only contains descriptions for the Black coat pattern
// and also a dummy gender description
public class Descriptions : MonoBehaviour {

    public static Dictionary<string, string> description = new Dictionary<string, string>()
    {
        {"Black", "The black base coat is activated when the B gene is present. The B gene is most dominant so it will override any other gene it is paired with." },
        {"Female", "I'm a lumberjack and I'm ok." },
        {"Male", "I'm useless but not for long the future is coming on." }
    };
}
