using Godot;

namespace Magicrime.Spells;

public partial class Test : SpellCastor
{
	[Export]
	public Spell spell;

	private Node spellNode;

	public override void _Ready()
	{
		GD.Print($"Complexity: {spell.GetComplexity()},\nMana cost: {spell.GetManaCost()},\nCode:\n");

		Script script = spell.CompileSpell();
		spellNode = new()
		{
			Name = spell.spellName
		};
		ulong nodeID = spellNode.GetInstanceId();
		spellNode.SetScript(script);
		spellNode = (Node) InstanceFromId(nodeID);
		AddChild(spellNode);
	}

	public override void _Input(InputEvent evt)
	{
		if(evt is InputEventKey key)
			if(key.Pressed && key.Keycode == Key.Space)
			{
				spellNode.Call("_exec_spell", this);
				GD.Print("cast spell");
			}
	}
}
