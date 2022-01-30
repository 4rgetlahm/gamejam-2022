using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

public class PointClickMovement : MonoBehaviour
{
    [SerializeField]
    private Camera localCamera;
    [SerializeField]
    private NavMeshAgent navMeshAgent;

    public Animator animator;

    public float animationSpeed;

    private Vector3 LastPosition;

    public float PlayerMovementSpeed;

    public AudioClip grass;
    public AudioClip sand;
    public AudioClip snow;

    private SoundType lastSound;

    private AudioSource Source;

    void Start()
    {
        Source = GetComponent<AudioSource>();
        animator = gameObject.GetComponent<Animator>();
        animator.speed = animationSpeed;
        LastPosition = Player.GetPosition();
    }

    void Update()
    {
        if (!DialogHandler.IsAnyDialogOpen() && Input.GetMouseButtonDown(0))
        {
            Ray ray = localCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitPoint;

            if (Physics.Raycast(ray, out hitPoint))
            {
                navMeshAgent.speed = PlayerMovementSpeed;
                navMeshAgent.SetDestination(hitPoint.point);
                animator.SetBool("IsWalking", true);
                PlayerMovementSounds();
            }
        }
        var newPosition = Player.GetPosition();
        if (LastPosition == newPosition)
        {
            animator.SetBool("IsWalking", false);
            Source.Stop();
        }
        else
        {
            animator.SetBool("IsWalking", true);
            PlayerMovementSounds();
        }
        LastPosition = newPosition;
    }

    private void PlaySound(SoundType sound)
    {
        if (Source.isPlaying && sound != lastSound)
        {
            Source.Stop();
        }
        if (!Source.isPlaying)
        {
            AudioClip clip = null;
            switch (sound)
            {
                case SoundType.Grass:
                    clip = grass;
                    break;
                case SoundType.Snow:
                    clip = snow;
                    break;
                default:
                    clip = sand;
                    break;
            }
            Source.PlayOneShot(clip, 0.5f);

        }
        lastSound = sound;
    }

    private void PlayerMovementSounds()
    {
        if(!TouchedGameObjects.GameObjects.Any())
            return;
        if (TouchedGameObjects.GameObjects.Select(p => p.tag).Any(p => p == "Grass"))
            PlaySound(SoundType.Grass);
        else if (TouchedGameObjects.GameObjects.Select(p => p.tag).Any(p => p == "Snow"))
            PlaySound(SoundType.Snow);
        else
            PlaySound(SoundType.Sand);
    }

    void OnTriggerEnter(Collider col)
    {
        TouchedGameObjects.GameObjects.Add(col.gameObject);
    }

    void OnTriggerExit(Collider col)
    {
        TouchedGameObjects.GameObjects.Remove(col.gameObject);
    }
}
