using MongoDB.Bson;

namespace OmniePDV.API.Data.Entities;

public abstract class Entity
{
    public Entity(Guid UID)
    {
        this.id = ObjectId.GenerateNewId();
        this.UID = UID;
    }

    private Entity()
    {
    }

    public ObjectId id { get; private set; }
    public Guid UID { get; private set; }
}
