using Godot;
using Godot.Collections;
using Magicrime.Spells;

namespace Magicrime.World;

[Tool]
[GlobalClass]
public partial class PlayerSpellLoadout : Resource
{
	private Spell spellL1;
	private Spell spellR1;

	private Spell spellL2;
	private Spell spellR2;

	private Spell spellL3;
	private Spell spellR3;

	/// <summary>
	/// Godot does not recognize the L/R 4/5 on steam deck.
	/// Will probably need more controllers to have those 4 buttons too.
	/// This is the best alternative, though might keep for keyboard + mouse (as bool option).
	/// Loadout sets with no spells will be skipped.
	/// </summary>
	private int selectedSet = 0;

	[Export]
	public int SelectedSet
	{
		get => selectedSet;
		set
		{
			if(value == selectedSet) return;
			bool isDescending = value < selectedSet;
			selectedSet = Mathf.PosMod(value, 3);

			if(IsSpellEquiped())
				while(GetLeftSpell() is null && GetRightSpell() is null)
					selectedSet = Mathf.PosMod(selectedSet + (isDescending ? -1 : 1), 3);
			GD.Print($"Changed set to {selectedSet}");
		}
	}

	[Export]
	public Spell SpellL1
	{
		get => spellL1;
		set => spellL1 = value;
	}

	[Export]
	public Spell SpellR1
	{
		get => spellR1;
		set => spellR1 = value;
	}

	[Export]
	public Spell SpellL2
	{
		get => spellL2;
		set => spellL2 = value;
	}

	[Export]
	public Spell SpellR2
	{
		get => spellR2;
		set => spellR2 = value;
	}

	[Export]
	public Spell SpellL3
	{
		get => spellL3;
		set => spellL3 = value;
	}

	[Export]
	public Spell SpellR3
	{
		get => spellR3;
		set => spellR3 = value;
	}

	public Spell GetLeftSpell()
	{
		switch(selectedSet)
		{
			case 0:
				return spellL1;
			case 1:
				return spellL2;
			case 2:
				return spellL3;
		}
		GD.PushError("Selected spell set out of bounds!");
		return null;
	}

	public Spell GetRightSpell()
	{
		switch(selectedSet)
		{
			case 0:
				return spellR1;
			case 1:
				return spellR2;
			case 2:
				return spellR3;
		}
		GD.PushError("Selected spell set out of bounds!");
		return null;
	}

	public bool IsSpellEquiped()
	{
		return 
			spellL1 is not null || spellR1 is not null ||
			spellL2 is not null || spellR2 is not null ||
			spellL3 is not null || spellR3 is not null;
	}

	private static Node CompileProvidedSpell(Spell spell)
	{
		Node node = new()
		{
			Name = spell.SpellName
		};
		ulong id = node.GetInstanceId();
		node.SetScript(spell.CompileSpell());
		return (Node) InstanceFromId(id);
	}

	public Dictionary<Spell, Node> CompileSpells()
	{
		Dictionary<Spell, Node> spells = [];
		if(spellL1 is not null) spells[spellL1] = CompileProvidedSpell(spellL1);
		if(spellR1 is not null) spells[spellR1] = CompileProvidedSpell(spellR1);

		if(spellL2 is not null) spells[spellL2] = CompileProvidedSpell(spellL2);
		if(spellR2 is not null) spells[spellR2] = CompileProvidedSpell(spellR2);

		if(spellL3 is not null) spells[spellL3] = CompileProvidedSpell(spellL3);
		if(spellR3 is not null) spells[spellR3] = CompileProvidedSpell(spellR3);
		return spells;
	}
}