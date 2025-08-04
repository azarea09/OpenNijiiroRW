using Newtonsoft.Json;

namespace OpenNijiiroRW;

class DBEncyclopediaMenus : CSavableT<DBEncyclopediaMenus.EncyclopediaMenu>
{
	public DBEncyclopediaMenus()
	{
		_fn = @$"{OpenNijiiroRW.strEXEのあるフォルダ}Encyclopedia{Path.DirectorySeparatorChar}Menus.json";
		base.tDBInitSavable();
	}

	#region [Auxiliary classes]
	public class EncyclopediaMenu
	{
		[JsonProperty("menus")]
		public KeyValuePair<int, EncyclopediaMenu>[] Menus;

		[JsonProperty("pages")]
		public int[] Pages;
	}

	#endregion
}
