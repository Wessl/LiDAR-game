using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Interactible : MonoBehaviour
{
    public Animator transition;
    public float transitionTime = 1f;
    
    enum Interaction
    {
        Move,
        Destroy,
        Rotate,
        Goal
        // More here in the future
    }
    [SerializeField] Interaction interactionType;       // Set this in inspector of each game object 
    [SerializeField] private Vector3 interactionMovement;
    [SerializeField] private Vector3 interactionRotation;
    [SerializeField] private float rotationSpeed;
    private float rotationTime;
    [SerializeField] private float timesRotated = 1;
    private bool isBusy;
    private AudioSource audioSource;
    public AudioClip doorOpeningSound;
    public AudioClip destroySound;
    public AudioClip winSound;
    public GameObject destroyedMesh;
    public GameObject tempAudioPlayer;
    

    private void Start()
    {
        rotationTime = 1 / rotationSpeed;
        isBusy = false;
        audioSource = GetComponent<AudioSource>();
        if (transition != null) transition.gameObject.SetActive(true);
    }


    public void Interact()
    {
        // Player should do occasional checks to see if they are in range of interactible object
        // If they are they are shown an iteraction button - (not just a button, think outside the box - maybe it causes all the lidar rays to concentrate on something, or maybe thats stupid and we are doing something completely different)
        if (isBusy) return;
        switch (interactionType)
        {
            case Interaction.Move:
                transform.Translate(interactionMovement);
                break;
            case Interaction.Rotate:
                StartCoroutine(RotateOverTime());
                break;
            case Interaction.Goal:
                StartCoroutine(EnterGoal());
                break;
            case Interaction.Destroy:
                DestroyInteractible();
                break;
            default:
                // nothing
                break;
            
        }
    }

    private IEnumerator RotateOverTime()
    {
        isBusy = true;
        float timePassed = 0;
        audioSource.clip = doorOpeningSound;
        audioSource.Play();
        while (timePassed < rotationTime)
        {
            transform.Rotate(interactionRotation * Time.deltaTime * rotationSpeed);
            timePassed += Time.deltaTime;
            yield return null;
        }

        timesRotated++;
        transform.localRotation = Quaternion.Euler(new Vector3(Mathf.Repeat(interactionRotation.x * timesRotated, 360),
            Mathf.Repeat(interactionRotation.y * timesRotated, 360),Mathf.Repeat(interactionRotation.z * timesRotated, 360)));
        isBusy = false;
    }

    IEnumerator EnterGoal()
    {
        // Play sound
        var persistentAudio = GameObject.FindObjectOfType<PersistentAudio>().GetComponent<AudioSource>();
        persistentAudio.clip = winSound;
        persistentAudio.Play();
        // Play animation
        transition.SetTrigger("Start");
        // Fade out colors... 
        GameObject.FindObjectOfType<DrawCircles>().DecreaseColorOverTime();
        // Wait
        yield return new WaitForSeconds(transitionTime);
        // Load scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private void DestroyInteractible()
    {
        // Also do other things first
        var tempAudio = Instantiate(tempAudioPlayer, transform.position, Quaternion.identity);
        var tempAS = tempAudio.GetComponent<AudioSource>();
        var length = destroySound.length;
        tempAS.clip = destroySound;
        tempAS.Play();
        Destroy(tempAS, length);
        // instead of destroying, shatter into cubes! do something in blender idk
        var destroyedGO = Instantiate(destroyedMesh, transform.position, transform.rotation);
        foreach (var meshChild in destroyedGO.GetComponentsInChildren<MeshCollider>())
        {
            meshChild.transform.localScale = this.transform.localScale;
        }
        Debug.Log(destroyedGO.name);
        
        DestroyImmediate(gameObject);
        Physics.SyncTransforms();
        // Destroy(this.gameObject);
    }
}
