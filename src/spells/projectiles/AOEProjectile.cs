using Godot;
using Godot.Collections;

namespace Magicrime.Spells.Projectile;

[GlobalClass]
public partial class AOEProjectile : BaseProjectile
{
	private float effectRadius = 5.0f;

	[Export]
	public float EffectRadius
	{
		get => effectRadius;
		set => effectRadius = value;
	}

	private float effectApplicationRate = 1.0f;

	[Export]
	public float EffectApplicationRate
	{
		get => effectApplicationRate;
		set => effectApplicationRate = value;
	}

	private Dictionary<SpellCastor, AOEProjectileTimer> applicationTimers = [];

	[Signal]
	public delegate void ApplyAffectToTargetEventHandler(SpellCastor who);

	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);
	}

	public void ApplyEffectTo(Node3D body)
	{
		if(body is not SpellCastor castor) return;

		EmitSignal(SignalName.ApplyAffectToTarget, castor);
		AOEProjectileTimer timer = new()
		{
			Castor = castor,
			WaitTime = EffectApplicationRate
		};
		timer.ApplyAffectToTarget += (SpellCastor castor) => EmitSignal(SignalName.ApplyAffectToTarget, castor);
		AddChild(timer);
		applicationTimers[castor] = timer;
	}

	public void StopEffectTo(Node3D body)
	{
		if(body is not SpellCastor castor) return;
		applicationTimers[castor].QueueFree();
		applicationTimers.Remove(castor);
	}

	public override void _EnterTree()
	{
		BodyEntered += ApplyEffectTo;
		BodyExited += StopEffectTo;
		base._EnterTree();

		CollisionShape3D shape3D = new()
		{
			Shape = new SphereShape3D()
			{
				Radius = effectRadius
			}
		};
		AddChild(shape3D);
	}
}