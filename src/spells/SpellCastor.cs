using Godot;

namespace Magicrime;

[GlobalClass]
public partial class SpellCastor : CharacterBody3D
{
	[Export]
	public Node3D LookDirection;

	[Export]
	private float maxMana = 100.0f;

	[Export]
	private float mana = 100.0f;

	public Vector3 GetLookDirection()
	{
		return new Vector3(0, 0, -1)
			.Rotated(Vector3.Right, LookDirection.GlobalRotation.X)
			.Rotated(Vector3.Up, LookDirection.GlobalRotation.Y);
	}

	public Vector3 GetSpawnSpot(float distance)
	{
		return GetLookDirection() * distance + GetEyePos();
	}

	public Vector3 GetEyePos()
	{
		return LookDirection.GlobalPosition;
	}
}