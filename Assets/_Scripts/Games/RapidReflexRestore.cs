namespace _Scripts.Games
{
    public class RapidReflexRestore
    {
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