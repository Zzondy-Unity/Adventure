using System;

public class FireButton : Button
{
    //������ �߻�
    public event Action FireButtonClicked;


    public override void OnInteract()
    {
        FireButtonClicked?.Invoke();
    }

}