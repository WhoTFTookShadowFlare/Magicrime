using Godot;
using Godot.Collections;

namespace Magicrime.Spells.Actions;

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

	[Export]
	public Array<SpellAction> OnLifeEndActions = [];

	public override string GenerateGDScript(int indentation)
	{
		string lifeEndActionScript = """
		func():

		""";

		string linePrefix = new('\t', indentation);

		if(OnLifeEndActions.Count > 0)
			foreach(SpellAction spellAct in OnLifeEndActions)
				lifeEndActionScript += $"{spellAct.GenerateGDScript(indentation + 1)}\n";
		else
			lifeEndActionScript += $"{linePrefix}\tpass";

		return $"{linePrefix}var {projectileVarName} := NormalProjectile.new()\n"
		+ $"{linePrefix}{spawnTarget}.add_sibling.call_deferred({projectileVarName})\n"
		+ $"{linePrefix}if \"{spawnTarget}\" == \"_castor\":\n"
		+ $"{linePrefix}\t{projectileVarName}.position = _castor.GetSpawnSpot({height})\n"
		+ $"{linePrefix}\t{projectileVarName}.Velocity = _castor.GetLookDirection() * {speed}\n"
		+ $"{linePrefix}else:\n"
		+ $"{linePrefix}\t{projectileVarName}.position = {spawnTarget}.position\n"
		+ $"{linePrefix}\tif \"Velocity\" in {spawnTarget}: {projectileVarName}.Velocity = {spawnTarget}.Velocity\n"
		+ $"{linePrefix}{projectileVarName}.Radius = {radius}\n"
		+ $"{linePrefix}{projectileVarName}.Height = {height}\n"
		+ $"{linePrefix}{projectileVarName}.LifeTimeRem = {lifeTime}\n"
		+ $"{linePrefix}{projectileVarName}.LifeSpanEnded.connect({lifeEndActionScript}\n{linePrefix})";
	}

	public override int GetComplexity()
	{
		int added = 0;
		foreach(SpellAction lifeEndAct in OnLifeEndActions)
			if(lifeEndAct is not null)
				added += lifeEndAct.GetComplexity();
		return 5 + added;
	}

	public override float GetManaCost()
	{
		float added = 0.0f;
		foreach(SpellAction lifeEndAct in OnLifeEndActions)
			if(lifeEndAct is not null)
				added += lifeEndAct.GetManaCost();
		return (radius * 10.0f) + (height * 10.0f) + (lifeTime / 10.0f) + speed;
	}
}