using Godot;
using Magicrime.Spells;

[GlobalClass]
public sealed partial class SpellComment : SpellAction
{
	[Export(PropertyHint.MultilineText)]
	public string comment = "Document the spell!";

	public override string GenerateGDScript(int indentation)
	{
		string commentCooked = comment.Replace("\n", $"\n{new string('\t', indentation)}# ");
		return $"{new string('\t', indentation)}# {commentCooked}\n";
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