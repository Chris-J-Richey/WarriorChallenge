using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour {

    private bool selected;
    private bool attacking;
    private Vector3 goTo;
    public string Owner;
    public ParticleSystem SelectIndicator;
    public ParticleSystem TeamIndicator;
    public Terrain goTerrain;
    public Vector3 holder3;
    public float Health = 100;
    public float damage = 1;
    public float steps;
    public float TimePerStep;
    public int counter;

    // Use this for initialization
    void Start () {
        holder3 = this.transform.position;
        goTo = transform.position;
        if (!TeamIndicator.isPlaying) TeamIndicator.Play();
        if (SelectIndicator.isPlaying) SelectIndicator.Stop();
        selected = false;
        attacking = false;
    }
	
	// Update is called once per frame
	void Update () {
        if (selected) {
            if (!SelectIndicator.isPlaying) SelectIndicator.Play();
            if (Input.GetMouseButtonDown(1)) {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (goTerrain.GetComponent<Collider>().Raycast(ray, out hit, Mathf.Infinity)) {
                    goTo = hit.point;
                    selected = false;
                    holder3 = transform.position;
                    steps = (Vector3.Distance(holder3, goTo)) / 1f;
                    TimePerStep = 1 / steps;
                    counter = 0;
                }
            }
        }
        else {
            if (SelectIndicator.isPlaying) {
                SelectIndicator.Stop();
            }
        }
        
        if (counter < steps) {
            TimePerStep += Time.deltaTime;
            transform.position = Vector3.Lerp(holder3, goTo, TimePerStep);
            transform.rotation = Quaternion.LookRotation(goTo - holder3);
            counter++;
        }
        if (Health <= 0) {
            Destroy(gameObject, 0);
        }
    }

    public string GetOwner() {
        return Owner;
    }


    private void OnMouseDown() {
        if (Owner == "Player") {
            selected = true;
        }
    }
    
    private void OnTriggerEnter(Collider other) {
        if (other != null) {
            StartCoroutine(Damage(other));
        }
        else {
            StopCoroutine(Damage(other));
        }
    }
    private void OnTriggerExit(Collider other) {
        if (other != null) {
            StopCoroutine(Damage(other));
        }
    }
    IEnumerator Damage(Collider other) {
        while (true && other != null) {
            if (other.gameObject.GetComponent<PlayerControls>() != null && other.gameObject.GetComponent<PlayerControls>().Owner != Owner) {
                other.gameObject.GetComponent<PlayerControls>().Health -= damage;
            }
            yield return new WaitForSeconds(.5f);
        }
    }
    /*
    private void OnTriggerEnter(Collider other) {
        if (other != null && !attacking) {
            attacking = true;
            StartCoroutine(Damage(other));
        }
        else {
            StopCoroutine(Damage(other));
        }
    }
    private void OnTriggerExit(Collider other) {
        if (other != null) {
            attacking = false;
            StopCoroutine(Damage(other));
        }
    }
    IEnumerator Damage(Collider other) {
        while (true && other != null) {
            if (other.gameObject.GetComponent<PlayerControls>() != null && other.gameObject.GetComponent<PlayerControls>().Owner != Owner) {
                other.gameObject.GetComponent<PlayerControls>().Health -= damage;
            }
            yield return new WaitForSeconds(.5f);
        }
        attacking = false;
    }*/
}
