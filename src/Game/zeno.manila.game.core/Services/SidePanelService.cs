namespace zeno.manila.game.core.Services;

public sealed class SidePanelService : ISidePanelService
{
    private string? _activePanel;
    private readonly BuildingsSidePanel _buildingsSidePanel;
    private readonly RelatedEntitySidePanel _relatedEntitySidePanel;

    public SidePanelService(
        BuildingsSidePanel buildingsSidePanel,
        RelatedEntitySidePanel relatedEntitySidePanel)
    {
        _buildingsSidePanel = buildingsSidePanel;
        _relatedEntitySidePanel = relatedEntitySidePanel;
    }


    public SidePanel? ActiveSidePanel { get; private set; }
    public EventHandler OnPanelChanged { get; set; } = default!;

    public void DisplayPanel(string panel)
    {
        _activePanel = panel;

        if (ActiveSidePanel is not null)
        {
            ActiveSidePanel.IsActive = false;
        }

        ActiveSidePanel = panel switch
        {
            "Buildings" => _buildingsSidePanel,
            "RelatedEntity" => _relatedEntitySidePanel,
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

    public string? GetActivePanel() => _activePanel;
}
