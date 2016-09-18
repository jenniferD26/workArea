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
        {"Brown", "I'm double sided, and I just can't hide, I kind of like it when I make you cry, 'Cause I'm twisted up, I'm twisted up inside." },
        {"Cinnamon", "I begin to assemble what weapons I can find, 'Cause sometimes to stay alive you gotta kill your mind." },
        {"Orange", "I'm insignificant, Please tell them, you have no plans for me, I will set my soul on fire, what have I become?" },
        {"Female", "I'm a lumberjack and I'm ok." },
        {"Tortoiseshell", "Can you save my heavydirtysoul?" },
        {"Male", "I'm useless but not for long the future is coming on." }
    };
}
