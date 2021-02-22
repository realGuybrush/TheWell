using UnityEngine;

public partial class BasicMovement: MonoBehaviour
{
    public bool crawling;
    public float crawlingMultiplier = 0.5f;

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
