using System;

public class FireButton : Button
{
    //누르면 발사
    public event Action FireButtonClicked;


    public override void OnInteract()
    {
        FireButtonClicked?.Invoke();
    }

}