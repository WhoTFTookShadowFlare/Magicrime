using Godot;

namespace Magicrime.Spells.Actions.CastorEffects;

[GlobalClass]
public partial class SpellDamage : SpellAction
{
	private float damage = 1.0f;

	[Export]
	public float Damage
	{
		get => damage;
		set => damage = Mathf.Max(1.0f, value);
	}

	public override string GenerateGDScript(int indentation)
	{
		return $"{new string('\t', indentation)}who.Health -= {damage}\n";
	}

	public override int GetComplexity()
	{
		return 1;
	}

	public override float GetManaCost()
	{
		return damage * 10.0f;
	}
}