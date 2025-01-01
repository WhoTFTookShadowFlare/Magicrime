using Godot;

namespace Magicrime;

[GlobalClass]
public partial class SpellCastor : CharacterBody3D
{
	private float manaGenDelay = 1.0f;
	private float manaGenDelayRem = 1.0f;
	private float manaGenRate = 25.0f;

	private float castDelay = 0.5f;
	private float castDelayRem = 0.0f;

	[Signal]
	public delegate void ManaChangedEventHandler();

	[Signal]
	public delegate void HealthChangedEventHandler();

	[Export]
	public Node3D LookDirection;

	[Export]
	private float maxMana = 100.0f;

	private float mana = 100.0f;

	[Export]
	private float maxHealth = 100.0f;

	private float health = 100.0f;

	[Export]
	public float Mana
	{
		get => mana;
		set
		{
			mana = Mathf.Clamp(value, 0.0f, maxMana);
			EmitSignal(SignalName.ManaChanged);
		}
	}

	[Export]
	public float Health
	{
		get => health;
		set
		{
			health = value;
			EmitSignal(SignalName.HealthChanged);
		}
	}

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

	public float UseMana(float amount)
	{
		float missing = amount - Mana;
		if(missing >= 0.0f) return missing;
		Mana -= amount;
		manaGenDelayRem = manaGenDelay;
		return 0.0f;
	}

	public void ApplyCastDelay()
	{
		castDelayRem = castDelay;
	}

	public bool CanCastSpell()
	{
		return castDelayRem <= 0.0f;
	}

	public override void _PhysicsProcess(double delta)
	{
		castDelayRem -= (float) delta;
		manaGenDelayRem -= (float) delta;
		if(manaGenDelayRem <= 0.0f) Mana += (float) delta * manaGenRate;
	}

	public virtual int GetMaxComplexity()
	{
		return 10;
	}
}