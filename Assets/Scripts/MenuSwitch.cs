using UnityEngine;

public class MenuSwitcher : MonoBehaviour
{
    public GameObject menu1;
    public GameObject menu2;

    private bool isMenu1Active = true;
    public void SwitchMenu()
    {
        isMenu1Active = !isMenu1Active;

        menu1.SetActive(isMenu1Active);
        menu2.SetActive(!isMenu1Active);
    }
}