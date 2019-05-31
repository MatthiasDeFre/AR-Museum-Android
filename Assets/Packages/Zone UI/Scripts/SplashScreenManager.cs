using UnityEngine;

namespace Michsky.UI.Zone
{
    public class SplashScreenManager : MonoBehaviour
    {
        [Header("RESOURCES")]
        public GameObject splashScreen;
        public GameObject mainPanels;
        public GameObject homePanel;
        public Animator backgroundAnimator;
        private Animator mainPanelsAnimator;
        private Animator homePanelAnimator;

        private BlurManager bManager;

        [Header("SETTINGS")]
        public bool disableSplashScreen;

        void Start()
        {
            bManager = gameObject.GetComponent<BlurManager>();
            mainPanelsAnimator = mainPanels.GetComponent<Animator>();
            homePanelAnimator = homePanel.GetComponent<Animator>();

            if (disableSplashScreen == true)
            {
                splashScreen.SetActive(false);
                mainPanels.SetActive(true);
         
                mainPanelsAnimator.Play("Start");
                homePanelAnimator.Play("Panel In");
                backgroundAnimator.Play("Switch");
                bManager.BlurInAnim();
            }

            else
            {
                mainPanelsAnimator.Play("Invisible");
                bManager.BlurOutAnim();
                splashScreen.SetActive(true);
            }
        }
    }
}