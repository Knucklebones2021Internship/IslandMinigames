using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// <para> This is a global manager class to handle our input system. </para>
/// <para> All of our minigames should either subscribe to the InputAction events or use it to poll input. </para>
/// <para> Add functionality as necessary. </para>
/// </summary>
[DefaultExecutionOrder(-60)]
public class Scripts_InputManager_Wyatt : MonoBehaviour {
	public static Scripts_InputManager_Wyatt Instance { get; private set; }

	public static PlayerInput input;

	#region INPUT_ACTIONS
	public static InputAction touchPosition;
	public static InputAction touchPress;
	public static InputAction touchTap;
	public static InputAction touchHold;
	public static InputAction touchSlowTap;
	public static InputAction touchDoubleTap;
	public static InputAction touchTripleTap;
	#endregion

	void Awake() {
		if (Instance == null) {
			Instance = this;
			DontDestroyOnLoad(gameObject);

			input = GetComponent<PlayerInput>();

			touchPosition	= input.actions["TouchPosition"];
			touchPress		= input.actions["TouchPress"];
			touchTap		= input.actions["TouchTap"];
			touchHold		= input.actions["TouchHold"];
			touchSlowTap	= input.actions["TouchSlowTap"];
			touchDoubleTap	= input.actions["TouchDoubleTap"];
			touchTripleTap	= input.actions["TouchTripleTap"];
		} else {
			Destroy(gameObject);
		}
	}

	#region ENABLE/DISABLE SENSORS 
	public static bool EnableAttitudeSensor() {
        if (AttitudeSensor.current != null) {
            InputSystem.EnableDevice(AttitudeSensor.current);
			return true;
        } return false;
	}

	public static void DisableAttitudeSensor() {
		InputSystem.DisableDevice(AttitudeSensor.current);
	}
	#endregion

	#region POLL SENSORS
	public static Quaternion GetAttitudeSensorValue() {
		return AttitudeSensor.current.attitude.ReadValue();
	}
	#endregion
}
