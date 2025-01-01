using Godot;

namespace Magicrime.Spells.Projectile;

public sealed partial class AOEProjectileTimer : Timer
{
	private SpellCastor castor;
	
	[Export]
	public SpellCastor Castor
	{
		get => castor;
		set => castor = value;
	}

	[Signal]
	public delegate void ApplyAffectToTargetEventHandler(SpellCastor castor);

	public AOEProjectileTimer()
	{
		Autostart = true;
		OneShot = false;
	}

	public override void _Ready()
	{
		Timeout += () => {
			EmitSignal(SignalName.ApplyAffectToTarget, castor);
		};
	}
}