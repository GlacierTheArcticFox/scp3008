using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class employee_ai : MonoBehaviour {

    public GameObject player;
    private NavMeshAgent navMeshAgent;

    public int chaseDistance;
    public int speakDistance;
    public int atackDistance;
    public int damage;

    private Vector3 target;

    void Awake() {
        navMeshAgent = GetComponent<NavMeshAgent>();
        GetComponent<AudioSource>().pitch = Random.Range(0.8f, 1.2f);
    }

    void Update() {
        if (Vector3.Distance(transform.position, player.transform.position) < chaseDistance){
            if ( Vector3.Distance(navMeshAgent.destination, target) < 5 || Vector3.Distance(target, player.transform.position) > 10) {
                target = player.transform.position;
                navMeshAgent.destination = target;
                
            }
        } 

        if (Vector3.Distance(transform.position, player.transform.position) < speakDistance){
            if (!GetComponent<AudioSource>().isPlaying){
                GetComponent<AudioSource>().PlayDelayed(5);
            }
        }
        
        if (Vector3.Distance(transform.position, player.transform.position) < atackDistance){
            player.GetComponent<player_logic>().Damage(damage, 1f);
        }
        
    }
}
