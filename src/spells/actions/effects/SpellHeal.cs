using Godot;

namespace Magicrime.Spells.Actions.CastorEffects;

[Tool]
[GlobalClass]
public partial class SpellHeal : SpellAction
{
	private float amount = 1.0f;

	[Export]
	public float Amount
	{
		get => amount;
		set => amount = Mathf.Max(1.0f, value);
	}

	public override string GenerateGDScript(int indentation)
	{
		return $"{new string('\t', indentation)}_who.Health += {amount}\n";
	}

	public override int GetComplexity()
	{
		return 1;
	}

	public override float GetManaCost()
	{
		return amount * 20.0f;
	}
}