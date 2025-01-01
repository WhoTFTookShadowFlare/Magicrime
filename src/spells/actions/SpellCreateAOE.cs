using Godot;
using Godot.Collections;
namespace Magicrime.Spells.Actions;

[Tool]
[GlobalClass]
public partial class SpellCreateAOE : SpellAction
{
	private float areaLifeTime = 5.0f;
	private float effectApplicationRate = 1.0f;

	[Export]
	public string projectileVarName = "AOE";

	[Export]
	public string spawnTarget = "_castor";

	[Export]
	public float AreaLifeTime
	{
		get => areaLifeTime;
		set => areaLifeTime = Mathf.Max(1.0f/60.0f, value);
	}

	[Export]
	public float EffectApplicationRate
	{
		get => effectApplicationRate;
		set => effectApplicationRate = Mathf.Max(1.0f/60.0f, value);
	}

	[Export]
	public float Radius = 5.0f;

	[Export]
	public Array<SpellAction> AffectedTargetsActions = [];

	[Export]
	public Array<SpellAction> OnLifeEndActions = [];

	public override string GenerateGDScript(int indentation)
	{
		string affectedTargetScript = """
		func(who: SpellCastor):

		""";

		string lifeEndActionScript = """
		func():

		""";

		string linePrefix = new('\t', indentation);

		if(AffectedTargetsActions.Count > 0)
			foreach(SpellAction spellAct in AffectedTargetsActions)
				if(spellAct is not null)
					affectedTargetScript += $"{spellAct.GenerateGDScript(indentation + 1)}\n";
		else
			affectedTargetScript += $"{linePrefix}\tpass";
		
		if(OnLifeEndActions.Count > 0)
			foreach(SpellAction spellAct in OnLifeEndActions)
				lifeEndActionScript += $"{spellAct.GenerateGDScript(indentation + 1)}\n";
		else
			lifeEndActionScript += $"{linePrefix}\tpass";

		return $"{linePrefix}var {projectileVarName} := AOEProjectile.new()\n"
		+ $"{linePrefix}{spawnTarget}.add_sibling.call_deferred({projectileVarName})\n"
		+ $"{linePrefix}{projectileVarName}.position = {spawnTarget}.position\n"
		+ $"{linePrefix}{projectileVarName}.LifeTimeRem = {AreaLifeTime}\n"
		+ $"{linePrefix}{projectileVarName}.EffectApplicationRate = {EffectApplicationRate}\n"
		+ $"{linePrefix}{projectileVarName}.EffectRadius = {Radius}\n"
		+ $"{linePrefix}{projectileVarName}.ApplyAffectToTarget.connect({affectedTargetScript})\n"
		+ $"{linePrefix}{projectileVarName}.LifeSpanEnded.connect({lifeEndActionScript}\n{linePrefix})";
	}

	public override int GetComplexity()
	{
		int added = 0;
		foreach(SpellAction action in AffectedTargetsActions)
			if(action is not null)
				added += action.GetComplexity();
		foreach(SpellAction action in OnLifeEndActions)
			if(action is not null)
				added += action.GetComplexity();
		return Mathf.RoundToInt(Radius) + Mathf.RoundToInt(EffectApplicationRate) + added;
	}

	public override float GetManaCost()
	{
		float added = 0.0f;
		foreach(SpellAction action in AffectedTargetsActions)
			if(action is not null)
				added += action.GetManaCost();
		foreach(SpellAction action in OnLifeEndActions)
			if(action is not null)
				added += action.GetManaCost();
		return Radius * Mathf.Max(1.0f, AreaLifeTime) + added;
	}
}