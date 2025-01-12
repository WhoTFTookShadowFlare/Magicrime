using Godot;
using Godot.Collections;

namespace Magicrime.Spells;

[Tool]
[GlobalClass]
public partial class Spell : Resource
{
	public const string SPELL_DIR = "user://spells";

	[Signal]
	public delegate void CompilationFailedEventHandler(Error error);

	private string spellName = "Example Spell";
	
	[Export]
	public string SpellName
	{
		get => spellName;
		set => spellName = value;
	}

	private Array<SpellAction> actions = [];

	[Export]
	public Array<SpellAction> Actions
	{
		get => actions;
		set => actions = value;
	}

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
		if(Engine.IsEditorHint()) return null;

		string spellCode = SPELL_BASE;
		foreach(SpellAction act in actions)
		{
			if(act is null) continue;
			spellCode += act.GenerateGDScript(1) + '\n';
		}
		spellCode += SPELL_END;
		if(OS.IsStdOutVerbose()) GD.Print($"Made spell code: \"\"\"\n{spellCode}\n\"\"\"");
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
