using Godot;

namespace Magicrime.Spells.Projectile;

// TODO: Projectile types:
//			- mine
//			- aoe
//			- explosive (aoe/frag)

[GlobalClass]
public abstract partial class BaseProjectile : Area3D
{
	private float lifeTimeRem = 0.0f;
	
	[Export]
	public float LifeTimeRem
	{
		get => lifeTimeRem;
		set
		{
			lifeTimeRem = value;
			if(lifeTimeRem <= 0.0f)
			{
				EmitSignal(SignalName.LifeSpanEnded);
			}
		}
	}

	[Signal]
	public delegate void LifeSpanEndedEventHandler();

	public override void _EnterTree()
	{
		LifeSpanEnded += QueueFree;
	}

	public override void _PhysicsProcess(double delta)
	{
		LifeTimeRem -= (float) delta;
	}
}