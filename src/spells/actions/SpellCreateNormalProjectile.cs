using Godot;
using Godot.Collections;

namespace Magicrime.Spells.Actions;

[Tool]
[GlobalClass]
public sealed partial class SpellCreateNormalProjectile : SpellAction
{
	[Export]
	public string projectileVarName = "projectile";

	[Export]
	public string spawnTarget = "_castor";

	[Export]
	public float radius = 0.1f;

	[Export]
	public float height = 0.5f;

	[Export]
	public float lifeTime = 30.0f;

	[Export]
	public float speed = 1.0f;

	private int pierce = 1;

	[Export]
	public int Pierce
	{
		get => pierce;
		set => pierce = Mathf.Max(1, value);
	}

	[Export]
	public Array<SpellAction> OnLifeEndActions = [];

	[Export]
	public Array<SpellAction> HitCharacterActions = [];

	public override string GenerateGDScript(int indentation)
	{
		string hitCharacterActionScript = """
		func(who: SpellCastor):

		""";

		string lifeEndActionScript = """
		func():

		""";

		string linePrefix = new('\t', indentation);

		if(HitCharacterActions.Count > 0)
			foreach(SpellAction spellAct in HitCharacterActions)
				if(spellAct is not null)
					lifeEndActionScript += $"{spellAct.GenerateGDScript(indentation + 1)}\n";
		hitCharacterActionScript += $"{linePrefix}\tpass\n";

		if(OnLifeEndActions.Count > 0)
			foreach(SpellAction spellAct in OnLifeEndActions)
				if(spellAct is not null)
					lifeEndActionScript += $"{spellAct.GenerateGDScript(indentation + 1)}\n";
		lifeEndActionScript += $"{linePrefix}\tpass\n";

		return $"{linePrefix}var {projectileVarName} := NormalProjectile.new()\n"
		+ $"{linePrefix}{spawnTarget}.add_sibling.call_deferred({projectileVarName})\n"
		+ $"{linePrefix}if \"{spawnTarget}\" == \"_castor\":\n"
		+ $"{linePrefix}\t{projectileVarName}.position = _castor.GetSpawnSpot({height + 0.1})\n"
		+ $"{linePrefix}\t{projectileVarName}.Velocity = _castor.GetLookDirection() * {speed}\n"
		+ $"{linePrefix}\t{projectileVarName}.Ignored = _castor\n"
		+ $"{linePrefix}else:\n"
		+ $"{linePrefix}\t{projectileVarName}.position = {spawnTarget}.position\n"
		+ $"{linePrefix}\t{projectileVarName}.Velocity = {spawnTarget}.Velocity\n"
		+ $"{linePrefix}{projectileVarName}.Radius = {radius}\n"
		+ $"{linePrefix}{projectileVarName}.Height = {height}\n"
		+ $"{linePrefix}{projectileVarName}.Pierce = {pierce}\n"
		+ $"{linePrefix}{projectileVarName}.LifeTimeRem = {lifeTime}\n"
		+ $"{linePrefix}{projectileVarName}.LifeSpanEnded.connect({lifeEndActionScript}{linePrefix})\n"
		+ $"{linePrefix}{projectileVarName}.HitCharacter.connect({hitCharacterActionScript}{linePrefix})\n";
	}

	public override int GetComplexity()
	{
		int added = 0;
		foreach(SpellAction lifeEndAct in OnLifeEndActions)
			if(lifeEndAct is not null)
				added += lifeEndAct.GetComplexity();
		foreach(SpellAction act in HitCharacterActions)
			if(act is not null)
				added += act.GetComplexity();
		return 5 + added;
	}

	public override float GetManaCost()
	{
		float added = 0.0f;
		foreach(SpellAction lifeEndAct in OnLifeEndActions)
			if(lifeEndAct is not null)
				added += lifeEndAct.GetManaCost();
		foreach(SpellAction act in HitCharacterActions)
			if(act is not null)
				added += act.GetManaCost();
		return (radius * 10.0f) + (height * 10.0f) + (lifeTime / 10.0f) + speed;
	}
}