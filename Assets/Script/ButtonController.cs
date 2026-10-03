using UnityEngine;

public class ButtonController : MonoBehaviour
{
    [SerializeField] private GameObject textKeyboardController;
    [SerializeField] private GameObject textDescription;


    public void ButtonClicked()
    {
        textKeyboardController.SetActive(false);
        textDescription.SetActive(false);
        gameObject.SetActive(false);
        Time.timeScale = 1;
    }
}
