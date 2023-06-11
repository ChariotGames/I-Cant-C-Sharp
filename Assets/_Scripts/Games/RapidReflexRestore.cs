namespace _Scripts.Games
{
    public class RapidReflexRestore
    {
        [SerializeField] private Difficulty difficulty;
        [SerializeField] private int timeToAnswerInMs;
        [SerializeField] private float lightTimer;
        [SerializeField] private GameObject lightsTop, lightsBottom, light, overlayContainer, background;
        [SerializeField] private Color darkRed, lightRed, darkGreen, lightGreen;
        [SerializeField] private List<Color> flashColor;
        [SerializeField] private TMP_Text gameStateText;
        
        
        private const int NUMBER_LIGHTS_ROW = 5;
        private SpriteRenderer[] bulbsSpriteTop = new SpriteRenderer[NUMBER_LIGHTS_ROW];
        private SpriteRenderer[] bulbsSpriteBottom = new SpriteRenderer[NUMBER_LIGHTS_ROW];
        private SpriteRenderer backroundSprite;
        private float timeElapsed = 0;
        private bool isButtonPressed = false;
        private float randomDelay = 0;
        
        
        if (difficulty == Difficulty.LVL3 && i == NUMBER_LIGHTS_ROW-1) StartCoroutine(randomDistraction());
        
        
        private IEnumerator randomDistraction()
        {
            if (Random.Range(0, 1.1f) > 0.5f)
            {
                UnityEngine.Debug.Log("No Distraction");
                yield break;
            }

            float delay = Random.Range(0.5f, lightTimer + randomDelay / 3);
            yield return new WaitForSeconds(delay);
            UnityEngine.Debug.Log("Flashdelay: " + delay);
            StartCoroutine(flashBackground());
        }

        private IEnumerator flashBackground()
        {
            backroundSprite.color = flashColor[Random.Range(0, flashColor.Count)];
            background.SetActive(true);
            yield return new WaitForSeconds(0.2f);
            background.SetActive(false);
        }
    }
}