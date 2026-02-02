namespace zeno.manila.game.core.Services;

public sealed class SidePanelService : ISidePanelService
{
    private string? _activePanel;
    private readonly BuildingsSidePanel _buildingsSidePanel;
    private readonly RelatedEntitySidePanel _relatedEntitySidePanel;
    private readonly ManilaGameData _manilaGameData;
    private readonly IBuildingService _buildingService;

    public SidePanelService(
        BuildingsSidePanel buildingsSidePanel,
        RelatedEntitySidePanel relatedEntitySidePanel,
        ManilaGameData manilaGameData,
        IBuildingService buildingService)
    {
        _buildingsSidePanel = buildingsSidePanel;
        _relatedEntitySidePanel = relatedEntitySidePanel;
        _manilaGameData = manilaGameData;
        _buildingService = buildingService;

        _buildingService.OnBuildingCreated += (s, e) =>
        {
            if (!e.CreatedManually)
            {
                return;
            }

            var (_, sprawl) = _manilaGameData.GetCityAndSprawlAtTile(e.X, e.Y);

            if (sprawl?.RelatedEntity is not null)
            {
                DisplayPanel(ManilaConstants.Panel_RelatedEntity);
                _relatedEntitySidePanel.SetRelatedEntity(sprawl.RelatedEntity);
            }
        };
    }


    public SidePanel? ActiveSidePanel { get; private set; }
    public EventHandler OnPanelChanged { get; set; } = default!;

    public void DisplayPanel(string panel)
    {
        _activePanel = panel;

        ActiveSidePanel?.IsActive = false;

        ActiveSidePanel = panel switch
        {
            ManilaConstants.Panel_Buildings => _buildingsSidePanel,
            ManilaConstants.Panel_RelatedEntity => _relatedEntitySidePanel,
            _ => throw new InvalidOperationException($"Trying to display an invalid panel: {panel}")
        };

        ActiveSidePanel.IsActive = true;
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
