using Godot;

namespace Magicrime.Spells;

public partial class Test : SpellCastor
{
	[Export]
	public Spell spell;

	public override void _Ready()
	{
		GD.Print($"Complexity: {spell.GetComplexity()},\nMana cost: {spell.GetManaCost()},\nCode:\n");

		Script script = spell.CompileSpell();
		Node node = new()
		{
			Name = spell.spellName
		};
		ulong nodeID = node.GetInstanceId();
		node.SetScript(script);
		node = (Node) InstanceFromId(nodeID);
		AddChild(node);
		node.Call("_exec_spell", this);
	}
}
