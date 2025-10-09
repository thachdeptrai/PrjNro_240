using System;
using System.Collections.Generic;
using UnityEngine;

public class VoiceChatManager : MonoBehaviour
{
    public static VoiceChatManager Instance { get; private set; }
    
    // Voice Chat States
    public bool isVoiceOn = false;
    public bool isMuted = false;
    
    // Audio Components
    private AudioSource audioSource;
    private AudioClip audioClip;
    private string microphone;
    private bool isRecording = false;
    
    // Voice Chat Settings
    public float masterVolume = 0.8f;
    public float allVolume = 0.6f;
    
    // Network - Tối ưu hóa
    private float lastSendTime = 0f;
    private float sendInterval = 0.5f; // Tăng lên 500ms để giảm lag
    private int frameSkip = 0; // Skip frames để giảm tải
    
    // Profanity Filter
    private List<string> bannedWords = new List<string>
    {
        "fuck", "shit", "damn", "bitch", "asshole", "cunt",
        "địt", "đụ", "đéo", "đĩ", "lồn", "cặc", "đụ má"
    };
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    void Start()
    {
        InitializeMicrophone();
        InitializeAudioSource();
    }
    
    void Update()
    {
        // Skip frames để giảm tải CPU
        frameSkip++;
        if (frameSkip < 3) return; // Chỉ xử lý mỗi 3 frames
        frameSkip = 0;
        
        // Chỉ xử lý voice khi cần thiết và không đang load map
        if (isVoiceOn && isRecording && !Char.isLoadingMap)
        {
            ProcessVoiceData();
        }
    }
    
    private void InitializeMicrophone()
    {
        if (Microphone.devices.Length > 0)
        {
            microphone = Microphone.devices[0];
            Debug.Log("Microphone initialized: " + microphone);
        }
        else
        {
            Debug.LogError("No microphone found!");
        }
    }
    
