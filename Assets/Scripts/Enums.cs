//GLOBAL ENUM CONTAINER

/// <summary>
/// Enums For Interactable objects
/// </summary>
public enum ObjectType
{
	Critical,
	Standard,
}

public enum ControllerMode
{
	VR,
	PC
}

public enum GameMode
{

	Tutorial,
	Training,
	Exam,
}

public enum PCControlSet
{
	First,
	Second,
}


public enum ColorblindMode
{
	Normal,
	Deuteranomaly,
	Protanomaly,
	Protanopia,
	Deuteranopia,
	Tritanopia,
	Tritanomaly,
	Monochromacy
}

public enum EyesightMode
{
	Normal,
	NearSighted,
	FarSighted,
	Bad
}

public enum FollowMode
{
	FollowBehind,
	FollowLeft,
	FollowLeftAndHide,
	Instant,
	ControllerInstant,
}

public enum TabletStateID
{
	Hold,
	Follow,
	FollowSide,
	FrontHMD,
	FrontController,
}

public enum MeshShape
{
	Stairs,
	Fence,
	Slope,
}