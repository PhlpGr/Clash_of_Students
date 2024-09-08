using System;
using System.Runtime.InteropServices;
using UnityEngine;
using TMPro;

public class JWTDisplayManager : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern IntPtr GetJWTPayload();

    public TMP_Text userInfoText;  // Reference to the TextMeshPro UI element
    private string jwtToken;       // Store the full JWT token
    private JWTData jwtData;       // Store the parsed JWT data

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
                //userInfoText.text = $"Welcome {jwtData.firstname} {jwtData.lastname}!";
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

    // Additional methods can use the full JWT token or parsed data as needed
}


// mocked Token data transfer
public getMockData()
{
    jwtData2 = new JWTData(
        "tom@one7.one", "Tom", "Mitrovic",  "tom@one7.one", "Digital Business Engineering", "it"
    );
    return jwtData2;

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
    // Add more fields as per your JWT payload structure
}
