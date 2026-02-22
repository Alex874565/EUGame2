public interface IInteractable
{
    public bool IsSelected { get; set; }
    
    public void OnClick()
    {
        return;
    }

    public void OnHoverEnter()
    {
        return;
    }
    
    public void OnHoverExit()
    {
        return;
    }
    
    public void Select()
    {
        return;
    }
    
    public void Deselect()
    {
        return;
    }
}