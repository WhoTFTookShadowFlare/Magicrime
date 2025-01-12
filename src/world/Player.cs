using Godot;
using Godot.Collections;
using Magicrime.Spells;

namespace Magicrime.World;

[Tool]
[GlobalClass]
public sealed partial class Player : SpellCastor
{
	private Range manaDisp;
	private Range healthDisp;

	private PlayerSpellLoadout loadout;

	[Export]
	public Range ManaDisp
	{
		get => manaDisp;
		set
		{
			manaDisp = value;
			UpdateManaBar();
		}
	}

	[Export]
	public Range HealthDisp
	{
		get => healthDisp;
		set
		{
			healthDisp = value;
			UpdateHealthBar();
		}
	}

	[Export]
	public PlayerSpellLoadout Loadout
	{
		get => loadout;
		set
		{
			loadout = value;
			if(Engine.IsEditorHint()) return;
			foreach(Node node in spellNodes.Values)
				node.QueueFree();
			
			if(loadout is null) return;

			spellNodes = loadout.CompileSpells();
			foreach(Node node in spellNodes.Values)
				CallDeferred(MethodName.AddChild, node);
		}
	}

	private Dictionary<Spell, Node> spellNodes = [];

	public void UpdateHealthBar()
	{
		healthDisp.Value = Health;
	}

	public void UpdateManaBar()
	{
		manaDisp.Value = Mana;
	}

	public override void _Ready()
	{
		HealthChanged += UpdateHealthBar;
		ManaChanged += UpdateManaBar;
	}

	public override void _PhysicsProcess(double delta)
	{
		if(Engine.IsEditorHint()) return;

		base._PhysicsProcess(delta);

		Vector2 inDir = Input.GetVector(
			"mv_leftward", "mv_rightward",
			"mv_forward", "mv_backward"
		);
		Vector2 mvDir = inDir.Normalized().Rotated(-Rotation.Y);
		Vector3 velo = Velocity;
		
		velo.X += mvDir.X;
		velo.Z += mvDir.Y;

		velo.Y -= 9.8f * (float) delta;
		
		velo.X /= 1.2f;
		velo.Z /= 1.2f;
		
		Velocity = velo;
		MoveAndSlide();

		if(CanCastSpell()) 
			if(Input.IsActionPressed("spell_left") && loadout.GetLeftSpell() is not null)
			{
				if(Mathf.IsZeroApprox(UseMana(loadout.GetLeftSpell().GetManaCost())))
				{
					spellNodes[loadout.GetLeftSpell()].Call("_exec_spell", this);
					ApplyCastDelay();
				}
			}
			else 
			if(Input.IsActionPressed("spell_right") && loadout.GetRightSpell() is not null)
			{
				if(Mathf.IsZeroApprox(UseMana(loadout.GetRightSpell().GetManaCost())))
				{
					spellNodes[loadout.GetRightSpell()].Call("_exec_spell", this);
					ApplyCastDelay();
				}
			}
	}

	public override void _Input(InputEvent evt)
	{
		if(Engine.IsEditorHint()) return;

		loadout.SelectedSet += Mathf.RoundToInt(Input.GetAxis("spell_set_prev", "spell_set_next"));

		if(evt is InputEventMouseMotion motion)
		{
			Input.MouseMode = Input.MouseModeEnum.Captured;
			Vector3 rot = LookDirection.Rotation;
			rot.X = Mathf.Clamp(rot.X + motion.Relative.Y * -0.0125f, Mathf.DegToRad(-90.0f), Mathf.DegToRad(90.0f));
			LookDirection.Rotation = rot;
			rot = Rotation;
			rot.Y += motion.Relative.X * -0.0125f;
			Rotation = rot;
		}
	}
}