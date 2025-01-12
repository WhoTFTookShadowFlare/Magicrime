using Godot;
using Godot.Collections;

namespace Magicrime.Spells.Actions;

[GlobalClass]
public sealed partial class SpellCreateMine : SpellAction
{
	[Export]
	public string projectileVarName = "mine";

	[Export]
	public string spawnTarget = "_castor";

	[Export]
	public float radius = 0.25f;

	private int procs = 1;

	[Export]
	public int Procs
	{
		get => procs;
		set => procs = Mathf.Max(1, value);
	}

	[Export]
	public Array<SpellAction> MineProcActions = [];

	public override string GenerateGDScript(int indentation)
	{
		string procActionScript = """
		func(_who: SpellCastor):

		""";

		string linePrefix = new('\t', indentation);

		if(MineProcActions.Count > 0)
			foreach(SpellAction spellAct in MineProcActions)
				if(spellAct is not null)
					procActionScript += $"{spellAct.GenerateGDScript(indentation + 1)}\n";
		procActionScript += $"{linePrefix}\tpass\n";

		return $"{linePrefix}var {projectileVarName} := MineProjectile.new()\n"
		+ $"{linePrefix}{spawnTarget}.add_sibling.call_deferred({projectileVarName})\n"
		+ $"{linePrefix}{projectileVarName}.position = {spawnTarget}.position\n"
		+ $"{linePrefix}{projectileVarName}.Radius = {radius}\n"
		+ $"{linePrefix}{projectileVarName}.RemainingProcs = {procs}\n"
		+ $"{linePrefix}{projectileVarName}.MineProc.connect({procActionScript}{linePrefix})\n";
	}

	public override int GetComplexity()
	{
		int added = 0;
		foreach(SpellAction act in MineProcActions)
			if(act is not null)
				added += act.GetComplexity();
		return 5 * procs + added;
	}

	public override float GetManaCost()
	{
		float added = 0.0f;
		foreach(SpellAction act in MineProcActions)
			if(act is not null)
				added += act.GetManaCost();
		return 2.5f * radius + added;
	}
}