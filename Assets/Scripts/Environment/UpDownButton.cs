using System;

public class UpDownButton : Button
{
    public event Action<int> UpDownButtonClick;

    public override void OnInteract()
    {
        if (buttonData.buttonType == ButtonType.Down)
        {
            UpDownButtonClick?.Invoke(-1);
        }
        else if (buttonData.buttonType == ButtonType.Up)
        {
            UpDownButtonClick?.Invoke(2);
        }
    }

}