using UnityEngine;

public class MicInput : MonoBehaviour {

    public static float MicLoudness;
    public static float MicThreshold = 0.1f;
    private string _device;
    AudioClip _clipRecord;
    int _sampleWindow = 64;
    bool _isInitialized;

    bool cooldown = false;
    float cooldownTimer = 10.0f;

    public ParticleSystem particleSystem;

    //mic initialization
    void InitMic(){
        if(_device == null) _device = Microphone.devices[0];
        _clipRecord = Microphone.Start(_device, true, 999, 44100);
    }

    void StopMicrophone()
    {
        Microphone.End(_device);
    }

    //get data from microphone into audioclip
    float  LevelMax()
    {
        float levelMax = 0;
        float[] waveData = new float[_sampleWindow];
        int micPosition = Microphone.GetPosition(null)-(_sampleWindow+1); // null means the first microphone
        if (micPosition < 0) return 0;
        _clipRecord.GetData(waveData, micPosition);
        // Getting a peak on the last 128 samples
        for (int i = 0; i < _sampleWindow; i++) {
            float wavePeak = waveData[i] * waveData[i];
            if (levelMax < wavePeak) {
                levelMax = wavePeak;
            }
        }
        return levelMax;
    }

    void Start()
    {
        InitMic();
        _isInitialized = true;
    }

    void Update()
    {
        // levelMax equals to the highest normalized value power 2, a small number because < 1
        // pass the value to a static var so we can access it from anywhere
        MicLoudness = LevelMax();

        if ((MicLoudness > MicThreshold || Input.GetButton("Fire3")) && !cooldown)
        {
            cooldownTimer -= Time.deltaTime;
            Debug.Log("firing: " + cooldownTimer);
            particleSystem.enableEmission = true;
        } else {
            particleSystem.enableEmission = false;
        }
        
        if (cooldown) {
            cooldownTimer += Time.deltaTime;
            Debug.Log("regen: " + cooldownTimer);
            if (cooldownTimer >= 10.0f) {
                cooldownTimer = 10.0f;
                cooldown = false;
            }
        }

        if (cooldownTimer <= 0.0f) {
            cooldownTimer = 0.0f;
            cooldown = true;
        }
    }

    void OnDestroy()
    {
        StopMicrophone();
    }
}