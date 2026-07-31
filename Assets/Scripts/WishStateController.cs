using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class WishStateController : MonoBehaviour
{
    [Header("Wish State")]
    public bool hasWish = false;

    [Header("Audio")]
    public AudioSource wishRecording;

    [Header("Visuals")]
    public Renderer wishRenderer;
    public Color emptyColor = Color.gray;

    private Color filledColor;
    public float forceAmount = 10f;
    private Rigidbody rb;
    public bool isBeingGrabbed = false;

    void Start()
    {
        // If the wish already has audio assigned at start
        if (wishRecording != null && wishRecording.clip != null)
        {
            hasWish = true;
            AssignRandomFilledColor();
        }
        else
        {
            SetEmptyVisual();
        }
        rb = GetComponent<Rigidbody>();
        UnityEngine.Vector3 randDirection = UnityEngine.Random.onUnitSphere;
        rb.AddForce(randDirection * forceAmount, ForceMode.Impulse);
    }

    public bool getBeingGrabbed()
    {
        return isBeingGrabbed;
    }

    public void setIsBeingGrabbed(bool state)
    {
        isBeingGrabbed = state;
    }

    // Called when user finishes recording
    public void SetWishAudio(AudioClip newClip)
    {
         Debug.Log(
        $"SetWishAudio received on {gameObject.name}. " +
        $"hasWish={hasWish}, clipLength={(newClip != null ? newClip.length : 0)}"
    );
        if (hasWish) return;
        wishRecording.clip = newClip;
        hasWish = true;

        AssignRandomFilledColor();
    }

    void AssignRandomFilledColor()
    {
        filledColor = Random.ColorHSV(
            0.6f, 1f,
            0.6f, 1f,
            0.6f, 1f
        );

        wishRenderer.material.color = filledColor;
    }

    void SetEmptyVisual()
    {
        wishRenderer.material.color = emptyColor;
    }

    // Controlled playback
    public void PlayWish()
    {
        if (!hasWish) return;

        if (!wishRecording.isPlaying)
            wishRecording.Play();
    }

    public void StopWish()
    {
        if (wishRecording.isPlaying)
            wishRecording.Stop();
    }
}