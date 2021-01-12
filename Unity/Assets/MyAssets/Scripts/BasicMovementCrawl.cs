using UnityEngine;

public partial class BasicMovement: MonoBehaviour
{
    public bool crawling;
    public float crawlingMultiplier = 0.5f;
    public float rollSpeedX = 10.0f;
    public float rollSpeedY = 0.0f;

    public void Crawl()
    {
        crawling = true;
    }

    public void UnCrawl()
    {
        crawling = false;
    }

    public bool CheckCrawl()
    {
        return crawling;
    }
}
