namespace zeno.manila.game.core.Components.RelatedEntities;

internal abstract class BaseRelatedEntitySidePanel : SidePanel, IRelatedEntitySidePanel
{
    public abstract string Metadata { get; }
    protected IEntity? RelatedEntity { get; set; }

    public void SetRelatedEntity(IEntity? relatedEntity)
    {
        RelatedEntity = relatedEntity;
        OnRelatedEntitySet();
    }

    protected virtual void OnRelatedEntitySet() { }
}
