using Godot;

namespace Magicrime.Spells.Projectile;

[GlobalClass]
public partial class NormalProjectile : BaseProjectile
{
	
	private float radius = 0.1f;
	private float height = 0.5f;

	private int pierce = 1;

	private SpellCastor ignored;

	private float projGravity = 0.0f;

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

	[Export]
	public float ProjGravity
	{
		get => projGravity;
		set => projGravity = value;
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
			},
			Rotation = new(90, 0, 0)
		};
		AddChild(shape3D);

		BodyEntered += HandleBodyEntry;
	}

	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);
		Vector3 velo = Velocity;
		velo.Y -= projGravity * (float) delta;
		Velocity = velo;
		Position += Velocity * (float) delta;

		if(!Velocity.IsZeroApprox() && !Vector3.Up.Cross(Velocity).IsZeroApprox()) LookAtFromPosition(Position, Position + Velocity);
	}
}