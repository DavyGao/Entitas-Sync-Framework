using System.Text;
using NetStack.Serialization;
using Sources.Tools;

public static class UnpackEntityUtility
{
    public static void CreateEntities(GameContext game, BitBuffer buffer, ushort entityCount)
    {
        for (int i = 0; i < entityCount; i++)
        {
		    var addedComponents = new StringBuilder(128);

            var e = game.CreateEntity();

            var componentsCount = buffer.ReadUShort();

            for (int j = 0; j < componentsCount; j++)
            {
                var componentId = buffer.ReadUShort();

                switch (componentId)
                {
                    case 0:
					{
					    addedComponents.Append(" Id ");
                        var lookup = GameComponentsLookup.Id;
						var comp = e.CreateComponent<IdComponent>(lookup);
						comp.Deserialize(buffer);
						e.AddComponent(lookup, comp);
					}
					    break;
                    case 1:
					{
					    addedComponents.Append(" Character ");
                        e.isCharacter = true;
					}
					    break;
                    case 2:
					{
					    addedComponents.Append(" ControlledBy ");
                        var lookup = GameComponentsLookup.ControlledBy;
						var comp = e.CreateComponent<ControlledBy>(lookup);
						comp.Deserialize(buffer);
						e.AddComponent(lookup, comp);
					}
					    break;
                    case 3:
					{
					    addedComponents.Append(" Connection ");
                        var lookup = GameComponentsLookup.Connection;
						var comp = e.CreateComponent<Connection>(lookup);
						comp.Deserialize(buffer);
						e.AddComponent(lookup, comp);
					}
					    break;
                    case 4:
					{
					    addedComponents.Append(" Sync ");
                        e.isSync = true;
					}
					    break;
                }
            }
			Logger.I.Log("UnpackEntityUtility", $" Entity-{e.id.Value}: created - ({addedComponents})");
        }
    }
	
	public static void ChangeComponents(GameContext game, BitBuffer buffer, ushort componentCount)
	{
		for (int i = 0; i < componentCount; i++)
		{
			var entityId    = buffer.ReadUShort();
			var componentId = buffer.ReadUShort();
			var e           = game.GetEntityWithId(entityId);

			switch (componentId)
			{
                    case 0:
					{
						Logger.I.Log("UnpackEntityUtility", $" Entity-{entityId}: Changed Id component");
                        var lookup = GameComponentsLookup.Id;
						var comp = e.CreateComponent<IdComponent>(lookup);
				        comp.Deserialize(buffer);
				        e.ReplaceComponent(lookup, comp);
					}
					    break;
                    case 1:
					{
						Logger.I.Log("UnpackEntityUtility", $" Entity-{entityId}: Changed Character component");
                        e.isCharacter = true;
					}
					    break;
                    case 2:
					{
						Logger.I.Log("UnpackEntityUtility", $" Entity-{entityId}: Changed ControlledBy component");
                        var lookup = GameComponentsLookup.ControlledBy;
						var comp = e.CreateComponent<ControlledBy>(lookup);
				        comp.Deserialize(buffer);
				        e.ReplaceComponent(lookup, comp);
					}
					    break;
                    case 3:
					{
						Logger.I.Log("UnpackEntityUtility", $" Entity-{entityId}: Changed Connection component");
                        var lookup = GameComponentsLookup.Connection;
						var comp = e.CreateComponent<Connection>(lookup);
				        comp.Deserialize(buffer);
				        e.ReplaceComponent(lookup, comp);
					}
					    break;
                    case 4:
					{
						Logger.I.Log("UnpackEntityUtility", $" Entity-{entityId}: Changed Sync component");
                        e.isSync = true;
					}
					    break;
			}
		}
	}
	
	public static void RemoveComponents(GameContext game, BitBuffer buffer, ushort componentCount)
	{
		for (int i = 0; i < componentCount; i++)
		{
			var entityId = buffer.ReadUShort();
			var componentId = buffer.ReadUShort();
			var e = game.GetEntityWithId(entityId);

			switch (componentId)
			{
                    case 0:
					{
					    Logger.I.Log("UnpackEntityUtility", $" Entity-{entityId}: Removed Id component");
                        e.RemoveId();
					}
					    break;
                    case 1:
					{
					    Logger.I.Log("UnpackEntityUtility", $" Entity-{entityId}: Removed Character component");
                        e.isCharacter = false;
					}
					    break;
                    case 2:
					{
					    Logger.I.Log("UnpackEntityUtility", $" Entity-{entityId}: Removed ControlledBy component");
                        e.RemoveControlledBy();
					}
					    break;
                    case 3:
					{
					    Logger.I.Log("UnpackEntityUtility", $" Entity-{entityId}: Removed Connection component");
                        e.RemoveConnection();
					}
					    break;
                    case 4:
					{
					    Logger.I.Log("UnpackEntityUtility", $" Entity-{entityId}: Removed Sync component");
                        e.isSync = false;
					}
					    break;
			}
		}
	}
	
	public static void RemoveEntities(GameContext game, BitBuffer buffer, ushort entityCount)
	{
		for (int i = 0; i < entityCount; i++)
		{
			var id = buffer.ReadUShort();
			var e = game.GetEntityWithId(id);
            e.isDestroyed = true;
			Logger.I.Log("UnpackEntityUtility", $" Entity-{id}: is removed");
		}
	}
}