namespace zeno.manila.game.core.Services;

public sealed class SidePanelService : ISidePanelService
{
    private string? _activePanel;
    private readonly BuildingsSidePanel _buildingsSidePanel;
    private readonly RelatedEntitySidePanel _relatedEntitySidePanel;
    private readonly ManilaGameData _manilaGameData;
    private readonly IBuildingService _buildingService;

    private readonly IList<IRelatedEntitySidePanel> _relatedEntitySidePanels;

    public SidePanelService(
        BuildingsSidePanel buildingsSidePanel,
        RelatedEntitySidePanel relatedEntitySidePanel,
        ManilaGameData manilaGameData,
        IBuildingService buildingService,
        IEnumerable<IRelatedEntitySidePanel> relatedEntitySidePanels)
    {
        _buildingsSidePanel = buildingsSidePanel;
        _relatedEntitySidePanel = relatedEntitySidePanel;
        _manilaGameData = manilaGameData;
        _buildingService = buildingService;
        _relatedEntitySidePanels = [.. relatedEntitySidePanels];

        _buildingService.OnBuildingCreated += (s, e) =>
        {
            if (!e.CreatedManually)
            {
                return;
            }

            var (_, sprawl) = _manilaGameData.GetCityAndSprawlAtTile(e.X, e.Y);

            if (sprawl?.RelatedEntity is not null)
            {
                // TODO: Prototype id or something other than name
                DisplayPanel(ManilaConstants.Panel_RelatedEntity, sprawl.RelatedEntity.Name ?? string.Empty);
                _relatedEntitySidePanel.SetRelatedEntity(sprawl.RelatedEntity);
                if (ActiveSidePanel is RelatedEntitySidePanel resp)
                {
                    resp.SetRelatedEntity(sprawl.RelatedEntity);
                }
                // TODO: Consolidate
                if (ActiveSidePanel is IRelatedEntitySidePanel respi)
                {
                    respi.SetRelatedEntity(sprawl.RelatedEntity);
                }
            }
        };
    }


    public SidePanel? ActiveSidePanel { get; private set; }
    public EventHandler OnPanelChanged { get; set; } = default!;

    public void DisplayPanel(string panel, string metadata)
    {
        _activePanel = panel;

        ActiveSidePanel?.IsActive = false;

        ActiveSidePanel = panel switch
        {
            ManilaConstants.Panel_Buildings => _buildingsSidePanel,
            ManilaConstants.Panel_RelatedEntity => GetRelatedEntitySidePanel(metadata),
            _ => throw new InvalidOperationException($"Trying to display an invalid panel: {panel}")
        };

        ActiveSidePanel.IsActive = true;
    }

    private SidePanel GetRelatedEntitySidePanel(string metadata)
    {
        // TODO: switch on specific related entity to display its own side panel???
        if (_relatedEntitySidePanels.FirstOrDefault(_ => _.Metadata == metadata) is { } relataedEntitySidePanel &&
            relataedEntitySidePanel is SidePanel sidePanel) // TODO: Bad casting
        {
            return sidePanel;
        }

        return _relatedEntitySidePanel;
    }

    public void ClearPanel()
    {
        if (ActiveSidePanel is not null)
        {
            ActiveSidePanel.IsActive = false;
        }

        ActiveSidePanel = null;
        _activePanel = null;
    }
    public void ClearPanel(string panel)
    {
        if (GetActivePanel() == panel)
        {
            ClearPanel();
        }
    }

    public string? GetActivePanel() => _activePanel;
}
