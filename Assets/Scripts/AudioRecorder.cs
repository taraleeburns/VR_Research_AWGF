using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class AudioRecorder : MonoBehaviour
{
    private AudioClip recordedClip;

    private static AudioRecorder activeRecorder;

    [SerializeField] private AudioSource audioSource;
    public AudioSource comeCloserSFX;

    public int wishId = 0;

    private string directoryPath;

    public bool clipRecorded = false;
    private bool canRecord = false;
    private bool isRecording = false;

    public WishStateController wishState;

    private const int MAX_RECORDING_SECONDS = 10;

    private void Awake()
    {
        directoryPath = Path.Combine(Application.persistentDataPath, "Recordings");

        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        foreach (var mic in Microphone.devices)
        {
            Debug.Log("Mic device: " + mic);
        }
    }

    public void setCanRecord(bool state)
    {
        canRecord = state;

        Debug.Log("canRecord changed: " + canRecord);
        Debug.Log($"[Trigger] {gameObject.name} | ID: {GetInstanceID()}");
    }
public void StopRecordingDelayed()
{
    StartCoroutine(StopNextFrame());
}

private IEnumerator StopNextFrame()
{
    yield return null;    // wait exactly one frame
    StopRecording();
}
    // -------------------------
    // START RECORDING
    // -------------------------
public void StartRecording()
{
    Debug.Log(
    $"StartRecording called on {gameObject.name}\n" +
    $"clipRecorded={clipRecorded}, isRecording={isRecording}, canRecord={canRecord}\n" +
    StackTraceUtility.ExtractStackTrace()
);

    if (clipRecorded)
    {
        Debug.Log("StartRecording aborted: clip already recorded.");
        return;
    }

    if (isRecording)
    {
        Debug.Log("StartRecording aborted: already recording.");
        return;
    }

    if (!canRecord)
    {
        Debug.Log("StartRecording aborted: canRecord is false.");
        playComeCloser();
        return;
    }

        string device = Microphone.devices.Length > 0
            ? Microphone.devices[0]
            : null;

        if (device == null)
        {
            Debug.LogError("No microphone device found!");
            return;
        }

        Debug.Log("Using mic: " + device);

        recordedClip = Microphone.Start(device, false, MAX_RECORDING_SECONDS, 44100);

        isRecording = true;

        Debug.Log("Recording STARTED");
    }

    // -------------------------
    // STOP RECORDING (CRITICAL FIX)
    // -------------------------
    public void StopRecording()
    {
        Debug.Log($"StopRecording CALLED | isRecording = {isRecording}");
        if (!isRecording)
            return;
try
    {
        string device = Microphone.devices.Length > 0
            ? Microphone.devices[0]
            : null;

        if (device == null)
        {
            Debug.LogError("No microphone device found on stop!");
            isRecording = false;
            return;
        }

        int position = Microphone.GetPosition(device);

        Microphone.End(device);

        if (position <= 0 || recordedClip == null)
        {
            Debug.LogWarning("No audio captured!");
            isRecording = false;
            return;
        }

        float[] data = new float[position * recordedClip.channels];
        recordedClip.GetData(data, 0);

        AudioClip newClip = AudioClip.Create(
            "RecordedClip",
            position,
            recordedClip.channels,
            recordedClip.frequency,
            false
        );

        newClip.SetData(data, 0);

        recordedClip = newClip;

        SaveRecording();

        audioSource.clip = recordedClip;

        Debug.Log(
    $"About to assign wish audio. " +
    $"wishState={(wishState != null ? wishState.gameObject.name : "NULL")} " +
    $"clip={(recordedClip != null ? recordedClip.length : 0)}"
);

        if (wishState != null)
            wishState.SetWishAudio(recordedClip);

        clipRecorded = true;
        Debug.Log($"Recorded samples: {position}");
        Debug.Log("Clip length: " + recordedClip.length);
       }
    catch (System.Exception e)
    {
        Debug.LogError($"StopRecording failed: {e}");
    }
    finally
    {
        isRecording = false;   // ALWAYS resets, no matter what went wrong above
    }

    }

    // -------------------------
    // SAVE WAV
    // -------------------------
    public void SaveRecording()
    {
        if (recordedClip == null)
        {
            Debug.LogError("No recording found to save.");
            return;
        }

        //int fileCount = Directory.GetFiles(directoryPath, "*.wav").Length;
        string filePath = Path.Combine(directoryPath, "recording_" + wishId + ".wav");

        WavUtility.Save(filePath, recordedClip);

        Debug.Log("Recording saved as " + filePath);
    }

    // -------------------------
    // COME CLOSER AUDIO
    // -------------------------
    private void playComeCloser()
    {
        if (comeCloserSFX != null && !clipRecorded)
        {
            comeCloserSFX.Play();
        }
    }

    // -------------------------
    // MANUAL USE (DEBUG / GRAB)
    // -------------------------
    public void UseRecording()
    {
        if (recordedClip == null)
        {
            Debug.LogWarning("No recording available.");
            return;
        }

        audioSource.clip = recordedClip;

        if (wishState != null)
            wishState.SetWishAudio(recordedClip);

        Debug.Log("Recording assigned to Wish.");
    }
}