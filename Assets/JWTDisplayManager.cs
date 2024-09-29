using System;
using System.Runtime.InteropServices;
using UnityEngine;
using TMPro;

public class JWTDisplayManager : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern IntPtr GetJWTPayload();

    public TMP_Text userInfoText; // Reference to the TextMeshPro UI element
    private string jwtToken; // Store the full JWT token
    private JWTData jwtData; // Store the parsed JWT data
    
    public static JWTDisplayManager Instance;

    private void Awake()
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
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            // Get the JWT payload from the JavaScript function
            string jsonPayload = Marshal.PtrToStringUTF8(GetJWTPayload());
            if (!string.IsNullOrEmpty(jsonPayload))
            {
                // Store the full JWT token
                jwtToken = jsonPayload;

                // Parse the JSON string into a C# object
                jwtData = JsonUtility.FromJson<JWTData>(jwtToken);

                // Display the user's name in the TextMeshPro UI element
                userInfoText.text = $"Welcome {jwtData.professor_email}!";
            }
            else
            {
                userInfoText.text = "No JWT payload found.";
            }
        }
        else
        {
            userInfoText.text = "This platform does not support WebGL plugins.";
        }
    }

    // Getter method to access JWTData safely
    public JWTData GetJWTData()
    {
        return jwtData;
    }
}

[Serializable]
public class JWTData
{
    public string email;
    public string firstname;
    public string lastname;
    public string professor_email;
    public string program;
    public string course;
}
