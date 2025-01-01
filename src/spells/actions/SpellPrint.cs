using Godot;

namespace Magicrime.Spells.Actions;

[GlobalClass]
public sealed partial class SpellPrint : SpellAction
{
	[Export]
	public string output = "HEWO :3";

	public override string GenerateGDScript(int indentation)
	{
		
		return $"{new string('\t', indentation)}print(\"{output}\")\n";
	}

	public override int GetComplexity()
	{
		return 0;
	}

	public override float GetManaCost()
	{
		return 0.0f;
	}
}