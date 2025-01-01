using Godot;

namespace Magicrime.Spells.Projectile;

[GlobalClass]
public partial class NormalProjectile : BaseProjectile
{
	
	private float radius = 0.1f;
	private float height = 0.5f;

	private int pierce = 1;

	private SpellCastor ignored;

	[Export]
	public float Radius
	{
		get => radius;
		set => radius = value;
	}

	[Export]
	public float Height
	{
		get => height;
		set => height = value;
	}

	[Export]
	public int Pierce
	{
		get => pierce;
		set => pierce = value;
	}

	[Export]
	public SpellCastor Ignored
	{
		get => ignored;
		set => ignored = value;
	}

	[Signal]
	public delegate void HitCharacterEventHandler(SpellCastor castor);

	public void HandleBodyEntry(Node3D body)
	{
		if(body is SpellCastor castor)
		{
			if(castor == ignored) return;
			EmitSignal(SignalName.HitCharacter, castor);
			if(--pierce <= 0) QueueFree();
		}
		else
			LifeTimeRem = -1.0f;
	}

	public override void _EnterTree()
	{
		base._EnterTree();
		CollisionShape3D shape3D = new()
		{
			Shape = new CapsuleShape3D()
			{
				Radius = Radius,
				Height = Height
			}
		};
		AddChild(shape3D);

		BodyEntered += HandleBodyEntry;
	}

	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);
		Position += Velocity;
	}
}