    private void InitializeAudioSource()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.loop = false;
    }
    
    public void ToggleVoice()
    {
        isVoiceOn = !isVoiceOn;
        
        if (isVoiceOn)
        {
            StartVoiceChat();
        }
        else
        {
            StopVoiceChat();
        }
        
        Debug.Log("Voice Chat: " + (isVoiceOn ? "ON" : "OFF") + " - Channel: All");
    }
    
    public void ToggleMute()
    {
        isMuted = !isMuted;
        Debug.Log("Mute: " + (isMuted ? "ON" : "OFF"));
    }
    
    private void StartVoiceChat()
    {
        if (isMuted || microphone == null) return;
        
        try
        {
            // Giảm sample rate và buffer size để giảm lag
            audioClip = Microphone.Start(microphone, true, 5, 22050); // Giảm từ 10s xuống 5s, từ 44100 xuống 22050
            isRecording = true;
            
            // Show voice indicator
            ShowVoiceIndicator();
            
            Debug.Log("Voice recording started");
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to start voice recording: " + e.Message);
        }
    }
    
    private void StopVoiceChat()
    {
        if (isRecording)
        {
            Microphone.End(microphone);
            isRecording = false;
            
            // Hide voice indicator
            HideVoiceIndicator();
            
            Debug.Log("Voice recording stopped");
        }
    }
    
    private void ProcessVoiceData()
    {
        // Tăng interval để giảm tần suất gửi
        if (Time.time - lastSendTime >= sendInterval)
        {
            // Kiểm tra kết nối trước khi xử lý
            if (!Session_ME.gI().isConnected())
            {
                return;
            }
            
            try
            {
                // Giảm kích thước sample để giảm lag
                int sampleSize = Mathf.Min(audioClip.samples, 1024); // Chỉ lấy 1024 samples
                float[] samples = new float[sampleSize];
                audioClip.GetData(samples, 0);
                
                // Convert to bytes
                byte[] audioData = ConvertFloatArrayToByteArray(samples);
                
                // Check for profanity (basic implementation)
                if (!ContainsProfanity(audioData))
                {
                    // Send voice data through network
                    SendVoiceData(audioData);
                }
                else
                {
                    Debug.Log("Profanity detected, blocking voice transmission");
                }
                
                lastSendTime = Time.time;
            }
            catch (Exception e)
            {
                Debug.LogError("Error processing voice data: " + e.Message);
            }
        }
    }
    
    private byte[] ConvertFloatArrayToByteArray(float[] floatArray)
    {
        byte[] byteArray = new byte[floatArray.Length * 4];
        Buffer.BlockCopy(floatArray, 0, byteArray, 0, byteArray.Length);
        return byteArray;
    }
    
    private bool ContainsProfanity(byte[] audioData)
    {
        // Simplified profanity check - always return false for now
        return false;
    }
    
    private void SendVoiceData(byte[] audioData)
    {
        if (Session_ME.gI().isConnected())
        {
            try
            {
                Message message = new Message((sbyte)(-100)); // Voice chat command
                message.writer().writeByte(0); // Luôn gửi với channel type = 0 (All)
                message.writer().writeInt(audioData.Length); // Data length

                // Chuyển byte[] -> sbyte[]
                sbyte[] sbyteAudio = Array.ConvertAll(audioData, b => (sbyte)b);
                message.writer().write(sbyteAudio); // Audio data

                Session_ME.gI().sendMessage(message);
            }
            catch (Exception e)
            {
                Debug.LogError("Error sending voice data: " + e.Message);
            }
        }
    }

    public void ReceiveVoiceData(Message message)
    {
        try
        {
            sbyte channelType = message.reader().readByte();
            int dataLength = message.reader().readInt();

            // Tạo buffer sbyte[]
            sbyte[] audioDataSbyte = new sbyte[dataLength];
            message.reader().read(ref audioDataSbyte, 0, dataLength);

            // Nếu bạn cần byte[] để PlayVoiceData(byte[])
            byte[] audioData = Array.ConvertAll(audioDataSbyte, b => (byte)b);

            // Chỉ phát voice từ All channel (channelType = 0)
            if (channelType == 0 && !isMuted)
            {
                PlayVoiceData(audioData);
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to receive voice data: " + e.Message);
        }
    }
    
    private void PlayVoiceData(byte[] audioData)
    {
        try
        {
            // Convert bytes back to float array
            float[] samples = new float[audioData.Length / 4];
            Buffer.BlockCopy(audioData, 0, samples, 0, audioData.Length);
            
            // Create AudioClip and play
            AudioClip clip = AudioClip.Create("VoiceData", samples.Length, 1, 22050, false); // Giảm sample rate
            clip.SetData(samples, 0);
            
            audioSource.clip = clip;
            audioSource.volume = allVolume;
            audioSource.Play();
        }
        catch (Exception e)
        {
            Debug.LogError("Error playing voice data: " + e.Message);
        }
    }
    
    private void ShowVoiceIndicator()
    {
        // Show voice indicator in UI
        if (GameScr.gI() != null)
        {
            GameScr.gI().SetVoiceIndicator(true);
        }
    }
    
    private void HideVoiceIndicator()
    {
        // Hide voice indicator in UI
        if (GameScr.gI() != null)
        {
            GameScr.gI().SetVoiceIndicator(false);
        }
    }
    
    // Thêm method để tạm dừng voice chat khi đang load map
    public void PauseVoiceChat()
    {
        if (isRecording)
        {
            StopVoiceChat();
        }
    }

    public void ResumeVoiceChat()
    {
        if (isVoiceOn && !isMuted && microphone != null)
        {
            StartVoiceChat();
        }
    }
    
    public void SetVolume(float volume, string volumeType)
    {
        switch (volumeType)
        {
            case "master":
                masterVolume = volume;
                break;
            case "all":
                allVolume = volume;
                break;
        }
        
        Debug.Log($"Volume set - {volumeType}: {volume}");
    }
    
    public bool IsVoiceOn()
    {
        return isVoiceOn;
    }
    
    public bool IsMuted()
    {
        return isMuted;
    }
    
    public string GetChannelName()
    {
        return "All";
    }
    
    public string GetVoiceStatus()
    {
        if (isMuted) return "Muted";
        if (isVoiceOn) return "Speaking";
        return "Off";
    }
}
