using Godot;

namespace Magicrime.Spells;

[GlobalClass]
public abstract partial class SpellAction : Resource
{
	/// <summary>
	/// Generates GDScript for what the action does.
	/// </summary>
	/// <returns>A GDScript snippet to run when the spell is cast</returns>
	public abstract string GenerateGDScript(int indentation);

	public abstract float GetManaCost();

	public abstract int GetComplexity();
}