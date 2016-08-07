using UnityEngine;
using System.Collections;

public enum CatState
{
    walk,
    sit,
    run,
    interact,
    idle,
    wait,
    useItem,
    choose
}

public class YardAI : MonoBehaviour {


    // the spawn area that the cat can appear in
    public GameObject spawnArea;
    public float randomPosition = 100f;

    private float pos = 0f;
    private Vector3 startPosition = Vector3.zero;
    private float min;
    private float max;
    private NavMeshAgent agent;
    private CatState currentState;
    private CatState nextState;
    private int waitTime = 0;   //the wait time to record the old position
    private int randomState = 0;

    // interacting fields
    private int layermask = 1 << 9;
    private YardAI otherCat;

    // allows other cats to tell this cat to stop if they want to interact
    public CatState State
    {
        get { return currentState; }
        set { currentState = value; }
    }


    // gets a random destination for the agent and sets it
    public void SetDestination(GameObject otherCat)
    {
        Vector3 newPosition = transform.position;

        // right now set the nes destionation to a random value
        if(otherCat == null)
        {
            float rand = Random.Range(transform.position.z - randomPosition, transform.position.z + randomPosition);
            newPosition.z = rand;
            rand = Random.Range(transform.position.x - randomPosition, transform.position.x + randomPosition);
            newPosition.x = rand;
        }

        // otherwise if the cat wants to interact
        else if( currentState == CatState.interact)
        {
            newPosition = otherCat.transform.position;
        }

        //calculate and set the agent's path
        NavMeshPath path = new NavMeshPath();
        if(agent.CalculatePath(newPosition, path))
        {
            agent.CalculatePath(newPosition, path);
            agent.SetPath(path);
        }
    }

    // sends out a sphere to check which cats are near by and decides which cat to interact with
    // is called when the interactable state is chosen
    GameObject CheckCats()
    {
        if (Physics.OverlapSphere(transform.position, 5, layermask) != null)
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, 5, layermask);

            foreach (Collider col in hitColliders)
            {
                // return the gameobject of the other cat
                if (col.gameObject != transform.FindChild("CatMesh").gameObject)
                {
                    //tell the other cat to wait
                    otherCat = col.gameObject.transform.GetComponentInParent<YardAI>();

                    //if the other cat is already waiting (it's interacting with a different cat) then return null
                    if(otherCat.State != CatState.wait && otherCat.State != CatState.interact)
                    {
                        otherCat.State = CatState.wait;
                        return otherCat.gameObject;
                    }
                }
            }
        }
        return null;
    }

    // Use this for initialization
    void Awake()
    {
        // start position is the default position
        startPosition = transform.position;

        // modify the x and z axis of the start position so that it is random for every cat
        min = spawnArea.transform.FindChild("Bottom").transform.position.z;
        max = spawnArea.transform.FindChild("Top").transform.position.z;
        pos = Random.Range(min, max);
        startPosition.z = pos;

        min = spawnArea.transform.FindChild("Left").transform.position.x;
        max = spawnArea.transform.FindChild("Right").transform.position.x;
        pos = Random.Range(min, max);
        startPosition.x = pos;

        // the the position to the new start position
        transform.position = startPosition;

        // set the current state to idle so that the cat can create a new destiation in the update
        agent = GetComponent<NavMeshAgent>();
        currentState = CatState.idle;
        nextState = CatState.choose;
    }

    // Update is called once per frame
    void FixedUpdate ()
    {
        // Wait state
        // cat should cancel everything it's doing
        if(currentState == CatState.wait)
        {
            if(agent.hasPath)
            {
                agent.ResetPath();
                Debug.Log(transform.GetComponentInChildren<CatGenetics>().CatName + ": " +  "STAHP");
            }
        }

        // Idle state
        // at the moment this just gives the cat a new destination to walk to
        else if (currentState == CatState.idle)
        {
            // set the next destination and reset the wait time
            if (waitTime == 100f)
            {
                SetDestination(null);
                currentState = CatState.walk;
                waitTime = 0;
            }
            else
            {
                waitTime++;
            }
        }

        // Interacting state
        else if (currentState == CatState.interact)
        {
            // checks if there are other cats near by
            // if CheckCats returns null it gives the cat a random destination
            if (nextState != CatState.idle)
            {
                SetDestination(CheckCats());
                nextState = CatState.idle;
                Debug.Log(transform.GetComponentInChildren<CatGenetics>().CatName + ": " + "Walking to other cat");
            }
            else if (agent.remainingDistance <= 0.45f && otherCat != null)
            {
                currentState = nextState;
                nextState = CatState.choose;
                otherCat.State = CatState.walk;
                otherCat.SetDestination(null);
                otherCat = null;
                Debug.Log(transform.GetComponentInChildren<CatGenetics>().CatName + ": " + "Leaving other cat");
            }
            else if(otherCat == null)
            {
                currentState = CatState.idle;
                nextState = CatState.choose;
            }
        }

        // decide what to do next while walking
        else if (nextState == CatState.choose && agent.remainingDistance < 0.5f)
        {
            randomState = (int)Random.Range(0.0f, 3.0f);
            switch (randomState)
            {
                case 0:
                    nextState = CatState.idle;
                    break;
                case 1:
                    // cat keeps walking
                    nextState = CatState.choose;
                    SetDestination(null);
                    break;
                case 2:
                    nextState = CatState.interact;
                    break;
            }
            Debug.Log(transform.GetComponentInChildren<CatGenetics>().CatName + ": " + "Choosing");
        }

        // check if the destination is reached and set the currentstate
        else if (agent.remainingDistance <= 0.1f)
        {
            currentState = nextState;
            nextState = CatState.choose;
            Debug.Log(transform.GetComponentInChildren<CatGenetics>().CatName + ": " + currentState);

        }
	}
}
