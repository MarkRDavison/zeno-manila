namespace zeno.manila.game.core.Services.Contracts;

public interface ISidePanelService
{
    SidePanel? ActiveSidePanel { get; }
    EventHandler OnPanelChanged { get; set; }
    void DisplayPanel(string panel);
    void ClearPanel();
    void ClearPanel(string panel);
    string? GetActivePanel();
}
