using Godot;
using Godot.Collections;

namespace Magicrime.Spells;

[GlobalClass]
public partial class Spell : Resource
{
	[Signal]
	public delegate void CompilationFailedEventHandler(Error error);

	[Export]
	public string spellName = "Example Spell";

	[Export]
	public Array<SpellAction> actions = [];

	const string SPELL_BASE = """
	extends Node

	func _exec_spell(_castor: SpellCastor) -> void:
	

	""";

	const string SPELL_END = """
	
	""";

	public float GetManaCost()
	{
		float cost = 0.0f;
		foreach(SpellAction action in actions)
			cost += action.GetManaCost();
		return cost;
	}

	public int GetComplexity()
	{
		int complexity = 0;
		foreach(SpellAction action in actions)
			complexity += action.GetComplexity();
		return complexity;
	}

	public Script CompileSpell()
	{
		string spellCode = SPELL_BASE;
		foreach(SpellAction act in actions)
		{
			if(act is null) continue;
			spellCode += act.GenerateGDScript(1) + '\n';
		}
		spellCode += SPELL_END;
		GD.Print(spellCode);
		GDScript script = new()
		{
			SourceCode = spellCode
		};
		Error error = script.Reload();
		if(error != Error.Ok)
		{
			GD.PushError($"Failed to build spell with error: {error}");
			return null;
		}
		return script;
	}
}
