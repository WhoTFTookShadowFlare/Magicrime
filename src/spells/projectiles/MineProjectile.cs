using Godot;

namespace Magicrime.Spells.Projectile;

[GlobalClass]
public partial class MineProjectile : BaseProjectile
{
	private float radius = 0.25f;

	[Export]
	public float Radius
	{
		get => radius;
		set => radius = Mathf.Max(0.1f, value);
	}

	[Export]
	public int RemainingProcs = 1;

	[Signal]
	public delegate void MineProcEventHandler(SpellCastor who);

	public void HandleBodyEntry(Node3D body)
	{
		if(body is not SpellCastor castor) return;
		EmitSignal(SignalName.MineProc, castor);
		if(--RemainingProcs == 0) QueueFree();
	}

	public override void _PhysicsProcess(double delta)
	{	}

	public override void _EnterTree()
	{
		BodyEntered += HandleBodyEntry;

		CollisionShape3D shape3D = new()
		{
			Shape = new SphereShape3D()
			{
				Radius = Radius
			}
		};
		AddChild(shape3D);
	}
}