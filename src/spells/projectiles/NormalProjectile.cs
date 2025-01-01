using Godot;

namespace Magicrime.Spells.Projectile;

[GlobalClass]
public partial class NormalProjectile : BaseProjectile
{
	private Vector3 velo;

	[Export]
	public Vector3 Velocity
	{
		get => velo;
		set => velo = value;
	}

	private float radius = 0.1f;
	private float height = 0.5f;

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
	}

	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);
		Position += velo;
	}
}