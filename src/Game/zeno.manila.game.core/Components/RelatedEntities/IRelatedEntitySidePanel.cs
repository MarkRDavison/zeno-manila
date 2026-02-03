namespace zeno.manila.game.core.Components.RelatedEntities;

public interface IRelatedEntitySidePanel
{
    string Metadata { get; }
    void SetRelatedEntity(IEntity? relatedEntity);
}
