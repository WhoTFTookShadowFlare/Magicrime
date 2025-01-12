using Godot;

namespace Magicrime.Spells;

public sealed partial class SpellImporter : Node
{
	public static void HandleFilesDropped(string[] files)
	{
		foreach(string filePath in files)
		{
			ImportExternalSpell(filePath);
		}
	}

	public static void ImportExternalSpell(string spellPath)
	{
		if(DirAccess.DirExistsAbsolute(Spell.SPELL_DIR))
		DirAccess.MakeDirRecursiveAbsolute("user://spells");
		FileAccess toImport = FileAccess.Open(spellPath, FileAccess.ModeFlags.Read);
		string addedExtension = spellPath.EndsWith(".tres") ? "" : ".tres";
		FileAccess imported = FileAccess.Open($"{Spell.SPELL_DIR}/{spellPath.GetFile()}{addedExtension}", FileAccess.ModeFlags.Write);
		imported.StoreBuffer(toImport.GetBuffer((long) toImport.GetLength()));
		imported.Flush();
	}

	public override void _Ready()
	{
		GetWindow().FilesDropped += HandleFilesDropped;
	}
}